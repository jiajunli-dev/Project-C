namespace Data.Exceptions;

public class MissingValueException : Exception
{
    public MissingValueException(string key) : base($"Missing value for key: {key}") { }
}