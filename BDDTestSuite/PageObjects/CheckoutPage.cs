using BDDTestSuite.Models;
using BDDTestSuite.Utils;
using OpenQA.Selenium;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.PageObjects
{
    public class CheckoutPage
    {
        readonly IWebDriver _driver;
        readonly ILogger _logger;
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckoutPage"/> class.
        /// </summary>
        /// <param name="driver">The Selenium WebDriver instance.</param>
        /// <param name="logger">The logger instance for logging errors and information.</param>
        public CheckoutPage(IWebDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        /// <summary>
        /// Enters the first name into the checkout form.
        /// </summary>
        /// <param name="firstName">The first name to enter.</param>
        /// <exception cref="Exception">Thrown if unable to enter the first name.</exception>
        public void EnterFirstName(string firstName)
        {
            try
            {
                var inputElement = WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.Id("first-name"), TimeSpan.FromSeconds(2));
                inputElement.Clear();
                inputElement.SendKeys(firstName);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to enter first name on checkout form");
                throw;
            }
        }

        /// <summary>
        /// Enters the last name into the checkout form.
        /// </summary>
        /// <param name="lastName">The last name to enter.</param>
        /// <exception cref="Exception">Thrown if unable to enter the last name.</exception>
        public void EnterLastName(string lastName)
        {
            try
            {
                var inputElement = WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.Id("last-name"), TimeSpan.FromSeconds(2));
                inputElement.Clear();
                inputElement.SendKeys(lastName);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to enter last name on checkout form");
                throw;
            }
        }

        /// <summary>
        /// Enters the zip/postal code into the checkout form.
        /// </summary>
        /// <param name="zip">The zip or postal code to enter.</param>
        /// <exception cref="Exception">Thrown if unable to enter the zip code.</exception>
        public void EnterZip(string zip)
        {
            try
            {
                var inputElement = WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.Id("postal-code"), TimeSpan.FromSeconds(2));
                inputElement.Clear();
                inputElement.SendKeys(zip);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to enter zip on checkout form");
                throw;
            }
        }

        /// <summary>
        /// Clicks the "Continue" button on the checkout form page.
        /// </summary>
        /// <exception cref="Exception">Thrown if unable to click the button.</exception>
        public void ClickContinueBtn()
        {
            try
            {
                WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.Id("continue"), TimeSpan.FromSeconds(2))
                    .Click();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to click on [Continue] button");
                throw;
            }
        }

        /// <summary>
        /// Retrieves the error message displayed on the checkout form, if any.
        /// </summary>
        /// <returns>The error message text.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve the error message.</exception>
        public string GetFormErrorMessage()
        {
            try
            {
                return WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//h3[@data-test='error']"), TimeSpan.FromSeconds(2))
                    .Text;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get the form error");
                throw;
            }
        }

        /// <summary>
        /// Retrieves the details of a product based on its position in the checkout page.
        /// </summary>
        /// <param name="productNumber">The one-based index representing the product's position in the checkout page.</param>
        /// <returns>An object containing the details of the product at the specified position.</returns>
        /// <exception cref="ArgumentNullException">Thrown if productNumber is null or empty.</exception>
        /// <exception cref="Exception">Thrown if unable to retrieve product details.</exception>
        public Product GetProductDetail(string productNumber)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(productNumber, nameof(productNumber));

            try
            {
                return new Product()
                {
                    Name = WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath($"(//div[@class='inventory_item_name'])[{productNumber}]"), TimeSpan.FromSeconds(2))
                    .Text,
                    Description = WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath($"(//div[@class='inventory_item_desc'])[{productNumber}]"), TimeSpan.FromSeconds(2))
                    .Text,
                    Price = WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath($"(//div[@class='inventory_item_price'])[{productNumber}]"), TimeSpan.FromSeconds(2))
                    .Text,
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get the details for product number {productNumber}", productNumber);
                throw;
            }

        }

        /// <summary>
        /// Retrieves the details of all products listed on the checkout page.
        /// </summary>
        /// <returns>A list of <see cref="Product"/> objects containing details of all products.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve product details.</exception>
        public List<Product> GetAllProductDetails()
        {
            try
            {
                var numberOfProducts = WaitUtils
                .WaitUntilAllElementsAreClickable(_driver, By.XPath("//div[@class='cart_item']"), TimeSpan.FromSeconds(5))
                .Count;

                var products = new List<Product>();

                for (int i = 1; i <= numberOfProducts; i++)
                {
                    products.Add(GetProductDetail(i.ToString()));
                }

                return products;

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get all product details");
                throw;
            }
        }

        /// <summary>
        /// Returns the total cost of all items listed on the checkout page before taxes and additional charges.
        /// </summary>
        /// <returns>The total price of all items.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve the item total.</exception>
        public string GetItemTotal()
        {
            try
            {
                return WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//div[@data-test='subtotal-label']"), TimeSpan.FromSeconds(2))
                    .Text;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get item total");
                throw;
            }
        }

        /// <summary>
        /// Gets the total tax applied to all products in the cart or checkout page.
        /// </summary>
        /// <returns>The total amount of tax applied to the current order.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve the tax amount.</exception>
        public string GetTax()
        {
            try
            {
                return WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//div[@data-test='tax-label']"), TimeSpan.FromSeconds(2))
                    .Text;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get item total");
                throw;
            }
        }

        /// <summary>
        /// Gets the total amount payable on the checkout page, including item costs, taxes, and any applicable fees.
        /// </summary>
        /// <returns>The final total amount for the current order.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve the total amount.</exception>
        public string GetTotal()
        {
            try
            {
                return WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//div[@data-test='total-label']"), TimeSpan.FromSeconds(2))
                    .Text;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get total amount");
                throw;
            }
        }

        /// <summary>
        /// Clicks the "Finish" button on the checkout page to complete the purchase.
        /// </summary>
        /// <exception cref="Exception">Thrown if unable to click the button.</exception>
        public void ClickFinishBtn()
        {
            try
            {
                WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.Id("finish"), TimeSpan.FromSeconds(2))
                    .Click();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to click on [Finish] button");
                throw;
            }
        }

        /// <summary>
        /// Retrieves the header text displayed on the purchase confirmation page after a successful transaction.
        /// </summary>
        /// <returns>A string containing the header message shown on the purchase complete page.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve the header text.</exception>
        public string GetHeaderText()
        {
            try
            {
                return WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.ClassName("complete-header"), TimeSpan.FromSeconds(2))
                    .Text;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get the header text from checkout complete page");
                throw;
            }
        }

        /// <summary>
        /// Gets the sub header text from the purchase complete page.
        /// </summary>
        /// <returns>The sub header text.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve the sub header text.</exception>
        public string GetSubHeaderText()
        {
            try
            {
                return WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.ClassName("complete-text"), TimeSpan.FromSeconds(2))
                    .Text;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get the sub header text from checkout complete page");
                throw;
            }
        }
    }
}
