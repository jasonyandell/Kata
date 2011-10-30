namespace ProblemTwentyOne.Tests

open EulerCore
open NUnit.Framework
open FsUnit

[<TestFixture>] 
type ``Problem Twenty One: Amicable Pairs`` ()= 
    [<Test>] member test.
     `` 220 is a member of allAmicableUnder 284`` ()=
        ProblemTwentyOne.CheckAmicable.ForNumbersLessThan 284 |> should contain 284

    [<Test>] member test.
     ``Answer to Question 21 is correct`` ()=
        10000 
            |> ProblemTwentyOne.CheckAmicable.ForNumbersLessThan
            |> Seq.fold (+) 0
            |> should equal 31626

