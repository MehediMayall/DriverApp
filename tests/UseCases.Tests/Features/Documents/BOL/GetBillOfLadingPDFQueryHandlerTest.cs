namespace UseCases.Tests;

public class GetBillOfLadingPDFQueryHandlerTest: IClassFixture<ConfigurationFixture>
{
    private readonly IConfiguration configuration;
    private readonly IHTMLReportService hTMLReportService;
    private readonly IContainerRepository containerRepo;
    private GetBillOfLadingPDFQueryHandler sut;
    private readonly ContainerMockData mockData;
    private readonly ConfigurationFixture fixture;

    public GetBillOfLadingPDFQueryHandlerTest(ConfigurationFixture fixture)
    {
        configuration = Substitute.For<IConfiguration>();
        containerRepo =Substitute.For<IContainerRepository>();
        hTMLReportService = Substitute.For<IHTMLReportService>();

        mockData = new ContainerMockData();
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetBillOfLadingPDFQueryHandler_ShouldReturn_ValidBOL()
    {
        // Arrange
        ContainerRequestDto requestDto = mockData.GetContainerRequestDto();
        var container = mockData.GetContainer(mockData.LegType_Delivery);


        containerRepo.GetContainer(requestDto).Returns(Result<DriverContainerModel>.Success(container.Value));
        hTMLReportService.GetBillOfLadingReportHTML(container.Value).Returns(mockData.GetPDFInfo());

        GetBillOfLadingPDFQuery query =  new GetBillOfLadingPDFQuery(requestDto); 
        sut = new GetBillOfLadingPDFQueryHandler(containerRepo, hTMLReportService,  configuration);

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Data.DocumentName.Should().Be("Bill of Lading");
        result.Data.DocumentPath.Should().Contain(".pdf");
    }



}