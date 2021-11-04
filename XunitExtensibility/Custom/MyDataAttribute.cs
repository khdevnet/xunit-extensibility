using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestClassExecution.Custom
{
    // [XunitTestCaseDiscoverer("CustomTestClassExecution.Custom.MyTheoryDiscoverer", "CustomTestClassExecution")]
    /// <summary>
    /// Provides a data source for a data theory, with the data coming from inline values.
    /// </summary>
    [DataDiscoverer("CustomTestClassExecution.Custom.MyDataDiscoverer", "XunitExtensibility")]
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public sealed class MyDataAttribute : DataAttribute
    {
        readonly object[] data;

        /// <summary>
        /// Initializes a new instance of the <see cref="InlineDataAttribute"/> class.
        /// </summary>
        /// <param name="data">The data values to pass to the theory.</param>
        public MyDataAttribute(string name, params object[] data)
        {
            Name = name;
            this.data = data;
        }

        public string Name { get; } = "hi test";

        /// <inheritdoc/>
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            // This is called by the WPA81 version as it does not have access to attribute ctor params
            return new[] { data };
        }
    }
}
