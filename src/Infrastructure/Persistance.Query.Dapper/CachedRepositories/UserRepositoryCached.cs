namespace Infrastructure.Persistance.Query.Dapper;

public class UserRepositoryCached : GenericRepository<Driver>, IUserRepository
{
    private readonly UserRepository userRepo;

    public UserRepositoryCached(IConfiguration configuration, UserRepository userRepo) : 
        base(configuration)
    {
        this.userRepo = userRepo;
    }

    public async Task<Result<Driver>> GetUserByPassword(LoginRequestDto requestDto) => await userRepo.GetUserByPassword(requestDto);

}