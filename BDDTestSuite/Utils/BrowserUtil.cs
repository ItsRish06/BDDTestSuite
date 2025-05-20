using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.Utils
{
    public class BrowserUtil
    {
        public static IWebDriver InitializeBrowser(string browserName, bool headless)
        {
            switch (browserName.ToLower()) {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");
                    chromeOptions.AddArgument("--incognito");
                    if (headless)
                        chromeOptions.AddArgument("--headless");
                    return new ChromeDriver(chromeOptions);

                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    firefoxOptions.AddArgument("--no-sandbox");
                    firefoxOptions.AddArgument("--disable-dev-shm-usage");
                    firefoxOptions.AddArgument("--incognito");
                    if (headless)
                        firefoxOptions.AddArgument("--headless");
                    return new FirefoxDriver(firefoxOptions);

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    edgeOptions.AddArgument("--no-sandbox");
                    edgeOptions.AddArgument("--disable-dev-shm-usage");
                    edgeOptions.AddArgument("--incognito");
                    if (headless)
                        edgeOptions.AddArgument("--headless");
                    return new EdgeDriver(edgeOptions);

                default:
                    throw new ArgumentException($"Browser '{browserName}' is not supported.");

            }
        }
        
    }
}
