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
[<Measure>] type CostSoFar

type BoardOrdering = int<score>*int<CostSoFar>*string

type SolutionIteration = Solver*int<MoveIndex>

and SolverContext (solver:Solver) =

    let foundBoards = new ConcurrentBag<string>()
    
//    let b = new BlockingCollection( 

    let queue = new SortedDictionary<BoardOrdering,SolutionIteration>()

    static let costPerMove = 50

    let enqueue (costSoFar:int<CostSoFar>,str:string) (s:Solver,index:int<MoveIndex>) = 
        queue.Add(( s.Score(costSoFar, index), costSoFar, str), (s,index))

    let dequeue () = 
        if queue.Count > 0 then 
            let (currScore:int<score>, costSoFar:int<CostSoFar>, boardKey:string) as firstKey = queue.Keys.First()
            let (solver:Solver,moveIndex) = queue.Values.First()
            queue.Remove(firstKey) |> ignore
            Some (costSoFar, solver, moveIndex)
        else None

    let pppush (costSoFar:int<CostSoFar>, boardKey:string) (solver:Solver, idx:int<MoveIndex>) =
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
                enqueue (costSoFar, boardKey) (solver, idx)

    let ppush (costSoFar,solver:Solver,index) =
        pppush (costSoFar, solver.Board.Key) (solver, index)

    let pushUpdate (costSoFar:int<CostSoFar>,solver:Solver,index) =
        ppush (costSoFar,solver,index)

    let addNew (costSoFar:int<CostSoFar>, costOfThisMove:int<MoveIndex>, solver:Solver) : Solver option =
        if (foundBoards.Contains(solver.Board.Key)) then
            None
        else
            let finalBoard = solver.Board
            if (not (foundBoards.Contains(finalBoard.Key))) then
                foundBoards.Add(finalBoard.Key)
            if (solver.IsValid) then
                ppush (costSoFar + costPerMove*(costOfThisMove/1<MoveIndex> + 1)*1<CostSoFar>, solver, 0<MoveIndex>)
                Some solver
            else
                None

    do
        foundBoards.Add(solver.Board.Key)
        ppush (0<CostSoFar>, solver, 0<MoveIndex>)
    
    let syncRoot = new obj()
    let protect (f, arg) = lock syncRoot (fun () -> f arg)

    member x.AddNew  (costSoFar:int<CostSoFar>, costOfThisMove:int<MoveIndex>, solver:Solver) = 
        protect (addNew, (costSoFar, costOfThisMove, solver))
//    member x.Queue = protect queue
    member x.PushUpdate (costSoFar:int<CostSoFar>,solver:Solver,index) = 
        protect (pushUpdate, (costSoFar, solver, index))

    member x.Dequeue () = protect (dequeue,())

// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
// ///////////////////////////////////////////////////////////////////////////////
/// Solves Sudoku boards
and Solver private (board:Board) =
    static let costPerScore = 20

    let _processor = new BoardProcessor(board)

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
        let c = new SolverContext(solver)

        let applyMove (costSoFar:int<CostSoFar>) (curr:Solver) (highestIndex:int<MoveIndex> ref) (moves:Move[]) (index:int<MoveIndex>) : Solver option =
            let typelessIndex = index/1<MoveIndex>
            if (typelessIndex < moves.Length) then
                highestIndex := index
                let move = moves.[typelessIndex]
                let veryTemporary = Solver.Create(curr.Board.Apply [move])

                Debug.WriteLine("Moving at {0} \n{1}\n",(curr.Processor.Moves.[!highestIndex/1<MoveIndex>]),Printer.Print(curr.Board))

                if (veryTemporary.Processor.IsValid()) && (veryTemporary.Board.IsComplete) then
                    Some veryTemporary
                else
                    match (c.AddNew (costSoFar,!highestIndex,veryTemporary)) with
                    | None -> None  // this board has been added before
                    | Some nextSolver ->
                        let newBoard = nextSolver.Board
                        if (nextSolver.Processor.NumericScore > curr.Processor.NumericScore) then
                            Debug.WriteLine("Dispute: made a move ("+move.ToString()+") and came up with more moves?? ({0}) > old priority ({1})",nextSolver.Processor.NumericScore, curr.Processor.NumericScore)
                            Debug.Indent()
                            Debug.WriteLine("Old board:\n" + Printer.Print(curr.Board))
                            Debug.WriteLine("New board:\n" + Printer.Print(newBoard))
                            None
                        else
                            None
            else
                None

        let doMoves (costSoFar:int<CostSoFar>) (curr:Solver) (highestIndex:int<MoveIndex> ref) (moves:Move[]) : Solver option =
            let startIndex = !highestIndex
            applyMove costSoFar curr highestIndex moves (startIndex)
        
        if (solver.IsComplete) then
            [solver] |> List.toSeq
        else
            let iterate () : Solver option = 
                match c.Dequeue() with
                | Some (costSoFar, currentSolver, moveIndex) as popped ->
    //                Debug.WriteLine("("+pri.ToString() + ", " + moveIndex.ToString() + ") " + currentSolver.Board.ToString())
//                    Debug.WriteLine("{0} {1}",(currentSolver.Processor.Moves.[moveIndex/1<MoveIndex>]),popped)
//                    Debug.WriteLine(Printer.Print(currentSolver.Board))

                    if (currentSolver.IsValid) 
                       && (currentSolver.Board.IsComplete) then 
                        Some currentSolver
                    else
                        let moves = currentSolver.Processor.Moves
                        let highestIndex = ref moveIndex

                        let res = doMoves costSoFar currentSolver highestIndex moves
                        
                        if (!highestIndex < (moves.Length-1)*1<MoveIndex>) then
                            c.PushUpdate (costSoFar,currentSolver,!highestIndex + 1<MoveIndex>)

                        res
                | None -> None

//            let bigSeq = 
            let coll = (fun i -> 
                match iterate() with 
                | Some s -> [|s|]
                | None -> [||] )
            let s = 
                Seq.initInfinite coll
                |> Seq.concat
            s


//            let s = Array.Parallel.init 1000 coll 
//            let t = s |> Array.concat |> Seq.ofArray
//            t 

//            seq {
//                let loop () = 
//                    [| iterate() |] |> Array.choose ( fun s -> s )
////                    Array.Parallel.choose iterate (Array.init 10 (fun i -> ()))
//                let s : Solver [] ref = ref (loop())
//                while !s |> Array.isEmpty do
////                    s := loop()
//                    s := loop()
//            }
//
    member x.Score(costSoFar:int<CostSoFar>,index:int<MoveIndex>) =
        let ``cost of this move`` = index/1<MoveIndex> * costPerScore
        let ``estimated cost to finish`` = (81 - board.Moves.Count) * 20

        // NOTE: Real cost so far is sum(MoveIndex) for each move made so far, starting from original board
        let ``cost so far`` = costSoFar / 1<CostSoFar>
        
        let nscore = 
            ``cost so far`` + ``cost of this move`` + ``estimated cost to finish``

        let retVal = nscore * 1<score>

        retVal
