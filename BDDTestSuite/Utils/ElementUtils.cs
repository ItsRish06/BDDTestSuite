using AngleSharp.Dom;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.Utils
{
    public static class ElementUtils
    {
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
