namespace UseCases.Features;
public record GetPurchaseOrderQuery(ContainerRequestDto requestDto) : IRequest<Result<IReadOnlyList<PurchaseOrder>>>{}
public class GetPurchaseOrderQueryHandler: IRequestHandler<GetPurchaseOrderQuery, Result<IReadOnlyList<PurchaseOrder>>>
{
    private readonly IContainerRepository repo;
    private readonly Result<SessionUserDto> sessionUser;

    public GetPurchaseOrderQueryHandler(IContainerRepository repo, IBaseService baseService)
    {
        this.repo = repo;
        sessionUser = baseService.GetSessionUser();
    }

    public async Task<Result<IReadOnlyList<PurchaseOrder>>> Handle(GetPurchaseOrderQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetPurchaseOrder(sessionUser.Value.companyID, request.requestDto.proNumber);
    }
}