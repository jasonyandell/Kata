namespace Domain

open System.Collections.Generic
open Microsoft.FSharp.Collections
open System.Threading.Tasks

type Solver (board:Board) =
    let _processor = new BoardProcessor(board)
    let _requiredMoves = _processor.RequiredMoves
    let _scoreMap = _processor.Score()

    let tryGetFromMap key = 
        if (_scoreMap.ContainsKey key) then _scoreMap.Item key
        else Set.empty       

    let _errorMoves = tryGetFromMap 0<score>

    let _requiredMoves = tryGetFromMap 1<score>    

    let _optionalMovesByScore = 
        Map.fold (fun newMap key set -> 
            match key with
            | x when x=0<score> || x=1<score> -> newMap |> Map.remove x
            | _ -> newMap) _scoreMap _scoreMap

    member x.House (pos:Position) : House =  _processor.House pos

    member x.DigitsPlayableAt (pos:Position) : IEnumerable<int<sd>> =        
        //if there are any required moves here, return those
        // else return choices
        _processor.DigitsPlayableAt pos
        |> Set.toSeq

    member x.CreateMoves (pos:Position) : Set<Move> = 
        let digits = _processor.DigitsPlayableAt pos
        digits |> Seq.map (fun d -> (pos, d)) |> Set.ofSeq

    member x.MakeRequiredMoves () : Solver = 
        if (_scoreMap.ContainsKey 0<score>) then 
            failwith "zero score.  error."
        else if (_scoreMap.ContainsKey 1<score>) then 
            let moves = 
                // for every required position, make seq of Moves for the position
                (_scoreMap.Item 1<score>)
                // then concat them all together
                |> Set.map (fun pos -> x.CreateMoves pos)
                |> Set.unionMany
            new Solver( board.Apply moves )
        else
            x

    member private x.PrintPosition (pos:Position) = 
        let playable = x.DigitsPlayableAt pos |> Set.ofSeq
        let debug = _processor.PrintAvailableDigits playable
        debug


    member x.PrintChoices () : string = 
        Printer.PrintTemplate board x.PrintPosition
            
    member x.Board = board
        