# Demonstrates all attachment types: single file, multiple files,
# byte array content, and step-level attachments.
# Attachments are added from step definitions using the Metadata API.

Feature: Attachment Tests

  @QaseId:401
  @QaseTitle:Login_page_captures_screenshot_on_successful_load
  Scenario: Login page captures screenshot
    Given the login page is loaded with title "Login - MyApp"
    Then a screenshot should be attached

  @QaseId:402
  @QaseTitle:API_test_captures_both_request_log_and_response_screenshot
  Scenario: API test captures request and response
    When an API call returns status code 200
    Then both the screenshot and log should be attached

  @QaseId:403
  @QaseTitle:Generate_HTML_report_and_attach_from_memory
  Scenario: Generate report and attach from memory
    When a test summary report is generated with 42 total, 40 passed and 2 failed
    Then the HTML report should be attached as byte array

  @QaseId:404
  @QaseTitle:Checkout_step_captures_state_screenshot_at_each_stage
  Scenario: Checkout captures screenshot per stage
    Given a shopping cart with total 149.99
    When shipping details are entered for "123 Main St, San Francisco, CA"
    Then the order "ORD-2026-0042" should be confirmed with attachment
