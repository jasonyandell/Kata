namespace Domain

type Printer () =

    static member PrintTemplate (board:Board) (lookup:Position->string) =
        let output = [|
            for row in 0..8 do
                if row>0 && row%3=0 then
                    for i in 0..21 do
                        yield '-'
                    yield '\n'
                for col in 0..8 do
                    if col>0 && (col%3=0) then 
                        yield '|'
                        yield ' ' 
                    let s:string = lookup {Position.Row = row*1<sr>; Col=col*1<sc>}
                    yield! s
                    yield ' '
                yield '\n'
        |]
        let sb = new System.Text.StringBuilder()
        sb.Append(output) |> ignore
        sb.ToString()        


    static member Print (board:Board) =
        Printer.PrintTemplate 
            board 
            (fun pos ->
                match board.At pos with
                | None -> "."
                | Some x -> x.ToString())




