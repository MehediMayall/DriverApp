using Domain.ValueObjects;

namespace API.Controllers;

[ApiController]
[Route("api/{Controller}")]
public class UserController: BaseController
{
    private readonly IMediator mediator;
    private readonly IDeserializeService deserializeService;

    public UserController(IMediator mediator, IHttpContextAccessor httpContext, IDeserializeService deserializeService): base(httpContext)
    {
        this.mediator = mediator;
        this.deserializeService = deserializeService;
    }


    [AllowAnonymous]
    [HttpPost("/api/driver/login")]
    public async Task<Response<LoginResponseDto>> AuthenticateUser([FromBody] LoginRequest requestDto)
    {
        return await mediator.Send(new GetUserTokenQuery(
            new LoginRequestDto(
                UserId:  requestDto.UserId.GetUserID(),
                Password: (NonEmptyString) requestDto.Password,
                ClientID: requestDto.ClientID
            )
            ));
    }


    [AllowAnonymous]
    [HttpPost]
    [Route("/api/driver/logout")]
    public async Task<Response<string>> driverLogout([FromForm] CommonRequestDto request)
    {
        // Log.Information(request.Parameters);
        // Document Request
        var deserializeResult = deserializeService.Get<LogoutRequestDto>(request.Parameters);
        if (deserializeResult.IsFailure) return Response<string>.Error(deserializeResult.Error);

        var requestDto = deserializeResult.Value;   
        await RemoveUserClaim(requestDto);        

        return Response<string>.OK("Successfully logged out");
    }
}