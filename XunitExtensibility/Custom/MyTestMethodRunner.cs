﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestClassExecution.Custom
{
    public class MyTestMethodRunner : TestMethodRunner<XunitTestCase>
    {
        readonly object[] constructorArguments;
        readonly IMessageSink diagnosticMessageSink;

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitTestMethodRunner"/> class.
        /// </summary>
        /// <param name="testMethod">The test method to be run.</param>
        /// <param name="class">The test class that contains the test method.</param>
        /// <param name="method">The test method that contains the tests to be run.</param>
        /// <param name="testCases">The test cases to be run.</param>
        /// <param name="diagnosticMessageSink">The message sink used to send diagnostic messages to.</param>
        /// <param name="messageBus">The message bus to report run status to.</param>
        /// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
        /// <param name="cancellationTokenSource">The task cancellation token source, used to cancel the test run.</param>
        /// <param name="constructorArguments">The constructor arguments for the test class.</param>
        public MyTestMethodRunner(ITestMethod testMethod,
                                     IReflectionTypeInfo @class,
                                     IReflectionMethodInfo method,
                                     IEnumerable<XunitTestCase> testCases,
                                     IMessageSink diagnosticMessageSink,
                                     IMessageBus messageBus,
                                     ExceptionAggregator aggregator,
                                     CancellationTokenSource cancellationTokenSource,
                                     object[] constructorArguments)
            : base(testMethod, @class, method, testCases, messageBus, aggregator, cancellationTokenSource)
        {
            this.constructorArguments = constructorArguments;
            this.diagnosticMessageSink = diagnosticMessageSink;
        }

        /// <inheritdoc/>
        protected override Task<RunSummary> RunTestCaseAsync(XunitTestCase testCase)
            => testCase.RunAsync(diagnosticMessageSink, MessageBus, constructorArguments, new ExceptionAggregator(Aggregator), CancellationTokenSource);
    }
}
