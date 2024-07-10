namespace Persistance.Query.Dapper.Test;

public class GetDivisionInfoTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepository sut;
    private readonly ContainerMockData mockData;
   
    public GetDivisionInfoTest(ConfigurationFixture fixture)
    {
        sut = new ContainerRepository(fixture.configuration);
        mockData = new ContainerMockData();
   
    }
 

    [Theory]
    [InlineData(15)]
    [InlineData(16)]
    public async Task GetDivisionInfo_ShouldReturn_List(int companyID)
    {
        // Arrange
        var companyid = new CompanyID((NonNegative) companyID);

        // Act
        var result = await sut.GetDivisionInfo(companyid);
 

        // Assert
        result.Should().NotBeNull();
        result.Value.CompanyId.Should().Be(companyID.ToString());
    }



    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    public async Task GetDivisionInfo_ShouldReturn_NotFound(int companyID)
    {
        // Arrange
        var companyid = new CompanyID((NonNegative) companyID);

        // Act
        var result = await sut.GetDivisionInfo(companyid);
 

        // Assert
        result.Should().NotBeNull();
        result.Error.Should().Be(GlobalSetupError<GlobalSetup>.GlobalSetupNotFound(companyid));
    }


}