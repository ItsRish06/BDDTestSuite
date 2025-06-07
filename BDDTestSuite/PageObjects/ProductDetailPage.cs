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
    public class ProductDetailPage
    {
        IWebDriver _driver;
        ILogger _logger;
        public ProductDetailPage(IWebDriver driver, ILogger logger) 
        { 
            _driver = driver;
            _logger = logger;
        }

        public Product GetProductDetail()
        {
            try
            {
                return new Product()
                {
                    Name = WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//div[@data-test='inventory-item-name']"), TimeSpan.FromSeconds(5))
                    .Text,
                    Description = WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//div[@data-test='inventory-item-desc']"), TimeSpan.FromSeconds(5))
                    .Text,
                    Price = WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//div[@data-test='inventory-item-price']"), TimeSpan.FromSeconds(5))
                    .Text
                };
            }
            catch (Exception ex) 
            {
                _logger.Error(ex, "Failed to get the product details");
                throw;
            }
            
        }


    }
}
