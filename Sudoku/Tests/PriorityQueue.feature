Feature: PriorityQueue
	In order to keep track of items by priority
	As a user
	I want to be able to get the highest priority item

Scenario Template: Enqueue two items, find highest priority item
	Given an exmpty queue
	When I enqueue <item> with priority <priority>
	And I enqueue <other_item> with priority <other_priority>
	When I peek in the priority queue 
	Then I should find <result>
Examples:
| item | priority | other_item | other_priority | result |
| 1    | 1        | 2          | 2              | 2      |
| 2    | 2        | 1          | 1              | 2      |
| 2    | 2        | 3          | 3              | 3      |
| 3    | 1        | 2          | 2              | 2      |

Scenario Template: Cannot enqueue two items with the same priority
	Given an empty queue
	When I enqueue <item> with priority <priority>
	And I enqueue <item> with priority <priority>
	Then an error should occur
Examples:
| item | priority | other_item | other_priority | result |
| 1    | 1        | 2          | 2              | 2      |
