namespace Domain

/// Sudoku Row
[<Measure>] type sr 

/// Sudoku Column
[<Measure>] type sc 

/// Sudoku Digit
[<Measure>] type sd

type Position = 
    { Row:int<sr>; Col:int<sc> }
with
    override x.ToString() =
        "("+x.Row.ToString()+", "+x.Col.ToString()+")"

type Move = Position*int<sd>

type Datapoint =
    | Options of Set<int<sd>>
    | Digit of int<sd>
with
    override x.ToString() = 
        let printSet (xs:Set<'a>) : string =
            let interim = Set.fold (fun (soFar:string) (element:'a) -> ","+element.ToString()) "" xs
            "{" + interim.Remove(0) + "}"
        match x with
        | Options o -> printSet o
        | Digit d -> d.ToString() 

