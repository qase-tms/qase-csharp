# Demonstrates automatic step reporting from Given/When/Then steps.
# Every Gherkin step is automatically captured as a StepResult in Qase.
# No additional code or attributes required.

Feature: Step Tests

  @QaseId:301
  @QaseTitle:User_registration_completes_all_required_steps
  Scenario: User registration completes all steps
    Given the registration page is open
    When the user fills in email "john.doe@example.com" and password "SecurePass123!"
    And the form is submitted
    Then a confirmation email should be sent to "john.doe@example.com"

  @QaseId:302
  @QaseTitle:Checkout_process_with_nested_address_and_payment_steps
  Scenario: Checkout process with address and payment
    Given an item "Wireless Keyboard" priced at 49.99 is in the cart
    When the shipping address is entered
    And payment details are entered for card "4111111111111111"
    Then the order should be confirmed

  @QaseId:303
  @QaseTitle:Order_creation_with_product_selection_and_discount
  Scenario: Order creation with discount
    Given a product "Mechanical Keyboard" with quantity 2 is selected
    When a discount code "SAVE10" is applied
    Then the total should be calculated with 10 percent discount on 99.98

  @QaseId:304
  @QaseTitle:Data_migration_partially_succeeds_with_expected_failures
  Scenario: Data migration partial success
    Given 100 user records to migrate
    And 50 order records to migrate
    When corrupted records are encountered
    Then the migration summary should show 150 migrated and 3 failed
