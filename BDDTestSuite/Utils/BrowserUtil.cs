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
    public class BrowserUtil
    {
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

        public static void NavigateToUrl(IWebDriver driver, string url)
        {
            ArgumentNullException.ThrowIfNull(driver, nameof(driver));
            ArgumentNullException.ThrowIfNull(url, nameof(url));

            driver.Navigate().GoToUrl(url);
        }
    }
}
