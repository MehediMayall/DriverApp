

namespace UseCases.Contracts.Persistance.Query;

public interface IUserRepository: IAsyncRepository<Driver>
{
    Task<Result<Driver>> GetUserByPassword(LoginRequestDto loginRequest);
    // Task<Result<LoggedUserDetails>> GetUserDetails(UserID UserId);
}