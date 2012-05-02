using System;
using System.Collections.Generic;
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
            var solution = Solver.Solve(_board).Take(1).ToList()[0];
            var output = BoardProcessor.AsText(solution);
        }
    }
}
