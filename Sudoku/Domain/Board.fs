namespace Domain

open Microsoft.FSharp.Collections

type Board (moves:Map<Position,int<sd>>) =
    
    static let allDigits = [1..9] |> Seq.map (fun i -> i * 1<sd>) |> Set.ofSeq

    static let index p =
        (p.Row*9)/1<sr> + p.Col/1<sc>

    static let position i = 
        {Position.Row = 1<sr>*(i/9);
         Col=1<sc>*(i%9)}

    let isComplete = 81 = moves.Count

    static member AllDigits = allDigits

    static member Empty = new Board(Map.empty)

    member this.Moves = moves

    override x.ToString() =
        let memory = Array.init 81 (fun i -> ".")
        let start = memory
        let interim = 
            Map.fold 
                (fun (output:string[]) pos digit -> 
                    let digitAsString = digit.ToString()
                    output.[index pos] <- digitAsString
                    output) 
                start
                moves
        System.String.Join("",memory)

    static member ofString (str:string)  = 
        let (moveCount, moveSet) =
            str.ToCharArray()
            |> Array.fold 
                (fun (i,moves) ch -> 
                    (i+1, 
                        match ch with
                        | '.' -> moves
                        | x when x>='1' && x<='9' -> 
                            let digit = System.Convert.ToInt32(ch) * 1<sd>
                            let pos = position i
                            Set.add (pos,digit) moves
                        | _ -> 
                            let message = "bad input at index"+(string i)+"\n"+str+"\n"+(String.replicate (i-1) " ")+"^\n"
                            failwith message
                    )
                )
                (0,Set.empty)
        new Board(moveSet |> Set.toSeq |> Map.ofSeq)

    member this.At p = 
        if (moves.ContainsKey p) then Some (moves.Item p)
        else None

    member this.PlayAt (position:Position) (digit:int<sd>) : Board =
        new Board(moves.Add (position,digit))

    member this.Play (digit:int) (row:int) (col:int) : Board =
        let pos:Position = {Position.Row=row*1<sr>;Col=col*1<sc>}
        this.PlayAt pos (digit*1<sd>)

    member this.IsComplete = isComplete

    member this.Apply (otherMoves:Move seq) : Board =
        let newMoves = 
            Seq.fold (fun newMoves' (p,d) -> newMoves' |> Map.add p d) moves otherMoves
        if (Seq.isEmpty newMoves) then this
        else new Board(newMoves)
