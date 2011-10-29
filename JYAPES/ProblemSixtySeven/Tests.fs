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
