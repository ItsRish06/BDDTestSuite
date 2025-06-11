using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            catch (WebDriverTimeoutException ex) 
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
            catch (WebDriverTimeoutException ex) 
            {
                throw new WebDriverTimeoutException($"Timed out after {timeout} waiting for element located by {by} to be clickable", ex);
            }
            
        }

        public static bool WaitUntilUrlChangesTo(IWebDriver driver, string url, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(driver, nameof(driver));
            ArgumentNullException.ThrowIfNullOrEmpty(url, nameof(url));

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

        public static bool WaitUntilUrlContains(IWebDriver driver, string urlString, TimeSpan timeout)
        {
            ArgumentNullException.ThrowIfNull(driver, nameof(driver));
            ArgumentNullException.ThrowIfNullOrEmpty(urlString, nameof(urlString));

            var wait = new WebDriverWait(driver, timeout);
            try
            {
                return wait.Until(d =>
                {
                    string currentUrl = d.Url;
                    if (currentUrl.Contains(urlString))
                        return true;

                    return false;
                });
            }
            catch (Exception ex)
            {
                throw new WebDriverTimeoutException($"The current Url did not contain the string - {urlString}. Current URL: {driver.Url}", ex);
            }

        }

        public static IList<IWebElement> WaitUntilAllElementsAreClickable(IWebDriver driver, By by, TimeSpan timeout)
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
                    var elements = d.FindElements(by);
                    if (elements.Count > 0 && elements.All(element => element.Displayed && element.Enabled))
                        return elements;

                    return null;

                });
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new WebDriverTimeoutException($"Timed out after {timeout} waiting for all elements located by {by} to be clickable", ex);
            }

        }

        public static IList<IWebElement> WaitUntilAllElementsAreVisible(IWebDriver driver, By by, TimeSpan timeout)
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
                    var elements = d.FindElements(by);
                    if (elements.Count > 0 && elements.All(element => element.Displayed))
                        return elements;

                    return null;

                });
            }
            catch (WebDriverTimeoutException ex)
            {
                throw new WebDriverTimeoutException($"Timed out after {timeout} waiting for all elements located by {by} to be clickable", ex);
            }

        }
    }
}
