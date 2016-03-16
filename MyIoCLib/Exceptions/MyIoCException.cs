using System;

namespace MyIoCLib.Exceptions
{
    public class MyIoCException : Exception
    {
        public MyIoCException(string message) : base(message)
        {            
        }
    }
}