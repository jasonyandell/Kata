namespace EulerCore

open NumericLiterals
open NUnit.Framework
open FsUnit
open System.Numerics

[<TestFixture>] 
type ``NumberTheory: Divisors`` ()= 
    [<Test>] member test.
     ``Divisors of 1I is [1I]`` ()=
        (NumberTheory.Divisors 1I) |> should equal [1I]

    [<Test>] member test.
     ``Divisors of 1 is [1]`` ()=
        (NumberTheory.Divisors 1) |> should equal [1]

    [<Test>] member test.
     ``Proper Divisors for 2 is [1]`` ()=
        NumberTheory.ProperDivisors 2 |> should equal [1]

    [<Test>] member test.
     ``Proper Divisors for 2I is [1I]`` ()=
        NumberTheory.ProperDivisors 2I |> should equal [1I]

    [<Test>] member test.
     ``Proper Divisors for 4 is [1;2]`` ()=
        NumberTheory.ProperDivisors 4 |> should equal [1;2]

    [<Test>] member test.
     ``Proper Divisors for 9 is [1;3]`` ()=
        NumberTheory.ProperDivisors 9
            |> Seq.toList 
            |> should equal [1;3]

    [<Test>] member test.
     ``Proper Divisors for 36 is [1;2;3;4;6;9;12;18]`` ()=
        NumberTheory.ProperDivisors 36 
            |> Seq.toList 
            |> should equal [1;2;3;4;6;9;12;18]

    [<Test>] member test.
     ``Proper Divisors of 220 are 1, 2, 4, 5, 10, 11, 20, 22, 44, 55 and 110`` ()=
        NumberTheory.ProperDivisors 220 |> should equal [1; 2; 4; 5; 10; 11; 20; 22; 44; 55; 110]

    [<Test>] member test.
     `` d(220) = 284`` ()=
        NumberTheory.d 220I |> should equal 284I
        
    [<Test>] member test.
     `` (220,284) is amicable`` ()=
        (220I, 284I) |> NumberTheory.Amicable |> should equal true 

    [<Test>] member test.
     `` 28 is perfect`` ()=
        NumberTheory.IsPerfect 28I |> should equal true

    [<Test>] member test.
     ``d(12) = 16`` ()=
        NumberTheory.d 12 |> should equal 16

    [<Test>] member test.
     ``d(12I) = 16I`` ()=
        NumberTheory.d 12I |> should equal 16I

    [<Test>] member test.
     `` 12 is abundant`` ()=
        NumberTheory.IsAbundant 12I |> should equal true 

    [<Test>] member test.
     `` 11 is deficient`` ()=
        NumberTheory.IsDeficient 11I |> should equal true 

