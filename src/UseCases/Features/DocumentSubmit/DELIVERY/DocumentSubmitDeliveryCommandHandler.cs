namespace UseCases.Features;

public record DocumentSubmitDeliveryCommand(DocumentSubmitRequest requestDto) : IRequest<Response<List<WorkQueueModel>>>{}
public class DocumentSubmitDeliveryCommandHandler: IRequestHandler<DocumentSubmitDeliveryCommand, Response<List<WorkQueueModel>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IContainerRepository containerRepo;
    private readonly IWorkQueueService workQueueService;
    private readonly IDirectoryService directoryService;

    private readonly IImageService imageService;
    private readonly IProofOfDeliveryService podService;
    private readonly IProofOfDeliveryEmailService podEmailService;
    private readonly Result<SessionUserDto> sessionUser;

    public DocumentSubmitDeliveryCommandHandler(IUnitOfWork unitOfWork, 
            IContainerRepository containerRepo, 
            IBaseService baseService,             
            IWorkQueueService workQueueService,
            IDirectoryService directoryService,
            IImageService imageService,
            IProofOfDeliveryService podService,
            IProofOfDeliveryEmailService podEmailService)
    {
        this.unitOfWork = unitOfWork;
        this.containerRepo = containerRepo;
        this.workQueueService = workQueueService;
        this.directoryService = directoryService;
        this.imageService = imageService;
        this.podService = podService;
        this.podEmailService = podEmailService;
        sessionUser = baseService.GetSessionUser() ?? throw new UnauthorizedAccessException(ERRORS.SESSION_USER_NOT_FOUND);
    }

    public async Task<Response<List<WorkQueueModel>>> Handle(DocumentSubmitDeliveryCommand request, CancellationToken cancellationToken)
    {
        // Arrange & Validate
        DocumentSubmitRequestDto requestDto = DocumentSubmitRequestDto.Get(request.requestDto);
        ContainerRequestDto containerRequest = new(this.sessionUser.Value.driverID, requestDto.ProNumber, requestDto.LegType);
  

        // VALIDATE Document
        var validator = new DocumentSubmitValidator();
        var validationResult = await validator.ValidateAsync(requestDto);
        if(validationResult.Errors.Count > 0) return Response<List<WorkQueueModel>>.ValidationError(validationResult);
 
 
        // GET Container
        var containerResult = await containerRepo.GetContainer(containerRequest);
        if (containerResult.IsFailure) 
            return Response<List<WorkQueueModel>>.Error(DriverContainerModelError<DriverContainerModel>.NoContainerFound(), containerRequest);

        DriverContainerModel? container = containerResult.Value;


        // GET Driver Document
        DocumentID documentID = DocumentIDs.INBOUND; 
        var lastDocumentResult = await containerRepo.GetDriverDocument(containerRequest, documentID);
        DriverDocuments lastDocument = lastDocumentResult  is null ? null: lastDocumentResult.Value;

        // PREVENT DUPLICATE SUBMIT:: Validate redundant document request.
        if (lastDocument is not null && DateTime.Now.Subtract(lastDocument.CreatedOn.GetValueOrDefault()).TotalMinutes <= 1)
        {
            Log.Warning($"Duplicate request found for pro# {lastDocument.ProNumber}, Leg Type: {lastDocument.LegType}, Document ID: {documentID}");
            return await ReturnWorkQueue(requestDto);
        }


        // Validate Image
        var imageResult = validator.ValidateSignatureImage(requestDto, container);
        if (imageResult.IsFailure) return Response<List<WorkQueueModel>>.Error(imageResult.Error, containerRequest);


        // Load POD base image path
        Result<string> result = await directoryService.GetPODImagesPath();
        if(result.IsFailure) return Response<List<WorkQueueModel>>.Error(result.Error, containerRequest);
        string PODImagesPath = result.Value;


        // GET POD IMAGE
        var podImageResult =  await imageService.GetPortraitImage( requestDto.ImagesInBase64, requestDto.ProNumber);
        if (podImageResult.IsFailure) return Response<List<WorkQueueModel>>.Error(podImageResult.Error, containerRequest);
   
        // UPDATE CONTAINER

        var submitResult = await UpdateContainerDelivery(container, requestDto, PODImagesPath);
        if (submitResult.IsFailure) return Response<List<WorkQueueModel>>.Error(submitResult.Error, requestDto);


        // UPDATE CONTAINER STATUS
        var containerUpdateResult = await unitOfWork.DriverRepo.UpdateContainerStatus(
            new DocumentStatusUpdateDto(
                sessionUser.Value.driverID, 
                container.OrderLogID.GetOrderLogID(), 
                requestDto.ProNumber, 
                requestDto.LegType, 
                false
            )
        );
                
        if (containerUpdateResult.IsFailure) return Response<List<WorkQueueModel>>.Error(containerUpdateResult.Error, requestDto);

        // DELETE POD IMAGES - CLEAN UP
        await imageService.DeletePODImages( container.Pro.Value.GetProNumber(), PODImagesPath);


        // COMMIT
        unitOfWork.Commit();

        return await ReturnWorkQueue(requestDto);
    }

    private async Task<Response<List<WorkQueueModel>>> ReturnWorkQueue(DocumentSubmitRequestDto requestDto)
    {
        // Return Work Queue
        var workQueueResult = await containerRepo.GetWorkQueue(this.sessionUser.Value.driverID);
        if (workQueueResult.IsFailure) return Response<List<WorkQueueModel>>.Error(workQueueResult.Error, requestDto);

        var WorkQueues = await workQueueService.Get(workQueueResult.Value);
        return Response<List<WorkQueueModel>>.OK(WorkQueues); 
    }



    // PROOF OF DELIVERY -- PDF GENERATION
    private async Task<Result<string>> UpdateContainerDelivery(DriverContainerModel container, DocumentSubmitRequestDto requestDto, string ImagesPath)
    {
  
        Result<FileInfoDto> folderResult = await directoryService.GetPODDirectoryAndFilename(container, sessionUser.Value);
        if(folderResult.IsFailure) return Result<string>.Failure(Error<string>.Set(folderResult.Error.Message));

        FileInfoDto fileInfo = folderResult.Value;
  

        // Check directory is exist. if not then create
        // Scripting.CheckAndCreateDir(fileInfo.FileParentFolder);

        var existingPRO = await containerRepo.GetOrderLogDoc( 
            new DocumentRequestDto( 
                sessionUser.Value.companyID,
                requestDto.ProNumber, 
                DocumentIDs.PROOF_OF_DELIVERY
            ));


        // GENERATING & SAVING PDF FROM TEMPLATE
        var podSaveResult = await podService.SaveProofOfDeliveryPDF(container, fileInfo.FileFullPath, requestDto, ImagesPath);
        if(podSaveResult.IsFailure) return podSaveResult;
        string filePath = podSaveResult.Value;


        // SAVE PRO DOCUMENT -- POD
  
        var result = await podService.SaveProDocument(
            sessionUser.Value, 
            container.OrderLogID.GetOrderLogID(), 
            fileInfo.ParentFolderName, 
            fileInfo.FileName, 
            DocumentIDs.PROOF_OF_DELIVERY
        );


        
        // EMAIL SENDING FOR PROOF OF DELIVERY
        if (!string.IsNullOrEmpty(filePath)) await podEmailService.SendPODASEmail(container, filePath);


        // SEND E MESSAGE IF IT HAS MISC. NOTE                
        if (!string.IsNullOrEmpty(requestDto.MiscellaneousNote)) await SendEmessage(requestDto);


        return Result<string>.Success("");
    }

    private async Task<Boolean> SendEmessage(DocumentSubmitRequestDto document)
    {

        string MiscellaneousNote = "";
        if (document.MiscellaneousNote != null) MiscellaneousNote = document.MiscellaneousNote;

        //saveLog("sendEmessage -> is being called");
        // var result = await this.repo.SendEMessage(document.ProNumber, document.DriverID, document.LegType, MiscellaneousNote);

        return true;

    }

}