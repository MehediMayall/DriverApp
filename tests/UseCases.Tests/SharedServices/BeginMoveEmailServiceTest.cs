// namespace UseCases.Tests;

// public class BeginMoveEmailServiceTest
// {
//      private readonly IConfiguration configuration;
//     private readonly IContainerRepository containerRepo;
//     private readonly IUnitOfWork_WebPortal unitOfWork;
//     private readonly IDirectoryService directoryService;
//     private readonly IHTMLReportService hTMLReportService;
//     private readonly IEmailService emailService;

//     private BeginMoveEmailService sut;
//     private readonly ContainerMockData mockData;

//     public BeginMoveEmailServiceTest()
//     {
//         this.configuration = Substitute.For<IConfiguration>();
//         this.containerRepo = Substitute.For<IContainerRepository>();
//         this.unitOfWork = Substitute.For<IUnitOfWork_WebPortal>();
//         this.directoryService = Substitute.For<IDirectoryService>();
//         this.hTMLReportService = Substitute.For<IHTMLReportService>();
//         this.emailService = Substitute.For<IEmailService>();

//         mockData = new ContainerMockData();
//         sut = new BeginMoveEmailService(containerRepo, unitOfWork, configuration, directoryService, hTMLReportService, emailService);
//     }

//     [Fact]
//     public async Task SendBeginMoveEmail_ShouldReturn_Sent()
//     {
//         // Arrange
//         ContainerRequestOldDto containerRequestOldDto = mockData.GetContainerRequestOldDto();
//         var legType =  new LegType((NonEmptyString) containerRequestOldDto.LegType);
//         var testContent = Result<BeginMoveEmailContentDto>.Success(new BeginMoveEmailContentDto(){ CONSIGNEE = "test email content", WAREHOUSE = "test email content"});
        
//         var containerRequestDto = new ContainerRequestDto(
//             driverID : mockData.driverID, 
//             LegType : legType.Value.Value.GetLegType(), 
//             ProNumber : new ProNumber((NonNegative)containerRequestOldDto.ProNumber)
//         );
//         var driverContainer = mockData.GetContainer(legType);

//         containerRepo.GetContainer(containerRequestDto).Returns(driverContainer);
//         containerRepo.GetDivisionInfo(Arg.Any<CompanyID>()).Returns(mockData.GetDivisionInfo());
//         hTMLReportService.GetBeginMoveReportHTML(Arg.Any<DriverContainerModel>(),Arg.Any<string>(), Arg.Any<GlobalSetup>(), Arg.Any<EmailLoginTokenDto>()).Returns(testContent);

//         // driverRepo.SaveEmailLoginToken(eToken).retu;
//         // configuration.GetSection("EmailConfiguration").Returns(mockData.GetEmailConfiguration());
//         // configuration.GetSection("IMF_WEBSITE").Returns(mockData.IMF_WEBSITE);


//         // Act
//         var result = await sut.SendBeginMoveEmail(driverContainer.Value, mockData.GetDriverMove(containerRequestDto.ProNumber, legType).Value);

//         // Assert

//         result.Should().NotBeNull();
//         result.Value.Should().Be("Sent!");

//     }

//     [Fact]
//     public async Task SendBeginMoveEmail_ShouldReturn_DeliveryLegMessage()
//     {
//         // Arrange
//         ContainerRequestOldDto containerRequestOldDto = mockData.GetContainerRequestOldDto();
//         var legType = new LegType((NonEmptyString)LegTypes.DELIVERY);
        

//         var containerRequestDto = new ContainerRequestDto(
//             driverID : mockData.driverID, 
//             LegType : legType.Value.Value.GetLegType(), 
//             ProNumber : new ProNumber((NonNegative) containerRequestOldDto.ProNumber)
//         );
//         var driverContainer = mockData.GetContainer(legType);


//         // Act
//         var result = await sut.SendBeginMoveEmail(driverContainer.Value, mockData.GetDriverMove(containerRequestDto.ProNumber, legType ).Value);

//         // Assert

//         result.Should().NotBeNull();
//         result.Value.Should().Be("Skipped due to Delivery Leg");

//     }

// }