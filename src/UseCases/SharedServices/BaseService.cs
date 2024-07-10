using Microsoft.AspNetCore.Http;

namespace UseCases.SharedServices;

public class BaseService : IBaseService
{
    private readonly IHttpContextAccessor httpContext;

    public BaseService(IHttpContextAccessor httpContextAccessor)
    {
        httpContext = httpContextAccessor;
    }

    public Result<SessionUserDto> GetSessionUser()
    {
        var context = httpContext.HttpContext ?? throw new UnauthorizedAccessException(ERRORS.SESSION_USER_NOT_FOUND);
        var claim = httpContext.HttpContext.User.Claims.FirstOrDefault() ?? throw new UnauthorizedAccessException(ERRORS.SESSION_USER_NOT_FOUND);
        var sessionUser = claim.Value.ConvertTo<SessionUserDto>();
        return Result<SessionUserDto>.Success(sessionUser);
    }

}