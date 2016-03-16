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
            var myIoC = new MyIoC();

            Action register = () => myIoC.Register<IComparable, object>();

            register.ShouldThrow<ImplementationDoesNotImplementInterfaceException>();
        }

    }
}
