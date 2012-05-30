namespace Domain

open System.Web.Caching
open System.Collections.Concurrent

//type SearchData =
//    { Context:Context; Mover:Mover }
//
//type SearchNode(data:SearchData) =
//    let node = {
//        Key = data.Context.Board.Key;
//        Score = data.Context.Processor.TotalScore;
//        Data = data
//    }
//
//    let edges : Node<SearchNode> seq = Seq.empty
//
//    member x.Node = node
//
//    member x.Edges = edges
//
//type SearchState =
//| Searching of SearchNode
//| DeadEnd
////
//
//
//type Search () =
//
//    let prioritize (n:SearchNode) : int<score> = 0<score>
//
//    let lookupState board : SearchState = DeadEnd
//
//    // true if complete
//    let iterate (board:Board) : bool = 
//        let state = lookupState board
//        match state with 
//        | DeadEnd -> false
//        | Searching node ->
//            //let  = node.Data.Moves.PopMove
//            //let nextBoard:Board = (!node.Data.Context).Board.Apply m
//            false
//            
//
//    member x.FindSolution =
//        ()
//        
//

// start with a board

















