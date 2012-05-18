namespace Domain

type House (pos:Position, board:Board) =
    let rowArea = Index.Row pos.Row
    let colArea = Index.Column pos.Col
    let boxArea = Index.Box pos

    let makeArea (position:Position) = 
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

    /// Not sure why I need this any more
    member private x.AvailableMoves =
        seq { 
            for pos in area do
                match (board.At pos) with
                | Some digit -> ()
                | _ -> yield pos
        } |> Set.ofSeq
        
    member x.AvailableDigits = availableDigits.Value

    member internal x.Choices = 
        match board.At pos with
        | Some x -> [x] |> Set.ofSeq
        | None -> 
            x.AvailableDigits

    member x.Row = rowArea
    member x.Column = colArea
    member x.Box = boxArea

    override x.ToString() =
        Printer.PrintTemplate 
            board
            (fun pos -> 
                if area.Contains pos then "#"
                else "*")


