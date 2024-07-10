
namespace Domain.Exceptions;

public static class  DocumentImageError<DocumentImageDto> where DocumentImageDto : class
{     
    public static Error<DocumentImageDto> InvalidLegType(string legType)
    {
        return new($"Invalid leg type. Image upload only works for Delivery Leg Type but received : {legType}", "DocumentImageUploadCommandHandler");
    }
}