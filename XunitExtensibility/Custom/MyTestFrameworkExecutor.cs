﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestClassExecution.Custom
{
    public class MyTestFrameworkExecutor : TestFrameworkExecutor<XunitTestCase>
    {
        readonly Lazy<MyTestFrameworkDiscoverer> discoverer;

        /// <summary>
        /// Initializes a new instance of the <see cref="XunitTestFrameworkExecutor"/> class.
        /// </summary>
        /// <param name="assemblyName">Name of the test assembly.</param>
        /// <param name="sourceInformationProvider">The source line number information provider.</param>
        /// <param name="diagnosticMessageSink">The message sink to report diagnostic messages to.</param>
        public MyTestFrameworkExecutor(AssemblyName assemblyName,
                                          ISourceInformationProvider sourceInformationProvider,
                                          IMessageSink diagnosticMessageSink)
            : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
        {
            string config = null;
#if NETFRAMEWORK
            config = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
#endif
            TestAssembly = new TestAssembly(AssemblyInfo, config, assemblyName.Version);
            discoverer = new Lazy<MyTestFrameworkDiscoverer>(() => new MyTestFrameworkDiscoverer(AssemblyInfo, SourceInformationProvider, DiagnosticMessageSink));
        }

        /// <summary>
        /// Gets the test assembly that contains the test.
        /// </summary>
        protected TestAssembly TestAssembly { get; set; }

        /// <inheritdoc/>
        protected override ITestFrameworkDiscoverer CreateDiscoverer()
            => discoverer.Value;

        /// <inheritdoc/>
        public override ITestCase Deserialize(string value)
        {
            if (value.Length > 3 && value.StartsWith(":F:"))
            {
                // Format from XunitTestFrameworkDiscoverer.Serialize: ":F:{typeName}:{methodName}:{defaultMethodDisplay}:{defaultMethodDisplayOptions}:{collectionId}"
                // Colons in values are double-escaped, so we can't use String.Split
                var parts = new List<string>();
                var idx = 3;
                var idxEnd = 3;
                while (idxEnd < value.Length)
                {
                    if (value[idxEnd] == ':')
                    {
                        if (idxEnd + 1 == value.Length || value[idxEnd + 1] != ':')
                        {
                            if (idx != idxEnd)
                                parts.Add(value.Substring(idx, idxEnd - idx).Replace("::", ":"));

                            idx = idxEnd + 1;
                        }
                        else if (value[idxEnd + 1] == ':')
                            ++idxEnd;
                    }

                    ++idxEnd;
                }

                if (idx != idxEnd)
                    parts.Add(value.Substring(idx, idxEnd - idx).Replace("::", ":"));

                if (parts.Count > 4)
                {
                    var typeInfo = discoverer.Value.MyAssemblyInfo.GetType(parts[0]);
                    var testCollectionUniqueId = Guid.Parse(parts[4]);
                    var testClass = discoverer.Value.CreateTestClass(typeInfo, testCollectionUniqueId);
                    var methodInfo = testClass.Class.GetMethod(parts[1], true);
                    var testMethod = new TestMethod(testClass, methodInfo);
                    var defaultMethodDisplay = (TestMethodDisplay)int.Parse(parts[2]);
                    var defaultMethodDisplayOptions = (TestMethodDisplayOptions)int.Parse(parts[3]);
                    return new XunitTestCase(DiagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod);
                }
            }

            return base.Deserialize(value);
        }

        /// <inheritdoc/>
        protected override async void RunTestCases(IEnumerable<XunitTestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
        {
            using (var assemblyRunner = new MyTestAssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions))
                await assemblyRunner.RunAsync();
        }
    }
    //public class MyTestFrameworkExecutor : XunitTestFrameworkExecutor
    //{
    //    public MyTestFrameworkExecutor(AssemblyName assemblyName,
    //                                      ISourceInformationProvider sourceInformationProvider,
    //                                      IMessageSink diagnosticMessageSink)
    //        : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
    //    {
    //    }

    //    protected override async void RunTestCases(IEnumerable<IXunitTestCase> testCases, IMessageSink executionMessageSink, ITestFrameworkExecutionOptions executionOptions)
    //    {
    //        using (var assemblyRunner = new MyTestAssemblyRunner(TestAssembly, testCases, DiagnosticMessageSink, executionMessageSink, executionOptions))
    //            await assemblyRunner.RunAsync();
    //    }
    //}
}