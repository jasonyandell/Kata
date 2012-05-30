namespace Domain

type House (pos:Position, board:Board) =
    let rowArea = Index.Row pos.Row
    let colArea = Index.Column pos.Col
    let boxArea = Index.Box pos
    let areas = [boxArea; rowArea; colArea;] |> Set.ofList

    let unionArea =
        let area' = Seq.concat [boxArea; rowArea; colArea;]
        area' |> Set.ofSeq

    static let alreadyPlayed (board:Board ref) (area:Set<Position> ref) : Set<int<Dig>> = 
        // foreach pos in area if unavailable@pos, append to unavailable
        (!area)
        |> Set.toArray
        |> Array.choose (fun pos -> (!board).At pos)
        |> Set.ofArray

    let unavailable = 
        match (board.At pos) with
        | Some digit -> Index.AllDigitSet
        | None -> 
            alreadyPlayed (ref board) (ref unionArea)

    let availableDigits = 
        lazy
            match (board.At pos) with
            | Some digit -> Set.empty
            | None ->
                Set.difference Board.AllDigits unavailable
 
    static member AlreadyPlayed board area = alreadyPlayed board area

    member x.DigitsThatCanBePlayedInThisPosition with get () = availableDigits.Value

    member x.DigitsThatMayAppearInThisPosition = 
        match board.At pos with
        | Some x -> [x] |> Set.ofSeq
        | None -> x.DigitsThatCanBePlayedInThisPosition

    member x.CantPlay with get() = unavailable

    member x.Row = rowArea
    member x.Column = colArea
    member x.Box = boxArea

    member x.Areas = areas    
    member x.Area = Set.unionMany areas

    override x.ToString() =
        Printer.PrintTemplate 
            board
            (fun pos -> 
                if unionArea.Contains pos then "#"
                else "*")

    member x.MovesExist = not (x.DigitsThatCanBePlayedInThisPosition.IsEmpty)
        