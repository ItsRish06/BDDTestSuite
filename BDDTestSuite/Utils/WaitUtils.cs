using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.Utils
{ 
    public class WaitUtils
    {
        public static IWebElement WaitUntilElementIsVisible(IWebDriver driver, By by, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(driver, nameof(driver));
            ArgumentNullException.ThrowIfNull(by, nameof(by));

            var wait = new WebDriverWait(driver, timeout);

            wait.IgnoreExceptionTypes(
                typeof(StaleElementReferenceException),
                typeof(NoSuchElementException)
                );

            try
            {
                return wait.Until(d =>
                {
                    IWebElement element = d.FindElement(by);
                    if (!element.Displayed)
                        return null;

                    return element;
                });
            }
            catch (Exception ex) 
            { 
                throw new WebDriverTimeoutException($"Element {by} is not visible even after {timeout}",ex);
            }
            
        }
        public static IWebElement WaitUntilElementIsClickable(IWebDriver driver, By by, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(driver, nameof(driver));
            ArgumentNullException.ThrowIfNull(by, nameof(by));

            var wait = new WebDriverWait(driver, timeout);

            wait.IgnoreExceptionTypes(
                typeof(ElementClickInterceptedException),
                typeof(StaleElementReferenceException), 
                typeof(NoSuchElementException)
                );

            try
            {
                return wait.Until(d =>
                {
                    IWebElement element = d.FindElement(by);
                    if (!element.Displayed || !element.Enabled)
                        return null;

                    return element;
                });
            }
            catch (Exception ex) 
            {
                throw new WebDriverTimeoutException($"Element {by} still not clickable after {timeout}", ex);
            }
            
        }

        public static bool WaitUntilUrlChangesTo(IWebDriver driver, string url, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(url, nameof(url));
            ArgumentNullException.ThrowIfNull(timeout, nameof(timeout));

            var wait = new WebDriverWait(driver, timeout);
            try
            {
                return wait.Until(d =>
                {
                    string currentUrl = d.Url;
                    if (currentUrl.Equals(url))
                        return true;

                    return false;
                });
            }
            catch (Exception ex) 
            {
                throw new WebDriverTimeoutException($"URL did not change to {url} after {timeout}. Current URL: {driver.Url}", ex);
            }
            

        }
    }
}
