using System.Net.Http.Headers;

namespace DriverAPI.IntegrationTests;
public class UserMockData
{
    public readonly UserID userID= new UserID((NonEmptyString)"TX-0088");
    private readonly string Password="f2b48d0c08db8d6b43d7b5d792ce6a60";
    public readonly DriverID driverID = new DriverID((NonNegative)2120);
    public readonly CompanyID companyID = new CompanyID((NonNegative)19);

    public const string LOGIN_API = "/api/driver/login";
    public const string WORK_QUEUE_API = "/api/work/queue/pending";
    public const string VIEW_MOVE_API = "/api/container/view/move/update";
    public const string BEGIN_MOVE_API = "/api/container/begin/move/update";
    public const string DOCUMENT_SUBMIT_API = "/api/driver/document/submit";
    public const string DOCUMENT_RESUBMIT_API = "/api/driver/document/resubmit";
    public const string DOCUMENT_SUBMIT_IMAGE_API = "/api/driver/document/submit/image/upload";
    public const string PREVIEW_INSTRUCTIONS_API = "/api/container/preview/instruction/";
    public const string INSTRUCTIONS_API = "/api/container/instruction/";
    public const string DOCUMENT_DETAILS_API = "/api/container/document/details/";
    public const string DOCUMENT_DETAILS_PDF_API = "/api/container/document/details/pdf/";
    public const string BOL_PDF_API = "/api/container/billoflading/pdf/";


    public LoginRequest GetLoginRequest()
    {
        return new LoginRequest(userID.Value.Value, Password, "");
    }
    public CommonRequestDto GetLoginRequestIncompleteData()
    {
        return new CommonRequestDto(
            Parameters : "{\"User\":\"TX-0088\"}"
        );
    }

    public LoginRequestDto GetLoginRequestDto() => new LoginRequestDto( UserId: userID, Password: new NonEmptyString(Password), ClientID:"");
   
    public Driver GetDriverData()
    {
        return new Driver{            
            DriverId = driverID.Value,
            CompanyId = companyID.Value.ToString(),
            FirstName= "Alfredo",
            UserId = userID.Value,        
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

    public AuthenticationHeaderValue GetToken()
    {
        return new AuthenticationHeaderValue("Bearer", new TestJwtToken().WithUserName(userID.Value).Build());
    }
}