using Qase.Csharp.Commons.Attributes;

namespace NUnitExamples;

/// <summary>
/// Demonstrates parameterized tests with NUnit: TestCase and Values/Range.
/// </summary>
[TestFixture]
public class ParameterizedTests
{
    [Test]
    [QaseIds(501)]
    [Title("Login accepts valid credential combinations")]
    [TestCase("admin@example.com", "Admin123!", true)]
    [TestCase("user@example.com", "UserPass1!", true)]
    [TestCase("guest", "guest", false)]
    [TestCase("", "nouser", false)]
    [Qase]
    public void Login_WithVariousCredentials(string username, string password, bool expectedSuccess)
    {
        var isValidUsername = !string.IsNullOrEmpty(username) && username.Contains("@");
        var isValidPassword = password.Length >= 8;
        var loginSuccess = isValidUsername && isValidPassword;

        Assert.That(loginSuccess, Is.EqualTo(expectedSuccess));
    }

    [Test]
    [QaseIds(502)]
    [Title("Email validation rejects invalid formats")]
    [TestCase("", "empty string")]
    [TestCase("plaintext", "missing @ symbol")]
    [TestCase("@example.com", "missing local part")]
    [TestCase("user@", "missing domain")]
    [TestCase("user@example.", "trailing dot in domain")]
    [Qase]
    public void EmailValidation_RejectsInvalidFormats(string email, string reason)
    {
        var isValid = !string.IsNullOrEmpty(email)
                      && email.Contains("@")
                      && email.Contains(".")
                      && !email.StartsWith("@")
                      && !email.EndsWith(".");

        Assert.That(isValid, Is.False, $"Email '{email}' should be invalid: {reason}");
    }

    [Test]
    [QaseIds(503)]
    [Title("Pagination returns correct page size")]
    [Qase]
    public void Pagination_ReturnsCorrectPageSize(
        [Values(10, 25, 50, 100)] int pageSize,
        [Range(1, 3)] int pageNumber)
    {
        var totalItems = 120;
        var startIndex = (pageNumber - 1) * pageSize;
        var itemsOnPage = Math.Min(pageSize, totalItems - startIndex);

        Assert.That(itemsOnPage, Is.GreaterThan(0), $"Page {pageNumber} with size {pageSize} should have items");
        Assert.That(itemsOnPage, Is.LessThanOrEqualTo(pageSize));
    }

    [Test]
    [QaseIds(504)]
    [Title("Shopping cart calculates total correctly")]
    [TestCase(1, 29.99, 0, ExpectedResult = 29.99)]
    [TestCase(3, 9.99, 0, ExpectedResult = 29.97)]
    [TestCase(2, 50.00, 10, ExpectedResult = 90.00)]
    [TestCase(1, 100.00, 25, ExpectedResult = 75.00)]
    [Qase]
    public double ShoppingCart_CalculatesTotal(int quantity, double unitPrice, int discountPercent)
    {
        var subtotal = quantity * unitPrice;
        var total = subtotal * (1 - discountPercent / 100.0);

        return Math.Round(total, 2);
    }
}
