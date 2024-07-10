
// namespace UseCases.Tests;

// public class DocumentSubmitDeliveryCommandHandlerTest: IClassFixture<ConfigurationFixture>
// {

//     private readonly IContainerRepository containerRepo;
//     private readonly IWorkQueueService workQueueService;
//     private readonly IUnitOfWork unitOfWork;
//     private readonly IBaseService baseService;
//     private readonly IDirectoryService directoryService;
//     private readonly IImageService imageService;

//     private readonly IProofOfDeliveryService podService;
//     private readonly ContainerMockData mockData;
//     private readonly WorkQueueMockData workQueueMockData;
//     private DocumentSubmitDeliveryCommandHandler sut;

//     public DocumentSubmitDeliveryCommandHandlerTest()
//     {
//         unitOfWork = Substitute.For<IUnitOfWork>();
//         containerRepo = Substitute.For<IContainerRepository>();
//         workQueueService = Substitute.For<IWorkQueueService>();
//         baseService = Substitute.For<IBaseService>();
//         directoryService = Substitute.For<IDirectoryService>();
//         podService = Substitute.For<IProofOfDeliveryService>();
//         imageService = Substitute.For<IImageService>();

//         mockData = new ContainerMockData();
//         workQueueMockData = new WorkQueueMockData();

//         baseService.GetSessionUser().Returns(mockData.GetSessionData());

//         sut = new DocumentSubmitDeliveryCommandHandler(
//             unitOfWork, containerRepo, baseService, 
//             workQueueService, directoryService, imageService, podService);

//     }


//     [Fact]
//     public async Task DocumentSubmitDeliveryCommandHandler_ShouldReturn_RequestValidationFailed()
//     {
//         // Arrange
//         var invalidRequestDto = mockData.GetDocumentSubmit_InvalidRequestDto();
//         DocumentSubmitDeliveryCommand command =  new DocumentSubmitDeliveryCommand(invalidRequestDto); 
        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.Should().NotBeNull();
//         result.Errors.Count().Should().BeGreaterThan(0);
//         result.Errors.FirstOrDefault().Details.Should().NotBeNullOrEmpty();
//     }



//     [Fact]
//     public async Task DocumentSubmitDeliveryCommandHandler_ShouldReturn_NoContainerFound()
//     {
//         // Arrange
//         var requestDto = mockData.GetDocumentSubmitRequestDto(mockData.ProNumber_Delivery.Value, LegTypes.DELIVERY);
//         ContainerRequestDto containerRequest = new(mockData.driverID, new ProNumber((NonNegative) requestDto.ProNumber), requestDto.LegType.GetLegType());
//         var errorResponse = DriverContainerModelError<DriverContainerModel>.NoContainerFound();


//         containerRepo.GetContainer(containerRequest).Returns(errorResponse);

//         DocumentSubmitDeliveryCommand command =  new DocumentSubmitDeliveryCommand(requestDto); 
        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.FirstOrDefault().Details.Should().Be(errorResponse.Message);
//     }


//     [Fact]
//     public async Task DocumentSubmitDeliveryCommandHandler_ShouldReturn_SignatureValidationFailed()
//     {
//         // Arrange
//         var requestDto = mockData.GetDocumentSubmit_NoImageRequestDto(mockData.ProNumber_Delivery.Value, LegTypes.DELIVERY);
//         ContainerRequestDto containerRequest = new(mockData.driverID, new ProNumber((NonNegative) requestDto.ProNumber), requestDto.LegType.GetLegType());
//         var DeliveryContainer =  JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetDeliveryContainer());


//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(DeliveryContainer));

//         DocumentSubmitDeliveryCommand command =  new DocumentSubmitDeliveryCommand(requestDto); 

//         var driverDocument = mockData.GetDocuments(LegTypes.DELIVERY);
//         containerRepo.GetDriverDocument(containerRequest, mockData.documentID_Delivery).Returns(driverDocument);

       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.Should().NotBeNull();
//         result.Errors.FirstOrDefault().Details.Should().Be(ERRORS.INVALID_SIGNATURE_IMAGE);
//     }


//     // [Fact]
//     // public async Task DocumentSubmitDeliveryCommandHandler_ShouldReturn_GetDivisionFolderNameIsNull()
//     // {
//     //     // Arrange
//     //     var requestDto = mockData.GetDocumentSubmitRequestDto(mockData.ProNumber_Delivery.Value, LegTypes.DELIVERY);
//     //     ContainerRequestDto containerRequest = new(mockData.driverID, new ProNumber(requestDto.ProNumber), requestDto.LegType);
//     //     var DeliveryContainer =  JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetDeliveryContainer());
//     //     var errorResponse =  FileInfoError<FileInfoDto>.GetDivisionFolderNameIsNull(mockData.companyID);

//     //     // Specific Pickup Container
//     //     containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(DeliveryContainer));
//     //     containerRepo.GetDriverDocument(containerRequest, Arg.Any<DocumentID>()).Returns(mockData.GetDocuments(containerRequest.LegType));
//     //     directoryService.GetPODImagesPath().Returns(Result<string>.Success(mockData.POD_ImagePath));

//     //     DocumentSubmitDeliveryCommand command =  new DocumentSubmitDeliveryCommand(requestDto); 
 
//     //     // Act
//     //     var result = await sut.Handle(command, default);

//     //     // Assert
//     //     result.Errors.FirstOrDefault().Details.Should().NotBeNullOrEmpty();
//     // }

//    [Fact]
//     public async Task DocumentSubmitDeliveryCommandHandler_ShouldReturn_DuplicateSubmitError()
//     {
//         // Arrange
//         var requestDto = mockData.GetDocumentSubmitRequestDto(mockData.ProNumber_Delivery.Value, LegTypes.DELIVERY);
//         ContainerRequestDto containerRequest = new(mockData.driverID, new ProNumber((NonNegative) requestDto.ProNumber), requestDto.LegType.GetLegType());
//         var DeliveryContainer =  JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetDeliveryContainer());

//         WorkQueueModel workQueue = JsonSerializer.Deserialize<WorkQueueModel>(workQueueMockData.GetWorkQueueDataInString());

//         // Specific Pickup Container
//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(DeliveryContainer));
//         directoryService.GetPODImagesPath().Returns(Result<string>.Success(mockData.POD_ImagePath));

//         var driverDocument = mockData.GetDocuments(LegTypes.DELIVERY);
//         driverDocument.Value.CreatedOn = DateTime.Now;
//         containerRepo.GetDriverDocument(containerRequest, mockData.documentID_Delivery).Returns(driverDocument);

//         // Driver's WorkQueue
//         var workQueueList = new List<WorkQueueModel>(){ workQueue };
//         containerRepo.GetWorkQueue( workQueueMockData.driverID).Returns(Result<List<WorkQueueModel>>.Success(workQueueList));
//         workQueueService.Get(workQueueList).Returns(workQueueList);

//         DocumentSubmitDeliveryCommand command =  new DocumentSubmitDeliveryCommand(requestDto);        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.Should().BeNull();
//         result.Data.Should().HaveCountGreaterThan(0);
//     }


//     [Fact]
//     public async Task DocumentSubmitDeliveryCommandHandler_ShouldReturn_PODImageError()
//     {
//         // Arrange
//         var requestDto = mockData.GetDocumentSubmitRequestDto(mockData.ProNumber_Delivery.Value, LegTypes.DELIVERY);
//         ContainerRequestDto containerRequest = new(mockData.driverID, new ProNumber((NonNegative) requestDto.ProNumber), requestDto.LegType.GetLegType());
//         var DeliveryContainer =  JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetDeliveryContainer());

//         WorkQueueModel workQueue = JsonSerializer.Deserialize<WorkQueueModel>(workQueueMockData.GetWorkQueueDataInString());

//         // Specific Pickup Container
//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(DeliveryContainer));
//         directoryService.GetDocsQueueDirectoryAndFilename(requestDto, mockData.GetSessionData().Value).Returns(mockData.GetPDFInfo());

//         var driverDocument = mockData.GetDocuments(LegTypes.DELIVERY);
//         containerRepo.GetDriverDocument(containerRequest, mockData.documentID_Delivery).Returns(driverDocument);

//         //
//         directoryService.GetPODImagesPath().Returns(Result<string>.Success(mockData.POD_ImagePath));


//         // Driver's WorkQueue
//         var workQueueList = new List<WorkQueueModel>(){ workQueue };
//         containerRepo.GetWorkQueue( workQueueMockData.driverID).Returns(Result<List<WorkQueueModel>>.Success(workQueueList));
//         workQueueService.Get(workQueueList).Returns(workQueueList);

//         // GET POD IMAGE
//         var errorImageResponse = Result<string>.Failure(Error<string>.Set("Failed to convert"));
//         imageService.GetPortraitImage(requestDto.ImagesInBase64,  new ProNumber((NonNegative) requestDto.ProNumber)).ReturnsForAnyArgs(errorImageResponse);


//         DocumentSubmitDeliveryCommand command =  new DocumentSubmitDeliveryCommand(requestDto);        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Errors.Should().NotBeNull();
//         result.Errors.FirstOrDefault().Details.Should().Be("Failed to convert");
//     }

//     [Fact]
//     public async Task DocumentSubmitDeliveryCommandHandler_ShouldReturn_WorkQueue()
//     {
//         // Arrange
//         var requestDto = mockData.GetDocumentSubmitRequestDto(mockData.ProNumber_Delivery.Value, LegTypes.DELIVERY);
//         ContainerRequestDto containerRequest = new(mockData.driverID, new ProNumber((NonNegative) requestDto.ProNumber), requestDto.LegType.GetLegType());
//         var DeliveryContainer =  JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetDeliveryContainer());

      
//         // Specific Pickup Container
//         containerRepo.GetContainer(containerRequest).Returns(Result<DriverContainerModel>.Success(DeliveryContainer));
//         directoryService.GetDocsQueueDirectoryAndFilename(requestDto, mockData.GetSessionData().Value).Returns(mockData.GetPDFInfo());

//         var driverDocument = mockData.GetDocuments(LegTypes.DELIVERY);
//         containerRepo.GetDriverDocument(containerRequest, mockData.documentID_Delivery).Returns(driverDocument);

//         directoryService.GetPODImagesPath().Returns(Result<string>.Success(mockData.POD_ImagePath));



//         // GET POD IMAGE
//         var imageResponse = Result<string>.Success(requestDto.ImagesInBase64);
//         imageService.GetPortraitImage(requestDto.ImagesInBase64,  new ProNumber((NonNegative) requestDto.ProNumber)).ReturnsForAnyArgs(imageResponse);


//         //
//         directoryService.GetPODDirectoryAndFilename(DeliveryContainer, mockData.GetSessionData().Value).Returns(mockData.GetPDFInfo());

//         // ORDERLOG DOC
//         containerRepo.GetOrderLogDoc(Arg.Any<DocumentRequestDto>()).Returns(mockData.GetOrderLogDocument(mockData.ProNumber_Delivery.Value, mockData.documentID_Delivery.Value));

//         podService.SaveProofOfDeliveryPDF(Arg.Any<DriverContainerModel>(), Arg.Any<string>(), Arg.Any<DocumentSubmitRequestDto>(), Arg.Any<string>())
//             .Returns(Result<string>.Success(""));

//         // UPDATE CONTAINER STATUS
//         unitOfWork.DriverRepo.UpdateContainerStatus(Arg.Any<DriverID>(), Arg.Any<OrderLogID>(), Arg.Any<ProNumber>(), Arg.Any<LegType>(), false)
//             .Returns(mockData.GetDriverMove(mockData.ProNumber_Delivery, new LegType((NonEmptyString) requestDto.LegType)));

//         // Driver's WorkQueue
//         var workQueueList = workQueueMockData.GetWorkQueues();
//         containerRepo.GetWorkQueue( workQueueMockData.driverID).Returns(Result<List<WorkQueueModel>>.Success(workQueueList));
//         workQueueService.Get(workQueueList).Returns(workQueueList);

//         DocumentSubmitDeliveryCommand command =  new DocumentSubmitDeliveryCommand(requestDto);        
       
//         // Act
//         var result = await sut.Handle(command, default);

//         // Assert
//         result.Data.Should().NotBeNull();
//         result.Data.Should().HaveCountGreaterThan(0);
//     }


// }