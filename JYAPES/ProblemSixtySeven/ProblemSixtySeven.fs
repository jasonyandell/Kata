namespace ProblemSixtySeven

open System.IO
open System

// issue is parsing the big text file into an array2D
type Parser = 

    static member parseInput:int [,] = 
        use reader = new StreamReader("input.txt")
        let separator = [|" "|]
        let stringLineSeq = seq {
            while not reader.EndOfStream do
                let line = reader.ReadLine()
                let tokens = line.Split(separator, StringSplitOptions.None)
                let numbers = tokens |> Array.map Convert.ToInt32
                yield numbers
        } 

        let stringLines = stringLineSeq |> Seq.toArray

        let normalize (ary:int[]) (width:int) = 
            let rhs = Array.init (width - ary.Length) (fun n -> 0)
            Array.append ary rhs
        
        let square = stringLines |> Array.map (fun line -> normalize line stringLines.Length)

        array2D square