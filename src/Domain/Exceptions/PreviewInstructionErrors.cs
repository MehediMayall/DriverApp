namespace Domain.Exceptions;

public static class PreviewInstructionErrors<T> where T : class
{
    public static Error<T> ContainerRequestIsNull() => 
        new("Request parameter is null.", "request.requestDto");
    public static Error<T> NoContainerFound() => 
        new("Container object is null. No container found.", "repo.GetContainer");
}