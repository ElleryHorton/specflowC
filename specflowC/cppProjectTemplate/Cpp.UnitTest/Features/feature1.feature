@integration
Feature: feature1
	as a
	i want to
	so that

Scenario: UnitTest1
	Given a sentence 't1'
	When a sentence
	Then a sentence
	Given a sentence 't1'

Scenario: UnitTest2
	Given a sentence 't1' 't2'
	When a sentence
	Then a sentence

Scenario Outline: UnitTest3
	Given a sentence '<t1>'
	Given a sentence '<t2>'

Examples:
	| t1 | t2 |
	| 1  | 2  |
	| 3  | 4  |
