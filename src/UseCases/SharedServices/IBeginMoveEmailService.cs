namespace UseCases.SharedServices;

public interface IBeginMoveEmailService
{
    Task<Result<string>> SendBeginMoveEmail(DriverContainerModel container, DriverMoves driverMove);
}