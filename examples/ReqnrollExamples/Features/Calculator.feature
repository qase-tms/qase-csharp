Feature: Calculator

  @QaseId:201
  Scenario: Add two numbers
    Given the calculator is open
    When I add 5 and 3
    Then the result should be 8

  @QaseId:202
  @QaseFields:priority:low
  Scenario Outline: Arithmetic operations
    Given the calculator is open
    When I perform "<operation>" on <a> and <b>
    Then the result should be <expected>

    Examples:
      | operation | a  | b  | expected |
      | add       | 10 | 20 | 30       |
      | subtract  | 50 | 15 | 35       |
      | multiply  | 7  | 6  | 42       |

  @QaseIgnore
  Scenario: WIP division by zero
    Given the calculator is open
    When I divide 10 by 0
    Then an error should be shown
