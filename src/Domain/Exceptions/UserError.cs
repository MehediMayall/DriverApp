namespace Domain.Exceptions;

public static class UserError<T> where T : class
{
    public static Error<T> NoUserDataFound(UserID userID) => 
        new($"Invalid username or password. No user data found. UserID:{userID.Value}", "UserRepository.GetUserDetails");
    public static Error<T> NoUserDataFoundByUserName(UserID userID) => 
        new( $"Invalid username or password. No user data found. Username:{userID.Value}", "UserRepository.GetUserByPassword");
    
}