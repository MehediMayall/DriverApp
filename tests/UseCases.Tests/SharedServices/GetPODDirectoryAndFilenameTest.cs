namespace UseCases.Tests;

public class GetPODDirectoryAndFilenameTest : IClassFixture<ConfigurationFixture>
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

    public GetPODDirectoryAndFilenameTest(ConfigurationFixture fixture)
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

    [Fact]
    public async Task GetPODDirectoryAndFilename_ShouldReturn_RootFolderNotFound()
    {
        // Arrange
        GlobalSetup division = null;
        DriverContainerModel container = mockData.GetPickupMTContainerModelDto();
        
        cacheService.GetOrCreateAsync<GlobalSetup>("Company",null).Returns(division);

        // containerRepo.GetDivisionInfo(Arg.Any<CompanyID>()).Returns(mockData.GetDivisionInfo());

        sut = new DirectoryService(webHost, cacheService, containerRepo, fileOperationService,
                attachmentDirectories,
                templatePaths, 
                staticFiles);

        // Act
        Result<FileInfoDto> result = await sut.GetPODDirectoryAndFilename(container, mockData.GetSessionData().Value);

        // Assert
        result.Should().NotBeNull();
        result.Error.Message.Should().Be(FileInfoError<FileInfoDto>.GetDivisionFolderNameIsNull(mockData.companyID).Message);
    }

    [Fact]
    public async Task GetPODDirectoryAndFilename_ShouldReturn_ValidFileInfo()
    {
        // Arrange
        GlobalSetup division = null;
        DriverContainerModel container = mockData.GetPickupMTContainerModelDto();
        
        cacheService.GetOrCreateAsync<GlobalSetup>("Company",null).Returns(division);

        containerRepo.GetDivisionInfo(mockData.companyID).Returns(mockData.GetDivisionInfo());

        sut = new DirectoryService(webHost,  cacheService, containerRepo, fileOperationService,
                attachmentDirectories,
                templatePaths, 
                staticFiles);

        // Act
        Result<FileInfoDto> result = await sut.GetPODDirectoryAndFilename(container, mockData.GetSessionData().Value);

        // Assert
        result.Should().NotBeNull();
        result.Value.Should().NotBeNull();
        result.Value.FileName.Should().NotBeNullOrEmpty();
        result.Value.FileFullPath.Should().NotBeNullOrEmpty();
        result.Value.FileParentFolder.Should().NotBeNullOrEmpty();
    }
}