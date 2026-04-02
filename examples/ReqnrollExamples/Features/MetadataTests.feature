# Demonstrates Metadata.Comment() to add contextual comments to test results.
# Comments are added from step definitions using the Metadata API.

Feature: Metadata Tests

  @QaseId:201
  @QaseTitle:Failed_login_records_attempt_details_in_report
  Scenario: Failed login logs attempt details
    Given a login attempt by "admin@example.com" from IP "192.168.1.42"
    When the login is denied
    Then the attempt details should be recorded

  @QaseId:202
  @QaseTitle:API_response_details_are_recorded_for_debugging
  Scenario: API response records payload details
    When a GET request is made to "/api/v1/users/42"
    Then the response status code should be 200
    And the response body should be recorded

  Scenario: Order processing adds tracking comment
    Given an order "ORD-2026-0042" with total 149.99
    When the order is processed
    Then the processing result should be recorded
