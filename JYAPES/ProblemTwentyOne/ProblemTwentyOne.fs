namespace ProblemTwentyOne

open System.IO
open System
open System.Collections.Generic;

type ProperDivisors(n:int) = 
    let rec divisors i = 
        seq {
            if (i > (n / i)) then 
                yield! Seq.empty
            else
                if ((i <> (n/i)) && (n % i) = 0) then yield i
                yield! divisors (i + 1)
                if ((i > 1) && (n % i = 0)) then yield n / i
        }
    let divisors = (divisors 1)

    member self.List = 
        divisors |> Seq.toList

type d = 
    static member Of n = 
        (ProperDivisors(n)).List |> List.fold (+) 0

type CheckAmicable =
    static member For (a,b) =
        d.Of(a) = b &&
        d.Of(b) = a

    static member ForNumbersLessThan x = 
        seq {
            let memoized = [|0..x|] |> (Array.map (fun n -> d.Of n))

            for i in 1..x do
                for j in (i+1)..x do
                    if ((memoized.[i] = j) && (memoized.[j] = i)) then
                        yield i
                        yield j
        }

