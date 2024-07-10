namespace UseCases.Tests;

public class GetDocumentPDFQueryHandlerTest: IClassFixture<ConfigurationFixture>
{
     private readonly IConfiguration configuration;
    private readonly IDirectoryService directoryService;
    private readonly IBaseService baseService;
    private readonly IContainerRepository containerRepo;
    private GetDocumentPDFQueryHandler sut;
    private readonly ContainerMockData mockData;
    private readonly ConfigurationFixture fixture;

    public GetDocumentPDFQueryHandlerTest(ConfigurationFixture fixture)
    {
        configuration = Substitute.For<IConfiguration>();
        directoryService = Substitute.For<IDirectoryService>();
        containerRepo =Substitute.For<IContainerRepository>();
        baseService =Substitute.For<IBaseService>();
        mockData = new ContainerMockData();
        this.fixture = fixture;
    }

    [Fact]
    public async Task GetDocumentPDFQueryHandler_ShouldReturn_GetDivisionFolderNameIsNull()
    {
        // Arrange
        var divisionInfo = mockData.GetDivisionInfo();
        var requestedPDF = mockData.GetPDFRequestDto();

        baseService.GetSessionUser().Returns(mockData.GetSessionData());
        containerRepo.GetDivisionInfo(mockData.companyID).Returns(divisionInfo);

        GetDocumentPDFQuery query =  new GetDocumentPDFQuery(requestedPDF); 
        sut = new GetDocumentPDFQueryHandler(containerRepo, directoryService, configuration, baseService);

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1);
    }

    [Fact]
    public async Task GetDocumentPDFQueryHandler_ShouldReturn_ValidPDFDetails()
    {
        // Arrange
        DocumentRequestDto requestDto = mockData.GetPDFRequestDto();
        var requestedPDF = mockData.GetPDFRequestDto();
        var pdfRequest = new DocumentRequestDto(requestDto.companyID, requestDto.proNumber, mockData.documentID);

        baseService.GetSessionUser().Returns(mockData.GetSessionData());
        directoryService.GetDivisionFolderName(mockData.companyID).Returns(mockData.DIVISION_FOLDER_NAME);
        containerRepo.GetOrderLogDoc(pdfRequest).Returns(mockData.GetOrderLogDocument(mockData.ProNumber.Value, mockData.documentID_Delivery.Value));

        GetDocumentPDFQuery query =  new GetDocumentPDFQuery(requestedPDF); 
        sut = new GetDocumentPDFQueryHandler(containerRepo, directoryService, configuration, baseService);

        // Act
        var result = await sut.Handle(query, default);

        // Assert
        result.Should().NotBeNull();
        result.Data.Should().NotBeNull();
        result.Data.DocumentID.Should().Be(requestedPDF.documentID.Value);
        result.Data.DocumentPath.Should().NotBeNull();
    }
}