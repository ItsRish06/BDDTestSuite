using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using BDDTestSuite.Models;
using BDDTestSuite.Utils;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.BrowsingContext;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.Firefox;
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
        private ScenarioContext _scenarioContext;
        private FeatureContext _featureContext;
        private static ExtentReports _extent;
        [ThreadStatic]
        private static ExtentTest _feature;
        [ThreadStatic]
        private static ExtentTest _scenario;
        [ThreadStatic]
        private static ExtentTest _node;
        [ThreadStatic]
        private static ILogger _scenarioLogger;
       
        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeTestRun]
        public static void TestRunSetUp()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables()
                .Build();

            string outputTemplate = "[{Level:u3}] [Scenario: {Scenario}] {Message:lj}{NewLine}{Exception}";

            _logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: outputTemplate)
                .WriteTo.File("log.txt",outputTemplate: outputTemplate)
                .CreateLogger();

            var htmlReporter = new ExtentSparkReporter(Path.Combine(Directory.GetCurrentDirectory(),"Reports","Report.html"));
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);

        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _feature = _extent.CreateTest<Feature>(featureContext.FeatureInfo.Title, featureContext.FeatureInfo.Description);
            
        }

        [BeforeScenario(Order = 0)]
        public void ReportConfiguration()
        {
            _scenario = _feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
            
        }

        [BeforeScenario(Order = 100)]
        public void BeforeScenario()
        {
            var browserName = _configuration?.GetValue<string>("browser");
            ArgumentNullException.ThrowIfNull(browserName, nameof(browserName));

            _scenarioLogger = _logger?.ForContext("Scenario", _scenarioContext.ScenarioInfo.Title);
            
            var driver = BrowserUtil.InitializeBrowser(browserName, headless : false);
            driver.Manage().Window.Maximize();

            var services = new Services()
            {
                Driver = driver,
                Config = _configuration,
                Logger = _scenarioLogger
            };

            _objectContainer.RegisterInstanceAs<Services>(services);

        }

        [BeforeStep]
        public void BeforeStep()
        {
            _node = ReportUtils.CreateStepNode(
                _scenario, 
                _scenarioContext.CurrentScenarioBlock, 
                _scenarioContext.StepContext.StepInfo.Text
                );
        }

        
        [AfterStep]
        public void AfterStep()
        {
            if (_scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError)
            {
                var driver = _objectContainer.Resolve<Services>().Driver;
                var base64 = ScreenshotUtils.ScreenCaptureBase64(driver);

                ReportUtils.LogFailure(_node, _scenarioContext.TestError, base64);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            
            var services = _objectContainer.Resolve<Services>();
            services.Driver?.Quit();
        }

        [AfterTestRun]
        public static void AfterTest()
        {
            _extent.Flush();
        }

    }
}
