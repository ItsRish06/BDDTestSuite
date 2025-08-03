using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using AventStack.ExtentReports.Reporter;
using BDDTestSuite.GenAi;
using BDDTestSuite.GenAI;
using BDDTestSuite.Models;
using BDDTestSuite.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.BiDi.Modules.BrowsingContext;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools;
using OpenQA.Selenium.DevTools.V134.Network;
using OpenQA.Selenium.Firefox;
using Reqnroll.Assist.ValueRetrievers;
using Reqnroll.BoDi;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Compact;
using Serilog.Formatting.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebDriverManager.Services.Impl;
using JsonSerializer = System.Text.Json.JsonSerializer;




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

        private static List<ExtentTest> _failedNodes;

        [ThreadStatic]
        private static Queue<string> _logCollection;

        private static ConcurrentBag<FeatureNode> _featureNodes;

        [ThreadStatic]
        private static FeatureNode _featureNode;
        [ThreadStatic]
        private static ScenarioNode _scenarioNode;
        [ThreadStatic]
        private static StepNode _stepNode;

        private static readonly object _lock = new object();

        public Hooks(IObjectContainer objectContainer, ScenarioContext scenarioContext, FeatureContext featureContext)
        {
            _objectContainer = objectContainer;
            _scenarioContext = scenarioContext;
            _featureContext = featureContext;
        }

        [BeforeTestRun]
        public static void TestRunSetUp()
        {
            var assembly = Assembly.GetExecutingAssembly();

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json")
                .AddEnvironmentVariables()
                .AddUserSecrets(assembly, optional: true)
                .Build();

            string outputTemplate = "[{Level:u3}] [Scenario: {Scenario}] {Message:lj}{NewLine}{Exception}";

            _logger = new LoggerConfiguration()
                .WriteTo.Console(outputTemplate: outputTemplate)
                .WriteTo.File("log.txt",outputTemplate: outputTemplate)
                .WriteTo.File(new CompactJsonFormatter(), "jlog.json")
                .CreateLogger();

            var htmlReporter = new ExtentSparkReporter(Path.Combine(Directory.GetCurrentDirectory(),"Reports","Report.html"));
            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);

            _featureNodes = [];
            _failedNodes = [];

        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            _feature = _extent.CreateTest<Feature>(featureContext.FeatureInfo.Title, featureContext.FeatureInfo.Description);
            _featureNode = new FeatureNode()
            {
                Title = featureContext.FeatureInfo.Title
            };

            lock (_lock)
                _featureNodes.Add(_featureNode);

        }

        [BeforeScenario(Order = 0)]
        public void ReportConfiguration()
        {
            _scenario = _feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
            _scenarioNode = new ScenarioNode()
            {
                Title = _scenarioContext.ScenarioInfo.Title
            };
        }

        [BeforeScenario(Order = 100)]
        public void BeforeScenario()
        {
            var browserName = _configuration?.GetValue<string>("browser");
            ArgumentNullException.ThrowIfNull(browserName, nameof(browserName));

            _logCollection = new Queue<string>();

            _scenarioLogger = new LoggerConfiguration()
                .WriteTo.Logger(_logger)
                .WriteTo.Sink(new ExtentSink(_logCollection))
                .Enrich.WithProperty("Scenario", _scenarioContext.ScenarioInfo.Title)
                .CreateLogger();
            
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

            _stepNode = new StepNode() 
            {
                Title = _scenarioContext.StepContext.StepInfo.Text 
            };
            _scenarioNode.Steps.Add(_stepNode);
        }


        [AfterStep]
        public void AfterStep()
        {

            for (int i = 0; i < _logCollection.Count; i++)
            {
                var logMessage = _logCollection.Dequeue();
                _node.Log(Status.Info, MarkupHelper.CreateCodeBlock(logMessage));
                _stepNode.Logs.Add(logMessage);
            }
            _stepNode.Status = "Pass";

            if (_scenarioContext.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError)
            {
                var driver = _objectContainer.Resolve<Services>().Driver;
                var base64 = ScreenshotUtils.ScreenCaptureBase64(driver);
                var compressed = ScreenshotUtils.CompressBase64Image(base64, 30L);

                ReportUtils.LogFailure(_node, _scenarioContext.TestError, base64);

                _failedNodes.Add(_node);

                _stepNode.Status = "Fail";
                _stepNode.Screenshot = compressed;
                _stepNode.ExceptionMessage = _scenarioContext.TestError.Message;
            }
   
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if(_stepNode.Status == "Fail")
                _featureNode.Scenarios.Add(_scenarioNode);

            var services = _objectContainer.Resolve<Services>();
            services.Driver?.Quit();
        }


        [AfterTestRun]
        public static void AfterTest()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = false,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            var failedFeatures = _featureNodes.Where(f=> f.Scenarios.Count() > 0).ToList();

            foreach (var featureNode in failedFeatures) 
            {
                var geminiRequest = new GeminiRequest()
                    .AddContentText("Here is the list of failed scenario of feature. Each scenario will have screenshot in the next part.");

                foreach (var scenarioNode in featureNode.Scenarios)
                {
                    var failedStep = scenarioNode.Steps.Where(s => s.Status == "Fail").First();
                    var base64 = failedStep.Screenshot;
                    failedStep.Screenshot = null;
                    var serialized = JsonSerializer.Serialize(scenarioNode, options);
                    geminiRequest.AddContentText(serialized);
                    geminiRequest.AddContentData("image/png", base64);
                }

                var responseText = Gemini.GenerateSummary(_configuration["geminiApiKey"], geminiRequest);
                var scenarioSummaries = JsonNode.Parse(responseText).AsArray();

                var featureName = featureNode.Title;

                foreach (var summary in scenarioSummaries)
                {
                    var scenarioName = summary["scenario"].ToString();

                    var failedNode = _failedNodes
                        .Where(node => node.Model.Parent.Parent.Name == featureName && node.Model.Parent.Name == scenarioName)
                        .First();

                    failedNode.Log(Status.Info, MarkupHelper.CreateCodeBlock(
                        "AI Summary - \n \n" +
                        $"Summary: {summary["summary"]} \n \n" +
                        $"Reasoning: {summary["reasoning"]} \n \n" +
                        $"Recommendation: {summary["recommendation"]}"
                        ));
                }
            }

            _extent.Flush();

        }

    }
}
