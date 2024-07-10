namespace Persistance.Query.Dapper.Test;

public class GetUserByPasswordTest : IClassFixture<ConfigurationFixture>
{
    private readonly UserRepository sut;
    private readonly ContainerMockData mockData;

    public GetUserByPasswordTest(ConfigurationFixture fixture)
    {
        sut = new UserRepository(fixture.configuration);
        mockData = new ContainerMockData();
    }

    [Fact]
    public async Task GetUserByPassword_ShouldReturn_User()
    {
        // Arrange
        LoginRequestDto loginRequest = mockData.GetLoginRequestDto();

        // Act
        var result = await sut.GetUserByPassword(loginRequest);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.UserId.Should().Be(loginRequest.UserId.Value);
        result.Value.FirstName.Should().NotBeNullOrEmpty();
        result.Value.CompanyId.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("0","0")]
    public async Task GetUserByPassword_ShouldReturn_UserNotFound(string UseId, string Password)
    {
        // Arrange
        // LoginRequestDto loginRequest = mockData.GetLoginRequestInCompleteDto();
        LoginRequestDto loginRequest = new LoginRequestDto(UseId.GetUserID(),(NonEmptyString)Password,null);

        // Act
        var result = await sut.GetUserByPassword(loginRequest);
 
        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(UserError<Driver>.NoUserDataFoundByUserName(loginRequest.UserId));
    }
}