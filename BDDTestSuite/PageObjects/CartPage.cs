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
    public class CartPage
    {
        readonly IWebDriver _driver;
        readonly ILogger _logger;

        public CartPage(IWebDriver driver, ILogger logger)
        {
            _driver = driver;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the details of a product based on its position in the cart display.
        /// </summary>
        /// <param name="productNumber">The one-based index representing the product's position in the cart.</param>
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
        /// Retrieves the details of all products currently in the cart.
        /// </summary>
        /// <returns>A list of <see cref="Product"/> objects containing details of all products in the cart.</returns>
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
        /// Removes a product from the cart based on its position.
        /// </summary>
        /// <param name="productNumber">The one-based index representing the product's position in the cart.</param>
        /// <exception cref="Exception">Thrown if unable to remove the product.</exception>
        public void RemoveProduct(string productNumber)
        {
            try
            {
                WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.XPath($"(//div[@class='cart_item'])[{productNumber}]//button[text()='Remove']"), TimeSpan.FromSeconds(2))
                    .Click();
            }
            catch (Exception ex) 
            {
                _logger.Error(ex, "Failed to remove product - {productNumber}", productNumber);
                throw;
            }
            
        }

        /// <summary>
        /// Removes all products from the cart.
        /// </summary>
        /// <exception cref="Exception">Thrown if unable to remove all products.</exception>
        public void RemoveAllProducts()
        {
            try
            {
                var productCount = WaitUtils
                .WaitUntilAllElementsAreVisible(_driver, By.XPath("//div[@class='cart_item']"), TimeSpan.FromSeconds(2))
                .Count;

                for (int i = 0; i < productCount; i++)
                {
                    RemoveProduct("1");
                }
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to remove all the products from the cart");
                throw;
            }     

        }

        /// <summary>
        /// Gets the number of products currently in the cart.
        /// </summary>
        /// <returns>The count of products in the cart.</returns>
        public int GetNumberOfProducts()
        {
            try
            {
                return WaitUtils
                    .WaitUntilAllElementsAreVisible(_driver, By.XPath("//div[@class='cart_item']"), TimeSpan.FromSeconds(2))
                    .Count;
                
            }
            catch (WebDriverTimeoutException)
            {
                return 0;
            }
        }

        /// <summary>
        /// Clicks the "Continue Shopping" button to return to the inventory page.
        /// </summary>
        /// <exception cref="Exception">Thrown if unable to click the button.</exception>
        public void ClickContinueShoppingBtn()
        {
            try
            {
                WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.Id("continue-shopping"), TimeSpan.FromSeconds(2))
                    .Click();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to click on [Continue Shopping] button");
                throw;
            }
        }

        /// <summary>
        /// Clicks the "Checkout" button to proceed to the checkout page.
        /// </summary>
        /// <exception cref="Exception">Thrown if unable to click the button.</exception>
        public void ClickCheckoutBtn()
        {
            try
            {
                WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.Id("checkout"), TimeSpan.FromSeconds(2))
                    .Click();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to click on [Checkout] button");
                throw;
            }
        }

    }
}
