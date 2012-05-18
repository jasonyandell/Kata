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
        |> Set.ofSeq

    static let columns = 
        seq {
        for c in columnIndexes ->
            (c, Set.ofSeq (seq { for r in rowIndexes do yield {Position.Row=r; Col=c}}))
        } 
        |> Map.ofSeq

    // TODO: Lazy,ref, Lazy-of-ref
    static let rows = 
        seq {
            for r in rowIndexes ->
                (r, Set.ofSeq (seq { for c in columnIndexes do yield {Position.Row=r; Col=c}}))
        }
         
        |> Map.ofSeq
        
    static let boxes = 
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
                        } |> Set.ofSeq

                    let out = 
                        ( {Position.Row=r*3; Col=c*3}, thisBox )
                    out
        } |> Map.ofSeq

    static let boxIndexes = 
        boxes 
        |> Map.fold (fun state key value -> state |> Set.add key) Set.empty

    static let sets mapOfSets = 
        mapOfSets
        |> Map.fold (fun (state:Set<Position> list) key value -> state |> List.append [value]) List.empty
        |> List.toArray

    static let allAreas : Set<Position> array = 
        Array.concat
            [sets boxes;sets columns; sets rows]

    static member AllAreas = allAreas
    static member Boxes = boxes
    static member Columns = columns
    static member Rows = rows

    static member BoxIndexes = boxIndexes
    static member ColumnIndexes = columnIndexes
    static member RowIndexes = rowIndexes

    static member AllPositions = allPositions
 
    static member Row (row:int<sr>) = rows.Item row
    static member Column (col:int<sc>) = columns.Item col
    static member Box (pos:Position) = 
        let boxPos = {Position.Row=3*(pos.Row/3);Col=3*(pos.Col/3)}
        boxes.Item boxPos
