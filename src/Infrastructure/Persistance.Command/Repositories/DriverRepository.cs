namespace Infrastructure.Persistance.Command;

public class DriverRepository: GenericRepository<Driver>, IDriverRepository
{
    private readonly DriverContext dbContext;
    private readonly ICacheService cacheService;

    public DriverRepository(DriverContext dbContext, ICacheService cacheService) : base(dbContext)
    {
        this.dbContext = dbContext;
        this.cacheService = cacheService;
    }

    public async Task<Result<DriverMoves>> GetDriverMove(CompanyID companyID, ContainerRequestDto requestDto)
    {
        var data = dbContext.DriverMoves.Where(dm =>
            dm.CompanyID == companyID.Value.Value.ToString()
            && dm.DriverID == requestDto.driverID.Value.Value
            && dm.ProNumber == requestDto.proNumber.Value.Value
            && dm.LegType == requestDto.legType.Value.Value
        ).OrderByDescending(m=> m.ViewMovedOn).FirstOrDefault();

        if (data is null) return DriverMovesError<DriverMoves>.DriverMovesNotFound(requestDto.proNumber);
        return Result<DriverMoves>.Success(data);
    }

    #region SAVE

    public async Task<Result<DriverMoves>> UpdateContainerStatus(DocumentStatusUpdateDto updateDto)
    {
        var sqlParameters = new[]
        {
            new SqlParameter { ParameterName = "@OrderLogID", Value = updateDto.orderLogID.Value.Value },
            new SqlParameter { ParameterName = "@LegType", Value = updateDto.legType.Value.Value },
            new SqlParameter { ParameterName = "@IsResubmit", Value = updateDto.IsResubmit },
        };
        string query = "exec APP.updateContainerStatus  @OrderLogID={0}, @LegType={1}, @IsResubmit={2}";

        var data = await new GenericRepository<DriverMoves>(dbContext).GetOneUsingSPAsync(query, sqlParameters);
        if (data is null) return DriverMovesError<DriverMoves>.DriverMovesNotFound(updateDto.proNumber);

        removeWorkQueueCache(updateDto.driverID.Value.Value);
        return Result<DriverMoves>.Success(data);
    }

    // SAVE
    public async Task<DriverMoves> SaveDriverMove(DriverMoves Entity)
    {
        await new GenericRepository<DriverMoves>(dbContext).Insert(Entity);
        removeWorkQueueCache(Entity.DriverID);
        return Entity;
    }

    #endregion


    #region  UPDATE
    public async Task<DriverMoves> UpdateDriverMove(DriverMoves Entity)
    {
        await new GenericRepository<DriverMoves>(dbContext).Update(Entity);
        removeWorkQueueCache(Entity.DriverID);
        return Entity;
    }

    #endregion
    // public async Task<DriverMoves> UpdateDriverMove(DriverMoves Entity)
    // {
    //     await new GenericRepository<DriverMoves>(dbContext).Update(Entity, 
    //         dm => dm.DriverID == Entity.DriverID 
    //             && dm.ProNumber == Entity.ProNumber 
    //             && dm.CompanyID == Entity.CompanyID
    //             && dm.LegType == Entity.LegType
    //     );
    //     removeWorkQueueCache(Entity.DriverID);
    //     return Entity;
    // }

    private void removeWorkQueueCache(int? driverID)
    {
        string key = $"workqueue{driverID.Value}";
        cacheService.RemoveAsync(key);
    }


    public async Task<OrderLogDoc> SaveProDocument(OrderLogDoc Entity)
    {
        return await new GenericRepository<OrderLogDoc>(dbContext).Insert(Entity);
    }

    public async Task UpdateProDocument(DocumentID ID, OrderLogDoc Entity)
    {
        await new GenericRepository<OrderLogDoc>(dbContext).Update(Entity, C => C.DocId == ID.Value);
    }


    public async Task<DriverDocuments> SaveDriverDocument(DriverDocuments Entity)
    {
        return await new GenericRepository<DriverDocuments>(dbContext).Insert(Entity);
    }

    public async Task<DriverDocuments> UpdateDriverDocument(int ID, DriverDocuments Entity)
    {
        return await new GenericRepository<DriverDocuments>(dbContext).Update(Entity, C => C.Id == ID);
    }

    public async Task<EmailLoginToken> SaveEmailLoginToken(EmailLoginToken Entity)
    {
        return await new GenericRepository<EmailLoginToken>(dbContext).Insert(Entity);
    }

    

    // public async Task<Boolean> sendEMessage(int ProNumber, int DriverID, string LegType, string MiscellaneousNote)
    // {
    //     return await new GenericRepository<OrderLog>(dbContext).ExecuteCommand("EXEC sendEMessage @ProNumber, @DriverID, @LegType, @MiscellaneousNote",
    //         new SqlParameter("@ProNumber", ProNumber),
    //         new SqlParameter("@DriverID", DriverID),
    //         new SqlParameter("@LegType", LegType),
    //         new SqlParameter(parameterName: "@MiscellaneousNote", MiscellaneousNote)
    //         );
    // }

}