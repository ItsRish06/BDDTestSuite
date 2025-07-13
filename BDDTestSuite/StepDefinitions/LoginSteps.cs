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
    public class LoginSteps
    {
        ILogger _logger;
        IConfigurationRoot _config;
        IWebDriver _driver;

        public LoginSteps(Services services) 
        {
            _config = services.Config;
            _logger = services.Logger;
            _driver = services.Driver;
        }

        [Given("user navigates to the login page")]
        public void GivenUserNavigatesToTheLoginPage()
        {
            var url = _config["url"];

            _logger.Information("Navigating to the login page: {url}", url);

            BrowserUtil.NavigateToUrl(_driver, url);
        }

        [When("user enters valid username and password")]
        public void WhenUserEntersValidUsernameAndPassword()
        {
            var username = _config["credentials:username:standard-user"];
            var password = _config["credentials:password:valid"];

            ArgumentNullException.ThrowIfNullOrEmpty(username, nameof(username));
            ArgumentNullException.ThrowIfNullOrEmpty(password, nameof(password));

            var loginPage = new LoginPage(_driver, _logger);

            _logger.Information("Entering username and password");

            loginPage.EnterCredentials(username, password);
            loginPage.SubmitLogin();
            
        }

        [Then("user should be redirected to the inventory page")]
        public void ThenUserShouldBeRedirectedToTheInventoryPage()
        {
            WaitUtils.WaitUntilUrlChangesTo(_driver, _config["url"] + "inventory.html", TimeSpan.FromSeconds(10));
        }

        [When("user enters invalid username and password")]
        public void WhenUserEntersInvalidUsernameAndPassword()
        {
            var username = "invalid_user";
            var password = "invalid_password";

            var loginPage = new LoginPage(_driver, _logger);

            _logger.Information("Entering invalid username and password");

            loginPage.EnterCredentials(username, password);
            loginPage.SubmitLogin();
        }

        [Then("user should see an error message")]
        public void ThenUserShouldSeeAnErrorMessage()
        {
            var loginPage = new LoginPage(_driver,_logger);
            var displayedError = loginPage.GetFormError();


            Assert.Equal("Epic sadface: Username and password do not match any user in this service", displayedError);
        }

        [Then("user should remain on the login page")]
        public void ThenUserShouldRemainOnTheLoginPage()
        {
            Thread.Sleep(5000);

            var expectedUrl = _config["url"];
            var displayedUrl = _driver.Url;

            ArgumentNullException.ThrowIfNullOrEmpty(expectedUrl, nameof(expectedUrl));

            Assert.Equal(expectedUrl, displayedUrl);
        }

        [When("user leaves username and password fields empty")]
        public void WhenUserLeavesUsernameAndPasswordFieldsEmpty()
        {
            var username = "";
            var password = "";

            var loginPage = new LoginPage(_driver, _logger);

            _logger.Information("Keeping the username and password field empty");

            loginPage.EnterCredentials(username, password);
            loginPage.SubmitLogin();
        }

        [When("user enters locked out username and password")]
        public void WhenUserEntersLockedOutUsernameAndPassword()
        {
            var username = _config["credentials:username:locked-out-user"];
            var password = _config["credentials:password:valid"];

            ArgumentNullException.ThrowIfNullOrEmpty(username, nameof(username));
            ArgumentNullException.ThrowIfNullOrEmpty(password, nameof(password));

            var loginPage = new LoginPage(_driver, _logger);

            _logger.Information("Entering username and password for a locked out user");

            loginPage.EnterCredentials(username, password);
            loginPage.SubmitLogin();
        }


        [Then("user should see an error message for {string} credentials")]
        public void ThenUserShouldSeeAnErrorMessageFor(string errorType)
        {
            var loginPage = new LoginPage(_driver, _logger);
            var displayedError = loginPage.GetFormError();

            var expectedError = errorType.ToLower() switch
            {
                "invalid" => "Epic sadface: Username and password do not match any user in this service",
                "empty" => "Epic sadface: Username is required",
                "locked out" => "Epic sadface: Sorry, this user has been locked out.",
                _ => throw new InvalidOperationException($"Invalid error type : {errorType}")
            };

            _logger.Information("Checking if error message is displayed for {errorType} credential", errorType);

            Assert.Equal(expectedError, displayedError);
        }


    }
}
