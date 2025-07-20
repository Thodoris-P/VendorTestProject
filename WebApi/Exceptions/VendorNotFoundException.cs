namespace WebApi.Exceptions;

public class VendorNotFoundException : Exception
{
    public VendorNotFoundException(string message) : base(message)
    {
        
    }
    public VendorNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
        
    }
}