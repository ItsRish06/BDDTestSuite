using BDDTestSuite.Models;
using BDDTestSuite.PageObjects;
using BDDTestSuite.Utils;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.StepDefinitions
{
    [Binding]
    public class CheckoutSteps
    {

        IWebDriver _driver;
        ILogger _logger;
        IConfigurationRoot _config;
        ScenarioContext _scenarioContext;

        public CheckoutSteps(IWebDriver driver, ILogger logger, IConfigurationRoot config, ScenarioContext scenarioContext)
        {
            _driver = driver;
            _logger = logger;
            _config = config;
            _scenarioContext = scenarioContext;
        }

        [When("user selects the checkout option")]
        public void WhenUserSelectsTheCheckoutOption()
        {
            new CartPage(_driver,_logger).ClickCheckoutBtn();

            WaitUtils.WaitUntilUrlChangesTo(_driver, "https://www.saucedemo.com/checkout-step-one.html", TimeSpan.FromSeconds(2));
        }

        [Then("the checkout form should validate all user entered inputs")]
        public void ThenTheCheckoutFormShouldValidateAllUserEnteredInputs()
        {
            var checkoutPage = new CheckoutPage(_driver,_logger);

            checkoutPage.ClickContinueBtn();

            var errorMsg = checkoutPage.GetFormErrorMessage();

            Assert.Equal("Error: First Name is required", errorMsg);

            checkoutPage.EnterFirstName("TestFN");
            checkoutPage.EnterLastName("TestLN");
            checkoutPage.EnterZip("");

            checkoutPage.ClickContinueBtn();

            errorMsg = checkoutPage.GetFormErrorMessage();

            Assert.Equal("Error: Postal Code is required", errorMsg);

            checkoutPage.EnterFirstName("TestFN");
            checkoutPage.EnterLastName("");
            checkoutPage.EnterZip("12333");

            checkoutPage.ClickContinueBtn();

            errorMsg = checkoutPage.GetFormErrorMessage();

            Assert.Equal("Error: Last Name is required", errorMsg);

            checkoutPage.EnterFirstName("");
            checkoutPage.EnterLastName("TestLN");
            checkoutPage.EnterZip("12333");

            checkoutPage.ClickContinueBtn();

            errorMsg = checkoutPage.GetFormErrorMessage();

            Assert.Equal("Error: First Name is required", errorMsg);

            checkoutPage.EnterFirstName("TestFN");
            checkoutPage.EnterLastName("TestLN");
            checkoutPage.EnterZip("ab1c");

            checkoutPage.ClickContinueBtn();

            errorMsg = checkoutPage.GetFormErrorMessage();

            Assert.Equal("Error: Postal Code must be 5 alpha-numeric characters long", errorMsg);

            checkoutPage.EnterZip("ab@1c");
            checkoutPage.ClickContinueBtn();

            errorMsg = checkoutPage.GetFormErrorMessage();

            Assert.Equal("Error: Postal Code must be alpha-numeric", errorMsg);

        }

        [When("user enters their information on the checkout form")]
        public void WhenUserEntersTheirInformationOnTheCheckoutForm()
        {
            var checkoutPage = new CheckoutPage(_driver, _logger);

            checkoutPage.EnterFirstName("TestFN");
            checkoutPage.EnterLastName("TestLN");
            checkoutPage.EnterZip("33212");

        }

        [When("user navigates to checkout overview page")]
        public void WhenUserNavigatesToCheckoutOverviewPage()
        {
            new CheckoutPage(_driver, _logger).ClickContinueBtn();

            WaitUtils.WaitUntilUrlChangesTo(_driver, "https://www.saucedemo.com/checkout-step-two.html", TimeSpan.FromSeconds(2));
        }

        [Then("user should see correct product information on the overview page")]
        public void ThenUserShouldSeeCorrectProductInformationOnTheOverviewPage()
        {
            var checkoutPage = new CheckoutPage(_driver, _logger);
            var displayedProducts = checkoutPage.GetAllProductDetails();
            var expectedProducts = _scenarioContext.Get<List<Product>>("CartAddedProducts");

            Assert.Equal(
                expectedProducts.Select(product => product.Name),
                displayedProducts.Select(product => product.Name)
                );
            Assert.Equal(
                expectedProducts.Select(product => product.Description),
                displayedProducts.Select(product => product.Description)
                );
            Assert.Equal(
                expectedProducts.Select(product => product.Price),
                displayedProducts.Select(product => product.Price)
                );

            var itemTotal = expectedProducts.Sum(product => Decimal.Parse(product.Price.Split('$')[1]));

            var expectedItemTotal = $"Item total: ${itemTotal}";
            var displayedItemTotal = checkoutPage.GetItemTotal();
            Assert.Equal(expectedItemTotal, displayedItemTotal);

            var tax = Math.Round(itemTotal * 0.08m,2);

            var expectedTax = $"Tax: ${tax}";
            var displayedTax = checkoutPage.GetTax();
            Assert.Equal(expectedTax, displayedTax);

            var total = itemTotal + tax;

            var expectedTotal = $"Total: ${total}";
            var displayedTotal = checkoutPage.GetTotal();
            Assert.Equal(expectedTotal, displayedTotal);

        }


        [When("user finishes checkout process")]
        public void WhenUserFinishesCheckoutProcess()
        {
            new CheckoutPage(_driver, _logger).ClickFinishBtn();
        }

        [Then("user should be navigated to the thank you page")]
        public void ThenUserShouldBeNavigatedToTheThankYouPage()
        {
            var checkoutPage = new CheckoutPage(_driver, _logger);

            WaitUtils.WaitUntilUrlChangesTo(_driver, "https://www.saucedemo.com/checkout-complete.html", TimeSpan.FromSeconds(2));

            var displayedHeadertext = checkoutPage.GetHeaderText();
            var displayedSubHeaderText = checkoutPage.GetSubHeaderText();

            Assert.Equal("Thank you for your order!", displayedHeadertext);
            Assert.Equal("Your order has been dispatched, and will arrive just as fast as the pony can get there!", displayedSubHeaderText);

        }


    }
}
