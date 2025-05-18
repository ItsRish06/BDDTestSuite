using Microsoft.Extensions.Configuration;
using Reqnroll.BoDi;
using System;
using System.Collections.Generic;
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
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _objectContainer.RegisterInstanceAs<IConfigurationRoot?>(_configuration);
        }
    }
}
