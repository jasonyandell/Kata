namespace Domain

open System.Collections.Generic
open Microsoft.FSharp.Collections
open System.Threading.Tasks

type Solver (board:Board) =
    let _processor = new BoardProcessor(board)

    member x.House (pos:Position) : House = 
        _processor.House pos

    member x.DigitsPlayableAt (pos:Position) : IEnumerable<int<sd>> =
        (x.House pos).AvailableDigits |> Set.toSeq

