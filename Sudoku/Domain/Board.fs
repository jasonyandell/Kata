namespace Domain

open Microsoft.FSharp.Collections

type House (constraints:(Set<int>)) =
    static member Empty = new House(Set.empty)
    member this.IsEmpty = constraints.IsEmpty
    member this.Constraints = constraints
    member this.AddConstraint (digit:int) = 
        new House(constraints.Add digit)
    static member unionMany (many:seq<House>) =
        let union = Set.unionMany (seq { for h in many -> h.Constraints })
        new House(union)

type Houses = Map<int,House>

type RawBoard = int option [,]

type Board (rowHouses:Houses, columnHouses:Houses, boxHouses:Houses, rawBoard:RawBoard, digitsPlayed:int)=
    let boxIndex (row:int) (col:int) = (col/3)*10+(row/3)

    static member AllDigits = [1..9] |> Set.ofList

    static member Empty () = new Board(Map.empty,Map.empty,Map.empty,Array2D.create<int option> 9 9 None,0)

    member this.DigitsPlayed = digitsPlayed

    member this.Digit (row:int) (col:int) = 
        rawBoard.[col,row]

    member this.RowHouse (row:int) = 
        if rowHouses.ContainsKey(row) then rowHouses.[row] else House.Empty
    member this.RowHouses = rowHouses

    member this.ColumnHouse (column:int) = 
        if columnHouses.ContainsKey(column) then columnHouses.[column] else House.Empty
    member this.ColumnHouses = columnHouses

    member this.BoxHouse (row:int) (column:int) = 
        let index = boxIndex row column
        if boxHouses.ContainsKey(index) then boxHouses.[index] else House.Empty
    member this.BoxHouses = boxHouses

    member this.PlayAt (digit:int) (row:int) (col:int) = 
        let boxi = boxIndex row col

        let newRow = (this.RowHouse row).AddConstraint digit
        let newCol = (this.ColumnHouse col).AddConstraint digit
        let newBox = (this.BoxHouse row col).AddConstraint digit

        let newRows = 
            rowHouses 
            |> Map.filter (fun key value -> key <> row)
            |> Map.add row newRow 

        let newCols = 
            columnHouses 
            |> Map.filter (fun key value -> key <> col)
            |> Map.add col newCol
             
        let newBoxes = 
            boxHouses 
            |> Map.filter (fun key value -> key <> boxi)
            |> Map.add boxi newBox

        let newBoard = Array2D.copy rawBoard
        newBoard.[col,row] <- Some digit

        new Board(newRows, newCols, newBoxes, newBoard,digitsPlayed+1)

    member this.ValidDigits (row:int) (col:int) = 
        let constraints = 
            Set.unionMany 
                [(this.RowHouse row).Constraints; 
                 (this.ColumnHouse col).Constraints;
                 (this.BoxHouse row col).Constraints]    
        Set.difference 
            Board.AllDigits
            constraints

    member this.CanPlay (digit:int) (row:int) (col:int) =
        let h = this.ValidDigits row col
        let m = this.ValidDigits row col

        (this.Digit row col = None) &&
        (not ((this.RowHouse row).Constraints.Contains digit)) && 
        (not ((this.ColumnHouse col).Constraints.Contains digit)) &&
        (not ((this.BoxHouse row col).Constraints.Contains digit))
