namespace Domain


type Index () = 
    static let to_sc x = x*1<sc>
    static let to_sr x = x*1<sr>

    static let columnIndexes = [0..8] |> Seq.map to_sc
    static let rowIndexes = [0..8] |> Seq.map to_sr

    static let allPositions = 
        seq { 
        for r in rowIndexes do 
            for c in columnIndexes -> 
                { Position.Row=r; Col=c} 
        } 
        |> Array.ofSeq

    static let byColumn = 
        seq {
        for c in columnIndexes ->
            (c, seq { for r in rowIndexes do yield {Position.Row=r; Col=c}})
        } 
        |> Map.ofSeq

    // TODO: Lazy,ref, Lazy-of-ref
    static let byRow = 
        seq {
            for r in rowIndexes ->
                (r, seq { for c in columnIndexes do yield {Position.Row=r; Col=c}})
        } |> Map.ofSeq

    static let byBox = 
        seq {
            let rows = Seq.filter (fun r -> r < 3<sr>) rowIndexes
            let cols = Seq.filter (fun c -> c < 3<sc>) columnIndexes
            for r in rows do
                for c in cols ->
                    let thisBox =
                        seq { 
                        for i in 0..2 do
                            for j in 0..2 ->
                                let r' = r*3 + i*1<sr>
                                let c' = c*3 + j*1<sc>
                                {Position.Row=r'; Col=c'}
                        } 

                    let out = 
                        ( {Position.Row=r; Col=c}, thisBox )
                    out
        } |> Map.ofSeq

    static member RowIndexes = rowIndexes
    static member ColumnIndexes = columnIndexes
    static member AllPositions = allPositions
    static member ByRow (row:int<sr>) = byRow.Item row
    static member ByColumn (col:int<sc>) = byColumn.Item col
    static member ByBox (pos:Position) = 
        let boxPos = {Position.Row=pos.Row/3;Col=pos.Col/3}
        byBox.Item boxPos

[<Measure>] type score

type BoardProcessor (board:Board) =
    let makeArea (position:Position) = 
        let rowArea = Index.ByRow position.Row
        let colArea = Index.ByColumn position.Col
        let boxArea = Index.ByBox position
        let area = Seq.concat [rowArea; colArea; boxArea]
        area |> Set.ofSeq

    let housesByPosition =         
        // for each position on the board, get the house
        let housesByPosition' = 
            Index.AllPositions 
            |> Seq.map (fun pos -> 
                let house = new House(makeArea pos, board)
                (pos,house))
            |> Map.ofSeq
        lazy housesByPosition'

    let isBlank pos =
        not (board.At pos).IsSome

    let movesExist (house:House) : bool =
        not house.AvailableDigits.IsEmpty

    let isValid pos (house:House) : bool = 
        not (isBlank pos) ||
        (movesExist house)

    let to_pos (row:int) (column:int) (digit:int) = {Row=row*1<sr>;Col=column*1<sc>}

    member x.HousesByPosition = housesByPosition.Force ()

    member x.House (pos:Position) : House = new House(makeArea pos, board)//x.HousesByPosition.[pos]

    /// Board is valid if something can be played at every position
    member x.Validate () : bool =
        x.HousesByPosition |> Map.forall isValid

    /// Higher score = fewer choices
    member x.Score (position:Position) : int<score> =        
        let raw = 9 - (x.House position).AvailableDigits.Count
        // breakpoint = invalid board
        1<score> * raw

    member x.IsBlank (row:int) (column:int) (digit:int) =
        isBlank (to_pos row column digit)

    member x.CanPlay (row:int) (column:int) (digit:int) = 
        let pos = to_pos row column digit
        let blank = isBlank pos
        let house' = x.House pos
        let digitIsAvailable = 
            house'.AvailableDigits.Contains(digit*1<sd>)
        blank && digitIsAvailable

