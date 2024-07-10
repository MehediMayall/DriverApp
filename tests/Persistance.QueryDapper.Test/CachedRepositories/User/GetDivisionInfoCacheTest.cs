namespace Persistance.Query.Dapper.Test;

public class GetDivisionInfoCacheTest : IClassFixture<ConfigurationFixture>
{
    private readonly ContainerRepositoryCached sut;
    private readonly ContainerRepository containerRepo;
     private readonly ICacheService cacheService;

    public GetDivisionInfoCacheTest(ConfigurationFixture fixture)
    {
        cacheService = Substitute.For<ICacheService>();
        containerRepo = new ContainerRepository(fixture.configuration);
        sut = new ContainerRepositoryCached(fixture.configuration, containerRepo, cacheService);
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
    [InlineData(99999)]
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