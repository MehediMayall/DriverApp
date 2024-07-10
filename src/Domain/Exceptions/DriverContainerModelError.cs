namespace Domain.Exceptions;

public class DriverContainerModelError<DriverContainerModel> where DriverContainerModel : class
{
    public static Error<DriverContainerModel> NoContainerFound() => new("Container object is null. No container found.", "repo.GetContainer"); 
}