using BDDTestSuite.Models;
using BDDTestSuite.Utils;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.PageObjects
{
    public class InventoryPage
    {
        readonly IWebDriver _driver;
        readonly ILogger _logger;

        public InventoryPage(IWebDriver driver, ILogger logger) 
        {
            _driver = driver;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves the details of a product based on its position in the inventory list.
        /// </summary>
        /// <param name="productNumber">The one-based index representing the product's position.</param>
        /// <returns>A <see cref="Product"/> object containing the product's details.</returns>
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
                    .WaitUntilElementIsVisible(_driver, By.XPath($"(//div[@class='inventory_item_name '])[{productNumber}]"), TimeSpan.FromSeconds(2))
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
        /// Retrieves the details of all products listed on the inventory page.
        /// </summary>
        /// <returns>A list of <see cref="Product"/> objects containing details of all products.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve product details.</exception>
        public List<Product> GetAllProductDetails()
        {

            try
            {
                var numberOfProducts = WaitUtils
                .WaitUntilAllElementsAreClickable(_driver, By.XPath("//div[@class='inventory_item']"), TimeSpan.FromSeconds(5))
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
        /// Clicks on the product link to navigate to its details page.
        /// </summary>
        /// <param name="productNumber">The one-based index representing the product's position.</param>
        /// <exception cref="ArgumentNullException">Thrown if productNumber is null or empty.</exception>
        /// <exception cref="Exception">Thrown if unable to click the product link.</exception>
        public void ClickOnProductLink(string productNumber)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(productNumber, nameof(productNumber));

            try
            {
                var titleLinkElement = WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.XPath($"(//div[@class='inventory_item_label'])[{productNumber}]/a"), TimeSpan.FromSeconds(2));

                ElementUtils.JSClick(_driver, titleLinkElement);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to click on title link for product number {productNumber}", productNumber);
                throw;
            }
            
        }

        /// <summary>
        /// Sorts the products by the specified field and order.
        /// </summary>
        /// <param name="sortBy">The field to sort by ("name" or "price").</param>
        /// <param name="sortOrder">The order to sort ("ascending" or "descending").</param>
        /// <exception cref="ArgumentNullException">Thrown if sortBy or sortOrder is null or empty.</exception>
        /// <exception cref="ArgumentException">Thrown if an invalid combination is provided.</exception>
        public void SortProductsBy(string sortBy, string sortOrder)
        {

            ArgumentNullException.ThrowIfNullOrEmpty(sortBy, nameof(sortBy));
            ArgumentNullException.ThrowIfNullOrEmpty(sortOrder, nameof(sortOrder));

            var sortOrderValue = (sortOrder, sortBy) switch
            {
                ("ascending", "name") => "az",
                ("ascending", "price") => "lohi",
                ("descending", "name") => "za",
                ("descending", "price") => "hilo",
                _ => throw new ArgumentException($"Invalid combination: sortOrder='{sortOrder}', sortBy='{sortBy}'")
            };
            try
            {
                var selectElement = WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.XPath("//select[@class='product_sort_container']"), TimeSpan.FromSeconds(2));

                var select = new SelectElement(selectElement);

                select.SelectByValue(sortOrderValue);

            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to sort the products by {sortby} in {sortOrder} order", sortBy, sortOrder);
            }

        }

        /// <summary>
        /// Adds a product to the cart based on its position in the inventory list.
        /// </summary>
        /// <param name="productNumber">The one-based index representing the product's position.</param>
        /// <exception cref="ArgumentNullException">Thrown if productNumber is null or whitespace.</exception>
        /// <exception cref="Exception">Thrown if unable to add the product to the cart.</exception>
        public void AddProductToCart(string productNumber)
        {
            ArgumentNullException.ThrowIfNullOrWhiteSpace(productNumber, nameof(productNumber));

            try 
            {
                WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.XPath($"(//div[@class='inventory_item'])[{productNumber}]//button"), TimeSpan.FromSeconds(2))
                    .Click();
            }
            catch(Exception ex)
            {
                _logger.Error(ex, "Failed to click on 'Add to Cart' button for product {productNumber}", productNumber);
                throw;
            }
   
        }

        /// <summary>
        /// Clicks the cart button to navigate to the cart page.
        /// </summary>
        /// <exception cref="Exception">Thrown if unable to click the cart button.</exception>
        public void ClickCartBtn()
        {
            try
            {
                WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.XPath("//a[@class='shopping_cart_link']"), TimeSpan.FromSeconds(2))
                    .Click();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to navigate to the cart page");
                throw;
            }
        }

        /// <summary>
        /// Retrieves the text of the cart button for a specific product.
        /// </summary>
        /// <param name="productNumber">The one-based index representing the product's position.</param>
        /// <returns>The text displayed on the product's cart button.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve the button text.</exception>
        public string GetProductCartBtnText(string productNumber)
        {
            try
            {
                return WaitUtils
                    .WaitUntilElementIsClickable(_driver, By.XPath($"(//div[@class='inventory_item'])[{productNumber}]//button"), TimeSpan.FromSeconds(2))
                    .Text;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get product cart button text for product number - {productNumber}", productNumber);
                throw;
            }
        }

    }
}
