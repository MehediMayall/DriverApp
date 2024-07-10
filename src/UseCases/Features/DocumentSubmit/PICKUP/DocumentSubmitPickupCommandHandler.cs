namespace UseCases.Features;
public record DocumentSubmitPickupCommand(DocumentSubmitRequest requestDto) : IRequest<Response<List<WorkQueueModel>>>{}
public class DocumentSubmitPickupCommandHandler: IRequestHandler<DocumentSubmitPickupCommand, Response<List<WorkQueueModel>>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IContainerRepository containerRepo;
    private readonly IWorkQueueService workQueueService;
    private readonly IDirectoryService directoryService;
    private readonly IImageService imageService;

    private readonly IPDFService pDFService;

    private readonly Result<SessionUserDto> sessionUser;

    public DocumentSubmitPickupCommandHandler(
        IUnitOfWork unitOfWork, 
        IContainerRepository containerRepo, 
        IBaseService baseService,             
        IWorkQueueService workQueueService,
        IDirectoryService directoryService,
        IImageService imageService,
        IPDFService pDFService
    )
    {
        this.unitOfWork = unitOfWork;
        this.containerRepo = containerRepo;
        this.workQueueService = workQueueService;
        this.directoryService = directoryService;

        this.imageService = imageService;
        this.pDFService = pDFService;
        sessionUser = baseService.GetSessionUser();
    }

    public async Task<Response<List<WorkQueueModel>>> Handle(DocumentSubmitPickupCommand request, CancellationToken cancellationToken)
    {
        // Arrange & Validate

        DocumentSubmitRequestDto requestDto = DocumentSubmitRequestDto.Get(request.requestDto);

        ContainerRequestDto containerRequest = new(this.sessionUser.Value.driverID, requestDto.ProNumber, requestDto.LegType);

        // Validate Document
        var validator = new DocumentSubmitPickupValidator();
        var validationResult = await validator.ValidateAsync(requestDto);
        if(validationResult.Errors.Count > 0) return Response<List<WorkQueueModel>>.ValidationError(validationResult);

        // Get Container
        var containerResult = await containerRepo.GetContainer(containerRequest);
        if (containerResult.IsFailure) 
            return Response<List<WorkQueueModel>>.Error(DriverContainerModelError<DriverContainerModel>.NoContainerFound(), containerRequest);

        DocumentID DocumentID = DocumentIDs.OUTBOUND;   // OUTBOUND


        // CHECK DUPLICATE REQUEST
        var lastDocumentResult = await containerRepo.GetDriverDocument(containerRequest, DocumentID);
        DriverDocuments lastDocument = lastDocumentResult is null ? null : lastDocumentResult.Value;

        // PREVENT DUPLICATE SUBMIT :: Validate redundant document request.
        if (lastDocument != null && DateTime.Now.Subtract(lastDocument.CreatedOn.GetValueOrDefault()).TotalMinutes <= 1)
        {
            Log.Warning($"Duplicate request found for pro# {lastDocument.ProNumber}, Leg Type: {lastDocument.LegType}, Document ID: {DocumentID.Value}");
            return await ReturnWorkQueue(requestDto);
        }


        DriverContainerModel? container = containerResult.Value;

        // Validate Image
        var imageValidationResult = validator.ValidateImage(requestDto, container);
        if (imageValidationResult.IsFailure) return Response<List<WorkQueueModel>>.Error(imageValidationResult.Error, requestDto);


        Result<FileInfoDto> result = await directoryService.GetDocsQueueDirectoryAndFilename(requestDto, sessionUser.Value);
        if(result.IsFailure) return Response<List<WorkQueueModel>>.Error(result.Error, requestDto);

        FileInfoDto fileInfo = result.Value;


        
        // GET POD IMAGE
        var imageResult =  await imageService.GetPortraitImage(requestDto.ImagesInBase64, requestDto.ProNumber);
        if (imageResult.IsFailure) return Response<List<WorkQueueModel>>.Error(imageResult.Error, requestDto);
        string ImagesInBase64 = imageResult.Value;


        var submitResult = await SubmitPickup(container, requestDto, fileInfo, DocumentID, ImagesInBase64);
        if (submitResult.IsFailure) return Response<List<WorkQueueModel>>.Error(submitResult.Error, requestDto);


        // UPDATE CONTAINER STATUS
        await UpdateContainerStatus(
                container.OrderLogID.GetOrderLogID(), 
                requestDto.ProNumber, 
                requestDto.LegType, 
                request.requestDto.IsResubmit
        );

        // COMMIT DB 
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


    // PICKUP SUBMIT
    private async Task<Result<string>> SubmitPickup(
        DriverContainerModel? container, 
        DocumentSubmitRequestDto requestDto,
        FileInfoDto fileInfo, DocumentID documentID,
        string ImagesInBase64
        )
    {
        ContainerRequestDto containerRequest = new ContainerRequestDto(
            sessionUser.Value.driverID, 
            container.Pro.GetValueOrDefault().GetProNumber(),  
            container.LegType.GetLegType()
        );
        
        var lastDocumentResult = await containerRepo.GetDriverDocument(containerRequest, documentID);
        // if (lastDocumentResult.IsFailure) return Result<string>.Failure(Error<string>.Set(lastDocumentResult.Error.Message));

        DriverDocuments lastDocument = lastDocumentResult.Value;
        DriverDocuments newDocument;

        // IF PAPERLESS TERMINAL
        newDocument = GetNewDocument(container, requestDto, container.IsPaperLess.Value, fileInfo, documentID, sessionUser.Value);

        // UPDATE PREVIOUS DOCUMENT
        if (container.IsPaperLess == TerminalTypes.PAPERLESS && lastDocument != null)
        {
            lastDocument.IsActive = false;
            lastDocument.UpdatedOn = DateTime.Now;
            await this.unitOfWork.DriverRepo.UpdateDriverDocument(lastDocument.Id, lastDocument);
        }
      

        // SAVE NEW DOCUMENT
        await this.unitOfWork.DriverRepo.SaveDriverDocument(newDocument);

        Log.Information($"PDF File: {fileInfo.FileFullPath}");

        // SAVING ATTACHMENT - ignoring if it is paperless terminal
        if(container.IsPaperLess == TerminalTypes.PRINT) await pDFService.ConvertImageToPDF(ImagesInBase64, fileInfo.FileFullPath, newDocument);

        

        return Result<string>.Success("");
    }

    private async Task<Boolean> UpdateContainerStatus(OrderLogID orderLogID, ProNumber ProNumber, LegType LegType, Boolean IsResubmit)
    {
        await this.unitOfWork.DriverRepo.UpdateContainerStatus(
            new DocumentStatusUpdateDto(
                sessionUser.Value.driverID, 
                orderLogID, 
                ProNumber, 
                LegType, 
                IsResubmit
            )
        );
        return true;
    }

    public DriverDocuments GetNewDocument(DriverContainerModel? container, 
            DocumentSubmitRequestDto requestDto, 
            bool IsPaperlessTerminal,
            FileInfoDto fileInfo, DocumentID DocumentID, SessionUserDto sessionUser)
    {

        DriverDocuments newDoc = new DriverDocuments();
        newDoc.ContainerCode = container.ContainerCode;
        newDoc.DriverID = sessionUser.driverID.Value;
        newDoc.ProNumber = requestDto.ProNumber.Value.Value;
        newDoc.LegType = container.LegType;
        newDoc.OutTime =  requestDto.OutTime;
        newDoc.InTime =  requestDto.InTime;
        newDoc.CompanyId = sessionUser.companyID.Value;

        newDoc.MiscellaneousNote = requestDto.MiscellaneousNote;
        newDoc.ReceivedBy = requestDto.ReceivedBy;

        // Received will be filled up from the iTMS application after its document approval
        newDoc.ReceiveDate = null;

        newDoc.CreatedOn = DateTime.Now;
        newDoc.IsActive = true;
        newDoc.DocumentId = DocumentID.Value;
        newDoc.PhysicalFilename = IsPaperlessTerminal is true ? "PaperlessTerminalSample" : fileInfo.FileName;
        newDoc.IsPaperlessTerminal = true;
        newDoc.CreatedById = sessionUser.driverID.Value;

        return newDoc;
    }



}