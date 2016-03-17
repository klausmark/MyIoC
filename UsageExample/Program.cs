using System.Threading;
using MyIoCLib;

namespace UsageExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Med MyIoC:
            RegisterDependencies();

            var øvelse2 = ResolvØvelse2();

            øvelse2.GoAhead();

            //Uden MyIoC
            IRandom random = new RandomWrapper();
            var øvelseUdenIoc = new Øvelse2(
                new ØvelsesConsoleLogger(), 
                new DieCup(
                    new Die(random), 
                    new Die(random)));
            øvelseUdenIoc.GoAhead();
        }

        private static void SleepALittleBecauseDefaultRandomSeedIsTimeBased()
        {
            Thread.Sleep(200);
        }

        private static Øvelse2 ResolvØvelse2()
        {
            MyIoC.Default.Register<Øvelse2>();
            var øvelse2 = MyIoC.Default.Resolv<Øvelse2>();
            return øvelse2;
        }

        private static void RegisterDependencies()
        {
            MyIoC.Default.Register<IDie, Die>(false);
            MyIoC.Default.Register<IDieRoller, DieCup>();
            MyIoC.Default.Register<IRandom, RandomWrapper>();
            MyIoC.Default.Register<IØvelsesLogger, ØvelsesConsoleLogger>();
        }
    }
}
