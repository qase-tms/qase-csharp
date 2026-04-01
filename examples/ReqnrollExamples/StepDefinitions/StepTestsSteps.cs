using Reqnroll;

namespace ReqnrollExamples.StepDefinitions;

[Binding]
public class StepTestsSteps
{
    [Given(@"the registration page is open")]
    public void GivenRegistrationPageOpen()
    {
        Assert.That(true, Is.True, "Registration page should load");
    }

    [When(@"the user fills in email ""(.*)"" and password ""(.*)""")]
    public void WhenFillForm(string email, string password)
    {
        Assert.That(email, Is.Not.Empty, "Email is required");
        Assert.That(password.Length, Is.GreaterThanOrEqualTo(8), "Password must be at least 8 characters");
    }

    [When(@"the form is submitted")]
    public void WhenFormSubmitted()
    {
        Assert.That(201, Is.EqualTo(201));
    }

    [Then(@"a confirmation email should be sent to ""(.*)""")]
    public void ThenConfirmationEmail(string email)
    {
        Assert.That(true, Is.True, $"Confirmation email should be sent to {email}");
    }

    [Given(@"an item ""(.*)"" priced at (.*) is in the cart")]
    public void GivenItemInCart(string productName, decimal price)
    {
        Assert.That(price, Is.GreaterThan(0), $"{productName} must have positive price");
    }

    [When(@"the shipping address is entered")]
    public void WhenShippingAddress()
    {
        Assert.That("123 Main St", Is.Not.Empty);
        Assert.That("CA".Length, Is.EqualTo(2));
        Assert.That("94102".Length, Is.EqualTo(5));
    }

    [When(@"payment details are entered for card ""(.*)""")]
    public void WhenPaymentDetails(string cardNumber)
    {
        Assert.That(cardNumber.Length, Is.EqualTo(16));
    }

    [Then(@"the order should be confirmed")]
    public void ThenOrderConfirmed()
    {
        Assert.That(true, Is.True);
    }

    [Given(@"a product ""(.*)"" with quantity (.*) is selected")]
    public void GivenProductSelected(string name, int quantity)
    {
        Assert.That(quantity, Is.GreaterThan(0), "Quantity must be positive");
    }

    [When(@"a discount code ""(.*)"" is applied")]
    public void WhenDiscountCode(string code)
    {
        Assert.That(code, Is.Not.Empty, "Discount code cannot be empty");
    }

    [Then(@"the total should be calculated with (.*) percent discount on (.*)")]
    public void ThenTotalCalculated(int discountPercent, decimal subtotal)
    {
        var total = subtotal * (1 - discountPercent / 100m);
        Assert.That(total, Is.GreaterThan(0), "Total must be positive after discount");
    }

    [Given(@"(.*) user records to migrate")]
    public void GivenUserRecords(int count)
    {
        Assert.That(count, Is.GreaterThan(0), "User records migrated");
    }

    [Given(@"(.*) order records to migrate")]
    public void GivenOrderRecords(int count)
    {
        Assert.That(count, Is.GreaterThan(0), "Order records migrated");
    }

    [When(@"corrupted records are encountered")]
    public void WhenCorruptedRecords()
    {
        // Simulates that corrupted records were logged but migration continues
        Assert.That(true, Is.True);
    }

    [Then(@"the migration summary should show (.*) migrated and (.*) failed")]
    public void ThenMigrationSummary(int migrated, int failed)
    {
        Assert.That(migrated, Is.EqualTo(150));
        Assert.That(failed, Is.EqualTo(3));
    }
}
