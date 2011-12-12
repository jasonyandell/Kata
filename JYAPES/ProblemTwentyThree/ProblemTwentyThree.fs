namespace ProblemTwentyThree

open EulerCore
open System.IO
open System
open System.Collections.Generic;

type hash = System.Collections.Generic.HashSet<int>

module Helpers = 

    let abundantNumbers = 
        let rec count i = 
            seq {
                if (NumberTheory.IsAbundant i) then yield i
                yield! count (i+1)
            }
        count 1
    
    let abundantNumbersLessThan max = 
        let abundants = new hash()
        let sums = new hash()
        let inRange n = n <= max

        let updateSums n = 
            abundants.Add(n) |> ignore
            abundants 
            |> Seq.iter (fun x -> 
                sums.Add(x + n) |> ignore)

        abundantNumbers 
            |> Seq.takeWhile inRange
            |> Seq.iter updateSums

        sums

    let hashAsList (h:hash) : int array =          
        let result = 
            h 
            |> Seq.toArray
        result

    let invertNumericSetUpToX max (set:hash) =
        let h = new hash()
        [1..max] |> List.iter (fun n -> h.Add(n) |> ignore) |> ignore
        seq {
            for item in set ->
                h.Remove(item)
        } |> Seq.toList |> ignore
        h
    
    let Answer =
        let set = invertNumericSetUpToX 28123 (abundantNumbersLessThan 28123)
        Seq.fold (+) 0 set
()

