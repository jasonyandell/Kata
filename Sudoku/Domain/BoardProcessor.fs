namespace Domain

type SudokuDatapoint = { Digit:string; Moves:House }

type BoardProcessor  =
    static member private fromBoard (board:Board) r c = 
        let digit = 
            match board.Digit r c with
            | Some x -> x.ToString()
            | None -> "."
        let constraints = House.unionMany [| board.RowHouse r; board.ColumnHouse c; board.BoxHouse r c |]
        let moves = new House(Set.difference Board.AllDigits constraints.Constraints)
        { Digit = digit; Moves = moves }

    static member AsModel (board:Board) = 
        Array2D.init 9 9 (BoardProcessor.fromBoard board)

    static member AsText (board:Board) = 
        let model = board |> BoardProcessor.AsModel
        Array2D.mapi (fun x y item -> item.Digit) model
    
    static member AsMoves (board:Board) =
        let model = board |> BoardProcessor.AsModel
        let moves = seq {
            for r in 0..8 do
                for c in 0..8 do
                    let retVal = ( (r,c), model.[r,c].Moves )
                    let somethingAlreadyPlayedThere = 
                        match board.Digit r c with
                        | Some x -> false
                        | None -> true
                    if somethingAlreadyPlayedThere then
                        yield retVal
        }
        
        let a = moves |> Seq.toArray
        let b = a |> Seq.groupBy (fun ( (r,c), moves ) -> moves.Constraints.Count)
        let c =  b |> Map.ofSeq

        c

    static member Print (board:Board) =
        let text = BoardProcessor.AsText(board)
        let output = [|
            for row in 0..8 do
                if row>0 && row%3=0 then
                    for i in 0..21 do
                        yield '-'
                    yield '\n'
                for col in 0..8 do
                    if col>0 && (col%3=0) then 
                        yield '|'
                        yield ' ' 
                    let row = text.[row,col]
                    yield row.[0]
                    yield ' '
                yield '\n'
        |]
        let sb = new System.Text.StringBuilder()
        sb.Append(output) |> ignore
        sb.ToString()        
