using CustomTestClassExecution.Custom;
using System;
using Xunit;

[assembly: TestFramework("CustomTestClassExecution.Custom.MyTestFramework", "XunitExtensibility")]
[TestCaseOrderer("CustomTestClassExecution.Custom.MyTestCaseOrderer", "XunitExtensibility")]
public class Theory_extend_name_examples
{
    MyStack<string> stack;

    public Theory_extend_name_examples()
    {
        stack = new MyStack<string>();
    }

    [Fact]
    public void S1_should_be_empty()
    {
        //Assert.True(stack.IsEmpty);
    }

    [MyTheory("S3")]
    [MyDataAttribute("ss1", 4, 4)]
    [MyDataAttribute("ss2", 5, 1)]
    [MyDataAttribute("ss3", 6, 6)]
    public void S3(int act, int exp)
    {
        Assert.Equal(exp, act);
        //Assert.Throws<InvalidOperationException>(() => stack.Pop());
    }

    [Fact]
    public void S2_should_not_allow_you_to_call_Pop()
    {
        //Assert.Null(new object());
        //Assert.Throws<InvalidOperationException>(() => stack.Pop());
    }

    [Fact]
    public void S4_should_not_allow_you_to_call_Pop1()
    {
        //Assert.Throws<InvalidOperationException>(() => stack.Pop());
    }
}
