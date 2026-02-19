using Qase.Csharp.Commons.Attributes;

namespace xUnitExamples;

/// <summary>
/// Demonstrates parameterized tests with xUnit: Theory/InlineData and Theory/MemberData.
/// </summary>
public class ParameterizedTests
{
    [Theory]
    [QaseIds(501)]
    [Title("Login accepts valid credential combinations")]
    [InlineData("admin@example.com", "Admin123!", true)]
    [InlineData("user@example.com", "UserPass1!", true)]
    [InlineData("guest", "guest", false)]
    [InlineData("", "nouser", false)]
    [Qase]
    public void Login_WithVariousCredentials(string username, string password, bool expectedSuccess)
    {
        var isValidUsername = !string.IsNullOrEmpty(username) && username.Contains("@");
        var isValidPassword = password.Length >= 8;
        var loginSuccess = isValidUsername && isValidPassword;

        Assert.Equal(expectedSuccess, loginSuccess);
    }

    [Theory]
    [QaseIds(502)]
    [Title("Email validation rejects invalid formats")]
    [MemberData(nameof(InvalidEmailTestData))]
    [Qase]
    public void EmailValidation_RejectsInvalidFormats(string email, string reason)
    {
        var isValid = !string.IsNullOrEmpty(email)
                      && email.Contains("@")
                      && email.Contains(".")
                      && !email.StartsWith("@")
                      && !email.EndsWith(".");

        Assert.False(isValid, $"Email '{email}' should be invalid: {reason}");
    }

    public static IEnumerable<object[]> InvalidEmailTestData()
    {
        yield return new object[] { "", "empty string" };
        yield return new object[] { "plaintext", "missing @ symbol" };
        yield return new object[] { "@example.com", "missing local part" };
        yield return new object[] { "user@", "missing domain" };
        yield return new object[] { "user@example.", "trailing dot in domain" };
    }

    [Theory]
    [QaseIds(503)]
    [Title("Shopping cart calculates total correctly")]
    [InlineData(1, 29.99, 0, 29.99)]
    [InlineData(3, 9.99, 0, 29.97)]
    [InlineData(2, 50.00, 10, 90.00)]
    [InlineData(1, 100.00, 25, 75.00)]
    [Qase]
    public void ShoppingCart_CalculatesTotal(int quantity, double unitPrice, int discountPercent, double expectedTotal)
    {
        var subtotal = quantity * unitPrice;
        var total = subtotal * (1 - discountPercent / 100.0);

        Assert.Equal(expectedTotal, Math.Round(total, 2));
    }
}
