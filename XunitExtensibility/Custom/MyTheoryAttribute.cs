using Xunit;
using Xunit.Sdk;

namespace CustomTestClassExecution.Custom
{
    [XunitTestCaseDiscoverer("CustomTestClassExecution.Custom.MyTheoryDiscoverer", "XunitExtensibility")]
    public class MyTheoryAttribute : TheoryAttribute
    {
        public MyTheoryAttribute(string d)
        {
            DisplayName = d;
        }
    }
}
