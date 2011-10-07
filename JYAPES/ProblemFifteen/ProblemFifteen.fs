// Learn more about F# at http://fsharp.net

module ProblemFifteen

let mutable Solutions = new System.Collections.Generic.Dictionary<int, list<BigNum>>()

let rec PascalsTriangle:(int -> list<BigNum>) = function
  | 1 -> [1N] 
  | 2 -> [1N;1N]
  | n when Solutions.ContainsKey(n) -> 
    Solutions.[n]
  | n when n>2 -> 
      Solutions.Add(n,       
        seq {       
          yield 1N;
          for i in 2..n-1 ->
            let tri = PascalsTriangle (n-1)
            (List.nth tri (i-1)) + (List.nth tri (i-2))
          yield 1N
        } |> Seq.toList)
      Solutions.[n]
  | x -> failwith (x.ToString())

let PathCount n =
  let depth = ((n*2)+1)
  PascalsTriangle depth |> Seq.nth ((depth-1)/2)

