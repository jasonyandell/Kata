namespace Domain
open System.Collections.Concurrent
open System.Collections.Generic
// 81 variables
// each variable has a domain


type BoardProcessor (board:Board) =

    let isAreaValid (area:Set<Position>) : bool = 
        let digits:List<int<Dig>> = new List<int<Dig>>()
        let blankSpaces = ref 0
        // valid if no digit played twice
        area
        |> Set.iter (fun pos ->
            match (board.At pos) with
            | Some d -> digits.Add d
            | None -> blankSpaces := !blankSpaces+1)
        let uniqueDigits = digits |> Set.ofSeq
        let total = !blankSpaces + uniqueDigits.Count
        9 = total

    let where : Position [] = Array.init 81 (fun pos -> Position.FromIndex pos)
//    let cant : Set<int<Dig>> [] = Array.init 81 (fun pos -> Set.empty)
//    let can : Set<int<Dig>> [] = Array.init 81 (fun pos -> Index.AllDigits)


    let housesByPosition = 
        Index.AllPositions 
        |> Array.ofSeq
        |> Array.Parallel.map (fun pos -> 
            let house = new House(pos, board)
            (pos,house))
        |> Map.ofArray

    let can' : Set<int<Dig>> [] = Array.init 81 (fun pos -> housesByPosition.[Position.FromIndex pos].DigitsThatCanBePlayedInThisPosition)
    let required : Set<int<Dig>> [] = Array.init 81 (fun pos -> Set.empty)

    let isValid =
        let areaResult = 
            Index.AllAreas
            |> Array.forall (fun area -> area |> isAreaValid)
        let positionResult = 
            lazy
                housesByPosition
                |> Map.toArray
                |> Array.forall (fun (pos,house:House) -> not house.DigitsThatMayAppearInThisPosition.IsEmpty)

        areaResult && positionResult.Value


    let computeRequiredMoves () = 
        board.Moves 
        |> Seq.iter (fun (pos, digit) ->
            let index = pos.ToIndex
            let nowCantArea = housesByPosition.[pos].Area
            ()
//            cant.[index] <- Set.empty
//            can.[index] <- Set.empty
//
//            nowCantArea
//            |> Set.iter (fun cantPos ->
//                let i = cantPos.ToIndex
//                cant.[i] <- cant.[i].Add digit
//                can.[i] <- can.[i].Remove digit
         //   )
        )

        Index.AllAreas
        |> Array.map (fun (area:Set<Position>) -> area |> Set.filter (fun item -> (board.At item).IsNone))
        |> Array.iter (fun (area:Set<Position>) ->            

            area 
            |> Set.iter (fun (here:Position) ->

                // for every other position in the area
                // if can move there, remove it from can move[here] 

                let iHere = here.ToIndex

                let cantMoveHere = housesByPosition.[here].CantPlay
                can'.[iHere] <- can'.[iHere] - cantMoveHere

                area
                |> Set.iter (fun (there:Position) ->
                    if (here <> there) then
                        let thereMoves = housesByPosition.[there].DigitsThatCanBePlayedInThisPosition                      
                        can'.[iHere] <- can'.[iHere] - thereMoves
                    else ()
                )

                if (can'.[iHere].Count = 1) then
                    required.[iHere] <- required.[iHere] + can'.[iHere]
            )
        )

        let requirements = 
            Array.zip where required
            |> Array.filter (fun (p,s) -> Set.empty <> s)
            |> Array.map (fun (p,s) -> (p,s.MinimumElement))
        
        let requirementsMap : Map<Position,int<Dig>> = 
            requirements
            |> Map.ofArray
        requirementsMap

    let requiredMoves =
        match isValid with
        | true ->
            computeRequiredMoves()
        | false -> Map.empty

    let digitsRemaining:ConcurrentDictionary<int<Dig>,ConcurrentStack<int<Dig>>> =
        let kvps:KeyValuePair<int<Dig> , ConcurrentStack<int<Dig>>> seq = 
            let makeStak (d:int<Dig>) = 
                new System.Collections.Generic.KeyValuePair<int<Dig>,ConcurrentStack<int<Dig>>>
                    (d, new ConcurrentStack<int<Dig>>([for x in 1..9 -> d] :> int<Dig> seq))
            let p = Index.AllDigits |> Array.map (fun digit -> makeStak digit)
            p |> Array.toSeq
        let dict = new ConcurrentDictionary<int<Dig>,ConcurrentStack<int<Dig>>>(kvps)
        dict

    let canIGetADigit digit : bool =
        let mutable digitRef = 0<Dig>
        digitsRemaining.[digit].TryPop( ref digitRef )

    let initializeDigitsRemaining =
        Index.AllPositions 
        |> Set.iter (fun pos ->
            match board.At pos with
            | Some digit -> canIGetADigit digit |> ignore
            | _ -> ())

    let scoreDigit digit : int<score> =
        digitsRemaining.[digit].Count * 1<score>

    let scorePotentials (position:Position,digits:Set<int<Dig>>) =
        let options = digits.Count * 1<score>
        (options, (position, digits))

    let potentialMoves : Map<Position,Set<int<Dig>>> =
        match isValid with
        | true -> 
            let requiredPotentials =
                requiredMoves
                |> Map.toArray
                |> Array.map (fun (p, d) -> (p,[d]|>Set.ofList))
            let potentials' = 
                Index.AllPositions
                |> Set.toArray
                |> Array.map (fun pos -> (pos,housesByPosition.[pos].DigitsThatCanBePlayedInThisPosition))
            let potentials = 
                potentials'
                |> Array.filter (fun (p,s:Set<int<Dig>>) -> 
                    (not s.IsEmpty) && (board.At p).IsNone)
                |> Array.append requiredPotentials
                |> Map.ofArray
            potentials
        | false -> Map.empty

    let computeMovesByScore () =
        let ``map (position,digit) to score->Move[]`` = ()

        let setToMoveArray (position:Position, s:Set<int<Dig>>) : Move[] =
            s
            |> Set.toArray
            |> Array.map (fun (digit:int<Dig>) -> (position,digit))
                
        let mapScoreToMoveArray scores :Map<int<score>,Move[]> =
            let flatten = 
                scores 
                |> Array.map (fun (score, (p,s:Set<int<Dig>>)) -> 
                    (score, setToMoveArray (p,s))
                )
            let flattenn = 
                flatten 
                |> Array.map (fun (score:int<score>, moves) -> moves |> Array.map (fun move -> (score, move)))
                |> Array.concat
            let flattennn = 
                flattenn
                |> Seq.groupBy (fun (score, move) -> score)
                |> Seq.toArray
                |> Array.map (fun (score, scoreMoves) -> 
                    (score, 
                        scoreMoves 
                        |> Seq.map (fun (score, move:Move) -> move) 
                        |> Array.ofSeq))
            flattennn |> Map.ofArray

        let scores =
            potentialMoves
            |> Map.toArray 
            |> Array.map scorePotentials
            |> Set.ofArray
            |> Set.toArray
                
        mapScoreToMoveArray scores

    let movesByScore : Map<int<score>,Move[]> = 
        match isValid with
        | true ->
            computeMovesByScore()
        | false -> Map.empty

    let movesInOrder = 
        movesByScore
        |> Map.toArray 
        |> Array.map (fun (score, moves) -> 
            let m = moves
            moves |> Array.sortInPlaceBy (fun (pos,digit) -> 
                        ((-1) * (scoreDigit digit), pos))
            moves)
        |> Array.concat

    let boardScore = movesInOrder.Length * 1<score>

    let requiredMoveByPosition : Map<Position,int<Dig>> = 
        requiredMoves

    let to_pos (row:int) (column:int) = 
        new Position(row*1<SRow>,column*1<SCol>)
        
    let cantPlay (position:Position) =
        if (board.At position).IsSome then
            Index.AllDigitSet
        else
            let house = housesByPosition.[position]
            house.CantPlay

    let requiredDigitAt (position:Position) : Set<int<Dig>> = 
        if (requiredMoveByPosition.ContainsKey position) then
            if (board.At position).IsSome then
                failwith "more bad mojo"
            else
                Set.empty.Add requiredMoveByPosition.[position]
        else
            Set.empty

    let digitsPlayableAt (pos:Position) : Set<int<Dig>> = 
        match (board.At pos) with
        | None -> 
            // if mustPlay is empty then canPlay
            let required = requiredDigitAt pos
            if (required.IsEmpty) then
                let h = housesByPosition.[pos]
                h.DigitsThatCanBePlayedInThisPosition
            else
                required
        | Some x -> Set.empty

    let movesByPosition (position:Position) : Move [] =
        match isValid with
        | true ->
            let result = 
                (digitsPlayableAt position)
                |> Seq.map (fun d -> (position, d))
            result |> Seq.toArray
        | _ -> [||]

    let scoreByPos pos = 
        (digitsPlayableAt pos).Count * 1<score>

    let _requiredMoves =
        let required = requiredMoves |> Map.toArray
        if (movesByScore.ContainsKey(1<score>)) then
            movesByScore.[1<score>] |> Seq.toArray |> (Array.append required)
        else
            required

    member x.House (pos:Position) : House = 
        housesByPosition.[pos]

    member x.IsBlank (row:int) (column:int) =
        (board.At (to_pos row column)).IsNone

    member x.CanPlay (row:int) (column:int) (digit:int) = 
        let pos = to_pos row column
        let blank = x.IsBlank row column
        blank &&
            ((fun () ->
                (x.DigitsPlayableAt(pos)).Contains(digit*1<Dig>))())

    member x.MovesByPosition (position:Position) : Move [] = movesByPosition position
    member x.DigitsPlayableAt (pos:Position) : Set<int<Dig>> = digitsPlayableAt pos

    member x.Score = movesByScore

    member x.RequiredMoves() = _requiredMoves

    member x.Moves = 
        movesInOrder

    member x.IsValid () : bool = isValid

    member x.NumericScore : int<score> = boardScore
        
    member x.PrintAvailableDigits (digits:Set<int<Dig>>) =
        let sb = new System.Text.StringBuilder()
        let chars = 
            seq {
                for x in digits do
                    sb.Append(x) |> ignore
                    yield x
            } |> Seq.toList
        let s = sb.ToString()
        s.PadRight(9)

    override x.ToString() =
        Printer.PrintTemplate board
            (fun pos -> 
                let s = 
                    match board.At pos with
                    | Some x -> "->" + x.ToString() + "<-"
                    | None -> x.PrintAvailableDigits (x.House pos).DigitsThatMayAppearInThisPosition
                s.PadRight(9)                   
            )
