namespace MyIoCTests
{
    public class RequiresParameterAndDependsOnITest : ITest2
    {
        private readonly int _parameter;
        private readonly ITest _test;

        public RequiresParameterAndDependsOnITest(int parameter, ITest test)
        {
            _parameter = parameter;
            _test = test;
        }
    }
}