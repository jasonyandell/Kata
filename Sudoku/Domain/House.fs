namespace Domain

type House (pos:Position, board:Board) =
    let makeArea (position:Position) = 
        let rowArea = Index.ByRow position.Row
        let colArea = Index.ByColumn position.Col
        let boxArea = Index.ByBox position
        let area = Seq.concat [rowArea; colArea; boxArea]
        area |> Set.ofSeq

    let area = makeArea pos

    let unavailable = 
        lazy
            // foreach pos in area if unavailable@pos, append to unavailable
            seq { 
                for pos in area do
                    match (board.At pos) with
                    | Some digit -> yield digit
                    | _ -> ()
            } |> Set.ofSeq

    let availableDigits = 
        lazy
            match (board.At pos) with
            | Some digit -> Set.empty
            | None ->
                let unavailable' = unavailable.Force()
                Set.difference 
                    Board.AllDigits 
                    unavailable'

    member x.AvailableMoves =
        seq { 
            for pos in area do
                match (board.At pos) with
                | Some digit -> ()
                | _ -> yield pos
        } |> Set.ofSeq
        
                
//    let availableMovesByDigit = 
//        lazy
//            let unavailable = unavailable.Value
//            let remaining = Set.difference Board.AllDigits unavailable
//            // now foreach d in remaining, find subset of area where that d can be unavailable
//            seq {
//                for d in remaining do
//                    for pos in area do
//                        let v = board.At pos
//                        match v with
//                        | Some d -> ()
//                        | Options o -> if (o.Contains d) then yield (d,pos)
//            } |> Map.ofSeq
//

        
    member x.AvailableDigits = availableDigits.Value
  //  member x.AvailableMovesByDigit = availableMovesByDigit.Value

    override x.ToString() =
        Printer.PrintTemplate 
            board
            (fun pos -> 
                if area.Contains pos then "#"
                else "*")


