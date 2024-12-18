namespace Ran.Core.Ran.Exceptions;

public class RanException : Exception
{
    public RanException(string message)
        : base(message) { }

    public RanException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
