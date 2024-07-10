
namespace Domain.Exceptions;

public static class  DriverDocumentError<DriverDocuments> where DriverDocuments : class
{     
    public static Error<DriverDocuments> DriverDocumentNotFound(ProNumber ProNumber)
    {
        return new($"Couldn't find any driver document for ProNumber: {ProNumber.Value}", "ContainerRepository.GetDivisionInfo");
    }
}