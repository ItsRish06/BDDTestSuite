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

        public InventorySteps(Services services, ScenarioContext scenarioContext)
        {
            _config = services.Config;
            _logger = services.Logger;
            _driver = services.Driver;
            _scenarioContext = scenarioContext;
        }

        [When("user clicks on the title link for product {string}")]
        public void WhenUserClickOnTheTitleLinkForProduct(string productNumber)
        {
            var inventoryPage = new InventoryPage(_driver,_logger);

            var productDetail = inventoryPage.GetProductDetail(productNumber);

            _scenarioContext["ProductDetail"] = productDetail;

            _logger.Information("Clicking on the product tile link for product - {name}", productDetail.Name);

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

            _logger.Information("Checking if the correct product details are displayed on the page");
            
            Assert.Equal(expectedProductDetail.Name, displayedProductDetail.Name);
            Assert.Equal(expectedProductDetail.Description, displayedProductDetail.Description);
            Assert.Equal(expectedProductDetail.Price, displayedProductDetail.Price);
        }

        [When("user selects the option to sort the products by {string} in {string} order")]
        public void WhenUserSelectsTheOptionToSortTheProductsByInOrder(string sortBy, string sortOrder)
        {
            var inventoryPage = new InventoryPage(_driver, _logger);

            _logger.Information("Selecting option to sort by {sortBy} in order {sortOrder}", sortBy, sortOrder);

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

            _logger.Information("Checking if the products are sorted by {sortBy} in {sortOrder} order", sortBy, sortOrder);

            Assert.Equal(
                    expectedSortedProducts.Select(keySelector),
                    displayedProducts.Select(keySelector)
                    );
        }

        [When("user adds a product to the cart")]
        public void WhenUserAddsAProductToTheCart()
        {
            var inventoryPage = new InventoryPage( _driver, _logger);

            _logger.Information("Adding product 1 to cart");

            inventoryPage.AddProductToCart("1");

            var productDetail = inventoryPage.GetProductDetail("1");

            _scenarioContext["CartAddedProducts"] = new List<Product>() { productDetail };

        }

        [When("user navigates to the cart page")]
        public void WhenUserNavigatesToTheCartPage()
        {
            _logger.Information("Clicking on the cart icon and navigating to the cart page");
            
            new InventoryPage(_driver, _logger)
                .ClickCartBtn();

            WaitUtils.WaitUntilUrlChangesTo(_driver, "https://www.saucedemo.com/cart.html", TimeSpan.FromSeconds(4));
        }

        

        [When("user adds multiple products to cart")]
        public void WhenUserAddsMultipleProductsToCart()
        {
            var inventoryPage = new InventoryPage(_driver, _logger);

            _logger.Information("Adding products 1, 3, 5 to the cart");

            inventoryPage.AddProductToCart("1");
            inventoryPage.AddProductToCart("3");
            inventoryPage.AddProductToCart("5");

            var cardAddedProducts = new List<Product>
            {
                inventoryPage.GetProductDetail("1"),
                inventoryPage.GetProductDetail("3"),
                inventoryPage.GetProductDetail("5")
            };

            _scenarioContext["CartAddedProducts"] = cardAddedProducts;

        }

        [Then("the [Add to Cart] button text for those products should change to [Remove]")]
        public void ThenTheAddToCartButtonTextForThoseProductsShouldChangeToRemove()
        {
            var inventoryPage = new InventoryPage(_driver, _logger);

            _logger.Information("Checking if the button text for the products that are added to the cart has changed to [Remove]");

            Assert.Equal("Remove", inventoryPage.GetProductCartBtnText("1"));
            Assert.Equal("Remove", inventoryPage.GetProductCartBtnText("3"));
            Assert.Equal("Remove", inventoryPage.GetProductCartBtnText("5"));
        }

        [When("user navigates back to inventory page")]
        public void WhenUserNavigatesBackToInventoryPage()
        {
            var cartPage = new CartPage(_driver, _logger);

            _logger.Information("Clicking the [Continue Shopping] button on cart page and navigating to the inventory page");

            cartPage.ClickContinueShoppingBtn();

            WaitUtils.WaitUntilUrlChangesTo(_driver, "https://www.saucedemo.com/inventory.html", TimeSpan.FromSeconds(3));
        }

        [When("user removes the added products from inventory page")]
        public void WhenUserRemovesTheAddedProductsFromInventoryPage()
        {
            var inventoryPage = new InventoryPage(_driver, _logger);

            _logger.Information("Removing products 1, 3, 5 from the cart by clicking on [Remove] button on inventory page");

            //Clicks on [Add to Cart]/[Remove] button
            inventoryPage.AddProductToCart("1");
            inventoryPage.AddProductToCart("3");
            inventoryPage.AddProductToCart("5");

        }



    }
}
