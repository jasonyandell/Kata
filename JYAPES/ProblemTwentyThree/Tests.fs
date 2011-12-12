namespace ProblemTwentyThree.Tests


open ProblemTwentyThree
open EulerCore
open NUnit.Framework
open FsUnit

[<TestFixture>] 
type ``Problem Twenty Three: Abundant Numbers`` ()= 

    [<Test>] member test.
     ``Verify 12 is the smallest abundant number`` ()=
        [1..12] |>
            List.filter NumberTheory.IsAbundant
            |> should equal [12]

    [<Test>] member test.
     ``Verify 24 is a sum of abundant numbers`` ()=
        Helpers.abundantNumbersLessThan 24 |> should contain 24 

    [<Test>] member test.
     ``Can compute list of all sums of two abundant numbers`` ()=        
        Helpers.abundantNumbersLessThan 28123 |> should contain 30

    [<Test>] member test.
     ``Can find the answer`` ()=        
        Helpers.Answer |> should equal 4179871


