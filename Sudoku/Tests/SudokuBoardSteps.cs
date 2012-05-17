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
        private Domain.Board _board;

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

        [Given("this board")]
        public void GivenThisBoard(string input)
        {
            var splitBoard = input.Split(new[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < 9; i++)
            {
                splitBoard[i] = Filter(splitBoard[i]);
            }
            _board = FromStringArray(splitBoard);
        }

        [Then("solve it")]
        public void SolveIt()
        {
            TechTalk.SpecFlow.ScenarioContext.Current.Pending();
            //Board sample = null;
            
            //var watch = new Stopwatch();
            //watch.Start();

            //var split = new Stopwatch();
            //split.Start();
            //Debug.WriteLine(BoardProcessor.Print(_board));
            //int solutionCount = 0,last = 0;
            //foreach (var solution in Solver.Solve(_board))
            //{
            //    sample = solution;
            //    if (solutionCount==0) Debug.WriteLine(BoardProcessor.Print(solution));
            //    solutionCount++;
            //    if (solutionCount%1000 != 0) continue;
            //    split.Stop();
            //    var time = split.ElapsedMilliseconds*1.0/1000.0;
            //    Debug.WriteLine("{0} boards / {1} seconds. Avg: {2}", solutionCount,
            //                    time, (solutionCount-last)*1.0/time);
            //    if (solutionCount % 10000 == 0) Debug.WriteLine(BoardProcessor.Print(solution));
            //    last = solutionCount;
            //    split.Restart();
            //}
            //watch.Stop();
            //var ts = watch.Elapsed;
            //Debug.WriteLine("Complete.\n{0} solutions in {1}. Avg:{2}solutions/sec",solutionCount,ts,solutionCount*1.0/ts.TotalSeconds);
            //Debug.WriteLine(BoardProcessor.Print(sample));
        }
    }
}
