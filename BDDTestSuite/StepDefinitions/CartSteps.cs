using BDDTestSuite.Models;
using BDDTestSuite.PageObjects;
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
    public class CartSteps
    {
        IWebDriver _driver;
        ILogger _logger;
        IConfigurationRoot _config;
        ScenarioContext _scenarioContext;

        public CartSteps(Services services, ScenarioContext scenarioContext)
        {
            _config = services.Config;
            _logger = services.Logger;
            _driver = services.Driver;
            _scenarioContext = scenarioContext;
        }


        [Then("user should see the added product\\(s) on the cart page")]
        public void ThenUserShouldSeeTheAddedProductOnTheCartPage()
        {
            var addedProducts = _scenarioContext.Get<List<Product>>("CartAddedProducts");
            var cartPage = new CartPage(_driver, _logger);
            var productsOnCart = cartPage.GetAllProductDetails();

            _logger.Information("Checking if the added products are visible in the cart page");

            Assert.Equal(
                addedProducts.Select(product => product.Name),
                productsOnCart.Select(product => product.Name)
                );
            Assert.Equal(
                addedProducts.Select(product => product.Description), 
                productsOnCart.Select(product => product.Description)
                );
            Assert.Equal(
                addedProducts.Select(product => product.Price), 
                productsOnCart.Select(product => product.Price)
                );
        }


        [When("user removes the product\\(s) from the cart")]
        public void WhenUserRemovesTheProductFromTheCart()
        {
            var cartPage = new CartPage( _driver, _logger);

            _logger.Information("Removing all the products from cart");

            cartPage.RemoveAllProducts();
        }

        [Then("there should not be any products on the cart page")]
        public void ThenThereShouldNotBeAnyProductsOnTheCartPage()
        {
            var cartPage = new CartPage(_driver, _logger);

            var numberOfProducts = cartPage.GetNumberOfProducts();

            _logger.Information("Checking if all the products are removed from the cart page");

            Assert.True( 
                numberOfProducts == 0,
                $"All the products are not removed from the cart page. Current number of products on the cart pafe - {numberOfProducts}"
                );

        }

        [When("user removes some products from the cart page")]
        public void WhenUserRemovesSomeProductsFromTheCartPage()
        {
            var cartPage = new CartPage(_driver, _logger);

            _logger.Information("Removing products 1 and 2 from the cart");

            //Products are removed at position.
            //After removing product at pos 1, product 2 moves to pos 1 and product 3 to pos 2
            //That means products 1 and 3 are removed.
            cartPage.RemoveProduct("1");
            cartPage.RemoveProduct("2");

            var addedProducts = _scenarioContext.Get<List<Product>>("CartAddedProducts");

            addedProducts.RemoveAt(0);
            addedProducts.RemoveAt(1);

            _scenarioContext["RemainingCartProducts"] = addedProducts;

        }

        [Then("the cart should contain product\\(s) that are not removed")]
        public void ThenThereTheCartShouldContainProductSThatAreNotRemoved()
        {
            var remainingProducts = _scenarioContext.Get<List<Product>>("RemainingCartProducts");
            var cartPage = new CartPage(_driver, _logger);
            var productsOnCart = cartPage.GetAllProductDetails();

            _logger.Information("Checking if only the products that are not removed are visibile on the cart page");

            Assert.Equal(
                remainingProducts.Select(product => product.Name),
                productsOnCart.Select(product => product.Name)
                );
            Assert.Equal(
                remainingProducts.Select(product => product.Description),
                productsOnCart.Select(product => product.Description)
                );
            Assert.Equal(
                remainingProducts.Select(product => product.Price),
                productsOnCart.Select(product => product.Price)
                );
        }

    }
}
