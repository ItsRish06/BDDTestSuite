using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.MarkupUtils;
using Reqnroll;
using Serilog.Core;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.Utils
{
    public class ReportUtils
    {
        public static ExtentTest CreateStepNode(ExtentTest scenario, ScenarioBlock scenarioBlock, string stepInfo, string stepDesc = "" )
        {
            ExtentTest node;

            switch (scenarioBlock)
            {
                case ScenarioBlock.Given:
                    node = scenario.CreateNode<Given>(stepInfo, stepDesc);
                    break;
                case ScenarioBlock.When:
                    node = scenario.CreateNode<When>(stepInfo, stepDesc);
                    break;
                case ScenarioBlock.Then:
                    node = scenario.CreateNode<Then>(stepInfo, stepDesc);
                    break;
                default:
                    node = scenario.CreateNode<And>(stepInfo, stepDesc);
                    break;
            }

            return node;
        }

        public static void LogFailure(ExtentTest node, Exception exception, string base64Img)
        {
            node.Log(Status.Fail, MarkupHelper.CreateCodeBlock(exception.Message));
            node.Log(Status.Fail, exception);
            node.Log(Status.Fail, exception.InnerException);
            node.Fail(MediaEntityBuilder.CreateScreenCaptureFromBase64String(base64Img).Build());
        }
    }
}
