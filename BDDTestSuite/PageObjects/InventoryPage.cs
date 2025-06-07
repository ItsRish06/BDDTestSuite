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

    }
}
