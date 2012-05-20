namespace Domain

type BoardProcessor (board:Board) =

    let housesByPosition =         
        // for each position on the board, get the house
        let housesByPosition' = 
            Index.AllPositions 
            |> Seq.map (fun pos -> 
                let house = new House(pos, board)
                (pos,house))
            |> Map.ofSeq
        lazy housesByPosition'

    let isBlank pos =
        not (board.At pos).IsSome

    let movesExist (house:House) : bool =
        not house.AvailableDigits.IsEmpty

    let isValid pos (house:House) : bool = 
        not (isBlank pos) ||
        (movesExist house)

    let to_pos (row:int) (column:int) (digit:int) = {Row=row*1<sr>;Col=column*1<sc>}

    let unfilledPositions (positions:Set<Position>) = 
        positions |> Set.filter (fun pos -> (board.At pos).IsNone)

    member x.MovesByPosition (position:Position) : Move seq =
        (x.DigitsPlayableAt position)
        |> Set.toSeq
        |> Seq.map (fun d -> (position, d))

    member x.ToPrioritizedMoveList (map:Map<int<score>,Set<Position>>) : Move seq =
        let list' = 
            map
            |> Map.toList
            |> List.map (fun (score, positions) ->
                let a = 
                    positions
                    |> Set.fold (fun (result:Move list) position -> 
                        let movesHere : Move list = 
                            x.MovesByPosition position
                            |> List.ofSeq
                        List.append result movesHere)
                        []
                a)
        list'
        |> List.concat
        |> List.toSeq

    member x.HousesByPosition = housesByPosition.Force ()

    member x.House (pos:Position) : House = 
        x.HousesByPosition.[pos]

    /// Board is valid if something can be played at every position
    member x.Validate () : bool =
        x.HousesByPosition |> Map.forall isValid

    /// Higher score = worse
    member x.ScorePosition (position:Position) : int<score> =        
        let playable = x.DigitsPlayableAt position |> Set.toArray
        match playable with
        | x when not (isBlank position) -> System.Int32.MaxValue * 1<score>
        | [||] -> 0<score>
        | xs -> xs.Length * 1<score>

    member x.Score () : Map<int<score>, Set<Position>> =
        let unfilled : Set<Position> = Index.AllPositions |> unfilledPositions
        let scoreMap = 
            unfilled
            |> Set.toSeq
            |> Seq.groupBy (fun p -> x.ScorePosition p)
            |> Seq.map (fun (score, seqOfPositions) -> (score, seqOfPositions |> Set.ofSeq))
            |> Map.ofSeq
        scoreMap        

    member x.IsBlank (row:int) (column:int) (digit:int) =
        isBlank (to_pos row column digit)

    member x.CanPlay (row:int) (column:int) (digit:int) = 
        let pos = to_pos row column digit
        let blank = isBlank pos
        let house' = x.House pos
        let digitIsAvailable = 
            house'.AvailableDigits.Contains(digit*1<sd>)
        blank && digitIsAvailable

    member x.PositionsByDigit (area:Set<Position>) : Map<int<sd>,Set<Position>> =
        // get Map<Position,Set<int<sd>>
        // transform to Map<int<sd>,Set<Position>>
        // What's that operation called?!
        area 
        |> Seq.map (fun pos -> 
            (x.House pos).Choices
            |> Seq.map (fun digit -> (digit, pos)))
        |> Seq.concat
        |> Seq.groupBy (fun (d,p) -> d)
        |> Map.ofSeq
        |> Map.map (fun d dps -> 
            dps
            |> Seq.map (fun (d,p) -> p)
            |> Set.ofSeq)

    member x.RequiredMovesByArea (area:Set<Position>) : Set<Move> =
        let unfilledArea = area |> unfilledPositions
        let unneededDigits = (area - unfilledArea) |> Set.map (fun pos -> (board.At pos).Value)
        let digitMap = 
            x.PositionsByDigit unfilledArea
            |> Map.filter (fun d ps -> not (unneededDigits.Contains d))
        let digitsWithOneMove = 
            digitMap
            |> Map.filter (fun d (poss:Set<Position>) -> 1 = Set.count poss)
        let moveTuples = 
            digitsWithOneMove
            |> Map.map (fun (digit:int<sd>) (moves:Set<Position>) -> 
                moves 
                |> Set.map (fun (position:Position) -> (position,digit))
                )
            |> Map.toSeq
            |> Seq.map (fun (d,tuples) -> tuples)
            |> Seq.concat
            |> Set.ofSeq
            
        moveTuples
        // foreach, if only 1 choice,  

    member x.IsAreaValid (area:Set<Position>) : bool =
        let availableDigits = 
            area 
            |> Set.fold (fun (state:Set<int<sd>>) pos -> 
                match board.At pos with
                | None -> state
                | Some d -> state.Remove d) 
               Index.AllDigits
            |> Set.count

        let availablePositions = 
            area
            |> unfilledPositions
            |> Set.count

        availableDigits = availablePositions

    member x.IsValid () : bool =
        let a = Index.AllAreas
        let result = 
            Index.AllAreas
            |> Array.forall (fun area -> area |> x.IsAreaValid)
        result

    member x.RequiredMoves : Set<Move> =
        let r = x.RequiredMovesByArea
        // union the results of every house
        let result = 
            Index.AllAreas
            |> Array.map r
            |> Set.unionMany
        result

    member x.DigitsPlayableAt (pos:Position) : Set<int<sd>> = 
        match (board.At pos) with
        | None -> 
            let required = x.RequiredMoves |> Set.filter (fun (p,d) -> p=pos)
            if (Set.isEmpty required) then
                let h = x.House pos
                h.AvailableDigits
            else
                required |> Set.fold (fun state (p,d) -> state.Add d) Set.empty
        | Some x -> Set.empty

    member x.PrintAvailableDigits (digits:Set<int<sd>>) =
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
                    | None -> x.PrintAvailableDigits (x.HousesByPosition.[pos].AvailableDigits)
                s.PadRight(9)                   
            )
