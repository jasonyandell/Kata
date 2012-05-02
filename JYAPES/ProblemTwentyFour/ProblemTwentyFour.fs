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

                let right = 
                    items 
                    |> Seq.skip i
                    |> Seq.toList

                yield left @ [item] @ right
                
            // wanna yiela ([1-3],[1|2;1|3])
                
        } |> Seq.toList

    let Permute (items:'a list) : list<'a list> =
        match items with
        | [] -> []
        | x::xs -> 
            itemInsertedAtEachPosition x xs

()

