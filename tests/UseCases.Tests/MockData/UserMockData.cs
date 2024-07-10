namespace API.Tests.MockData;

public class UserMockData
{
    public readonly UserID userID= new UserID((NonEmptyString)"TX-0088");
    private readonly string Password="f2b48d0c08db8d6b43d7b5d792ce6a60";
    public readonly DriverID driverID = new DriverID((NonNegative) 2120);
    public readonly CompanyID companyID = new CompanyID((NonNegative) 19);

    public CommonRequestDto GetLoginRequest()
    {
        return new CommonRequestDto(
            Parameters : "{\"User\":\""+ userID.Value +"\",\"Password\":\""+ Password +"\",\"ClientID\":\"\",\"ClientDetails\":\"\"}"
        );
    }
    public CommonRequestDto GetLoginRequestIncompleteData()
    {
        return new CommonRequestDto(
            Parameters : "{\"User\":\"TX-0088\"}"
        );
    }

    public LoginRequestDto GetLoginRequestDto() => new LoginRequestDto( UserId: userID, Password: (NonEmptyString)Password, ClientID:"");

    
    public Driver GetDriverData()
    {
        return new Driver{            
            DriverId = driverID.Value.Value,
            CompanyId = companyID.Value.Value.ToString(),
            FirstName= "Alfredo",
            UserId = userID.Value.Value,        
        };
    }

    public Result<SessionUserDto> GetSessionData()
    {
        return Result<SessionUserDto>.Success(
            new SessionUserDto(
            userID : userID,
            driverID : driverID,
            companyID : companyID,
            DriverName: new NonEmptyString("Test User"),
            Division : "TX"
        ));
    }

    public Result<GlobalSetup> GetDivisionInfo()
    {
        return Result<GlobalSetup>.Success(
            new GlobalSetup()
            {                
                CompanyId = companyID.Value.ToString(),
                CompanyName = "TEST COMPANY",
                ScannedDocPath = "scanneddocs",
                State = "TX"
            });
    }
}