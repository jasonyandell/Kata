namespace ProblemTwentyTwo

open System.IO
open System.Text
open System.Collections.Generic

type ProblemTwentyTwo =

    static member Lookup = 
        dict (List.zip ['A'..'Z'] [1..26])

    static member ReadNames:string[] = 
        use reader = new StreamReader("names.txt")
        let contents = reader.ReadToEnd()
        let quoteless = 
            (new StringBuilder(contents))
                .Replace("\"", "")
                .ToString()
        let names = quoteless.Split([|','|])
        names

    static member Names = 
        let initial = [|""|] 
        let sorted = (ProblemTwentyTwo.ReadNames |> Array.sort)
        Array.append initial sorted

    static member Score (name:string) = 
        name.ToCharArray() |> Array.fold (fun sum (ch:char) -> sum + ProblemTwentyTwo.Lookup.[ch]) 0

    static member Answer = 
        (ProblemTwentyTwo.Names 
            |> Array.fold 
                (fun (idx,sum) name -> 
                    (idx+1, sum + idx*(ProblemTwentyTwo.Score name))) 
                (0, 0))
            |> snd
        