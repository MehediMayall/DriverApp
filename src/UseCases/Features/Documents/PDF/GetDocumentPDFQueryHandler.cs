namespace UseCases.Features;

public record GetDocumentPDFQuery(DocumentRequestDto requestDto) : IRequest<Response<DocumentResponseDto>>{}
public class GetDocumentPDFQueryHandler: IRequestHandler<GetDocumentPDFQuery, Response<DocumentResponseDto>>
{
    private readonly IContainerRepository repo;
    private readonly IDirectoryService directoryService;
    private readonly IConfiguration configuration;
    private readonly Result<SessionUserDto> sessionUser;

    public GetDocumentPDFQueryHandler(IContainerRepository repo, IDirectoryService directoryService, IConfiguration configuration, IBaseService baseService)
    {
        this.repo = repo;
        this.directoryService = directoryService;
        this.configuration = configuration;
        this.sessionUser = baseService.GetSessionUser();
    }

    public async Task<Response<DocumentResponseDto>> Handle(GetDocumentPDFQuery request, CancellationToken cancellationToken)
    {
        string folderName = await directoryService.GetDivisionFolderName(this.sessionUser.Value.companyID);
        if(string.IsNullOrEmpty(folderName)) 
            return Response<DocumentResponseDto>.Error(FileInfoError<DocumentResponseDto>.GetDivisionFolderNameIsNull(this.sessionUser.Value.companyID), sessionUser.Value);


        string documentPath = $"{configuration.GetSection("API_Domain_Address").Value}documents/{folderName}{sessionUser.Value.Division}/";
        string DocumentName = "", DocumentPath = "";
        
        // LOAD ORDER LOG DOCUMENT
        var orderLogDocResult = await GetOrderLogDoc(request.requestDto);
        
        // if (orderLogDocResult.IsFailure) return Response<DocumentResponseDto>.Error(orderLogDocResult.Error.Message, orderLogDocResult.Error.Code);
        if (orderLogDocResult.IsFailure) return Response<DocumentResponseDto>.Error(orderLogDocResult.Error, request.requestDto);


        //"Document Does not exist for PRO # " + request.requestDto.ProNumber.Value + ", DocumentID = " + request.requestDto.documentID.Value.ToString();
        else DocumentPath = $"{documentPath}{orderLogDocResult.Value.SubFolder?.Trim()}/{orderLogDocResult.Value.DocNamePro}.pdf";

        DocumentResponseDto document = new DocumentResponseDto(
            request.requestDto.documentID.Value,
            DocumentName,
            DocumentPath
        );
        return Response<DocumentResponseDto>.OK(document);
    }

    public async Task<Result<OrderLogDocument>> GetOrderLogDoc(DocumentRequestDto requestDto)
    {
        var docId = 8;
        int DocumentID = requestDto.documentID.Value;
        if (DocumentID == 1 || DocumentID == 8) docId = 8;
        else if (DocumentID == 2 || DocumentID == 11) docId = 11;
        return await this.repo.GetOrderLogDoc(new DocumentRequestDto(requestDto.companyID, requestDto.proNumber, docId.GetDocumentID()));
    }

}