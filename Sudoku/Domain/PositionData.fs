namespace Domain
//
//// Graph
//// node, children
//
//
//
//type Context =
//    { Board : Board; Processor:BoardProcessor }
//
//    // two constraints
//    // can't play the same digit twice, ie
//    //    let ``can't play`` = union of (digits already played)
//    // must play each of 1..9, ie 
//    //    intersection of all unavailable in each area
//
//type PositionData (context: Context ref, position:Position) = 
//    let c = !context // does this copy?
//
//    let isBlank = c.Board.IsBlank position
//
//    let playableDigits : int<Dig> [] = 
//        match isBlank with
//        | true -> 
//            (c.Processor.CantPlay position) |> Array.ofSeq
//        | false -> Array.empty
//
//    let score = 
//        match isBlank with
//        | true -> System.Int32.MaxValue * 1<score>
//        | false -> playableDigits.Length * 1<score>
//
//    let moves = 
//        playableDigits |> Array.map (fun digit -> (position,digit))
//
//    member x.IsBlank with get() = isBlank
//    member x.PlayableDigits with get() = playableDigits |> List.ofArray
//    member x.Score with get() = score
//    member x.Moves with get() = moves |> List.ofArray
