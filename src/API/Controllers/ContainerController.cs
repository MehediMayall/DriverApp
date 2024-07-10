namespace  API.Controllers;


[Authorize]
[ApiController]
[Route("api/[Controller]")]
public class ContainerController: BaseController
{
    private readonly IMediator mediator;

    public ContainerController(IMediator mediator, IHttpContextAccessor httpContext):base(httpContext)
    {
        this.mediator = mediator;
    }

    // WORK QUEUE
    [HttpGet("/api/work/queue/pending")]
    public async Task<Response<List<WorkQueueModel>>> GetWorkQueue()
    {
        return await mediator.Send(new GetWorkQueueQuery());
    }

    // // PREVIEW INSTRUCTION
    [HttpGet("preview/instruction/{pronumber}/{legtype}")]
    public async Task<Response<DriverContainerModel>> GetPreviewInstruction(int pronumber, string legtype)
    {
        var user =  GetSessionUser();
        return await mediator.Send(new GetPreviewInstructionQuery(
            ContainerRequestDto.Get(user.driverID, pronumber, legtype)
        ));
    }

    // INSTRUCTION
    [HttpGet("instruction/{pronumber}/{legtype}")]
    public async Task<Response<DriverContainerModel>> GetInstructions(int pronumber, string legtype)
    {
        var user =  GetSessionUser();
        return await mediator.Send(new GetPreviewInstructionQuery(ContainerRequestDto.Get(user.driverID, pronumber, legtype)));
    }

    // DOCUMENT & TOTAL DETAILS
    [HttpGet("document/details/{pronumber}/{legtype}")]
    public async Task<Response<DocumentDetailAndPODto>> GetDocumentDetails(int pronumber, string legtype)
    {
        var user =  GetSessionUser();

        // Container
        var instructionsResponse = await this.mediator.Send(new GetPreviewInstructionQuery(
            ContainerRequestDto.Get( user.driverID, pronumber, legtype)
        ));
        
        if (instructionsResponse.Status == ResponseStatus.ERROR)
            return Response<DocumentDetailAndPODto>.Error(instructionsResponse.Errors.FirstOrDefault()!.Details);
        
        DriverContainerModel? container = instructionsResponse.Data;

        // PO List
        var purchaseOrdersResult = await this.mediator.Send(new GetPurchaseOrderQuery(
            ContainerRequestDto.Get(user.driverID, pronumber, legtype)
        ));

        var POList = new List<PODto>();
        if (purchaseOrdersResult.IsSuccess)
            POList.AddRange(purchaseOrdersResult.Value.Select(x => new PODto(x.PurchaseOrderNo)));

        
        if (container != null) POList.Add(new PODto(container?.PO));
        var data = new DocumentDetailAndPODto(container, POList);

        return Response<DocumentDetailAndPODto>.OK(data);
    }

    // DOCUMENT PDF
    [HttpGet("document/details/pdf/{pronumber}/{documentid}")]
    public async Task<Response<DocumentResponseDto>> GetDocument(int pronumber, int documentid)
    {
        var user =  GetSessionUser();
        var result = await mediator.Send(new GetDocumentPDFQuery(
            DocumentRequestDto.Get(user.companyID, pronumber, documentid)
        ));
            
        return result;
    }

    // BOL = BILL OF LADING
    [HttpGet("/api/container/billoflading/pdf/{pronumber}/{legtype}")]
    public async Task<Response<DocumentResponseDto>> GetProofOfDeliveryPDF(int pronumber, string legtype)
    {
        var user =  GetSessionUser();
        return await mediator.Send(new GetBillOfLadingPDFQuery
        (
            ContainerRequestDto.Get(user.driverID, pronumber, legtype)
        ));
    }
}
