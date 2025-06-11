using BDDTestSuite.Utils;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using Reqnroll.BoDi;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite
{
    [Binding]
    public class Hooks
    {
        private readonly IObjectContainer _objectContainer;
        private static IConfigurationRoot? _configuration;
        private static ILogger? _logger;
         
        public Hooks(IObjectContainer objectContainer) 
        {
            _objectContainer = objectContainer;
        }

        [BeforeTestRun]
        public static void TestRunSetUp()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables()
                .Build();

            string outputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3} {ProcessId}] {ThreadId} {Message:lj}{NewLine}{Exception}";

            _logger = new LoggerConfiguration()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .WriteTo.Console(outputTemplate: outputTemplate)
                .WriteTo.File("log.txt",outputTemplate: outputTemplate)
                .CreateLogger();
            
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var browserName = _configuration?.GetValue<string>("browser");

            ArgumentNullException.ThrowIfNull(browserName, nameof(browserName));

            var driver = BrowserUtil.InitializeBrowser(browserName, headless : false);

            driver.Manage().Window.Maximize();

            _objectContainer.RegisterInstanceAs<IWebDriver>(driver);
            _objectContainer.RegisterInstanceAs<IConfigurationRoot?>(_configuration);
            _objectContainer.RegisterInstanceAs<ILogger?>(_logger);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = _objectContainer.Resolve<IWebDriver>();
            driver?.Quit();
        }

    }
}
