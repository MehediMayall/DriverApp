namespace UseCases.SharedServices;

public interface IProofOfDeliveryService
{
    Task<Result<string>> SaveProofOfDeliveryPDF(DriverContainerModel container, string PDFPath, DocumentSubmitRequestDto document, string ImagePath);
    Task<Result<OrderLogDoc>> SaveProDocument(SessionUserDto sessionUser, OrderLogID orderLogID,  string FolderName, string DocumentFileName, DocumentID documentID);
}