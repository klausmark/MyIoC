namespace MyIoCTests
{
    public class RequiresParameter : ITest2
    {
        private readonly int _parameter;

        public RequiresParameter(int parameter)
        {
            _parameter = parameter;
        }
    }
}