namespace API.Controllers;

public abstract class BaseController: ControllerBase
{
    private readonly IHttpContextAccessor httpContextAccessor;
    public SessionUserDto USER;

    public BaseController(IHttpContextAccessor HttpContextAccessor)
    {
        httpContextAccessor = HttpContextAccessor ?? throw new ArgumentNullException(nameof(HttpContextAccessor));
    }
    
    internal SessionUserDto GetSessionUser()
    {
        var httpContext = httpContextAccessor.HttpContext ?? 
            throw new ArgumentNullException(nameof(HttpContext));
        
        var userClaims = httpContext?.User?.Claims;
        
        if (userClaims is null || !userClaims.Any())
            throw new UnauthorizedAccessException(ERRORS.INVALID_CREDENTIALS);
    
        var claim = userClaims.FirstOrDefault() ??
            throw new UnauthorizedAccessException("Login failed. Please try again. Claim object is empty.");
     
        USER = JsonSerializer.Deserialize<SessionUserDto>(claim.Value) ??
            throw new InvalidOperationException(ERRORS.SESSION_USER_NOT_FOUND);

        return USER;
    }

    internal async Task<bool> RemoveUserClaim(LogoutRequestDto requestDto)
    {
        try
        {
            var claim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault();
            httpContextAccessor.HttpContext.User = null;

        }
        catch (Exception ex){
            Log.Error(ex.GetAllExceptions());
        }
        return true;
    }
   
}
