namespace Domain.Exceptions;

public class ObjectNotFoundException : Exception
{
    public ObjectNotFoundException(string message) : base(message) {}
    public ObjectNotFoundException() : base() {}
}