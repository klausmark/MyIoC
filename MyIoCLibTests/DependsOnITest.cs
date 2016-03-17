namespace MyIoCTests
{
    public class DependsOnITest
    {
        private ITest _dependency;

        public DependsOnITest(ITest dependency)
        {
            _dependency = dependency;
        }
    }
}