namespace UseCases.SharedServices;

public interface IBaseService
{
    Result<SessionUserDto> GetSessionUser();
}
