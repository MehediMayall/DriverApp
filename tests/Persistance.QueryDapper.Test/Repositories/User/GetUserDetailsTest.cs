// namespace Persistance.Query.Dapper.Test;

// public class GetUserDetailsTest : IClassFixture<ConfigurationFixture>
// {
//     private readonly UserRepository sut;
//     private readonly ContainerMockData mockData;

//     public GetUserDetailsTest(ConfigurationFixture fixture)
//     {
//         sut = new UserRepository(fixture.configuration);
//         mockData = new ContainerMockData();
//     }

//     [Fact]
//     public async Task GetUserByPassword_ShouldReturn_User()
//     {
//         // Arrange
//         LoginRequestDto loginRequest = mockData.GetLoginRequestDto();

//         // Act
//         var result = await sut.GetUserDetails(new UserID("0"));
 

//         // Assert
//         result.Should().NotBeNull();
//         // result.Value.UserId.Should().Be(loginRequest.UserId);
//         // result.Value.FirstName.Should().NotBeNullOrEmpty();
//         // result.Value.CompanyId.Should().NotBeNullOrEmpty();
//     }

//     [Theory]
//     [InlineData("CA206")]
//     [InlineData("NJ-0000")]
//     [InlineData(null)]
//     [InlineData("00")]
//     [InlineData("sc")]
//     public async Task GetUserDetailsTest_ShouldReturn_UserNotFound(string UseId)
//     {
//         // Arrange
//         var userid = new UserID(UseId);

//         // Act
//         var result = await sut.GetUserDetails(userid);
 
//         // Assert
//         result.Should().NotBeNull();
//         result.Error.Should().Be(UserError<LoggedUserDetails>.NoUserDataFound(userid));
//     }
// }