using AventStack.ExtentReports;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDDTestSuite.Utils
{
    public class ExtentSink : ILogEventSink
    {
        private Queue<string> _logCollection;
        public ExtentSink(Queue<string> logCollection) 
        {
            _logCollection = logCollection;
        }
        public void Emit(LogEvent logEvent)
        {
            _logCollection.Enqueue(logEvent.RenderMessage());
        }
    }
}
