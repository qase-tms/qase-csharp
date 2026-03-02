using Qase.Csharp.Commons.Attributes;

namespace MSTestExamples;

/// <summary>
/// Demonstrates parameterized tests with MSTest: DataRow and DynamicData.
/// </summary>
[TestClass]
public class ParameterizedTests
{
    [TestMethod]
    [QaseIds(501)]
    [Title("Login accepts valid credential combinations")]
    [DataRow("admin@example.com", "Admin123!", true)]
    [DataRow("user@example.com", "UserPass1!", true)]
    [DataRow("guest", "guest", false)]
    [DataRow("", "nouser", false)]
    [Qase]
    public void Login_WithVariousCredentials(string username, string password, bool expectedSuccess)
    {
        var isValidUsername = !string.IsNullOrEmpty(username) && username.Contains("@");
        var isValidPassword = password.Length >= 8;
        var loginSuccess = isValidUsername && isValidPassword;

        Assert.AreEqual(expectedSuccess, loginSuccess);
    }

    [TestMethod]
    [QaseIds(502)]
    [Title("Email validation rejects invalid formats")]
    [DataRow("", "empty string")]
    [DataRow("plaintext", "missing @ symbol")]
    [DataRow("@example.com", "missing local part")]
    [DataRow("user@", "missing domain")]
    [DataRow("user@example.", "trailing dot in domain")]
    [Qase]
    public void EmailValidation_RejectsInvalidFormats(string email, string reason)
    {
        var isValid = !string.IsNullOrEmpty(email)
                      && email.Contains("@")
                      && email.Contains(".")
                      && !email.StartsWith("@")
                      && !email.EndsWith(".");

        Assert.IsFalse(isValid, $"Email '{email}' should be invalid: {reason}");
    }

    [TestMethod]
    [QaseIds(503)]
    [Title("Pagination returns correct page size for various configurations")]
    [DynamicData(nameof(PaginationData), DynamicDataSourceType.Method)]
    [Qase]
    public void Pagination_ReturnsCorrectPageSize(int pageSize, int pageNumber)
    {
        var totalItems = 120;
        var startIndex = (pageNumber - 1) * pageSize;
        var itemsOnPage = Math.Min(pageSize, totalItems - startIndex);

        Assert.IsTrue(itemsOnPage > 0, $"Page {pageNumber} with size {pageSize} should have items");
        Assert.IsTrue(itemsOnPage <= pageSize);
    }

    private static IEnumerable<object[]> PaginationData()
    {
        int[] pageSizes = { 10, 25, 50, 100 };
        int[] pageNumbers = { 1, 2, 3 };

        foreach (var size in pageSizes)
        {
            foreach (var page in pageNumbers)
            {
                yield return new object[] { size, page };
            }
        }
    }

    [TestMethod]
    [QaseIds(504)]
    [Title("Shopping cart calculates total correctly")]
    [DataRow(1, 29.99, 0, 29.99)]
    [DataRow(3, 9.99, 0, 29.97)]
    [DataRow(2, 50.00, 10, 90.00)]
    [DataRow(1, 100.00, 25, 75.00)]
    [Qase]
    public void ShoppingCart_CalculatesTotal(int quantity, double unitPrice, int discountPercent, double expectedTotal)
    {
        var subtotal = quantity * unitPrice;
        var total = subtotal * (1 - discountPercent / 100.0);

        Assert.AreEqual(expectedTotal, Math.Round(total, 2));
    }
}
