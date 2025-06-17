using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.Utils
{
    /// <summary>
    /// Utility class for browser initialization and navigation.
    /// </summary>
    public class BrowserUtil
    {
        /// <summary>
        /// Initializes a browser driver based on the specified browser name.
        /// </summary>
        /// <param name="browserName">Name of the browser (chrome, firefox, edge).</param>
        /// <param name="headless">Whether to run the browser in headless mode.</param>
        /// <returns>An initialized IWebDriver instance.</returns>
        /// <exception cref="ArgumentException">Thrown if the browser is not supported.</exception>
        public static IWebDriver InitializeBrowser(string browserName, bool headless = false)
        {
            ArgumentNullException.ThrowIfNull(browserName, nameof(browserName));

            IWebDriver driver;

            switch (browserName.ToLower())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");
                    chromeOptions.AddArgument("--incognito");
                    chromeOptions.AddArgument("--maximize-window");
                    if (headless)
                        chromeOptions.AddArgument("--headless");

                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArgument("--no-sandbox");
                    firefoxOptions.AddArgument("--disable-dev-shm-usage");
                    firefoxOptions.AddArgument("--incognito");
                    firefoxOptions.AddArgument("--maximize-window");
                    if (headless)
                        firefoxOptions.AddArgument("--headless");

                    driver = new FirefoxDriver(firefoxOptions);
                    break;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("--no-sandbox");
                    edgeOptions.AddArgument("--disable-dev-shm-usage");
                    edgeOptions.AddArgument("--incognito");
                    edgeOptions.AddArgument("--maximize-window");
                    if (headless)
                        edgeOptions.AddArgument("--headless");

                    driver = new EdgeDriver(edgeOptions);
                    break;

                default:
                    throw new ArgumentException($"Browser '{browserName}' is not supported.");
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

            return driver;
        }

        /// <summary>
        /// Navigates the browser to the specified URL.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="url">The URL to navigate to.</param>
        public static void NavigateToUrl(IWebDriver driver, string url)
        {
            ArgumentNullException.ThrowIfNull(driver, nameof(driver));
            ArgumentNullException.ThrowIfNull(url, nameof(url));

            driver.Navigate().GoToUrl(url);
        }
    }
}
