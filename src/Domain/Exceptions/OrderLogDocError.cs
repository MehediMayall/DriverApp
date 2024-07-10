namespace Domain.Exceptions;

public static class OrderLogDocError<T> where T : class
{     
    public static Error<T> OrderLogDocNotFound(ProNumber ProNumber)
    {
        return new($"Couldn't find any Order Log Document using ProNumber: {ProNumber.Value}.", "ContainerRepository.GetOrderLogDoc");
    }
}