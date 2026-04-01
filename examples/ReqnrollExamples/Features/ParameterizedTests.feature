# Demonstrates parameterized tests with Scenario Outlines.
# Each example row creates a separate test result with parameters captured.

Feature: Parameterized Tests

  @QaseId:501
  @QaseTitle:Login_accepts_valid_credential_combinations
  Scenario Outline: Login with various credentials
    When the user logs in with "<username>" and "<password>"
    Then the login result should be <expected>

    Examples:
      | username            | password    | expected |
      | admin@example.com   | Admin123!   | true     |
      | user@example.com    | UserPass1!  | true     |
      | guest               | guest       | false    |
      | (empty)             | nouser      | false    |

  @QaseId:502
  @QaseTitle:Email_validation_rejects_invalid_formats
  Scenario Outline: Email validation rejects invalid formats
    When the email "<emailInput>" is validated
    Then it should be rejected because "<reason>"

    Examples:
      | emailInput    | reason                  |
      | (empty)       | empty string            |
      | plaintext     | missing @ symbol        |
      | @example.com  | missing local part      |
      | user@         | missing domain          |
      | user@example. | trailing dot in domain  |

  @QaseId:503
  @QaseTitle:Shopping_cart_calculates_total_correctly
  Scenario Outline: Shopping cart calculates total
    When <quantity> items at <unitPrice> with <discountPercent> percent discount are added
    Then the cart total should be <expectedTotal>

    Examples:
      | quantity | unitPrice | discountPercent | expectedTotal |
      | 1        | 29.99     | 0               | 29.99         |
      | 3        | 9.99      | 0               | 29.97         |
      | 2        | 50.00     | 10              | 90.00         |
      | 1        | 100.00    | 25              | 75.00         |
