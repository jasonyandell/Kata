namespace Domain

open Microsoft.FSharp.Collections

type Board (moves:Set<(Position * int<Dig>)>) =    
    let cache =
        lazy
            let cache' = Array.create 81 None
            let init = 
                moves
                |> Seq.fold (fun state (key:Position, value:int<Dig>) ->
                    cache'.[key.ToIndex] <- Some value
                    state)
                    []
            cache'
    
    static let allDigits = [1..9] |> Seq.map (fun i -> i * 1<Dig>) |> Set.ofSeq

    let isComplete = 81 = moves.Count

    let key =
        let memory = Array.init 81 (fun i -> ".")
        let start = memory
        let interim = 
            Seq.fold 
                (fun (output:string[]) (pos:Position, digit) -> 
                    let digitAsString = digit.ToString()
                    output.[pos.ToIndex] <- digitAsString
                    output) 
                start
                moves
        System.String.Join("",memory)

    static member AllDigits = allDigits

    static member Empty = new Board(Set.empty)

    member this.Moves = moves

    member this.Key = key

    override x.ToString() = key

    static member ofString (str:string)  = 
        let (moveCount, moveSet) =
            let s = str.Replace(" ", "")
            s.ToCharArray()
            |> Array.fold 
                (fun (i,moves) ch -> 
                    (i+1, 
                        match ch with
                        | '.' -> moves
                        | x when x>='1' && x<='9' -> 
                            let digit = (System.Convert.ToInt32(ch) - System.Convert.ToInt32('0')) * 1<Dig>
                            let pos = Position.FromIndex i
                            Set.add (pos,digit) moves
                        | _ -> 
                            let message = "bad input at index"+(string i)+"\n"+str+"\n"+(String.replicate (i-1) " ")+"^\n"
                            failwith message
                    )
                )
                (0,Set.empty)
        new Board(moveSet)

    member this.At (p:Position) : int<Dig> option = 
        (cache.Value).[p.ToIndex]

    member this.IsBlank (p:Position) : bool =
        match (this.At p) with
        | Some x -> false
        | _ -> true

    member this.PlayAt (position:Position) (digit:int<Dig>) : Board =
        let move = (position,digit)

        new Board(Set.add move moves)

    member this.Play (digit:int) (row:int) (col:int) : Board =
        let pos:Position = new Position(row*1<SRow>, col*1<SCol>)
        this.PlayAt pos (digit*1<Dig>)

    member this.IsComplete = isComplete

    member this.Apply (otherMoves:Move seq) : Board =
        if (Seq.isEmpty otherMoves) then this
        else
            let otherMoveSet = otherMoves |> Set.ofSeq

            let sanityCheck = Set.intersect otherMoveSet moves

            if (not (sanityCheck.IsEmpty)) then
                failwith "bad mojo"
            else
                new Board(Set.union (otherMoves |> Set.ofSeq) moves)
