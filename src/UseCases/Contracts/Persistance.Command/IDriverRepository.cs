namespace UseCases.Contracts.Persistance.Command;


public interface IDriverRepository: IAsyncRepository<Driver>
{

    Task<Result<DriverMoves>> GetDriverMove(CompanyID companyID, ContainerRequestDto requestDto);

    Task<DriverMoves> SaveDriverMove(DriverMoves Entity);
    Task<DriverMoves> UpdateDriverMove(DriverMoves Entity);


    Task<EmailLoginToken> SaveEmailLoginToken(EmailLoginToken Entity);

    Task<DriverDocuments> SaveDriverDocument(DriverDocuments Entity);

    Task<OrderLogDoc> SaveProDocument(OrderLogDoc Entity);

    Task<Result<DriverMoves>> UpdateContainerStatus(DocumentStatusUpdateDto updateDto);
    Task<DriverDocuments> UpdateDriverDocument(int ID, DriverDocuments Entity);

    
}