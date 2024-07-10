using System.Runtime.CompilerServices;
using Persistance.Exceptions;

namespace Persistance.Query.Dapper.Test;

public class GetDriverDocumentCacheTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository repo;
    private readonly ContainerMockData mockData;

    private readonly ContainerRepositoryCached sut;
    private readonly ICacheService cacheService;

    public GetDriverDocumentCacheTest(ConfigurationFixture fixture)
    {
        cacheService = Substitute.For<ICacheService>();
        repo = new ContainerRepository(fixture.configuration);
        sut = new ContainerRepositoryCached(fixture.configuration, repo, cacheService);
        
        mockData = new ContainerMockData();
    }
 

    [Theory]
    [InlineData(27047,8, LegTypes.RETURN_MT )]
    // [InlineData(25692,8, LegTypes.PICKUP )]
    public async Task GetDriverDocument_ShouldReturn_List(int ProNumber, int documentID, string LegType)
    {
        // Arrange
        var documentid = new DocumentID((NonNegative) documentID);
        ContainerRequestDto requestDto = new ContainerRequestDto(mockData.driverID, ProNumber.GetProNumber(), LegType.GetLegType());

        // Act
        var result = await sut.GetDriverDocument(requestDto, documentid);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.ProNumber.Should().Be(ProNumber);
    }


    [Theory]
    [InlineData(0,10, LegTypes.RETURN_MT )]
    [InlineData(0,8, LegTypes.PICKUP )]
    public async Task GetDriverDocument_ShouldReturn_NotFound(int ProNumber, int documentID, string LegType)
    {
        // Arrange
        var documentid = new DocumentID((NonNegative) documentID);
        ContainerRequestDto requestDto = new ContainerRequestDto(mockData.driverID, ProNumber.GetProNumber(), LegType.GetLegType());

        // Act
        var result = await sut.GetDriverDocument(requestDto, documentid);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(DriverDocumentError<DriverDocuments>.DriverDocumentNotFound(requestDto.proNumber));
    }



}