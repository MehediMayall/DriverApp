using UseCases.Contracts.Infrastructure;

namespace Infrastructure.Persistance.Query.Dapper;

public class ContainerRepositoryCached: GenericRepository<OrderLog>, IContainerRepository
{
    private readonly ContainerRepository containerRepo;
    private readonly ICacheService cacheService;

    public ContainerRepositoryCached(IConfiguration configuration, ContainerRepository containerRepo, ICacheService cacheService ):base(configuration)
    {
        this.containerRepo = containerRepo;
        this.cacheService = cacheService;
    }

    public async Task<Result<List<WorkQueueModel>>> GetWorkQueue(DriverID driverID)
    {
        string key = $"workqueue{driverID.Value.Value}";
        // var data = await cacheService.GetOrCreateAsync<List<WorkQueueModel>>(key,
        //     async() => 
        //     {
        //         return (await containerRepo.GetWorkQueue(driverID)).Value;
        //     },
        //     TimeSpan.FromMinutes(1)
        // );

        var data = await cacheService.GetAsync<List<WorkQueueModel>>(key);
        if (data is null) data = (await containerRepo.GetWorkQueue(driverID)).Value;
        if (data is null || !data.Any()) return ContainerRepositoryErrors<List<WorkQueueModel>>.WorkQueueNotFound(driverID);
        
        await cacheService.SetAsync<List<WorkQueueModel>>(key, data, TimeSpan.FromSeconds(20), default);
        return  Result<List<WorkQueueModel>>.Success(data);
    }

    public async Task<Result<DriverContainerModel>> GetContainer(ContainerRequestDto requestDto) => 
        await containerRepo.GetContainer(requestDto);

    public async Task<Result<IReadOnlyList<PurchaseOrder>>> GetPurchaseOrder(CompanyID companyID, ProNumber proNumber) => 
        await containerRepo.GetPurchaseOrder(companyID, proNumber);

    public async Task<Result<OrderLogDocument>> GetOrderLogDoc(DocumentRequestDto documentRequest) => 
        await containerRepo.GetOrderLogDoc(documentRequest);

    public async Task<Result<GlobalSetup>> GetDivisionInfo(CompanyID companyID)
    {
        string key= $"Company{companyID.Value.Value}";
        
        var data = await cacheService.GetAsync<GlobalSetup>(key);
        if(data is null) data = (await containerRepo.GetDivisionInfo(companyID)).Value;
        if (data is null) return GlobalSetupError<GlobalSetup>.GlobalSetupNotFound(companyID);

        await cacheService.SetAsync<GlobalSetup>(key, data, TimeSpan.FromHours(24), default);
        return Result<GlobalSetup>.Success(data);
    }

    public async Task<Result<DriverDocuments>> GetDriverDocument(ContainerRequestDto requestDto, DocumentID documentID) =>       
        await containerRepo.GetDriverDocument(requestDto, documentID);
    

}