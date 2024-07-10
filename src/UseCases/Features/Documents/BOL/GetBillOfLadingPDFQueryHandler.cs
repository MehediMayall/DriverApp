namespace UseCases.Features;

public record GetBillOfLadingPDFQuery(ContainerRequestDto requestDto) : IRequest<Response<DocumentResponseDto>>{}
public class GetBillOfLadingPDFQueryHandler: IRequestHandler<GetBillOfLadingPDFQuery, Response<DocumentResponseDto>>
{
    private readonly IContainerRepository repo;

    private readonly IHTMLReportService htmlReportService;

    private readonly IConfiguration configuration;

    public GetBillOfLadingPDFQueryHandler(
        IContainerRepository repo, 
        IHTMLReportService htmlReportService,
        IConfiguration configuration)
    {
        this.repo = repo;
        this.htmlReportService = htmlReportService;
        this.configuration = configuration;
    }

    public async  Task<Response<DocumentResponseDto>> Handle(GetBillOfLadingPDFQuery request, CancellationToken cancellationToken)
    {
        // GET CONTAINER
        var container = await repo.GetContainer(request.requestDto);
        if (container.IsFailure) 
            return Response<DocumentResponseDto>.Error(container.Error);

        // GET BILL OF LADING HTML
        var htmlReportResult = await htmlReportService.GetBillOfLadingReportHTML(container.Value);
        if (htmlReportResult.IsFailure) return Response<DocumentResponseDto>.Error(htmlReportResult.Error);

        var apiDomain = configuration.GetSection("API_Domain_Address").Value.ToString();

        var pdfURL = Path.Combine(apiDomain, "bol/" + htmlReportResult.Value.FileName);

        // RETURN PDF PATH
        var data = new DocumentResponseDto(DocumentIDs.BILL_OF_LADING.Value,"Bill of Lading",pdfURL);
        return Response<DocumentResponseDto>.OK(data);
    }


}