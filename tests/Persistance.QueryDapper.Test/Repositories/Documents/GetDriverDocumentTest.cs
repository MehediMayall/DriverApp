namespace Persistance.Query.Dapper.Test;

public class GetDriverDocumentTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository sut;
    private readonly ContainerMockData mockData;
   
    public GetDriverDocumentTest(ConfigurationFixture fixture)
    {
        sut = new ContainerRepository(fixture.configuration);
        mockData = new ContainerMockData();
   
    }
 

    [Theory]
    [InlineData(27047,8, LegTypes.RETURN_MT )]
    // [InlineData(588967,8, LegTypes.PICKUP )]
    public async Task GetDriverDocument_ShouldReturn_List(int ProNumber, int documentID, string LegType)
    {
        // Arrange
        var documentid = new DocumentID((NonNegative)documentID);
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