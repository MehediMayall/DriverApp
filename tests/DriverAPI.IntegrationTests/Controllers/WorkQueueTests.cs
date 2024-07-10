// namespace DriverAPI.IntegrationTests;

// public class WorkQueueTests: IntegrationTest
// {
//     private readonly string API;

//     public WorkQueueTests(TestApplication application): base(application)
//     {
//         API = $"{UserMockData.WORK_QUEUE_API}";
//     }

//     [Fact]
//     public async void GetWorkQueue_ShouldReturn_Authorization_Error()
//     {
//         // Arrange
//         client.DefaultRequestHeaders.Authorization = null;


//         // Act
//         var response = await client.GetAsync(API);

//         // Assert
//         response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
//     }
    

//     [Fact]
//     public async void GetWorkQueue_ShouldReturn_WorkQueueList()
//     {
//         // Arrange

//         // Act
//         var response = await Get<Response<List<WorkQueueModel>>>(API);

//         // Assert
//         response.Data.Should().HaveCountGreaterThan(0);
//     }


// }