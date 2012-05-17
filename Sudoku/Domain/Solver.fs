namespace Domain

open Microsoft.FSharp.Collections
open System.Threading.Tasks
(*
type Solver () =
    let mutable count = 0

    let rec applyMoves board moves =
        Seq.fold 
            (fun (failedMoves:int,oldBoard:Board) ((r,c),digit) -> 
                if (oldBoard.CanPlay digit r c) then
                    (failedMoves,oldBoard.PlayAt digit r c)
                else
                    (failedMoves+1,oldBoard)
            )
            (0,board)
            moves

    let rand = new System.Random()

    let swap (a: _[]) x y =
        let tmp = a.[x]
        a.[x] <- a.[y]
        a.[y] <- tmp

    // shuffle an array (in-place)
    let shuffle a =
        Array.iteri (fun i _ -> swap a i (rand.Next(i, Array.length a))) a

    let rec allSolutions (board:Board) = 
        count <- count+1
        // recurse, trying every remaining move, 
        seq {
            if board.DigitsPlayed=81 then 
                yield board

            let moves = board |> BoardProcessor.AsMoves
            match moves with 
            // if there are any places we cannot move, this is a bad board.  Bail.
            | x when Map.containsKey 0 moves -> 
                yield! Seq.empty
            // if there are any moves we are forced to make, make them all.  
            //   If the board is valid, success.  
            //   If we were forced to make a bad move, bail
            | x when Map.containsKey 1 moves -> 
                let result = (applyMoves board moves.[1])
                let (failedMoves,newBoard) = result
                let nextSeq = 
                    if (failedMoves>0) then 
                        Seq.empty
                    else 
                        allSolutions newBoard
                yield! nextSeq
            // now we have all optional moves
            | x -> 
                for entry in moves do
                    let choiceCount = entry.Key
                    let movesWithNChoices = entry.Value
                    movesWithNChoices |> shuffle
                    let boards = 
                        seq { for ((r,c),digit) in movesWithNChoices -> board.PlayAt digit r c }
                    for b in boards do 
                        let next = (allSolutions b)
                        yield! next
        }

    member private x.AllSolutions (board:Board) = 
        allSolutions board

    static member Solve (board:Board) = 
        (new Solver ()).AllSolutions board
        *)