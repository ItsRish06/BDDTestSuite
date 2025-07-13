using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BDDTestSuite.Utils
{
    public class ScreenshotUtils
    {
        public static string ScreenCaptureBase64(IWebDriver driver)
        {
            string base64 = "";

            if (driver is ChromeDriver chromeDriver)
            {
                var metrics = chromeDriver.ExecuteCdpCommand("Page.getLayoutMetrics", new Dictionary<string, object>());
                var metricsDict = metrics as IDictionary<string, object>;
                var contentSize = metricsDict?["contentSize"] as IDictionary<string, object>;
                int width = Convert.ToInt32(contentSize?["width"]);
                int height = Convert.ToInt32(contentSize?["height"]);


                var clip = new Dictionary<string, object>
                    {
                        { "x", 0 },
                        { "y", 0 },
                        { "width", width },
                        { "height", height },
                        { "scale", 1 }
                    };
                var screenshotParams = new Dictionary<string, object>
                    {
                        { "format", "png" },
                        { "fromSurface", true },
                        { "clip", clip }
                    };

                var result = chromeDriver.ExecuteCdpCommand("Page.captureScreenshot", screenshotParams);
                var resultDict = result as IDictionary<string, object>;

                base64 = resultDict?["data"] as string;
            }
            else if (driver is FirefoxDriver firefoxDriver)
            {
                base64 = firefoxDriver.GetFullPageScreenshot().AsBase64EncodedString;
            }

            return base64;
        }
    }
}
