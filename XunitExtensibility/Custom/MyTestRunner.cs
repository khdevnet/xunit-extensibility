using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CustomTestClassExecution.Custom
{
    //public class MyTestRunner : TestRunner<MyTestCase>
    //{
    //    readonly IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes;

    //    /// <summary>
    //    /// Initializes a new instance of the <see cref="XunitTestRunner"/> class.
    //    /// </summary>
    //    /// <param name="test">The test that this invocation belongs to.</param>
    //    /// <param name="messageBus">The message bus to report run status to.</param>
    //    /// <param name="testClass">The test class that the test method belongs to.</param>
    //    /// <param name="constructorArguments">The arguments to be passed to the test class constructor.</param>
    //    /// <param name="testMethod">The test method that will be invoked.</param>
    //    /// <param name="testMethodArguments">The arguments to be passed to the test method.</param>
    //    /// <param name="skipReason">The skip reason, if the test is to be skipped.</param>
    //    /// <param name="beforeAfterAttributes">The list of <see cref="BeforeAfterTestAttribute"/>s for this test.</param>
    //    /// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
    //    /// <param name="cancellationTokenSource">The task cancellation token source, used to cancel the test run.</param>
    //    public MyTestRunner(ITest test,
    //                           IMessageBus messageBus,
    //                           Type testClass,
    //                           object[] constructorArguments,
    //                           MethodInfo testMethod,
    //                           object[] testMethodArguments,
    //                           string skipReason,
    //                           IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
    //                           ExceptionAggregator aggregator,
    //                           CancellationTokenSource cancellationTokenSource)
    //        : base(test, messageBus, testClass, constructorArguments, testMethod, testMethodArguments, skipReason, aggregator, cancellationTokenSource)
    //    {
    //        if (test is null)
    //        {
    //            throw new ArgumentNullException(nameof(test));
    //        }

    //        if (messageBus is null)
    //        {
    //            throw new ArgumentNullException(nameof(messageBus));
    //        }

    //        if (testClass is null)
    //        {
    //            throw new ArgumentNullException(nameof(testClass));
    //        }

    //        if (constructorArguments is null)
    //        {
    //            throw new ArgumentNullException(nameof(constructorArguments));
    //        }

    //        if (testMethod is null)
    //        {
    //            throw new ArgumentNullException(nameof(testMethod));
    //        }

    //        if (testMethodArguments is null)
    //        {
    //            throw new ArgumentNullException(nameof(testMethodArguments));
    //        }

    //        if (string.IsNullOrEmpty(skipReason))
    //        {
    //            throw new ArgumentException($"'{nameof(skipReason)}' cannot be null or empty.", nameof(skipReason));
    //        }

    //        if (aggregator is null)
    //        {
    //            throw new ArgumentNullException(nameof(aggregator));
    //        }

    //        if (cancellationTokenSource is null)
    //        {
    //            throw new ArgumentNullException(nameof(cancellationTokenSource));
    //        }

    //        this.beforeAfterAttributes = beforeAfterAttributes ?? throw new ArgumentNullException(nameof(beforeAfterAttributes));
    //    }

    //    /// <summary>
    //    /// Gets the list of <see cref="BeforeAfterTestAttribute"/>s for this test.
    //    /// </summary>
    //    protected IReadOnlyList<BeforeAfterTestAttribute> BeforeAfterAttributes
    //        => beforeAfterAttributes;

    //    /// <inheritdoc/>
    //    protected override async Task<Tuple<decimal, string>> InvokeTestAsync(ExceptionAggregator aggregator)
    //    {
    //        var output = string.Empty;

    //        TestOutputHelper testOutputHelper = null;
    //        foreach (object obj in ConstructorArguments)
    //        {
    //            testOutputHelper = obj as TestOutputHelper;
    //            if (testOutputHelper != null)
    //                break;
    //        }

    //        if (testOutputHelper != null)
    //            testOutputHelper.Initialize(MessageBus, Test);

    //        var executionTime = await InvokeTestMethodAsync(aggregator);

    //        if (testOutputHelper != null)
    //        {
    //            output = testOutputHelper.Output;
    //            testOutputHelper.Uninitialize();
    //        }

    //        return Tuple.Create(executionTime, output);
    //    }




    //    /// <summary>
    //    /// Override this method to invoke the test method.
    //    /// </summary>
    //    /// <param name="aggregator">The exception aggregator used to run code and collect exceptions.</param>
    //    /// <returns>Returns the execution time (in seconds) spent running the test method.</returns>
    //    protected virtual Task<decimal> InvokeTestMethodAsync(ExceptionAggregator aggregator)
    //        => new XunitTestInvoker(Test, MessageBus, TestClass, ConstructorArguments, TestMethod, TestMethodArguments, BeforeAfterAttributes, aggregator, CancellationTokenSource).RunAsync();
    //}
}
