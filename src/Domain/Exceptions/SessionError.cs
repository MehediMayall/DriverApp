

namespace Domain.Exceptions;

public static class SessionError<SessionUserDto> where SessionUserDto : class
{
    public static Error<SessionUserDto> SessionUserNotFound() => new(ERRORS.SESSION_USER_NOT_FOUND, "BaseService.GetSessionUser");

}