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
    /// <summary>
    /// Utility class for explicit wait operations on web elements and URLs.
    /// </summary>
    public class WaitUtils
    {
        /// <summary>
        /// Waits until the specified element is visible on the page.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="by">The locator for the element.</param>
        /// <param name="timeout">The maximum wait time.</param>
        /// <returns>The visible IWebElement.</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown if the element is not visible in time.</exception>
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

        /// <summary>
        /// Waits until the specified element is clickable.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="by">The locator for the element.</param>
        /// <param name="timeout">The maximum wait time.</param>
        /// <returns>The clickable IWebElement.</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown if the element is not clickable in time.</exception>
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

        /// <summary>
        /// Waits until the browser's URL changes to the specified value.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="url">The expected URL.</param>
        /// <param name="timeout">The maximum wait time.</param>
        /// <returns>True if the URL matches; otherwise, false.</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown if the URL does not change in time.</exception>
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

        /// <summary>
        /// Waits until the browser's URL contains the specified string.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="urlString">The substring to look for in the URL.</param>
        /// <param name="timeout">The maximum wait time.</param>
        /// <returns>True if the URL contains the string; otherwise, false.</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown if the URL does not contain the string in time.</exception>
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

        /// <summary>
        /// Waits until all elements located by the specified selector are clickable.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="by">The locator for the elements.</param>
        /// <param name="timeout">The maximum wait time.</param>
        /// <returns>A list of clickable IWebElements.</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown if the elements are not clickable in time.</exception>
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

        /// <summary>
        /// Waits until all elements located by the specified selector are visible.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="by">The locator for the elements.</param>
        /// <param name="timeout">The maximum wait time.</param>
        /// <returns>A list of visible IWebElements.</returns>
        /// <exception cref="WebDriverTimeoutException">Thrown if the elements are not visible in time.</exception>
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
