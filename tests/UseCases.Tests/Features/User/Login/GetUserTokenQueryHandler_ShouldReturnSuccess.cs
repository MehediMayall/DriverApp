

namespace UseCases.Tests.Features;

public class GetUserTokenQueryHandler_ShouldReturnSuccess : IClassFixture<ConfigurationFixture>
{
    private readonly UserMockData userMockData;
    private readonly IUserRepository userRepository;
    private readonly IContainerRepository containerRepo;
    private readonly ConfigurationFixture fixture;

    private readonly IOptions<TokenManagementDto> TokenManagement;

    public GetUserTokenQueryHandler_ShouldReturnSuccess(ConfigurationFixture fixture)
    {
        userMockData = new UserMockData();
        userRepository = Substitute.For<IUserRepository>();
        containerRepo = Substitute.For<IContainerRepository>();
        TokenManagement = Substitute.For<IOptions<TokenManagementDto>>();  

        this.fixture = fixture;
    }

    

    [Fact]
    public async Task GetUserTokenQueryHandler_ShouldReturnValidToken()
    {
        // Arrange
        LoginRequestDto mockUserDto = userMockData.GetLoginRequestDto();

        
        var query = new GetUserTokenQuery(mockUserDto);
        var handler = new GetUserTokenQueryHandler(userRepository, containerRepo, TokenManagement);
        Driver driver = userMockData.GetDriverData();


        containerRepo.GetDivisionInfo(Arg.Any<CompanyID>()).Returns(userMockData.GetDivisionInfo());

        userRepository.GetUserByPassword(mockUserDto).Returns(Result<Driver>.Success(driver));

        // Act         
        var result = await handler.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();

        
        LoginResponseDto response =  result.Data;
        response.token.Should().NotBeNullOrEmpty();
        response.user.Should().NotBeNull();
        response.user.UserID.Should().Be(mockUserDto.UserId.Value.Value);
    }


}