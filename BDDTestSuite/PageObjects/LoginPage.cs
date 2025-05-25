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
    public class LoginPage
    {
        ILogger _logger;
        IWebDriver _driver;

        public LoginPage(IWebDriver driver, ILogger logger)
        {
            _driver = driver ?? throw new ArgumentNullException(nameof(driver));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void EnterCredentials(string username, string password)
        {
            ArgumentNullException.ThrowIfNull(username, nameof(username));
            ArgumentNullException.ThrowIfNull(password, nameof(password));

            try
            {
                var usernameField = WaitUtils.WaitUntilElementIsClickable(_driver, By.Id("user-name"), TimeSpan.FromSeconds(5));
                var passwordField = WaitUtils.WaitUntilElementIsClickable(_driver, By.Id("password"), TimeSpan.FromSeconds(5));

                usernameField.Clear();
                usernameField.SendKeys(username);
                passwordField.Clear();
                passwordField.SendKeys(password);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to enter username or password field on the login page.");
                throw;
            }
        }

        public void SubmitLogin()
        {
            try
            {
                _driver.FindElement(By.Id("login-button")).Click();
            }
            catch (Exception ex) 
            {
                _logger.Error(ex, "Failed to click on login form submit button.");
                throw;
            }
        }

        public string GetFormError()
        {

            try
            {
                var errorMessage = WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//h3[@data-test='error']"), TimeSpan.FromSeconds(5))
                    .Text;
                return errorMessage;

            }catch(Exception ex)
            {
                _logger.Information(ex, "Failed to get the login form error message.");
                throw;
            }
        }
       

    }
}