namespace UseCases.Dtos;

public record LoginRequest(string UserId, string Password, string ClientID);
public record LoginRequestDto(UserID UserId, NonEmptyString Password, string ClientID)
{
    public static LoginRequestDto Get(LoginRequest request)
    {
        return new LoginRequestDto(
            request.UserId.GetUserID(),
            (NonEmptyString)request.Password,
            request.ClientID
        );
    }
}