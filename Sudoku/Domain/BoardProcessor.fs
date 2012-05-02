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
            for r in [0..8] do 
                for c in [0..8] do 
                    match board.Digit r c with
                    | Some x -> yield ( (r,c), model.[r,c].Moves )
                    | None -> ()
        }
        
        let a = 
            moves 
            |> Seq.toArray
        let b = 
            a |> Seq.groupBy (fun ( (r,c), moves ) -> moves.Constraints.Count)
        let c = 
            b |> Map.ofSeq

        c