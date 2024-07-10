namespace Domain.Exceptions;

public static class SerializeError<T> where T : class
{
    public static Error<T> ContainerRequestIsNull() => new(ERRORS.ARGUMENT_OBJECT_NULL, "request.requestDto");
    public static Error<T> RequestedParameterIsNull() => new(ERRORS.ARGUMENT_OBJECT_NULL, "request.requestDto");
    public static Error<T> FailedToDeserializeObject() => new(ERRORS.FAILED_DESERIALIZE_OBJECT);
}