using System;
using FluentAssertions;
using MyIoCLib;
using MyIoCLib.Exceptions;
using NUnit.Framework;

namespace MyIoCTests
{
    [TestFixture]
    public class MyIoCTests
    {
        [Test]
        public void You_Cannot_Register_An_Implelentation_That_Does_Not_Implement_The_Interface_You_Are_Registering()
        {
            var myIoC = new MyIoCBuilder().Build();

            Action register = () => myIoC.Register<ITest, DoesNotImplementITest>();

            register.ShouldThrow<ImplementationDoesNotImplementInterfaceException>();
        }

        [Test]
        public void Register_Accepts_Valid_Registration()
        {
            var myIoC = new MyIoCBuilder().Build();

            Action register = () => myIoC.Register<ITest, ImplementsITest>();

            register.ShouldNotThrow();
        }

        [Test]
        public void Throws_Exception_If_Resolving_Something_Not_Registered()
        {
            var myIoC = new MyIoCBuilder().Build();

            Action resolv = () => myIoC.Resolv<ITest>();

            resolv.ShouldThrow<Exception>();
        }

        [Test]
        public void Throws_Exception_If_Resolving_Something_Depending_On_Something_Not_Registered()
        {
            var myIoC = new MyIoCBuilder()
                .With_DependsOnITest_Registered()
                .Build();

            Action resolv = () => myIoC.Resolv<ITest>();

            resolv.ShouldThrow<Exception>();
        }

        [Test]
        public void Can_Resolv_Valid_Implementation_With_Empty_Constructor()
        {
            var myIoC = new MyIoCBuilder().With_Implements_ITest_Registered().Build();

            var resolvedInstance = myIoC.Resolv<ITest>();

            resolvedInstance.GetType()
                .Should()
                .Be<ImplementsITest>();
        }

        [Test]
        public void Can_Resolv_Implementation_With_Constructor_Dependency_When_Dependency_Is_Registered()
        {
            var myIoC = new MyIoCBuilder()
                .With_DependsOnITest_Registered()
                .With_Implements_ITest_Registered()
                .Build();

            var resolvedInstance = myIoC.Resolv<DependsOnITest>();

            resolvedInstance.GetType()
                .Should()
                .Be<DependsOnITest>();
        }

        [Test]
        public void Can_Resolv_Implementation_With_Parameter()
        {
            var myIoC = new MyIoCBuilder().Build();
            myIoC.Register<ITest2, RequiresParameter>(true, new ImplementationParameter { Parameter = 42, ParameterName = "parameter" });

            var instance = myIoC.Resolv<ITest2>();

            instance.GetType().Should().Be<RequiresParameter>();
        }

        [Test]
        public void Can_Resolv_Implementation_With_Parameter_And_Other_Dependency()
        {
            var myIoC = new MyIoCBuilder().Build();
            myIoC.Register<ITest, ImplementsITest>(true);
            myIoC.Register<RequiresParameterAndDependsOnITest, RequiresParameterAndDependsOnITest>(true, new ImplementationParameter { Parameter = 42, ParameterName = "parameter" });

            var instance = myIoC.Resolv<RequiresParameterAndDependsOnITest>();

            instance.GetType().Should().Be<RequiresParameterAndDependsOnITest>();
        }
    }
}
