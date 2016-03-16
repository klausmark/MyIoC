namespace MyIoCLib.Exceptions
{
    public class NoValidConstructor : MyIoCException
    {
        public NoValidConstructor(string message) : base(message)
        {
        }
    }
}