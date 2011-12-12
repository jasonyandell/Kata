namespace ProblemTwentyFour

open System.IO
open System
open System.Numerics;
open System.Collections.Generic;

module Helpers =     

    let itemInsertedAtEachPosition (item:'a) items =
        // take each i in items
        // return i::
        let len = List.length items
        seq {            
            for i in 0..len do
                let left = items |> Seq.take i |> Seq.toList

                let rhs = 
                    items 
                    |> Seq.skip i
                    |> Seq.toList

                let split () =
                    match rhs with
                    | [] -> ([],[])
                    | y:ys -> ([y],ys)


                yield List.concat [x;left;[item];right]
                
            // wanna yiela ([1-3],[1|2;1|3])
                
        } |> Seq.toList

    let rec Permute (items:'a list) : list<'a list> =
        match items with
        | x::xs -> 
            seq {
                    let allPermuted = seq {
                        for x' in xs do
                            let r = List.concat [[x]; rhs]
                            printf "%O" r
                            yield r 
                        } 
                    let res = allPermuted |> Seq.toList
                    printf "\n"

                    yield! res
            } |> Seq.toList

()

