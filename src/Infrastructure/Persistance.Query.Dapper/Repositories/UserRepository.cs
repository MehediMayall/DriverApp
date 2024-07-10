namespace Infrastructure.Persistance.Query.Dapper;

public class UserRepository : GenericRepository<Driver>, IUserRepository
{
    public UserRepository(IConfiguration configuration) : 
        base(configuration)
    {
        
    }

    public async Task<Result<Driver>> GetUserByPassword(LoginRequestDto loginRequest)
    {      
        string query = "SELECT TOP(1) * FROM DRIVER WHERE UserID=@Username AND Password=@Password;";
        var data = await db.QueryAsync<Driver>(query, 
        new {
            Username = loginRequest.UserId.Value.Value, 
            Password = loginRequest.Password.Value
        });
        
        if (data.IsNullOrEmpty()) return UserError<Driver>.NoUserDataFoundByUserName(loginRequest.UserId);
        return Result<Driver>.Success(data.FirstOrDefault());
    }

}