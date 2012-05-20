﻿Feature: Sudoku board
	In order to represent the rules of Sudoku
	As an user of the Board class
	I want the proper constraints to be mapped

 Scenario Outline: Box constraint
	Given an empty board
	When a <digit> placed at <row>, <column>
	Then cannot play <digit> at <otherRow>,<otherCol>
Examples:
| digit | row | column | otherRow | otherCol |
| 1     | 0   | 0      | 0        | 0        |
| 1     | 2   | 2      | 0        | 0        |
| 2     | 2   | 2      | 0        | 0        |
| 3     | 3   | 3      | 4        | 5        |
| 4     | 8   | 8      | 7        | 7        |
| 5     | 6   | 4      | 7        | 4        |
| 6     | 1   | 3      | 0        | 5        |  

Scenario Outline: Cannot move in the same place twice
	Given an empty board
	When a 1 placed at <row>, <column>
	Then cannot play <digit> at <row>, <column>
Examples:
| digit | row | column |
| 1     | 2   | 3      |
| 2     | 2   | 3      |
| 3     | 2   | 3      |
| 4     | 2   | 3      |
| 5     | 2   | 3      |
| 6     | 2   | 3      |
| 7     | 2   | 3      |
| 8     | 2   | 3      |
| 9     | 2   | 3      |

Scenario Outline: Cannot place a digit in the same row twice
	Given an empty board
	When a 1 placed at 1, 1
	Then cannot play 1 at <row>, 1
Examples:
| row    |
| 0      |
| 1      |
| 2      |
| 3      |
| 4      |
| 5      |
| 6      |
| 7      |
| 8      |

Scenario Outline: Cannot place a digit in the same column twice
	Given an empty board
	When a 1 placed at 1, 1
	Then cannot play 1 at 1, <column>
Examples:
| column |
| 0      |
| 1      |
| 2      |
| 3      |
| 4      |
| 5      |
| 6      |
| 7      |
| 8      |


