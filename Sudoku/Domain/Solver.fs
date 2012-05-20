namespace Domain

open System.Collections.Generic
open Microsoft.FSharp.Collections
open System.Threading.Tasks

type Solver (board:Board) =
    let _processor = new BoardProcessor(board)
    
    let _scoreMap = _processor.Score()

    let tryGetFromMap key : Set<Position> = 
        if (_scoreMap.ContainsKey key) then _scoreMap.Item key
        else Set.empty       

    let createMoves pos = 
        let digits = _processor.DigitsPlayableAt pos
        digits |> Seq.map (fun d -> (pos, d)) |> Set.ofSeq

    let _errorMoves = tryGetFromMap 0<score>

    let _requiredMoves = 
        let permutationRequired = _processor.RequiredMoves
        let digitRequired = 
            tryGetFromMap 1<score> 
            |> Set.map (fun pos -> createMoves pos)
            |> Set.unionMany
        Set.union permutationRequired digitRequired

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

    member x.CreateMoves (pos:Position) : Set<Move> = createMoves pos

    member x.MakeRequiredMoves () : Board = 
        if (not _requiredMoves.IsEmpty) then 
            board.Apply _requiredMoves
        else board

    member private x.PrintPosition (pos:Position) = 
        let playable = x.DigitsPlayableAt pos |> Set.ofSeq
        let debug = _processor.PrintAvailableDigits playable
        debug

    member x.PrintChoices () : string = 
        Printer.PrintTemplate board x.PrintPosition

    member x.Board = board
    
    member x.IsValid() =
        _processor.IsValid()

    member x.Solve () : Board seq =
        if (not _errorMoves.IsEmpty) then 
            Seq.empty
        elif (board.IsComplete) then
            if (x.IsValid()) then 
                [board] |> List.toSeq
            else    
                Seq.empty
        elif (not _requiredMoves.IsEmpty) then 
            let next = new Solver( x.MakeRequiredMoves() )
            if (next.IsValid()) then 
                next.Solve ()
            else
                Seq.empty 
        else 
            if (_optionalMovesByScore.IsEmpty) then Seq.empty
            else
                let waste = 
                    _optionalMovesByScore
                    |> Map.toSeq
                    |> Seq.take(1)
                    |> Seq.toArray

                let (firstKey,bestPositions) = waste.[0]

                let bestMoves = 
                    bestPositions
                    |> Set.toSeq
                    |> Seq.map _processor.MovesByPosition
                    |> Seq.concat                        
                    |> Array.ofSeq

                let otherMoves = 
                    (_optionalMovesByScore.Remove firstKey)
                    |> _processor.ToPrioritizedMoveList
                    |> Array.ofSeq

                let apply move = 
                    let newBoard = board.Apply [move]
                    let newSolver = new Solver(newBoard)
                    newSolver.Solve()

                seq {
                    let bestSolutions = 
                        bestMoves
                        |> Array.Parallel.map apply
                        |> Seq.concat
                    yield! bestSolutions

                    let otherSolutions = 
                        otherMoves
                        |> Array.Parallel.map apply
                        |> Seq.concat
                    yield! otherSolutions                
                }

