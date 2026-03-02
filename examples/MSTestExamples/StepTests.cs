using Qase.Csharp.Commons.Attributes;

namespace MSTestExamples;

/// <summary>
/// Demonstrates test steps: basic, nested, with custom titles, and mixed status.
/// The [Qase] attribute is required to enable step tracking.
/// </summary>
[TestClass]
public class StepTests
{
    [TestMethod]
    [QaseIds(301)]
    [Title("User registration completes all required steps")]
    [Qase]
    public void UserRegistration_CompletesAllSteps()
    {
        OpenRegistrationPage();
        FillRegistrationForm("john.doe@example.com", "SecurePass123!");
        SubmitForm();
        VerifyConfirmationEmail("john.doe@example.com");
    }

    [TestMethod]
    [QaseIds(302)]
    [Title("Checkout process with nested address and payment steps")]
    [Qase]
    public void CheckoutProcess_WithNestedSteps()
    {
        AddItemToCart("Wireless Keyboard", 49.99m);
        FillShippingAddress();
        FillPaymentDetails("4111111111111111");
        ConfirmOrder();
    }

    [TestMethod]
    [QaseIds(303)]
    [Title("Order creation with custom step names for clarity")]
    [Qase]
    public void OrderCreation_WithCustomStepNames()
    {
        SelectProduct("Mechanical Keyboard", 2);
        ApplyDiscountCode("SAVE10");
        CalculateTotal(99.98m, 10);
    }

    [TestMethod]
    [QaseIds(304)]
    [Title("Data migration partially succeeds with expected failures")]
    [Qase]
    public void DataMigration_PartialSuccess()
    {
        MigrateUserRecords();
        MigrateOrderRecords();

        try
        {
            MigrateCorruptedRecords();
        }
        catch
        {
            // Expected: corrupted records fail migration
        }

        VerifyMigrationSummary(migrated: 150, failed: 3);
    }

    // --- Basic steps ---

    [Step]
    private void OpenRegistrationPage()
    {
        var pageLoaded = true;
        Assert.IsTrue(pageLoaded, "Registration page should load");
    }

    [Step]
    private void FillRegistrationForm(string email, string password)
    {
        Assert.IsFalse(string.IsNullOrEmpty(email), "Email is required");
        Assert.IsTrue(password.Length >= 8, "Password must be at least 8 characters");
    }

    [Step]
    private void SubmitForm()
    {
        var responseCode = 201;
        Assert.AreEqual(201, responseCode);
    }

    [Step]
    private void VerifyConfirmationEmail(string email)
    {
        var emailSent = true;
        Assert.IsTrue(emailSent, $"Confirmation email should be sent to {email}");
    }

    // --- Nested steps ---

    [Step]
    private void FillShippingAddress()
    {
        FillStreetAddress("123 Main St");
        FillCityStateZip("San Francisco", "CA", "94102");
    }

    [Step]
    private void FillStreetAddress(string street)
    {
        Assert.IsFalse(string.IsNullOrEmpty(street));
    }

    [Step]
    private void FillCityStateZip(string city, string state, string zip)
    {
        Assert.IsFalse(string.IsNullOrEmpty(city));
        Assert.AreEqual(2, state.Length);
        Assert.AreEqual(5, zip.Length);
    }

    [Step]
    private void AddItemToCart(string productName, decimal price)
    {
        Assert.IsTrue(price > 0, $"{productName} must have positive price");
    }

    [Step]
    private void FillPaymentDetails(string cardNumber)
    {
        Assert.AreEqual(16, cardNumber.Length);
    }

    [Step]
    private void ConfirmOrder()
    {
        var orderConfirmed = true;
        Assert.IsTrue(orderConfirmed);
    }

    // --- Steps with custom titles ---

    [Step]
    [Title("Select product and set quantity")]
    private void SelectProduct(string name, int quantity)
    {
        Assert.IsTrue(quantity > 0, "Quantity must be positive");
    }

    [Step]
    [Title("Apply promotional discount code")]
    private void ApplyDiscountCode(string code)
    {
        Assert.IsFalse(string.IsNullOrEmpty(code), "Discount code cannot be empty");
    }

    [Step]
    [Title("Calculate total with discount applied")]
    private void CalculateTotal(decimal subtotal, int discountPercent)
    {
        var total = subtotal * (1 - discountPercent / 100m);
        Assert.IsTrue(total > 0, "Total must be positive after discount");
    }

    // --- Mixed status steps ---

    [Step]
    private void MigrateUserRecords()
    {
        var count = 100;
        Assert.IsTrue(count > 0, "User records migrated");
    }

    [Step]
    private void MigrateOrderRecords()
    {
        var count = 50;
        Assert.IsTrue(count > 0, "Order records migrated");
    }

    [Step]
    private void MigrateCorruptedRecords()
    {
        throw new InvalidOperationException("3 records have invalid format and cannot be migrated");
    }

    [Step]
    private void VerifyMigrationSummary(int migrated, int failed)
    {
        Assert.AreEqual(150, migrated);
        Assert.AreEqual(3, failed);
    }
}
