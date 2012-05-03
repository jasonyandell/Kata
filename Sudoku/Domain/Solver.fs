namespace Domain

type Solver () =
    let rec makeMoves board moves =
        Seq.fold 
            (fun (oldBoard:Board) ((r,c),(house:House)) -> 
                let digit = house.Constraints.MinimumElement
                oldBoard.PlayAt digit r c)
            board
            moves

    let getMoves ( (r,c), (house:House) ) =
        seq {
            match house with
            | x when house.IsEmpty -> yield! Seq.empty
            | x -> yield! seq { 
                for digit in house.Constraints -> 
                    ( (r,c), digit ) 
                }
        }

    let getAllMoves allMoves = 
        seq { for ((r:int,c:int),house:House) in allMoves do yield! getMoves ((r,c),house) }

    let getAllMovesInConstraintLengthOrder (moves:Map<int,'a>) = 
        seq { 
            for m in moves do 
                let length = m.Key
                let moveList = m.Value

                if length<2 then ()
                else yield! getAllMoves moveList 
        }

    let rec allSolutions (board:Board) = 
        let moves = board |> BoardProcessor.AsMoves

        // recurse, trying every remaining move, 
        seq {
            match moves with 
            // if there are no moves, we have a solution, yield it
            | x when Map.isEmpty moves ->
                yield board
            // if there are any places we cannot move, this is a bad board.  Bail.
            | x when Map.containsKey 0 moves -> 
                yield! Seq.empty
            // if there are any moves we are forced to make, make them all, solve the result           
            | x when Map.containsKey 1 moves ->
                let newBoard = makeMoves board moves.[1]
                yield! allSolutions newBoard
            // now we have all optional moves
            | x -> 
                let nextMoves:seq<(int*int)*int> = getAllMovesInConstraintLengthOrder moves
                yield! seq { for ((r,c),digit) in nextMoves do yield! allSolutions (board.PlayAt digit r c) }
        }

    member private x.AllSolutions (board:Board) = 
        allSolutions board

    static member Solve (board:Board) = 
        (new Solver ()).AllSolutions board
        