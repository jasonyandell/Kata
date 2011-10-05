// Learn more about F# at http://fsharp.net

module BowlingScore.FSharp

let Score (s : string) =     
    let rec MakeString (a : char list) =
        match a with
        | x::[] -> x.ToString()
        | x::xs -> x.ToString() + MakeString(xs)
        | [] -> ""        

    let rec Score (s : string) (index : int) = 
        match s.Chars(index) with
        | 'X' -> 10 
        | '-' -> 0
        | '/' -> 10 - (Score s (index-1))
        | x when x>='0' && x<='9' -> System.Int32.Parse(x.ToString())
        | _ -> failwith "bad arg"

    let rec ScoreList = function
        | 'X'::a::b::xs when s = MakeString([a;b]) -> 10 + (Score s 0) + (Score s 1) + (ScoreList (a::b::xs))
        | 'X'::a::[] when (s = MakeString([a])) -> 10 + (Score s 0)
        | 'X'::[] -> 10
        | a::'/'::b::xs when s = MakeString([b]) -> 10 + (Score s 0) + ScoreList (b::xs)
        | a::'/'::[] -> 10
        | a::xs when s = MakeString([a]) -> (Score s 0) + ScoreList xs
        | [] -> 0
        | _ -> failwith "bad list"

    s.ToCharArray() |> Array.toList |> ScoreList

()

