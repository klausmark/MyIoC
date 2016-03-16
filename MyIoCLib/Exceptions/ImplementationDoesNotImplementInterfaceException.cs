namespace MyIoCLib.Exceptions
{
    public class ImplementationDoesNotImplementInterfaceException : MyIoCException
    {
        public ImplementationDoesNotImplementInterfaceException(string message) : base(message)
        {
        }
    }
}