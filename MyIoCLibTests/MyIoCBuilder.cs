using MyIoCLib;

namespace MyIoCTests
{
    public class MyIoCBuilder
    {
        private readonly MyIoC _myIoC = new MyIoC();
        public MyIoC Build()
        {
            return _myIoC;
        }

        public MyIoCBuilder With_Implements_ITest_Registered()
        {
            _myIoC.Register<ITest, ImplementsITest>();
            return this;
        }

        public MyIoCBuilder With_DependsOnITest_Registered()
        {
            _myIoC.Register<DependsOnITest>();
            return this;
        }
    }
}