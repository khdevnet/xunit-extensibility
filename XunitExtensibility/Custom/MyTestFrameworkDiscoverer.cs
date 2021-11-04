using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestClassExecution.Custom
{
    public class MyTestFrameworkDiscoverer : XunitTestFrameworkDiscoverer
    {
        public IAssemblyInfo MyAssemblyInfo { get; set; }
        public MyTestFrameworkDiscoverer(IAssemblyInfo assemblyInfo,
                                            ISourceInformationProvider sourceProvider,
                                            IMessageSink diagnosticMessageSink,
                                            IXunitTestCollectionFactory collectionFactory = null)
            : base(assemblyInfo, sourceProvider, diagnosticMessageSink)
        {
            MyAssemblyInfo = assemblyInfo;
        }

        public new ITestClass CreateTestClass(ITypeInfo @class, Guid testCollectionUniqueId)
        {
            // This method is called for special fact deserialization, to ensure that the test collection unique
            // ID lines up with the ones that will be deserialized through normal mechanisms.
            var discoveredTestCollection = TestCollectionFactory.Get(@class);
            var testCollection = new MyTestCollection(discoveredTestCollection.TestAssembly, discoveredTestCollection.CollectionDefinition, discoveredTestCollection.DisplayName, testCollectionUniqueId);
            return new TestClass(testCollection, @class);
        }

        public bool FindTestsForMethod1(ITestMethod testMethod, bool includeSourceInformation, IMessageBus messageBus, ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            var factAttributes = testMethod.Method.GetCustomAttributes(typeof(FactAttribute)).CastOrToList();
            if (factAttributes.Count > 1)
            {
                var message = $"Test method '{testMethod.TestClass.Class.Name}.{testMethod.Method.Name}' has multiple [Fact]-derived attributes";
                var testCase = new ExecutionErrorTestCase(DiagnosticMessageSink, TestMethodDisplay.ClassAndMethod, TestMethodDisplayOptions.None, testMethod, message);
                return ReportDiscoveredTestCase(testCase, includeSourceInformation, messageBus);
            }

            var factAttribute = factAttributes.FirstOrDefault();
            if (factAttribute == null)
                return true;

            var factAttributeType = (factAttribute as IReflectionAttributeInfo)?.Attribute.GetType();

            Type discovererType = null;
            if (factAttributeType == null || !DiscovererTypeCache.TryGetValue(factAttributeType, out discovererType))
            {
                var testCaseDiscovererAttribute = factAttribute.GetCustomAttributes(typeof(XunitTestCaseDiscovererAttribute)).FirstOrDefault();
                if (testCaseDiscovererAttribute != null)
                {
                    var args = testCaseDiscovererAttribute.GetConstructorArguments().Cast<string>().ToList();
                    discovererType = Xunit.Sdk1.SerializationHelper.GetType(args[1], args[0]);
                }

                if (factAttributeType != null)
                    DiscovererTypeCache[factAttributeType] = discovererType;

            }
            if (discovererType == null)
                return true;

            var discoverer = GetDiscoverer(discovererType);
            if (discoverer == null)
                return true;

            foreach (var testCase in discoverer.Discover(discoveryOptions, testMethod, factAttribute))
                if (!ReportDiscoveredTestCase(testCase, includeSourceInformation, messageBus))
                    return false;

            return true;
        }

        /// <inheritdoc/>
        protected override bool FindTestsForType(ITestClass testClass, bool includeSourceInformation, IMessageBus messageBus, ITestFrameworkDiscoveryOptions discoveryOptions)
        {
            foreach (var method in testClass.Class.GetMethods(true))
            {
                var testMethod = new TestMethod(testClass, method);
                if (!FindTestsForMethod(testMethod, includeSourceInformation, messageBus, discoveryOptions))
                    return false;
            }

            return true;
        }
    }
}
