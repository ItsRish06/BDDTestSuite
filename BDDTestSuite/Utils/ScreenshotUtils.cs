using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.Network;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
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

        public static string CompressBase64Image(string base64, long quality, int maxW = 0, int maxH = 0)
        {
            byte[] bytes = Convert.FromBase64String(base64);
            using var ms = new MemoryStream(bytes); 
            using var img = Image.FromStream(ms);

            Image processed = img;
            if (maxW > 0 && maxH > 0)
            {
                var ratioX = (double)maxW / img.Width;
                var ratioY = (double)maxH / img.Height;
                var ratio = Math.Min(ratioX, ratioY);

                if (ratio < 1.0)
                {
                    int newW = (int)(img.Width * ratio);
                    int newH = (int)(img.Height * ratio);
                    var thumb = new Bitmap(newW, newH);

                    using (var gfx = Graphics.FromImage(thumb))
                        gfx.DrawImage(img, 0, 0, newW, newH);

                    processed = thumb;
                }
            }

            //Find JPEG encoder
            var jpeg = Array.Find(
              ImageCodecInfo.GetImageEncoders(),
              c => c.MimeType == "image/jpeg"
            ) ?? throw new InvalidOperationException("JPEG codec not found");

            //Set quality
            var eps = new EncoderParameters(1);
            eps.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

            //Save to MemoryStream → byte[]
            using var outStream = new MemoryStream();
            processed.Save(outStream, jpeg, eps);
            var compressedBytes = outStream.ToArray();

            //byte[] → Base64
            return Convert.ToBase64String(compressedBytes);
        }
    }
}
