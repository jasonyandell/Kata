using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            _board = Board.Empty();
        }

        [When("a (.*) placed at (.*), (.*)")]
        public void WhenDigitPlacedAtRowCol(int digit, int row, int col)
        {
            //TODO: implement act (action) logic
            _board = _board.PlayAt(digit, row, col);
        }

        [Then(@"RowHouse\((.*)\) should have a constraint against (.*)")]
        public void RowHouseShouldHaveAConstraintAgainstDigit(int row, int digit)
        {
            Assert.IsTrue(_board.RowHouse(row).Constraints.Contains(digit));
        }

        [Then(@"ColumnHouse\((.*)\) should have a constraint against (.*)")]
        public void ColumnHouseShouldHaveAConstraintAgainstDigit(int column, int digit)
        {
            Assert.IsTrue(_board.ColumnHouse(column).Constraints.Contains(digit));
        }

        [Then(@"BoxHouse\((.*),(.*)\) should also have a constraint against (.*)")]
        public void BoxHouseShouldHaveAConstraintAgainstDigit(int otherRow, int otherCol, int digit)
        {
            Assert.IsTrue(_board.BoxHouse(otherRow,otherCol).Constraints.Contains(digit));
        }

        [Then(@"cannot play (.*) at (.*), (.*)")]
        public void CannotPlayAtRowCol(int digit, int row, int col)
        {
            Assert.IsFalse(_board.CanPlay(digit,row,col));
        }

        [Then("solve it")]
        public void SolveIt()
        {
            var watch = new Stopwatch();
            watch.Start();

            int i = 0;
            var solutions = Solver.Solve(_board).AsParallel();
            foreach (var solution in Solver.Solve(_board))
            {
                i++;
                if (i % 1000 == 0)
                {
                    var text = BoardProcessor.Print(solution);
                    var time = watch.ElapsedMilliseconds*1.0/1000.0;
                    if (i%1000 == 0) Debug.WriteLine("{0} boards in {1} seconds. Average: {2} boards/sec", ++i, time, i*1.0/time);
                    if (i%100000 == 0) Debug.WriteLine(text);
                }
            }
        }
    }
}
