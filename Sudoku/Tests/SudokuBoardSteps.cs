using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using Domain;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Tests
{
    [Binding]
    public class SudokuBoardSteps
    {
        private Board _board;
        private Solver _solver;

        [Given("an empty board")]
        public void GivenAnEmptyBoard()
        {
            _board = Board.Empty;
        }

        [When("a (.*) placed at (.*), (.*)")]
        public void WhenDigitPlacedAtRowCol(int digit, int row, int col)
        {
            //TODO: implement act (action) logic
            _board = _board.Play(digit, row, col);
        }

        [Then(@"cannot play (.*) at (.*),(.*)")]
        public void CannotPlayAtRowCol(int digit, int row, int col)
        {
            var p = new BoardProcessor(_board);

            Assert.IsFalse(p.CanPlay(row, col, digit),
                "Could play {2} at {0},{1}\n{3}",row,col,digit, 
                Printer.Print(_board));
        }

        private static Board FromStringArray(string[] input)
        {
            var board = Board.Empty;
            for (var x = 0; x < 9; x++)
            {
                for (var y = 0; y < 9; y++)
                {
                    var i = input[y].ElementAt(x);
                    if (i == '.') continue;
                    if (i == '0') continue;
                    var d = System.Convert.ToInt32(i.ToString(CultureInfo.InvariantCulture));
                    board = board.Play(d, y, x);
                }
            }

            return board;
        }

        [Given("an example Euler board")]
        public void GivenAnExampleEulerBoard()
        {
            var input = new string[]
                            {
                                "..3.2.6..",
                                "9..3.5..1",
                                "..18.64..",
                                "..81.29..",
                                "7.......8",
                                "..67.82..",
                                "..26.95..",
                                "8..2.3..9",
                                "..5.1.3..",
                            };
            _board = FromStringArray(input);
        }

        [Given("a difficult board")]
        public void GivenADifficultBoard()
        {
            var input = new string[]
                            {
                                "...5...6.",
                                "8.9....1.",
                                "16..87...",
                                "3...26...",
                                "..7.1.6..",
                                "...85...3",
                                "...47..21",
                                ".4....9.8",
                                ".8...3..."
                            };

            _board = FromStringArray(input);


        }

        private static string Filter(string s)
        {
            var outS = "";
            foreach (char ch in s)
            {
                if (ch=='.') outS += ch;
                if (ch == '0') outS += '.';
                if (ch < '1') continue;
                if (ch > '9') continue;
                outS += ch;
            }
            return outS;
        }

        private static Board ParseInput(string input)
        {
            var splitBoard = input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 9; i++)
            {
                splitBoard[i] = Filter(splitBoard[i]);
            }
            return FromStringArray(splitBoard);            
        }

        [Given("this board")]
        public void GivenThisBoard(string input)
        {
            _board = ParseInput(input);
        }

        [Then("solve it")]
        public void SolveIt()
        {
            Board sample = null;

            var watch = new Stopwatch();
            watch.Start();

            var split = new Stopwatch();
            split.Start();
            Debug.WriteLine(Printer.Print(_board));
            int solutionCount = 0, last = 0;
            var solver = Solver.Create(_board);
            foreach (var solution in Solver.Solve(solver))
            {
                solutionCount++;
                sample = solution.Board;
                break;
                if (solutionCount < 10)
                {
                    Debug.WriteLine(Printer.Print(solution.Board));
                    Debug.WriteLine("");
                }
                if (solutionCount % 1000 != 0) continue;
                split.Stop();
                var time = split.ElapsedMilliseconds * 1.0 / 1000.0;
                Debug.WriteLine("{0} boards / {1} seconds. Avg: {2}", solutionCount,
                                time, (solutionCount - last) * 1.0 / time);
                if (solutionCount % 10000 == 0) Debug.WriteLine(Printer.Print(solution.Board));
                last = solutionCount;
                split.Restart();
            }
            watch.Stop();
            var ts = watch.Elapsed;
            Debug.WriteLine("Complete.\n{0} solutions in {1}. Avg:{2}solutions/sec", solutionCount, ts, solutionCount * 1.0 / ts.TotalSeconds);
            Debug.WriteLine(sample == null ? "No solution found" : Printer.Print(sample));
            Assert.Greater(solutionCount,0);
        }

        [When(@"the solver evaluates the board")]
        public void TheSolverEvaluatesTheBoard()
        {
            _solver = Solver.Create(_board);
        }

        [When(@"the solver makes the required moves")]
        public void TheSolverMakesTheRequiredMoves()
        {
            _solver = _solver.MakeRequiredMoves();
            _board = _solver.Board;

            Debug.WriteLine("Board after required moves made:\n" + Printer.Print(_board));
        }

        [Then(@"the board looks like this")]
        public void TheBoardLooksLikeThis(string input)
        {
            var otherBoard = ParseInput(input);

            Assert.AreEqual(Printer.Print(otherBoard), Printer.Print(_board));
        }

        [Then(@"I can place (.*) digits at (.*),(.*)")]
        public void ICanPlaceNDigitsAtRowCol(int n, int row, int col)
        {
            var pos = new Position(row,col);
            var playableDigits = _solver.DigitsPlayableAt(pos).ToList();
            
            var list = new StringBuilder();
            list.Append("[");
            foreach (var item in playableDigits)
            {
                list.Append(item);
            }
            list.Append("]");


            var error = String.Format("\r\nCan play:{3} at ({2})\r\nBoard:\r\n{0}\r\nHouse({2}):\r\n{1}\r\nOptions:\r\n{4}", 
                Printer.Print(_board), 
                _solver.House(pos), 
                pos,
                list,
                (new BoardProcessor(_board)).ToString());

            Assert.AreEqual(n, playableDigits.Count(), error);
        }

    }
}
