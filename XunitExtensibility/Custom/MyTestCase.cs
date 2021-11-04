using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestClassExecution.Custom
{
    public class MyTestCase : XunitTestCase
    {
        public MyTestCase()
        {

        }

        public MyTestCase(IMessageSink diagnosticMessageSink,
                     TestMethodDisplay defaultMethodDisplay,
                     TestMethodDisplayOptions defaultMethodDisplayOptions,
                     ITestMethod testMethod,
                     object[] testMethodArguments = null)
    : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, SkipFirst(testMethodArguments))
        {
            MyDefaultMethodDisplay = defaultMethodDisplay;
            MyDefaultMethodDisplayOptions = defaultMethodDisplayOptions;
            TestCaseName = testMethodArguments[0];
        }

        private static object[] SkipFirst(object[] testMethodArguments)
        {
            return testMethodArguments.Skip(1).ToArray();
        }

        public TestMethodDisplay MyDefaultMethodDisplay { get; set; }

        public TestMethodDisplayOptions MyDefaultMethodDisplayOptions { get; set; }
        public object TestCaseName { get; }
        public object[] TestMethodArguments1 { get; }

        public override Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                                IMessageBus messageBus,
                                                object[] constructorArguments,
                                                ExceptionAggregator aggregator,
                                                CancellationTokenSource cancellationTokenSource)
           => new MyTestCaseRunner(this, DisplayName, SkipReason, constructorArguments, TestMethodArguments, messageBus, aggregator, cancellationTokenSource).RunAsync();

        protected override string GetDisplayName(IAttributeInfo factAttribute, string displayName)
        {
            return base.GetDisplayName(factAttribute, displayName + "_" + TestCaseName);
        }
    }
}
