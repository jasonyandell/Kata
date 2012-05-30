Feature: End to end
	In order to demonstrate end-to-end capabilities of the system
	As a system
	I want to be able to solve any Sudoku puzzle

Scenario: Solve empty board
	Given an empty board
	Then solve it

Scenario: Solve a difficult board
	Given a difficult board
	Then solve it

Scenario: Solve a board from project Euler
	Given an example Euler board
	Then solve it

Scenario: Can take board input from scenario file
	Given this board
	"""
4 . . . . . 8 . 5
. 3 . . . . . . .
. . . 7 . . . . .
. 2 . . . . . 6 .
. . . . 8 . 4 . .
. 4 . . 1 . . . .
. . . 6 . 3 . 7 .
5 . 3 2 . . . . .
1 . 4 . . . . . .
	"""
	Then solve it
