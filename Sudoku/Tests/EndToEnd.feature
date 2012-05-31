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

Scenario: Solve a hard problem from the internet
	Given this board
	"""
. . . . 6 5 . . .
6 . 9 4 . 3 . . 7
. . . 2 . . . 4 .
. . 1 . . . . 7 6
7 . . . 3 . . . 8
3 8 . . . . 2 . .
. 6 . . . 4 . . .
1 . . 3 . 7 9 . 4
. . . 8 5 . . . .
	"""
	Then solve it

Scenario: Solve an extreme problem from the internet
	Given this board
	"""
2 . . . 8 . . 7 6
. . . . . . 4 . .
. . . 2 . . 5 . 8
5 . . 1 . . 8 6 .
. . 7 6 . 9 3 . .
. 6 2 . . 7 . . 4
3 . 6 . . 1 . . .
. . 1 . . . . . .
8 5 . . 6 . . . 3
	"""
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
