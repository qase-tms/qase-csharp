# Demonstrates all Qase Gherkin tags: @QaseId, @QaseTitle, @QaseFields, @QaseSuite, @QaseIgnore.
# Feature-level tags apply to all scenarios in this file.

@QaseFields:layer:e2e
@QaseSuite:Authentication\Smoke_Tests
Feature: Tag Tests

  @QaseId:101
  Scenario: User registration creates new account
    Given a registration form is submitted for "john.doe@example.com"
    Then the account should be created successfully

  @QaseId:102,103
  Scenario: Login page accepts both email and username
    Given the login page is open
    When the user logs in with email "admin@example.com"
    Then login should succeed
    When the user logs in with username "admin"
    Then login should succeed

  @QaseId:104
  @QaseTitle:Verify_that_the_login_page_displays_the_correct_title_and_branding
  Scenario: Login page displays correct branding
    Given the login page is open
    Then the page title should be "Welcome to Qase"
    And the logo should be visible

  @QaseId:105
  @QaseFields:severity:critical
  @QaseFields:priority:high
  @QaseFields:layer:api
  Scenario: Password reset sends email to registered user
    Given a registered user "john.doe@example.com"
    When a password reset is requested
    Then a reset email should be sent

  @QaseSuite:User_Profile\Settings
  Scenario: User profile updates display name
    Given a user with display name "John"
    When the display name is changed to "John Doe"
    Then the display name should be "John Doe"

  @QaseIgnore
  Scenario: Deprecated feature - legacy password policy
    Given the legacy password policy is active
    Then the policy check should pass
