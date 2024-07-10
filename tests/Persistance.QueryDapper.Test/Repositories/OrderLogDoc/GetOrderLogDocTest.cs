using System.Runtime.CompilerServices;
using Persistance.Exceptions;

namespace Persistance.Query.Dapper.Test;

public class GetOrderLogDocTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository sut;
    private readonly ContainerMockData mockData;
   
    public GetOrderLogDocTest(ConfigurationFixture fixture)
    {
        sut = new ContainerRepository(fixture.configuration);
        mockData = new ContainerMockData();
   
    }
 

    [Theory]
    [InlineData(26190,10)]
    [InlineData(25445,8)]
    [InlineData(25445,6)]
    [InlineData(26190,10)]
    public async Task GetOrderLogDoc_ShouldReturn_List(int ProNumber, int documentID)
    {
        // Arrange
        var documentid = new DocumentID((NonNegative) documentID);
        DocumentRequestDto requestDto = new DocumentRequestDto(mockData.companyID, ProNumber.GetProNumber(),documentid);

        // Act
        var result = await sut.GetOrderLogDoc(requestDto);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.ProNumber.Should().Be(ProNumber);
    }


    [Theory]
    [InlineData(25445,0)]
    [InlineData(26190,0)]
    public async Task GetOrderLogDoc_ShouldReturn_NotFound(int ProNumber, int documentID)
    {
        // Arrange
        var proNumber = ProNumber.GetProNumber();
        var documentid = new DocumentID((NonNegative) documentID);
        DocumentRequestDto requestDto = new DocumentRequestDto(mockData.companyID, proNumber,documentid);

        // Act
        var result = await sut.GetOrderLogDoc(requestDto);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(OrderLogDocumentError<OrderLogDocument>.OrderLogDocNotFound(proNumber));
    }

 

    

}