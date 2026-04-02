using Reqnroll;

namespace ReqnrollExamples.StepDefinitions;

[Binding]
public class ParameterizedTestsSteps
{
    private bool _loginResult;
    private bool _emailValid;
    private double _cartTotal;

    [When(@"the user logs in with ""(.*)"" and ""(.*)""")]
    public void WhenLoginWith(string username, string password)
    {
        var actualUsername = username == "(empty)" ? "" : username;
        var isValidUsername = !string.IsNullOrEmpty(actualUsername) && actualUsername.Contains("@");
        var isValidPassword = password.Length >= 8;
        _loginResult = isValidUsername && isValidPassword;
    }

    [Then(@"the login result should be (.*)")]
    public void ThenLoginResult(bool expected)
    {
        Assert.That(_loginResult, Is.EqualTo(expected));
    }

    [When(@"the email ""(.*)"" is validated")]
    public void WhenEmailValidated(string emailInput)
    {
        var email = emailInput == "(empty)" ? "" : emailInput;
        _emailValid = !string.IsNullOrEmpty(email)
                      && email.Contains("@")
                      && email.Contains(".")
                      && !email.StartsWith("@")
                      && !email.EndsWith(".");
    }

    [Then(@"it should be rejected because ""(.*)""")]
    public void ThenRejected(string reason)
    {
        Assert.That(_emailValid, Is.False, $"Email should be invalid: {reason}");
    }

    [When(@"(.*) items at (.*) with (.*) percent discount are added")]
    public void WhenItemsAdded(int quantity, double unitPrice, int discountPercent)
    {
        var subtotal = quantity * unitPrice;
        _cartTotal = Math.Round(subtotal * (1 - discountPercent / 100.0), 2);
    }

    [Then(@"the cart total should be (.*)")]
    public void ThenCartTotal(double expectedTotal)
    {
        Assert.That(_cartTotal, Is.EqualTo(expectedTotal));
    }
}
