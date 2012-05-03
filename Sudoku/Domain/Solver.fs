namespace Domain

open Microsoft.FSharp.Collections
open System.Threading.Tasks

type Solver () =
    let rec applyMoves board moves =
        Seq.fold  (fun (oldBoard:Board) ((r,c),digit) -> oldBoard.PlayAt digit r c)
            board
            moves

    let rec allSolutions (board:Board) = 
        // recurse, trying every remaining move, 
        seq {
            if board.DigitsPlayed=81 then yield board

            let moves = board |> BoardProcessor.AsMoves
            match moves with 
            // if there are any places we cannot move, this is a bad board.  Bail.
            | x when Map.containsKey 0 moves -> 
                yield! Seq.empty
            // if there are any moves we are forced to make, make them all, solve the result           
            | x when Map.containsKey 1 moves ->
                let newBoard = applyMoves board moves.[1]
                yield! allSolutions newBoard
            // now we have all optional moves
            | x -> 
                for entry in moves do
                    let choiceCount = entry.Key
                    let movesWithNChoices = entry.Value
                    let boards = 
                        seq { for ((r,c),digit) in movesWithNChoices -> board.PlayAt digit r c }
                    for b in boards do yield! allSolutions b
        }

    member private x.AllSolutions (board:Board) = 
        allSolutions board

    static member Solve (board:Board) = 
        (new Solver ()).AllSolutions board
        