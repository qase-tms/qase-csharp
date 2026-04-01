@QaseFields:component:authentication
@QaseSuite:Auth\Login
Feature: User Login

  @QaseId:101
  @QaseFields:severity:critical
  Scenario: Successful login with valid credentials
    Given the user is on the login page
    When the user enters "admin" and "password123"
    Then the user should see the dashboard

  @QaseId:102,103
  Scenario Outline: Login with different credentials
    Given the user is on the login page
    When the user enters "<username>" and "<password>"
    Then the login result should be "<result>"

    Examples:
      | username | password    | result  |
      | admin    | password123 | success |
      | user1    | secret      | success |
      | invalid  | wrong       | failure |

  @QaseId:104
  @QaseTitle:Login_page_loads_correctly
  Scenario: Login page display
    Given the user is on the login page
    Then the login form should be visible
    And the "Sign In" button should be present
