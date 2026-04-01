# Demonstrates combining multiple reporter features in realistic E2E scenarios:
# automatic steps + attachments + comments + tags together.

@QaseFields:layer:e2e
@QaseSuite:E2E_Tests\User_Workflows
Feature: Combined Tests

  @QaseId:601
  @QaseTitle:New_user_onboarding:_register,_verify,_complete_profile
  @QaseFields:severity:critical
  @QaseFields:priority:high
  Scenario: User onboarding end-to-end flow
    Given a new user registration for "alice@example.com" with password "SecurePass123!"
    When the email "alice@example.com" is verified
    And the profile is completed with first name "Alice" and last name "Johnson"
    Then the onboarding report should be generated

  @QaseId:602
  @QaseTitle:Product_search_returns_relevant_results
  @QaseFields:severity:normal
  Scenario Outline: Product search returns results
    When a search is performed for "<query>"
    Then at least <minResults> results should be returned
    And the search results page should be captured

    Examples:
      | query               | minResults |
      | mechanical keyboard | 15         |
      | wireless mouse      | 23         |
      | usb hub             | 8          |
