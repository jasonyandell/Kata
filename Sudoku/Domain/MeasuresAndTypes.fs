namespace Domain

/// Sudoku Row
[<Measure>] type SRow

/// Sudoku Column
[<Measure>] type SCol

/// Sudoku Digit
[<Measure>] type Dig

[<Measure>] type score

type Position(row:int<SRow>,col:int<SCol>) = 

    let index = 9 * (row / 1<SRow>) + (col / 1<SCol>)

    member x.Row = row
    member x.Col = col

    override x.Equals(o:obj) =
        if (o :? Position) then
            let p:Position = downcast o
            p.Row=row && p.Col=col
        else
            false

    override x.GetHashCode() =
        index

    interface System.IComparable with
        member x.CompareTo(o:obj) =
            if (o :? Position) then
                (index - o.GetHashCode())
            else
                -1

    static member FromIndex (i:int) : Position =
        new Position( 1<SRow> * (i / 9), 1<SCol> * (i % 9) )

    member x.ToIndex : int = index

    override x.ToString() =
        "("+x.Row.ToString()+", "+x.Col.ToString()+")"

type Move = Position*int<Dig>

type Score = int<score>*Move

//type Node<'T> = 
//    { 
//        Key: string; 
//        Score : int<score>; 
//        Data : 'T
//    }
