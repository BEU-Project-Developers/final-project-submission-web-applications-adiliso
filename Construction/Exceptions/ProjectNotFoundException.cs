namespace Construction.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string message)
            : base(message)
        {
        }

        public ProjectNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}