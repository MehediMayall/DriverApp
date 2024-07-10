namespace UseCases.Tests;

public class ProofOfDeliveryServiceTest: IClassFixture<ConfigurationFixture>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IImageService imageService;
    private readonly IDirectoryService directoryService;
    private readonly IPDFService pDFService;
    private readonly IFileOperationService fileOperationService;
    private readonly ContainerMockData mockData;

    private ProofOfDeliveryService sut;

    public ProofOfDeliveryServiceTest()
    {
        unitOfWork = Substitute.For<IUnitOfWork>();
        directoryService = Substitute.For<IDirectoryService>();
        imageService = Substitute.For<IImageService>();
        pDFService = Substitute.For<IPDFService>();
        fileOperationService = Substitute.For<IFileOperationService>();

        mockData = new ContainerMockData();


        sut = new ProofOfDeliveryService(unitOfWork, imageService, directoryService, pDFService, fileOperationService);
    }

    [Fact]
    public async Task  SaveProDocument_ShouldReturn_SaveProofOfDeliveryPDF()
    {
        // Arrange
        var sessionUser = mockData.GetSessionData().Value;
        var fileinfo = mockData.GetPDFInfo().Value;
        var orderLogDoc = mockData.GetOrderLogDoc(mockData.ProNumber_Delivery.Value, mockData.documentID_Delivery.Value);
        var deliveryContainer = JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetDeliveryContainer());
        var documentRequest = mockData.GetDocumentSubmitRequestDto(mockData.ProNumber_Delivery.Value, LegTypes.DELIVERY);
        var loadType = new LoadType(deliveryContainer.LoadType);

        directoryService.GetPODTemplatePath(loadType)
            .Returns(mockData.GetPODTemplatePath(loadType));
        
        fileOperationService.ReadFile(Arg.Any<string>()).Returns(mockData.GetPODTemplate(loadType));

        imageService.GetImagesAsString(Arg.Any<ProNumber>(), Arg.Any<string>()).Returns(Result<string>.Success(mockData.GetPODImageAsString()));

        pDFService.GeneratePODPDF(Arg.Any<string>(), Arg.Any<string>()).Returns(fileinfo.FileFullPath);
        // repo.SaveProDocument(Arg.Any<OrderLogDoc>()).Returns(orderLogDoc.Value);


        // Act
        var result = await sut.SaveProofOfDeliveryPDF(
                deliveryContainer, fileinfo.FileFullPath, documentRequest, fileinfo.FileParentFolder
            );
            

        // Assert
        result.Value.Should().NotBeNullOrEmpty(fileinfo.FileFullPath);
    }


    // [Fact]
    // public async Task  SaveProDocument_ShouldReturn_SendPODASEmail()
    // {
    //     // Arrange
    //     var deliveryContainer = JsonSerializer.Deserialize<DriverContainerModel>(mockData.GetDeliveryContainer());

    //     List<string> attachments= null;
    //     emailService.Send(Arg.Any<string>(),Arg.Any<string>(),Arg.Any<string>(), true, attachments).Returns(true);

    //     // Act
    //     var result = await sut.SendPODASEmail(deliveryContainer, mockData.GetSamplePODPDFPath());

    //     // Assert
    //     result.Value.Should().BeEmpty();
    // }


    [Fact]
    public async Task  SaveProDocument_ShouldReturn_OrderLogDoc()
    {
        // Arrange
        var sessionUser = mockData.GetSessionData().Value;
        var fileinfo = mockData.GetPDFInfo().Value;
        var orderLogDoc = mockData.GetOrderLogDoc(mockData.ProNumber_Delivery.Value, mockData.documentID_Delivery.Value);

        fileOperationService.GetFileWithoutExtension(fileinfo.FileName).Returns(mockData.FILE_NAME_WITHOUT_EXT);
        unitOfWork.DriverRepo.SaveProDocument(Arg.Any<OrderLogDoc>()).Returns(orderLogDoc.Value);


        // Act
        var result = await sut.SaveProDocument(sessionUser, mockData.orderLogID, fileinfo.FileParentFolder, fileinfo.FileName, mockData.documentID_Delivery);

        // Assert
        result.Value.Should().BeEquivalentTo<OrderLogDoc>(orderLogDoc.Value);
    }

}