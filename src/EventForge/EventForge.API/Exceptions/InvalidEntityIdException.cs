namespace EventForge.API.Exceptions;

public sealed class InvalidEntityIdException : DomainValidationException
{
    public InvalidEntityIdException(string entity, string reason)
        : base($"{entity} Id is invalid: {reason}", $"{entity.ToLower()}.id.invalid")
    {
    }
}

