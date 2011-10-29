namespace ProblemTwentyOne.Tests


open NUnit.Framework
open FsUnit

[<TestFixture>] 
type ``Problem Twenty One: Amicable Pairs`` ()= 
    let act n = (ProblemTwentyOne.ProperDivisors(n)).List
    let d n = (ProblemTwentyOne.d.Of n)
    let amicable (a,b) = (ProblemTwentyOne.CheckAmicable.For (a,b))
    let allAmicableUnder n = (ProblemTwentyOne.CheckAmicable.ForNumbersLessThan n)
    let a = allAmicableUnder
    let y = 284 |> a |> Seq.toArray    

    [<Test>] member test.
     ``proper divisors of 2 is 1`` ()=
        act 2 |> should equal [1]

    [<Test>] member test.
     ``proper divisors of 4 is 1,2`` ()=
        act 4 |> should equal [1;2]

    [<Test>] member test.
     ``proper divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110`` ()=
        act 220 |> should equal [1; 2; 4; 5; 10; 11; 20; 22; 44; 55; 110]

    [<Test>] member test.
     `` d(220) = 284`` ()=
        d 220 |> should equal 284

    [<Test>] member test.
     `` d(284) = 220`` ()=
        d 284 |> should equal 220
        
    [<Test>] member test.
     `` (220,284) is amicable`` ()=
        (220, 284) |> amicable |> should equal true 

    [<Test>] member test.
     `` 220 is a member of allAmicableUnder 284`` ()=
        y |> should contain 284

    [<Test>] member test.
     ``Answer to Question 21 is correct`` ()=
        10000 
            |> allAmicableUnder 
            |> Seq.fold (+) 0
            |> should equal 31626

