namespace ProblemTwentyOne

open EulerCore
open System.IO
open System
open System.Numerics;
open System.Collections.Generic;


type CheckAmicable =
    static member For (a,b) =
        (NumberTheory.d a = b) &&
        (NumberTheory.d b = a)

    static member ForNumbersLessThan x = 
        seq {
            let memoized = [|0..x|] |> (Array.map (fun n -> NumberTheory.d n) )

            for i in 1..x do
                for j in (i+1)..x do
                    if ((memoized.[i] = j) && (memoized.[j] = i)) then
                        yield i
                        yield j
        }

