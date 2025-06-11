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
        public CheckoutPage(IWebDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

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

        public string GetHeaderText()
        {
            try
            {
                return WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.ClassName("complete-header"), TimeSpan.FromSeconds(2))
                    .Text;
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to get the header text from checkout complete page");
                throw;
            }
        }

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
