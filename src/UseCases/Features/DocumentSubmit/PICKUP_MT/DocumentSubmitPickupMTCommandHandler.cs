namespace UseCases.Features;
public record DocumentSubmitPickupMTCommand(DocumentSubmitRequest requestDto) : IRequest<Response<List<WorkQueueModel>>>{}
public class DocumentSubmitPickupMTCommandHandler: IRequestHandler<DocumentSubmitPickupMTCommand, Response<List<WorkQueueModel>>>
{
    private readonly IDriverRepository repo;
    private readonly IContainerRepository containerRepo;
    private readonly IWorkQueueService workQueueService;
    private readonly Result<SessionUserDto> sessionUser;

    public DocumentSubmitPickupMTCommandHandler(IDriverRepository repo, 
            IContainerRepository containerRepo, 
            IBaseService baseService,             
            IWorkQueueService workQueueService
            )
    {
        this.repo = repo;
        this.containerRepo = containerRepo;
        this.workQueueService = workQueueService;
        sessionUser = baseService.GetSessionUser();
    }

    public async Task<Response<List<WorkQueueModel>>> Handle(DocumentSubmitPickupMTCommand request, CancellationToken cancellationToken)
    {
        // Arrange & Validate

       DocumentSubmitRequestDto requestDto = DocumentSubmitRequestDto.Get(request.requestDto);
        ContainerRequestDto containerRequest = new(
            this.sessionUser.Value.driverID, 
            requestDto.ProNumber, 
            requestDto.LegType
        );

        // Validate Document
        var validator = new DocumentSubmitPickupValidator();
        var validationResult = await validator.ValidateAsync(requestDto);
        if(validationResult.Errors.Count > 0) return Response<List<WorkQueueModel>>.ValidationError(validationResult);

        // GET Driver Document
        DocumentID documentID = DocumentIDs.INBOUND; 
        var lastDocumentResult = await containerRepo.GetDriverDocument(containerRequest, documentID);
        DriverDocuments lastDocument = lastDocumentResult  is null ? null: lastDocumentResult.Value;

        // PREVENT DUPLICATE SUBMIT:: Validate redundant document request.
        if (lastDocument is not null && DateTime.Now.Subtract(lastDocument.CreatedOn.GetValueOrDefault()).TotalMinutes <= 1)
        {
            Log.Warning($"Duplicate request found for pro# {lastDocument.ProNumber}, Leg Type: {lastDocument.LegType}, Document ID: {documentID.Value}");            
            return await ReturnWorkQueue(requestDto);
        }

        // GET Container
        var containerResult = await containerRepo.GetContainer(containerRequest);
        if (containerResult.IsFailure) 
            return Response<List<WorkQueueModel>>.Error(DriverContainerModelError<DriverContainerModel>.NoContainerFound(), containerRequest);

        DriverContainerModel? container = containerResult.Value;



        // // UPDATE CONTAINER STATUS
        await UpdateContainerStatus(
            container.OrderLogID.GetOrderLogID(),
            requestDto.ProNumber, 
            requestDto.LegType, 
            SubmitTypes.NEW_SUBMIT
        );

        // Return Work Queue
        var workQueueResult = await containerRepo.GetWorkQueue(this.sessionUser.Value.driverID);
        if (workQueueResult.IsFailure) return Response<List<WorkQueueModel>>.Error(workQueueResult.Error, containerRequest);

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


    private async Task<Boolean> UpdateContainerStatus(OrderLogID orderLogID, ProNumber ProNumber, LegType LegType, Boolean IsResubmit)
    {
        await this.repo.UpdateContainerStatus(
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



}