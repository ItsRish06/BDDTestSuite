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

        /// <summary>
        /// Enters the provided username and password into the login form fields.
        /// </summary>
        /// <param name="username">The username to enter.</param>
        /// <param name="password">The password to enter.</param>
        /// <exception cref="ArgumentNullException">Thrown if username or password is null.</exception>
        /// <exception cref="Exception">Thrown if unable to interact with the login fields.</exception>
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

        /// <summary>
        /// Clicks the submit button to attempt login.
        /// </summary>
        /// <exception cref="Exception">Thrown if unable to click the submit button.</exception>
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

        /// <summary>
        /// Retrieves the error message displayed on the login form, if any.
        /// </summary>
        /// <returns>The error message text.</returns>
        /// <exception cref="Exception">Thrown if unable to retrieve the error message.</exception>
        public string GetFormError()
        {

            try
            {
                var errorMessage = WaitUtils
                    .WaitUntilElementIsVisible(_driver, By.XPath("//h3[@data-test='error']"), TimeSpan.FromSeconds(5))
                    .Text;
                return errorMessage;

            }
            catch (Exception ex)
            {
                _logger.Information(ex, "Failed to get the login form error message.");
                throw;
            }
        }
       

    }
}