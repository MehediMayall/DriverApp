// namespace UseCases.Tests;

// public class DocumentSubmitPickupCommandHandlerTest: IClassFixture<ConfigurationFixture>
// {

//     private readonly IContainerRepository containerRepo;
//     private readonly IWorkQueueService workQueueService;
//     private readonly IUnitOfWork unitOfWork;
//     private readonly IBaseService baseService;
//     private readonly IDirectoryService directoryService;
//     private readonly IImageService imageService;
//     private readonly IPDFService pDFService;
//     private readonly ContainerMockData mockData;
//     private readonly WorkQueueMockData workQueueMockData;
//     private DocumentSubmitPickupCommandHandler sut;

//     public DocumentSubmitPickupCommandHandlerTest()
//     {
//         unitOfWork = Substitute.For<IUnitOfWork>();
//         containerRepo = Substitute.For<IContainerRepository>();
//         workQueueService = Substitute.For<IWorkQueueService>();
//         baseService = Substitute.For<IBaseService>();
//         directoryService = Substitute.For<IDirectoryService>();
//         imageService = Substitute.For<IImageService>();
//         pDFService = Substitute.For<IPDFService>();

//         mockData = new ContainerMockData();
//         workQueueMockData = new WorkQueueMockData();

//         baseService.GetSessionUser().Returns(mockData.GetSessionData());

//         sut = new DocumentSubmitPickupCommandHandler(unitOfWork, containerRepo, baseService, workQueueService, directoryService, imageService, pDFService);

//     }


//     [Fact]
//     public async Task DocumentSubmitPickupCommandHandler_ShouldReturn_RequestValidationFailed()
//     {
//         // Arrange
//         var invalidRequestDto = mockData.GetDocumentSubmit_InvalidRequestDto();

//         DocumentSubmitPickupCommand command =  new DocumentSubmitPickupCommand(invalidRequestDto); 

       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.Should().NotBeNull();
//         result.Errors.Count().Should().BeGreaterThan(0);
//         result.Errors.FirstOrDefault().Details.Should().NotBeNullOrEmpty();
//     }



//     [Fact]
//     public async Task DocumentSubmitPickupCommandHandler_ShouldReturn_NoContainerFound()
//     {
//         // Arrange
//         var requestDto = mockData.GetDocumentSubmitRequestDto(mockData.ProNumber.Value);
//         ContainerRequestDto containerRequest = new(mockData.driverID, mockData.ProNumber, requestDto.LegType.GetLegType());
//         var errorResponse = DriverContainerModelError<DriverContainerModel>.NoContainerFound();


//         containerRepo.GetContainer(containerRequest).Returns(errorResponse);

//         DocumentSubmitPickupCommand command =  new DocumentSubmitPickupCommand(requestDto);         
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.FirstOrDefault().Details.Should().Be(errorResponse.Message);
//     }


//      [Fact]
//     public async Task DocumentSubmitPickupCommandHandler_ShouldReturn_ImageValidationFailed()
//     {
//         // Arrange
//         var documentID = new DocumentID((NonNegative) 8);
//         var driverDocument = mockData.GetDocuments(LegTypes.PICKUP);

//         var requestDto = mockData.GetDocumentSubmit_NoImageRequestDto(mockData.ProNumber_Pickup.Value, LegTypes.PICKUP);
//         ContainerRequestDto containerRequest = new(mockData.driverID, new ProNumber((NonNegative) requestDto.ProNumber), requestDto.LegType.GetLegType());
//         var pickupContainer = JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetPickupContainer());


//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(pickupContainer));
//         containerRepo.GetDriverDocument(containerRequest, documentID).Returns(driverDocument);

//         DocumentSubmitPickupCommand command =  new DocumentSubmitPickupCommand(requestDto); 
        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.Should().NotBeNull();
//         result.Errors.FirstOrDefault().Details.Should().Be(ERRORS.INVALID_IMAGE);
//     }



//     [Fact]
//     public async Task DocumentSubmitPickupCommandHandler_ShouldReturn_GetDivisionFolderNameIsNull()
//     {
//         // Arrange
//         var documentID = new DocumentID((NonNegative) 8);
//         var driverDocument = mockData.GetDocuments(LegTypes.PICKUP);

//         var requestDto = mockData.GetDocumentSubmitRequestDto(mockData.ProNumber.Value, LegTypes.PICKUP);
//         ContainerRequestDto containerRequest = new(mockData.driverID, mockData.ProNumber, requestDto.LegType.GetLegType());
//         var errorResponse =  FileInfoError<FileInfoDto>.GetDivisionFolderNameIsNull(mockData.companyID);
//         var pickupContainer = JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetPickupContainer());

//         // Specific Pickup Container
//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(pickupContainer));
//         containerRepo.GetDriverDocument(containerRequest, documentID).Returns(driverDocument);
//         directoryService.GetDocsQueueDirectoryAndFilename(requestDto, mockData.GetSessionData().Value).Returns(errorResponse);

//         DocumentSubmitPickupCommand command =  new DocumentSubmitPickupCommand(requestDto); 
 
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.FirstOrDefault().Details.Should().NotBeNullOrEmpty();
//     }

//     [Fact]
//     public async Task DocumentSubmitPickupCommandHandler_ShouldReturnWorkQueue_WhenDuplicateSubmit()
//     {
//         // Arrange
//         var documentID = new DocumentID((NonNegative) 8);
//         var pickupContainer = JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetPickupContainer());
//         var requestDto = mockData.GetDocumentSubmitRequestDto(pickupContainer.Pro.Value, LegTypes.PICKUP);
//         ContainerRequestDto containerRequest = new(new DriverID((NonNegative) requestDto.DriverID), new ProNumber((NonNegative) pickupContainer.Pro.Value) , requestDto.LegType.GetLegType());

//         WorkQueueModel workQueue = JsonSerializer.Deserialize<WorkQueueModel>(workQueueMockData.GetWorkQueueDataInString());

//         // Specific Pickup Container
//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(pickupContainer));
//         directoryService.GetDocsQueueDirectoryAndFilename(requestDto, mockData.GetSessionData().Value).Returns(mockData.GetPDFInfo());

//         var driverDocument = mockData.GetDocuments(LegTypes.PICKUP);
//         driverDocument.Value.CreatedOn = DateTime.Now;
//         containerRepo.GetDriverDocument(containerRequest, documentID).Returns(driverDocument);

//         // GET POD IMAGE
//         var imageResponse = Result<string>.Success(requestDto.ImagesInBase64);
//         imageService.GetPortraitImage(requestDto.ImagesInBase64, new ProNumber((NonNegative) requestDto.ProNumber)).ReturnsForAnyArgs(imageResponse);

//         // Driver's WorkQueue
//         var workQueueList = new List<WorkQueueModel>(){ workQueue };
//         containerRepo.GetWorkQueue( workQueueMockData.driverID).Returns(Result<List<WorkQueueModel>>.Success(workQueueList));
//         workQueueService.Get(workQueueList).Returns(workQueueList);

//         DocumentSubmitPickupCommand command =  new DocumentSubmitPickupCommand(requestDto);        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.Should().BeNull();
//         result.Data.Should().HaveCountGreaterThan(0);
//     }


//     [Fact]
//     public async Task DocumentSubmitPickupCommandHandler_ShouldReturn_WorkQueue()
//     {
//         // Arrange
//         var documentID = new DocumentID((NonNegative) 8);
//         var pickupContainer = JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetPickupContainer());
//         var requestDto = mockData.GetDocumentSubmitRequestDto(pickupContainer.Pro.Value, LegTypes.PICKUP);
//         ContainerRequestDto containerRequest = new(new DriverID((NonNegative) requestDto.DriverID), new ProNumber((NonNegative) pickupContainer.Pro.Value) , requestDto.LegType.GetLegType());

//         WorkQueueModel workQueue = JsonSerializer.Deserialize<WorkQueueModel>(workQueueMockData.GetWorkQueueDataInString());

//         // Specific Pickup Container
//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(pickupContainer));
//         directoryService.GetDocsQueueDirectoryAndFilename(requestDto, mockData.GetSessionData().Value).Returns(mockData.GetPDFInfo());
//         containerRepo.GetDriverDocument(containerRequest, documentID).Returns(mockData.GetDocuments(LegTypes.PICKUP));

//         // GET POD IMAGE
//         var imageResponse = Result<string>.Success(requestDto.ImagesInBase64);
//         imageService.GetPortraitImage(requestDto.ImagesInBase64,  new ProNumber((NonNegative) requestDto.ProNumber)).ReturnsForAnyArgs(imageResponse);

//         // Driver's WorkQueue
//         var workQueueList = new List<WorkQueueModel>(){ workQueue };
//         containerRepo.GetWorkQueue( workQueueMockData.driverID).Returns(Result<List<WorkQueueModel>>.Success(workQueueList));
//         workQueueService.Get(workQueueList).Returns(workQueueList);

//         DocumentSubmitPickupCommand command =  new DocumentSubmitPickupCommand(requestDto);        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Data.FirstOrDefault().Should().Be(workQueueList.FirstOrDefault());
//     }


//     [Fact]
//     public async Task DocumentSubmitPickupCommandHandler_ShouldReturn_WorkQueue_Paperless()
//     {
//         // Arrange
//         var documentID = new DocumentID((NonNegative) 8);
//         var pickupContainer = JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetPickupContainer());
//         pickupContainer.IsPaperLess = true;
//         var requestDto = mockData.GetDocumentSubmitRequestDto(pickupContainer.Pro.Value, LegTypes.PICKUP);
//         ContainerRequestDto containerRequest = new(new DriverID((NonNegative) requestDto.DriverID), new ProNumber((NonNegative) pickupContainer.Pro.Value) , requestDto.LegType.GetLegType());

//         WorkQueueModel workQueue = JsonSerializer.Deserialize<WorkQueueModel>(workQueueMockData.GetWorkQueueDataInString());

//         // Specific Pickup Container
//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(pickupContainer));
//         directoryService.GetDocsQueueDirectoryAndFilename(requestDto, mockData.GetSessionData().Value).Returns(mockData.GetPDFInfo());
//         containerRepo.GetDriverDocument(containerRequest, documentID).Returns(mockData.GetDocuments(LegTypes.PICKUP));

//         // GET POD IMAGE
//         var imageResponse = Result<string>.Success(requestDto.ImagesInBase64);
//         imageService.GetPortraitImage(requestDto.ImagesInBase64, requestDto.ProNumber.GetProNumber()).ReturnsForAnyArgs(imageResponse);

//         // Driver's WorkQueue
//         var workQueueList = new List<WorkQueueModel>(){ workQueue };
//         containerRepo.GetWorkQueue( workQueueMockData.driverID).Returns(Result<List<WorkQueueModel>>.Success(workQueueList));
//         workQueueService.Get(workQueueList).Returns(workQueueList);


//         DocumentSubmitPickupCommand command =  new DocumentSubmitPickupCommand(requestDto);        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Data.FirstOrDefault().Should().Be(workQueueList.FirstOrDefault());
//     }

// }