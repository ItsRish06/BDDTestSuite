using AngleSharp.Dom;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.Utils
{
    /// <summary>
    /// Utility class for common element interactions.
    /// </summary>
    public static class ElementUtils
    {
        /// <summary>
        /// Performs a JavaScript-based click on the specified element.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="element">The element to click.</param>
        /// <exception cref="Exception">Thrown if the JS click fails.</exception>
        public static void JSClick(IWebDriver driver, IWebElement element)
        {
            try
            {
                var js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].click();", element);

            }catch (Exception ex)
            {
                throw new Exception($"Unexpected error during JS click: {ex.Message}");
            }
            
        }
    }
}
