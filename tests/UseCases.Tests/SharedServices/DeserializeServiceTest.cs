
using Domain.Exceptions;

namespace UseCases.Tests;

public class DeserializeServiceTest
{
    private readonly DeserializeService sut;
    private readonly ContainerMockData mockData;


    public DeserializeServiceTest()
    {
        this.sut = new DeserializeService();
        this.mockData = new ContainerMockData();
    }


    [Fact]
    public async void DeserializeService_ReturnException_WhenJsonStringIsEmpty()
    {
        // Arrange
        Result<CommonRequestDto> result = SerializeError<CommonRequestDto>.RequestedParameterIsNull();

        //Act
        var ex = await Record.ExceptionAsync(async()=>
        {
            result = sut.Get<CommonRequestDto>("");
        });

        //Assert

        ex.Should().BeNull();
        result.Error.Message.Should().Be(ERRORS.ARGUMENT_OBJECT_NULL);
    }


    [Fact]
    public async void DeserializeService_ReturnException_WhenDeserializedObjectIsNull()
    {
        // Arrange 
        Result<ContainerRequestDto> result;

        //Act
        var ex = await Record.ExceptionAsync(async()=>
        {
            result = sut.Get<ContainerRequestDto>("a invalid json string");
        });

        //Assert

        ex.Should().NotBeNull();
        ex.GetAllExceptions().Should().Contain("invalid");
    }

    [Fact]
    public async void DeserializeService_ReturnDeserializedData()
    {
        // Arrange 
        Result<DriverContainerModel> result = null;
        var mockContainer = mockData.GetPickupMTContainer();

        //Act
        var ex = await Record.ExceptionAsync(async()=>
        {
            result = sut.Get<DriverContainerModel>(mockContainer);
        });

        //Assert

        ex.Should().BeNull();
        result.Should().NotBeNull();
        result.Value.Pro.Should().NotBeNull();
        result.Value.LegType.Should().Be(LegTypes.PICKUP_MT);
    }

}