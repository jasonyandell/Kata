namespace Domain

open Microsoft.FSharp.Collections

type Board (moves:Map<Position,int<sd>>) =
    
    static let allDigits = [1..9] |> Seq.map (fun i -> i * 1<sd>) |> Set.ofSeq

    static let index p =
        (p.Row*9)/1<sr> + p.Col/1<sc>

    static let position i = 
        {Position.Row = 1<sr>*(i/9);
         Col=1<sc>*(i%9)}

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

    member this.Apply (moves:Move seq) : Board =
        moves
        |> Seq.fold 
            (fun (newBoard:Board) (p,d) -> newBoard.PlayAt p d)
            this

//    member this.tryMove (((r,c),digit):Move) : Board = 
//        let affected = affectedPositions (r,c) |> Set.ofSeq
//        let newBoard =
//            data 
//            |> Array.mapi
//                (fun i (dp:Datapoint) -> 
//                    let p = position i
//                    if not (affected.Contains p) then dp
//                    else
//                        match dp with
//                        | Options o -> Options(o.Remove digit)
//                        | Digit d -> Digit d)
//        new Board(newBoard)
//            
//        let newRow = (this.RowHouse row).AddConstraint digit
//        // check that a column or box can have the digit
//
//        let newCol = (this.ColumnHouse col).AddConstraint digit
//
//
//        let newBox = (this.BoxHouse row col).AddConstraint digit
//
//        let newRows = 
//            rowHouses
//            |> Map.filter (fun key value -> key <> row)
//            |> Map.add row newRow 
//
//        let newCols = 
//            columnHouses 
//            |> Map.filter (fun key value -> key <> col)
//            |> Map.add col newCol
//             
//        let newBoxes = 
//            boxHouses 
//            |> Map.filter (fun key value -> key <> boxi)
//            |> Map.add boxi newBox
//
//        let newBoard = Array2D.copy rawBoard
//        newBoard.[col,row] <- Some digit
//
//        new Board(newRows, newCols, newBoxes, newBoard,digitsPlayed+1,Set.add ((row,col),digit) movesSoFar)

//    member this.ValidDigits (row:int) (col:int) = 
//        if (this.Digit row col).IsSome then 
//            Set.empty
//        else
//            let constraints = 
//                Set.unionMany 
//                    [(this.RowHouse row).Constraints; 
//                     (this.ColumnHouse col).Constraints;
//                     (this.BoxHouse row col).Constraints]
//            Set.difference 
//                Board.AllDigits
//                constraints
//
//    member this.CanPlay (digit:int) (row:int) (col:int) =
//        let res = 
//            (this.Digit row col).IsNone &&
//            (not ((this.RowHouse row).Constraints.Contains digit)) && 
//            (not ((this.ColumnHouse col).Constraints.Contains digit)) &&
//            (not ((this.BoxHouse row col).Constraints.Contains digit))
//        res