namespace UseCases.SharedServices;

public class MoveStatusService : IMoveStatusService
{

    public Result<MoveStatusID> GetStatus<T>(T container) where T: class
    {
        var ViewMovedOn = container.Get("ViewMovedOn");
        var BeginMovedOn = container.Get("BeginMovedOn");
        int moveStatusID = 0;

        if (ViewMovedOn == null) moveStatusID = 0;
        else if (ViewMovedOn != null && BeginMovedOn == null) moveStatusID = 1;
        else moveStatusID = 2;        

        return Result<MoveStatusID>.Success(new MoveStatusID((NonNegative)moveStatusID));
    }
}