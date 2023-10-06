namespace Data.Exceptions;
public class ModelNotFoundException : Exception
{
    public string PropertyName { get; }

    public ModelNotFoundException(string name)
    {
        PropertyName = name;
    }
}
