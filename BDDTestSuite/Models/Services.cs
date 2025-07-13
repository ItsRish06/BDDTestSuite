using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.Models
{
    public class Services
    {
        public ILogger Logger;
        public IWebDriver Driver;
        public IConfigurationRoot Config;
    }
}
