using System;
using System.Runtime.Serialization;

public class FunctionalException : Exception
{
    public FunctionalException()
    {
    }

    public FunctionalException(string message) : base(message)
    {
    }

    public FunctionalException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected FunctionalException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}