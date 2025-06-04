namespace Construction.Exceptions
{
    public class ServiceNotFoundException : Exception
    {
        public ServiceNotFoundException(string message)
            : base(message)
        {
        }

        public ServiceNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}