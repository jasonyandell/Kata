namespace Domain

//open System.Collections.Generic
//open System.Linq
//
// type Mover (source:seq<Move>, context:Context ref) =
//    
//    let data : Dictionary< Position, PositionData > = new Dictionary<Position, PositionData>()
//    do
//        Index.AllPositions
//        |> Set.toArray
//        |> Array.map (fun position -> (position, new PositionData(context,position)))
//        |> Array.filter (fun (p,d) -> not d.IsBlank)
//        |> Array.iter (fun (p,d) -> data.Add(p,d) )
//
//    let moves : Move list = 
//        data
//        |> Seq.map (fun (kvp:System.Collections.Generic.KeyValuePair<Position,PositionData>) ->
//            kvp.Value.Moves)
//        |> List.concat
//
//    let movesByScore : Dictionary< int<score>, List<Move> > = new Dictionary<int<score>, List<Move>>()
//    do
//        data
//        |> Seq.iter (fun kvp -> 
//            if not (movesByScore.ContainsKey kvp.Value.Score) then movesByScore.Add(kvp.Value.Score, new List<Move>())
//            else ()
//            
//            (movesByScore.Item kvp.Value.Score).AddRange(kvp.Value.Moves)
//        )
//
////    member x.ToPrioritizedMoveList (map:Map<int<score>,Set<Position>>) : Move seq =
////        let list' = 
////            map
////            |> Map.toList
////            |> List.map (fun (score, positions) ->
////                let a = 
////                    positions
////                    |> Set.fold (fun (result:Move list) position -> 
////                        let movesHere : Move list = 
////                            x.MovesByPosition position
////                            |> List.ofSeq
////                        List.append result movesHere)
////                        []
////                a)
////        list'
////        |> List.concat
////        |> List.toSeq
//
//
//
////type Mover (board:Board) =
//    // given a board
//    // give me a Moves
//
//
//    // given a list of moves
//    // give me 
//    // - (a board with the required moves made
//    //    ,the list of moves with the required ones removed)
//    