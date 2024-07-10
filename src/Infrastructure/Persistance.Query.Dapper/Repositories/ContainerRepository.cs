namespace Infrastructure.Persistance.Query.Dapper;

public class ContainerRepository: GenericRepository<OrderLog>, IContainerRepository
{
    public ContainerRepository(IConfiguration configuration):base(configuration)
    {
    }

    public async Task<Result<List<WorkQueueModel>>> GetWorkQueue(DriverID driverID)
    {
        int ProNumber = 0;
        string LegType = "";
        string query = "exec APP.getDriverContainers  @DriverID, @ProNumber, @LegType";
        var data = await db.QueryAsync<WorkQueueModel>(query,
            new {
                DriverID = driverID.Value.Value, 
                ProNumber,
                LegType
            });
        if (data.IsNullOrEmpty()) return ContainerRepositoryErrors<List<WorkQueueModel>>.WorkQueueNotFound(driverID);
        return Result<List<WorkQueueModel>>.Success(data.ToList());
    }

    public async Task<Result<DriverContainerModel>> GetContainer(ContainerRequestDto requestDto)
    {
        string query = "exec APP.getDriverContainers  @DriverID, @ProNumber, @LegType";
        var data = await db.QueryAsync<DriverContainerModel>( query, 
                new {
                    DriverID = requestDto.driverID.Value.Value, 
                    ProNumber = requestDto.proNumber.Value.Value, 
                    LegType = requestDto.legType.Value.Value.ToString()
                });
        if (data.IsNullOrEmpty()) return  PreviewInstructionErrors<DriverContainerModel>.NoContainerFound();
        return Result<DriverContainerModel>.Success(data.FirstOrDefault());
    }

    public async Task<Result<IReadOnlyList<PurchaseOrder>>> GetPurchaseOrder(CompanyID companyID, ProNumber proNumber) 
    {
        var data = await db.QueryAsync<PurchaseOrder>(
            @"SELECT PO.* FROM PurchaseOrder PO 
                LEFT JOIN OrderLog O ON PO.OrderLogId = O.OrderLogId 
                WHERE O.CompanyID = @CompanyID 
                    AND O.Pro = @ProNumber",
            new {CompanyID = companyID.Value.Value, ProNumber = proNumber.Value.Value});

        if (data.IsNullOrEmpty()) return PurchaseOrderError<IReadOnlyList<PurchaseOrder>>.PurchaseOrderNotFound(proNumber.Value);
        return Result<IReadOnlyList<PurchaseOrder>>.Success(data.ToList());
    }

    public async Task<Result<OrderLogDocument>> GetOrderLogDoc(DocumentRequestDto documentRequest)
    {
        var data = await db.QueryAsync<OrderLogDocument>(
            @"SELECT  O.Pro AS ProNumber, OD.OrderLogId, OD.CompanyId, OD.DocId, OD.DocNamePro, OD.SubFolder 
            FROM OrderLogDoc OD
            LEFT JOIN OrderLog O ON OD.OrderLogId = O.OrderLogId 
            WHERE  OD.CompanyId = @CompanyID AND O.Pro = @ProNumber AND DocID = @DocumentID",
            new { 
                CompanyID = documentRequest.companyID.Value.Value,
                ProNumber = documentRequest.proNumber.Value.Value, 
                DocumentID = documentRequest.documentID.Value.Value
            });
    
        
        if(data.IsNullOrEmpty()) return OrderLogDocumentError<OrderLogDocument>.OrderLogDocNotFound(documentRequest.proNumber);
        return Result<OrderLogDocument>.Success(data.FirstOrDefault());
    }

    public async Task<Result<GlobalSetup>> GetDivisionInfo(CompanyID companyID)
    {
        var data = await db.QueryAsync<GlobalSetup>(
            "SELECT TOP(1) * FROM DBO.GlobalSetup WHERE CompanyID = @CompanyID;",
            new { CompanyID = companyID.Value.Value }
        );

        if (data.IsNullOrEmpty()) return GlobalSetupError<GlobalSetup>.GlobalSetupNotFound(companyID);
        return Result<GlobalSetup>.Success(data.FirstOrDefault());
    }
    
    

    public async Task<Result<DriverDocuments>> GetDriverDocument(ContainerRequestDto requestDto, DocumentID documentID)
    {
        var data = await db.QueryAsync<DriverDocuments>(
          @"SELECT TOP(1) * FROM APP.Driver_Documents 
            WHERE DriverID = @DriverID 
                AND ProNumber = @ProNumber 
                AND LegType = @LegType 
                AND DocumentId = @DocumentId;",
          new {   
            DriverID = requestDto.driverID.Value.Value,         
            ProNumber = requestDto.proNumber.Value.Value, 
            LegType = requestDto.legType.Value.Value, 
            DocumentId = documentID.Value.Value
            }  
        );

        if (data.IsNullOrEmpty())  return DriverDocumentError<DriverDocuments>.DriverDocumentNotFound(requestDto.proNumber);
        return Result<DriverDocuments>.Success(data.FirstOrDefault());
    }


}