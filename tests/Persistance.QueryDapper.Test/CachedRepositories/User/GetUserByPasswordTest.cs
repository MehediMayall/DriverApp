namespace Persistance.Query.Dapper.Test;

public class GetUserByPasswordCacheTest : IClassFixture<ConfigurationFixture>
{
    private readonly UserRepositoryCached sut;
    private readonly UserRepository userRepo;
    private readonly UserMockData mockData;

    public GetUserByPasswordCacheTest(ConfigurationFixture fixture)
    {
        userRepo = new UserRepository(fixture.configuration);
        sut = new UserRepositoryCached(fixture.configuration, userRepo);
        mockData = new UserMockData();
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
        result.Value.UserId.Should().Be(loginRequest.UserId.Value.Value);
    }

    // [Fact]
    // public async Task GetUserByPassword_ShouldReturn_NotFound()
    // {
    //     // Arrange
    //     LoginRequestDto loginRequest = mockData.GetLoginRequestInCompleteDto();

    //     // Act
    //     var result = await sut.GetUserByPassword(loginRequest);

    //     // Assert
    //     result.Should().NotBeNull();
    //     result.Error.Should().Be(UserError<Driver>.NoUserDataFoundByUserName(new UserID(loginRequest.UserId)));
    // }


     
}