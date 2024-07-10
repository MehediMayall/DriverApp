namespace UseCases.Contracts.Persistance.Query;

public interface IContainerRepository: IAsyncRepository<OrderLog>
{
    Task<Result<List<WorkQueueModel>>> GetWorkQueue(DriverID driverID);
    Task<Result<DriverContainerModel>> GetContainer(ContainerRequestDto requestDto);
    Task<Result<IReadOnlyList<PurchaseOrder>>> GetPurchaseOrder(CompanyID companyID, ProNumber proNumber);

    Task<Result<GlobalSetup>> GetDivisionInfo(CompanyID companyID);

    Task<Result<OrderLogDocument>> GetOrderLogDoc(DocumentRequestDto documentRequest);
    Task<Result<DriverDocuments>> GetDriverDocument(ContainerRequestDto requestDto, DocumentID documentID);
   
}