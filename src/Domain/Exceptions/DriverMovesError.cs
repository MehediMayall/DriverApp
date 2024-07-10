
namespace Domain.Exceptions;

public static class  DriverMovesError<DriverMoves> where DriverMoves : class
{     
    public static Error<DriverMoves> DriverMovesNotFound(ProNumber ProNumber)
    {
        return new($"Couldn't find any driver moves for ProNumber: {ProNumber.Value}");
    }
 
    
}