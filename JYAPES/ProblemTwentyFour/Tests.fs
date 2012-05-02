namespace ProblemKatas.ProblemTwentyFour

open NUnit.Framework
open FsUnit

[<TestFixture>] 
type ``Problem Twenty Four`` ()=
    member this.remove_nth (list:'a list) (n:int) = 
        let filter (i, so_far) (item:'a) =
            match i with
            | x when x=n -> (i+1, so_far)
            | _ -> (i+1, so_far @ [item])
        let (_, result) = 
            List.fold filter (0, []) list
        result

    member this.perm list_to_perm = 
        let rec p (list:'a list) (state:'a list) = seq<'a list> {
            match list with
            | [] -> yield state
            | _ ->
                let len = list |> List.length
                for n in 0..(len-1) do
                    let x = List.nth list n
                    let xs = this.remove_nth list n
                    let result = p xs (state @ [x])
                    yield! result
            }
        let permuted = p list_to_perm []
        permuted

    member test.toNum (list:int list) = 
        list |> (List.fold (fun (state:int64) (item:int) -> 10L*state + (int64 item)) 0L)

    member test.answer = 
        let res = [0..9] |> test.perm |> Seq.nth 999999
        let result = res |> test.toNum
        printf "%A" result
        result

    [<Test>] member test.
     ``[1]->[[1]]`` ()=
        [1] |> test.perm |> should contain [1]

    [<Test>] member test.
     ``[1;2] -> [[1;2];[2;1]]`` ()=
        [1;2] |> test.perm |> Seq.toList |> should equal [[1;2]; [2;1]]

    [<Test>] member test.
     ``[A;B;C] -> [[A;B;C]...]`` ()=
        [1;2;3] |> test.perm |> Seq.head |> should equal [1;2;3]

    [<Test>] member test.
     ``[A;B;C] -> [...;[C;B;A]]`` ()=
        [1;2;3] |> test.perm |> Seq.toList |> List.rev |> List.head |> should equal [3;2;1]

    [<Test>] member test.
     ``[A;B;C] -> [...;[B;C;A];...]`` ()=
        [1;2;3] |> test.perm |> should contain [2;3;1]

    [<Test>] member test.
     ``[A;B;C;D] should have 4! results`` ()=
        [1;2;3;4] |> test.perm |> Seq.length |> should equal (4*3*2)

    [<Test>] member test.
     ``[A;B;C;D;E;F;G;H] should have 8! results`` ()=
        [1;2;3;4;5;6;7;8] |> test.perm |> Seq.length |> should equal (8*7*6*5*4*3*2)

    [<Test>] member test.
     ``fourth lexicographic permutation of [0..2] is [1;2;0]`` ()=
        [0..2] |> test.perm |> Seq.nth 3 |> test.toNum |> should equal 120

    [<Test>] member test.
     ``millionth lexicographic permutation of [0..9] is 2783915460`` ()=
        test.answer |> should equal 2783915460L

