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

