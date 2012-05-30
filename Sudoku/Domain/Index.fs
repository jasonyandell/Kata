namespace Domain

type Index () = 
    static let to_sc x = x*1<SCol>
    static let to_sr x = x*1<SRow>
    static let to_sd x = x*1<Dig>

    static let columnIndexes = [|0..8|] |> Array.map to_sc
    static let rowIndexes = [|0..8|] |> Array.map to_sr
    static let allDigits = [|1..9|] |> Array.map to_sd //|> Set.ofSeq
    static let allDigitSet = Index.AllDigits |> Set.ofArray

    static let allPositions = 
        seq { 
        for r in rowIndexes do 
            for c in columnIndexes -> 
                new Position(r,c)
        } 
        |> Set.ofSeq

    static let columns = 
        seq {
        for c in columnIndexes ->
            (c, Set.ofSeq (
                seq { 
                    for r in rowIndexes do 
                        yield new Position(r,c) 
                } 
            ))
        } 
        |> Map.ofSeq

    static let rows = 
        seq {
            for r in rowIndexes ->
                (r, Set.ofSeq (
                    seq { 
                        for c in columnIndexes do 
                            yield new Position(r,c)
                    }
                ))
        }
         
        |> Map.ofSeq
        
    static let boxes = 
        seq {
            let rows = Seq.filter (fun r -> r < 3<SRow>) rowIndexes
            let cols = Seq.filter (fun c -> c < 3<SCol>) columnIndexes
            for r in rows do
                for c in cols ->
                    let thisBox =
                        seq { 
                        for i in 0..2 do
                            for j in 0..2 ->
                                let r' = r*3 + i*1<SRow>
                                let c' = c*3 + j*1<SCol>
                                new Position(r',c')
                        } |> Set.ofSeq

                    let out = 
                        ( new Position(r*3,c*3), thisBox )
                    out
        } |> Map.ofSeq

    static let boxIndexes = 
        boxes 
        |> Map.fold (fun state key value -> state |> Set.add key) Set.empty

    static let sets mapOfSets = 
        mapOfSets
        |> Map.fold 
            (fun (state:Set<Position> list) key value -> 
                [value] |> List.append state) List.empty
        |> List.toArray

    static let allAreas : Set<Position> array = 
        Array.concat
            [sets boxes; sets columns; sets rows; ]

    static member AllDigits = allDigits
    static member AllDigitSet = allDigitSet
    static member AllAreas = allAreas
    static member Boxes = boxes
    static member Columns = columns
    static member Rows = rows

    static member BoxIndexes = boxIndexes
    static member ColumnIndexes = columnIndexes
    static member RowIndexes = rowIndexes

    static member AllPositions = allPositions
 
    static member Row (row:int<SRow>) = rows.Item row
    static member Column (col:int<SCol>) = columns.Item col
    static member Box (pos:Position) = 
        let boxPos = new Position(3*(pos.Row/3),3*(pos.Col/3))
        if (boxes.ContainsKey boxPos) then
            boxes.Item boxPos
        else
            failwith "bad lookup"