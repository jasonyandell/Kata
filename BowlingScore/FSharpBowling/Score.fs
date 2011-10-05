// Learn more about F# at http://fsharp.net

module BowlingScore.FSharp

let Score (s : string) =     
    let rec MakeString (a : char list) =
        System.String.Concat(a |> Array.ofList)

    let rec Score (l : char list) (index : int) = 
        let s = MakeString l
        match s.Chars(index) with
        | 'X' -> 10 
        | '-' -> 0
        | '/' -> 
            10 - (Score l (index-1))
        | x when x>='0' && x<='9' -> System.Int32.Parse(x.ToString())
        | _ -> failwith ("bad arg: " + s)

    let rec ScoreList (l : char list) = 
        ()
        match l with
        | 'X'::a::b::[] -> 
            10 + (Score [a] 0) + (Score [b] 0)
        | 'X'::a::[] -> 
            10 + (Score [a] 0)
        | 'X'::a::b::xs -> 
            10 + (Score [a;b] 0) + (Score [a;b] 1) + (ScoreList (a::b::xs))
        | 'X'::[] -> 
            10
        | a::'/'::b::[] -> 
            10 + (Score [b] 0)
        | a::'/'::b::xs -> 
            10 + (Score [b] 0) + ScoreList (b::xs)
        | a::'/'::[] -> 
            10
        | a::b::xs -> 
            (Score [a] 0) + (Score [b] 0) + ScoreList xs
        | [a] -> 
            Score [a] 0
        | [] -> 0

    s.ToCharArray() |> Array.toList |> ScoreList

()

