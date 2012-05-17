Feature: Solver
	In order to solve Sudoku puzzles
	As a library
	I want to be able to find step by step solutions to the problem

@mytag
Scenario Template: Evaluate a board
	Given this board
	"""
4 . . . . . 8 . 5
. 3 . . . . . . .
. . . 7 . . . . .
. 2 . . . . . 6 .
. . . . 8 . 4 . .
. . . . 1 . . . .
. . . 6 . 3 . 7 .
5 . . 2 . . . . .
1 . 4 . . . . . .
	"""
	When the solver evaluates the board
	Then I can place <digit_count> digits at <row>,<col>
Examples:
| row | col | digit_count |
| 0   | 0   | 0           |
| 7   | 5   | 1           |
| 7   | 2   | 1           |
