namespace WebApi.Exceptions;

public class ConflictingIdException : Exception
{
    public ConflictingIdException(string message) : base(message)
    {
        
    }
    public ConflictingIdException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}