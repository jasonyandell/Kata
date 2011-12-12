namespace ProblemTwentyFour.Tests

open ProblemTwentyFour.Helpers
open NUnit.Framework
open FsUnit

[<TestFixture>] 
type ``Problem Twenty Four: Permutations`` ()= 
    [<Test>] member test.
     `` permutations of [1;2] are [[1;2];[2;1]]`` ()=
        Permute [1;2] |> Seq.toList |> should equal [[1;2];[2;1]]

    [<Test>] member test.
     `` permutations of [1;2;3] are [[1;2;3];[1;3;2];[2;1;3];[2;3;1];[3;2;1];[3;1;2]]`` ()=
        Permute [1;2;3] |> Seq.toList |> should equal [[1;2;3];[1;3;2];[2;1;3];[2;3;1];[3;2;1];[3;1;2]]


