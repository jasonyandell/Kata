// Learn more about F# at http://fsharp.net

open Domain

let steps = 
    let board = 
        Domain.Board.ofString (
            "4 . . . . . 8 . 5 "+
            ". 3 . . . . . . . "+
            ". . . 7 . . . . . "+
            ". 2 . . . . . 6 . "+
            ". . . . 8 . 4 . . "+
            ". 4 . . 1 . . . . "+
            ". . . 6 . 3 . 7 . "+
            "5 . 3 2 . . . . . "+
            "1 . 4 . . . . . . "
            )
    let solver = Solver.Create board

    Solver.Solve solver |> Seq.toArray