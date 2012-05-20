Feature: Solver
	In order to solve Sudoku puzzles
	As a library
	I want to be able to find step by step solutions to the problem

Background: 
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

Scenario Template: Evaluate a board
	When the solver evaluates the board
	 And the solver makes the required moves
	Then I can place <digit_count> digits at <row>,<col>
Examples:
| row | col | digit_count |
| 0   | 0   | 0           |
| 5   | 1   | 0           |
| 7   | 5   | 0           |
| 7   | 2   | 0           |
| 7   | 6   | 2           |
| 0   | 2   | 5           |

Scenario: Play required moves
	When the solver evaluates the board
	 And the solver makes the required moves
	Then the board looks like this
	"""
4 . . . . . 8 . 5
. 3 . . . . . . .
. . . 7 . . . . .
. 2 . . . . . 6 .
. . . . 8 . 4 . .
. 4 . . 1 . . . .
. . . 6 . 3 . 7 .
5 . 3 2 . 1 . . .
1 . 4 . . . . . .
	"""
