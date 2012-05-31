namespace Domain

//visitor of sourcecode
//- mark place to add a text
//- inspect data
//- execute

open System.Collections.Generic
open Microsoft.FSharp.Collections
open System.Threading.Tasks
open System.Collections.Concurrent
open System.Diagnostics
open System.Linq

[<Measure>] type MoveIndex
[<Measure>] type ScoreSoFar

type Solver private (board:Board) =

    let _processor = new BoardProcessor(board)

    //let priority = _processor.NumericScore
    
    static let foundBoards = new ConcurrentBag<string>()
    static let queue = new SortedDictionary<int<score>*string,Solver*int<MoveIndex>>()

    static let getScore(s:Solver,index:int<MoveIndex>,str:string) =
        let moves:Move[] = s.Processor.Moves
        let (posAtIndex:Position, digitAtIndex:int<Dig>) = moves.[index/1<MoveIndex>]

        let movesAtIndex = (s.House posAtIndex).DigitsThatCanBePlayedInThisPosition.Count

       // let score = movesAtIndex * (s.Processor.Moves.Count)

//        let moveCountAsFloat : float = float movesAtIndex
//        let percentWrong = 1.0 - (1.0 / moveCountAsFloat)
//        let nscore = System.Convert.ToInt32( percentWrong * (float s.Priority) ) * 1<score>
//
//        queue.Any(fun kvp -> 
//            let (score, idx, boardKey) = kvp.Key
//            boardKey = str) 
//        |> ignore

        // NOTE: Real cost so far is sum(MoveIndex) for each move made so far, starting from original board
        let ``cost so far`` = index / 1<MoveIndex>
        let ``cost of this move`` = movesAtIndex
        let ``estimated cost to finish`` = 81 - s.Board.Moves.Count
        
        let score = 
            ``cost so far`` + ``cost of this move`` + ``estimated cost to finish``

        score * 1<score>

    static let enqueue (str:string) (s:Solver,index:int<MoveIndex>) = 
        if (not (foundBoards.Contains(str))) then
            let len = foundBoards.Count
            ()
        else
            ()

        let nscore = getScore (s, index, str)

        queue.Add(( nscore, str), (s,index))
//        queue.AddOrUpdate(pri, s, (fun (key:int<score>) (value:Solver*int<MoveIndex>) -> value))
  //      |> ignore

    static let dequeue () = 
        let (currScore:int<score>, boardKey:string) as firstKey = queue.Keys.First()
        let (solver:Solver,moveIndex) = queue.Values.First()
        queue.Remove(firstKey) |> ignore
        (currScore, solver, moveIndex)

    static let pppush (boardKey:string) (solver:Solver, idx:int<MoveIndex>) =
//        if (pri = 0<score>) then ()
//        else
//            if (queue.ContainsKey (pri,boardKey)) then
////                let (solver',moveIndex) = queue.Item pri
////                if (solver'.Board = solver.Board) then
//                    let alreadyThere = queue.[(pri, boardKey)]
//                    let same = ((solver,idx)=alreadyThere)
//                    ()
////                else
////                    push (pri+1<score>) s
//            else
                enqueue (boardKey) (solver, idx)

    static let ppush (solver:Solver,index) =
        pppush (solver.Board.Key) (solver, index)

    static let pushUpdate (solver:Solver,index) =
        ppush (solver,index)

    static let addNew (solver:Solver) : Solver option =
        if (foundBoards.Contains(solver.Board.Key)) then
            None
        else
            let finalBoard = solver.Board
            if (not (foundBoards.Contains(finalBoard.Key))) then
                foundBoards.Add(finalBoard.Key)
            if (solver.IsValid) then
                ppush (solver, 0<MoveIndex>)
                Some solver
            else
                None

    static let mutable initialized = false

    static member Initialize(solver:Solver) =
        if (not initialized) && (queue.Count=0) then
            initialized <- true
            foundBoards.Add(solver.Board.Key)
            if (solver.IsValid && (not solver.Board.IsComplete)) then
                ppush (solver, 0<MoveIndex>)
    
    override x.ToString() : string =
        x.Board.Key


    //member x.Priority : int<score> = priority

    member x.House (pos:Position) : House =  _processor.House pos

    static member Create(board:Board) : Solver = 
        (new Solver(board)).MakeRequiredMoves()

    member x.DigitsPlayableAt (pos:Position) : IEnumerable<int<Dig>> =        
        //if there are any required moves here, return those
        // else return choices
        _processor.DigitsPlayableAt pos
        |> Set.toSeq

//    member x.CreateMoves (pos:Position) : Set<Move> = createMoves pos

    member private x.PrintPosition (pos:Position) = 
        let playable = x.DigitsPlayableAt pos |> Set.ofSeq
        let debug = _processor.PrintAvailableDigits playable
        debug

    member x.PrintChoices () : string = 
        Printer.PrintTemplate board x.PrintPosition

    member x.Board : Board = board
    member x.Processor : BoardProcessor = _processor
    
    member x.IsValid = _processor.IsValid()

    /// Private now, factory method first ensures all required moves are made
    member x.MakeRequiredMoves() : Solver =
        let b = ref board 
        let currSolver = ref x

        while (((!currSolver).Processor.RequiredMoves().Length) > 0) do
            b := (!b).Apply ((!currSolver).Processor.RequiredMoves())
            currSolver := new Solver(!b)
        (!currSolver)

    member x.IsComplete : bool =
        x.IsValid && (x.Board.IsComplete)

    static member Solve(solver:Solver) : Solver seq =        

        let applyMove (curr:Solver) (highestIndex:int<MoveIndex> ref) (moves:Move[]) (index:int<MoveIndex>) : Solver seq =
            let typelessIndex = index/1<MoveIndex>
            if (typelessIndex < moves.Length) then
                highestIndex := index
                let move = moves.[typelessIndex]
                let veryTemporary = Solver.Create(curr.Board.Apply [move])
                if (veryTemporary.Processor.IsValid()) && (veryTemporary.Board.IsComplete) then
                    seq { yield veryTemporary }
                else
                    match (addNew veryTemporary) with
                    | None -> Seq.empty  // this board has been added before
                    | Some nextSolver ->
                        let newBoard = nextSolver.Board
                        if (nextSolver.Processor.NumericScore > curr.Processor.NumericScore) then
                            Debug.WriteLine("Dispute: made a move ("+move.ToString()+") and came up with more moves?? ({0}) > old priority ({1})",nextSolver.Processor.NumericScore, curr.Processor.NumericScore)
                            Debug.Indent()
                            Debug.WriteLine("Old board:\n" + Printer.Print(curr.Board))
                            Debug.WriteLine("New board:\n" + Printer.Print(newBoard))
                            Seq.empty
                        else
                            Seq.empty
            else
                Seq.empty

        let doMoves (curr:Solver) (highestIndex:int<MoveIndex> ref) (moves:Move[]) : Solver seq =
            let startIndex = !highestIndex
            seq {
                for i in 0..0 do
                   yield! applyMove curr highestIndex moves (startIndex+(i*1<MoveIndex>))
            }

        if (solver.IsComplete) then
            [solver] |> List.toSeq
        else
            Solver.Initialize(solver)
            seq {
                while (queue.Count>0) do
                    let (pri, currentSolver, moveIndex) as popped = dequeue()
                
    //                Debug.WriteLine("("+pri.ToString() + ", " + moveIndex.ToString() + ") " + currentSolver.Board.ToString())
                    Debug.WriteLine(popped)
//                    Debug.WriteLine(Printer.Print(currentSolver.Board))

                    if (not (currentSolver.IsValid)) then 
                        ()
                    elif (currentSolver.Board.IsComplete) then 
                        yield currentSolver
                    else
                        let moves = currentSolver.Processor.Moves
                        let highestIndex = ref moveIndex

                        yield! doMoves currentSolver highestIndex moves

                        if (!highestIndex < (moves.Length-1)*1<MoveIndex>) then
                            pushUpdate (currentSolver,!highestIndex + 1<MoveIndex>)
            }
