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

    public class MyDataDiscoverer : IDataDiscoverer
    {
        /// <inheritdoc/>
        public virtual IEnumerable<object[]> GetData(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            // The data from GetConstructorArguments does not maintain its original form (in particular, collections
            // end up as generic IEnumerable<T>). So we end up needing to call .ToArray() on the enumerable so that
            // we can restore the correct argument type from InlineDataAttribute.
            //
            // In addition, [InlineData(null)] gets translated into passing a null array, not a single array with a null
            // value in it, which is why the null coalesce operator is required (this is covered by the acceptance test
            // in Xunit2TheoryAcceptanceTests.InlineDataTests.SingleNullValuesWork).

            var args = (IEnumerable<object>)dataAttribute.GetConstructorArguments().ToArray()[1] ?? new object[] { null };
            return new[] { args.ToArray() };
        }

        /// <inheritdoc/>
        public virtual bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod)
        {
            return true;
        }
    }
}
