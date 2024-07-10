namespace UseCases.Dtos;

public record DocumentRequestDto(CompanyID companyID, ProNumber proNumber, DocumentID documentID)
{

    public static DocumentRequestDto Get(CompanyID companyID, int proNumber, int documentID)
    {
        return new DocumentRequestDto(
            companyID,
            proNumber.GetProNumber(),
            documentID.GetDocumentID()
        );
    }
}