namespace ProblemTwentyTwo.Tests

open ProblemTwentyTwo
open NUnit.Framework
open FsUnit

[<TestFixture>] 
type ``Problem Twenty Two: Scoring Names`` ()= 
    let s = ProblemTwentyTwo.Score
    let ns = ProblemTwentyTwo.Names

    [<Test>] member test.
     ``Score of C is 3`` ()=
        "C" |> s |> should equal 3

    [<Test>] member test.
     ``Score of COLIN is 53`` ()=
        "COLIN" |> s |> should equal 53

    [<Test>] member test.
     ``938th name is COLIN`` ()=
        ns.[938] |> should equal "COLIN"

    [<Test>] member test.
     ``answer is 871198282`` ()=
        ProblemTwentyTwo.Answer |> should equal 871198282