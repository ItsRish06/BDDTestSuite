using BDDTestSuite.Models;
using BDDTestSuite.PageObjects;
using BDDTestSuite.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using OpenQA.Selenium;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.StepDefinitions
{
    [Binding]
    public class InventorySteps
    {
        IWebDriver _driver;
        ILogger _logger;
        IConfigurationRoot _config;
        ScenarioContext _scenarioContext;

        public InventorySteps(IWebDriver driver, ILogger logger, IConfigurationRoot config ,ScenarioContext scenarioContext) 
        {
            _driver = driver;
            _logger = logger;   
            _config = config;
            _scenarioContext = scenarioContext;
        }

        [When("user clicks on the title link for product {string}")]
        public void WhenUserClickOnTheTitleLinkForProduct(string productNumber)
        {
            var inventoryPage = new InventoryPage(_driver,_logger);

            var productDetail = inventoryPage.GetProductDetail(productNumber);

            _scenarioContext["ProductDetail"] = productDetail;

            inventoryPage.ClickOnProductLink(productNumber);
        }

        [Then("user should be redirected to inventory item detail page")]
        public void ThenUserShouldBeRedirectedToInventoryItemDetailPage()
        {
            var baseUrl = _config["url"];

 //           WaitUtils.WaitUntilUrlContains(_driver, baseUrl + "/inventory-item.html?", TimeSpan.FromSeconds(2));
        }

        [Then("user should see the product details")]
        public void ThenUserShouldSeeTheProductDetails()
        {

            var expectedProductDetail = _scenarioContext.Get<Product>("ProductDetail");

            var productDetailPage = new ProductDetailPage(_driver,_logger);

            var displayedProductDetail = productDetailPage.GetProductDetail();
            
            Assert.Equal(expectedProductDetail.Name, displayedProductDetail.Name);
            Assert.Equal(expectedProductDetail.Description, displayedProductDetail.Description);
            Assert.Equal(expectedProductDetail.Price, displayedProductDetail.Price);
        }

        [When("user selects the option to sort the products by {string} in {string} order")]
        public void WhenUserSelectsTheOptionToSortTheProductsByInOrder(string sortBy, string sortOrder)
        {
            var inventoryPage = new InventoryPage(_driver, _logger);

            inventoryPage.SortProductsBy(sortBy, sortOrder);
        }

        [Then("the products on the inventory page should get sorted by {string} in {string} order")]
        public void ThenTheProductsOnTheInventoryPageShouldGetSortedByInOrder(string sortBy, string sortOrder)
        {
            var inventoryPage = new InventoryPage(_driver, _logger);
            var displayedProducts = inventoryPage.GetAllProductDetails();

            Func<Product, IComparable> keySelector = sortBy switch
            {
                "name" => p => p.Name,
                "price" => p => double.Parse(p.Price.Split('$')[1]),
                _ => throw new ArgumentException($"Invalid sortBy: {sortBy}")
            };

            var expectedSortedProducts = sortOrder switch
            {
                "ascending" => displayedProducts.OrderBy(keySelector),
                "descending" => displayedProducts.OrderByDescending(keySelector),
                _ => throw new ArgumentException($"Invalid sortOrder: {sortOrder}")
            };

            Assert.Equal(
                    expectedSortedProducts.Select(keySelector),
                    displayedProducts.Select(keySelector)
                    );
        }

    }
}
