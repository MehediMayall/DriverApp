namespace UseCases.Tests.Features;

public class GetUserTokenQueryHandler_ShouldReturnExpception: IClassFixture<ConfigurationFixture>
{
    private readonly UserMockData userMockData;
    private readonly IUserRepository userRepository;
    private readonly IContainerRepository containerRepo;

    private readonly ConfigurationFixture fixture;
    private readonly IOptions<TokenManagementDto> TokenManagement;
    public GetUserTokenQueryHandler_ShouldReturnExpception(ConfigurationFixture fixture)
    {
        userMockData = new UserMockData();
        userRepository = Substitute.For<IUserRepository>();
        containerRepo = Substitute.For<IContainerRepository>();
        TokenManagement = Substitute.For<IOptions<TokenManagementDto>>();
 
        this.fixture = fixture;
    }

    [Fact]
    public async Task Handle_Should_ReturnException_WhenRequestDtoIsNull()
    {
        // Arrange
        var query = new GetUserTokenQuery(null);
        var handler = new GetUserTokenQueryHandler(userRepository, containerRepo, TokenManagement);
        Response<LoginResponseDto> response = Response<LoginResponseDto>.Error("");

        // Act
        var ex = await Record.ExceptionAsync(async() => 
        {
            response = await handler.Handle(query, default);
        });

        // Assert
        ex.Should().BeNull();
        Assert.Equal(ERRORS.ARGUMENT_OBJECT_NULL, response.Errors.FirstOrDefault().Details);
    }

    // [Fact]
    // public async Task Handle_Should_ReturnException_WhenRequestDtoFailedToDeserialize()
    // {
    //     // Arrange
    //     var mockUser = userMockData.GetLoginRequestDto();
    //     Result<LoginRequestDto> mockUserDto = SerializeError<LoginRequestDto>.FailedToDeserializeObject();

      
    //     var query = new GetUserTokenQuery(mockUser);
    //     var handler = new GetUserTokenQueryHandler(deserializeService, userRepository, containerRepo, this.fixture.configuration, mapper);

    //     Response<LoginResponseDto> response = Response<LoginResponseDto>.Error("");

    //     // Act
    //     var ex = await Record.ExceptionAsync(async() => 
    //     {
    //         response  = await handler.Handle(query, default);
    //     });

    //     // Assert
    //     ex.Should().BeNull();
    //     Assert.Equal(ERRORS.FAILED_DESERIALIZE_OBJECT, response.Errors.FirstOrDefault().Details);
    // }

    // [Fact]
    // public async Task Handle_ShouldReturnException_WhenRequestDtoValidationFailed()
    // {
    //     // Arrange
    //     LoginRequestDto mockRequest = userMockData.GetLoginRequestInCompleteDto();

    //     var query = new GetUserTokenQuery(mockRequest);
    //     var handler = new GetUserTokenQueryHandler(deserializeService, userRepository, containerRepo, this.fixture.configuration, mapper);

    //     // Act
         
    //     var result = await handler.Handle(query, default);
       

    //     // Assert
    //     result.Should().NotBeNull();
    //     result.Errors.Should().NotBeNull();
    //     result.Errors.FirstOrDefault().Should().NotBeNull();
    //     result.Errors.FirstOrDefault().Details.Should().Be("'Password' must not be empty.");
    // }


    [Fact]
    public async Task Handle_ShouldReturnException_WhenUserNotFound()
    {
        // Arrange
        var mockRequest = userMockData.GetLoginRequestDto();
        LoginRequestDto mockUserDto = userMockData.GetLoginRequestDto();

        
        var query = new GetUserTokenQuery(mockRequest);
        var handler = new GetUserTokenQueryHandler(userRepository, containerRepo, TokenManagement);
        Result<Driver> driver =  UserError<Driver>.NoUserDataFound(new UserID((NonEmptyString) "3"));
        Response<LoginResponseDto> response = Response<LoginResponseDto>.Error("");

        userRepository.GetUserByPassword(mockUserDto).Returns(driver);


        // Act         
        var ex = await Record.ExceptionAsync(async() => 
        {
            response = await handler.Handle(query, default);
        });

        // Assert
        ex.Should().BeNull();
        response.Errors.FirstOrDefault().Details.Should().Contain("Invalid username or password.");        
    }



}