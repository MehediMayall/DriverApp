

namespace UseCases.Tests;

public class DirectoryServiceTest: IClassFixture<ConfigurationFixture>
{
    private readonly ICacheService cacheService;
    private readonly IContainerRepository containerRepo;
    private readonly IWebHostEnvironment webHost;
    private readonly IFileOperationService fileOperationService;

    private DirectoryService sut;
    private readonly ContainerMockData mockData;
    private readonly ConfigurationFixture fixture;

    private readonly IOptions<AttachmentDirectoriesDto> attachmentDirectories;
    private readonly IOptions<TemplatePathsDto> templatePaths;
    private readonly IOptions<StaticFileDto> staticFiles;

    public DirectoryServiceTest(ConfigurationFixture fixture)
    {
        cacheService = Substitute.For<ICacheService>();
        containerRepo =Substitute.For<IContainerRepository>();
        webHost = Substitute.For<IWebHostEnvironment>();
        fileOperationService = Substitute.For<IFileOperationService>();


        attachmentDirectories = Substitute.For<IOptions<AttachmentDirectoriesDto>>();
        templatePaths = Substitute.For<IOptions<TemplatePathsDto>>();
        staticFiles = Substitute.For<IOptions<StaticFileDto>>();

        mockData = new ContainerMockData();
        this.fixture = fixture;
    }

    [Theory]
    [InlineData("IMPORT")]
    [InlineData("EXPORT")]
    [InlineData("LOOSE_FRIEGHT")]
    public async Task GetPODTemplatePath_ShouldValidPath(string loadType)
    {
        // Arrange
        DriverContainerModel container = mockData.GetPickupMTContainerModelDto();
        container.LoadType = loadType;
        webHost.WebRootPath.Returns(mockData.API_Project_Dir);
        sut = new DirectoryService(webHost, cacheService, containerRepo, fileOperationService,
                attachmentDirectories,
                templatePaths, 
                staticFiles);

        // Act
        string result = sut.GetPODTemplatePath(new LoadType(container.LoadType));

        // Assert

        result.Should().NotBeNull();
        if(loadType == "IMPORT") result.Should().Be(mockData.GetPODTemplatePath(new LoadType(loadType)) , result);
        else if(loadType == "EXPORT") result.Should().Be(mockData.GetPODTemplatePath(new LoadType(loadType)), result);
        else result.Should().Be(mockData.GetPODTemplatePath(new LoadType(loadType)), result);
    }

    

}
