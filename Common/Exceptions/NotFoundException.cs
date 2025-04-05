namespace Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(Type type, int id) : base($"{type.Name} not found with id {id}")
    {

    }

    public NotFoundException(string entityName, string identifier) : base($"{entityName} not found with identifier: {identifier}")
    {
        
    }

    public NotFoundException(string entityName, int id) : base($"{entityName} not found with identifier: {id}")
    {

    }
}
