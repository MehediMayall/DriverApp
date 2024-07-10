using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using UseCases.Features.Login;

namespace UseCases.Features;

public record GetUserTokenQuery(LoginRequestDto requestDto) : IRequest<Response<LoginResponseDto>> {}
public class GetUserTokenQueryHandler: IRequestHandler<GetUserTokenQuery, Response<LoginResponseDto>>
{
    private readonly IUserRepository repo;
    private readonly IContainerRepository containerRepo;
    private readonly TokenManagementDto tokenConfig;

    public GetUserTokenQueryHandler(
        IUserRepository repo, 
        IContainerRepository containerRepo, 
        IOptions<TokenManagementDto> AppSettingsOptions)
    {
        this.repo = repo;
        this.containerRepo = containerRepo;
        tokenConfig = AppSettingsOptions.Value;
    }

    public async Task<Response<LoginResponseDto>> Handle(GetUserTokenQuery request, CancellationToken cancellationToken)
    {
        if (request.requestDto is null) return Response<LoginResponseDto>.Error(ERRORS.ARGUMENT_OBJECT_NULL, "", request.requestDto);
        LoginRequestDto? requestDto = request.requestDto;


        // Validate Login Request Dto
        var validator = new GetUserTokenValidation();
        var validationResult = await validator.ValidateAsync(requestDto);

        if(validationResult.Errors.Count > 0) return Response<LoginResponseDto>.ValidationError(validationResult);

        // Now, find the user using requested credentials
        var userResult = await GetUser(requestDto);
        if(userResult.IsFailure) return Response<LoginResponseDto>.Error(userResult.Error, requestDto);
        Driver driver = userResult.Value;

        CompanyID companyID = driver.CompanyId.GetCompanyID();

        var divisionResult = await containerRepo.GetDivisionInfo(companyID);
        if(divisionResult.IsFailure) return Response<LoginResponseDto>.Error(divisionResult.Error, requestDto);



        SessionUserDto User = new( 
            driverID : driver.DriverId.GetDriverID(), 
            userID : requestDto.UserId, 
            companyID: companyID,
            DriverName: (NonEmptyString) $"{driver?.FirstName} {driver?.LastName}",
            Division:  divisionResult.Value.State
        );

        DriverDetailsDto UserDetails = new(){ 
            KEYSEQUENCE = driver.DriverId, 
            DRIVER_ID = driver.DriverId.ToString(),  
            COMPANYID = driver.CompanyId, 
            FNAME = driver.FirstName,
            LNAME = driver.LastName,
            UserID = driver.UserId,
            MI = driver.MiddleName
        };

        Claim[] claims = GetUserClaims(User);

        string? token = GenerateToken(claims);
        if (token.IsEmptyOrWhiteSpace()) return Response<LoginResponseDto>.Error(ERRORS.FAILED_TO_GENERATE_TOKEN, requestDto);

        return Response<LoginResponseDto>.OK(new LoginResponseDto(token!, UserDetails));
    }


    private string? GenerateToken(Claim[] claims)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenConfig.Secret!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(
            tokenConfig.Issuer,
            tokenConfig.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(tokenConfig.AccessExpirationInMinute),
            signingCredentials: credentials
        );

    
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }

    internal Claim[] GetUserClaims(SessionUserDto userDetails)
    {
        return [
            new Claim(ClaimTypes.Name, userDetails.GetJsonString<SessionUserDto>())
        ];
    }

    private async Task<Result<Driver>> GetUser(LoginRequestDto request)
    {
        // if (!request.IsValid()) throw new Exception(ERRORS.INVALID_CREDENTIALS);
        return await repo.GetUserByPassword(request);        
    }
}