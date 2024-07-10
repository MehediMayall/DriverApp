namespace UseCases.SharedServices;

public interface IMoveStatusService
{
    Result<MoveStatusID> GetStatus<T>(T container) where T: class;
}