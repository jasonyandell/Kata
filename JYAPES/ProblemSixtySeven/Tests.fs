namespace ProblemSixtySeven.Tests

open ProblemEighteen
open ProblemSixtySeven

open NUnit.Framework
open FsUnit

[<TestFixture>] 
type ``Given a parser`` ()= 
    let input = ProblemSixtySeven.Parser.parseInput

    [<Test>] member test.
     ``can find the greatest score`` ()=
        (ProblemEighteen.Triangle(input)).Solve |> should equal 7273

    [<Test>] member test.
     ``can count natural numbers`` ()=
        [0;1;2] |> should equal (ProblemSixtySeven.Parser.fakeLoop |> Seq.take 3)