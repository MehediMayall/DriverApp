using UseCases.Responses;

namespace UseCases.Tests;

public class ResponseTest
{
    private readonly ContainerMockData mockData;
    public ResponseTest()
    {
        mockData = new ContainerMockData();
    }

    [Theory]
    [InlineData(ERRORS.ARGUMENT_OBJECT_NULL)]
    [InlineData(ERRORS.FAILED_DESERIALIZE_OBJECT)]
    [InlineData(ERRORS.FAILED_TO_GENERATE_TOKEN)]
    [InlineData(ERRORS.INVALID_CREDENTIALS)]
    public async Task Response_ShouldReturn_ErrorResponse(string Message)
    {
        // Arrange
        Exception ex = new Exception(Message);


        // Act
        Response<string> response = Response<string>.Error(ex);

        // Assert
        response.Should().NotBeNull();
        response.Errors.FirstOrDefault().Details.Should().Contain(Message);
    }

    [Fact]
    public async Task Response_ShouldReturn_GenericErrorResponse()
    {
        // Arrange
        Error<FileInfo> fileError = FileInfoError<FileInfo>.GetDivisionFolderNameIsNull(mockData.companyID);

        // Act
        Response<FileInfo> response = Response<FileInfo>.Error(fileError);

        // Assert
        response.Should().NotBeNull();
        response.Errors.Should().NotBeNull();
    }


    [Fact]
    public async Task Response_ShouldReturn_ErrorDetailResponse()
    {
        // Arrange
        Response<Exception> response;
 
        // Act
        try
        {            
            throw  new Exception(ERRORS.ARGUMENT_OBJECT_NULL, new Exception(ERRORS.DATA_OBJECT_NULL, new Exception(ERRORS.INCORRECT_USERNAME)));            
        }
        catch(Exception ex)
        {           
            response = Response<Exception>.Error(ex);
            // Assert
            response.Should().NotBeNull();
            response.Errors.Should().NotBeNull();
        }
    }
}