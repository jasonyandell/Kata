module StartOver

/// Array index
[<Measure>] type A

type Spot =
    {Index:int<A> ref}
with
    member inline x.Row = (!x.Index / 1<A>) / 9
    member inline x.Col = (!x.Index / 1<A>) % 9
    override x.ToString() = "("+x.Row.ToString()+", "+x.Col.ToString()+")"
and Room =
    {Spots : (Spot ref) []}
with
    override x.ToString() =
        let sb = new System.Text.StringBuilder("{ ")
        x.Spots |> Array.iter (fun s -> sb.Append(s.ToString() + " ") |> ignore)
        sb.Append("}").ToString()
and Casa =
    {Rooms : (Room ref) []}

let spots = [0..80] |> List.map (fun i -> {Index = ref (i*1<A>)})
