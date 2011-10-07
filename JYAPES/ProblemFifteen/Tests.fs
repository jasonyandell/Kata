namespace Tests

open NUnit.Framework
open FsUnit

[<TestFixture>]
type ``Pascal's Triangle`` ()=
  let f = ProblemFifteen.PascalsTriangle

  [<Test>]member test.
   ``1 -> [1]`` ()=
     f 1 |> should equal [1N]

  [<Test>]member test.
   ``2 -> [1;1]`` ()=
     f 2 |> should equal [1N;1N]

  [<Test>]member test.
   ``3 -> [1;2;1]`` ()=
     f 3 |> should equal [1N;2N;1N]

  [<Test>]member test.
   ``5 -> [1;4;6;4;1]`` ()=
     f 5 |> should equal [1N;4N;6N;4N;1N]


[<TestFixture>]
type ``Path Counts`` ()=
  [<Test>]member test.
   ``0 -> 1`` ()=
     ProblemFifteen.PathCount 0 |> should equal 1N

  [<Test>]member test.
   ``1 -> 2`` ()=
     ProblemFifteen.PathCount 1 |> should equal 2N

  [<Test>]member test.
   ``2 -> 6`` ()=
     ProblemFifteen.PathCount 2 |> should equal 6N

  [<Test>]member test.
   ``20  -> 137846528820`` ()=
     ProblemFifteen.PathCount 20 |> should equal 137846528820N

  