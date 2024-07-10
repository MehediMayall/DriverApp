namespace UseCases.Features;

public class DocumentSubmitPickupValidator: AbstractValidator<DocumentSubmitRequestDto>
{
    public DocumentSubmitPickupValidator()
    {
        

    }
    public Result<string> ValidateSignatureImage(DocumentSubmitRequestDto requestDto, DriverContainerModel container)
    {
        // IMAGE VALIDATION
        if (container.LegType == LegTypes.DELIVERY && 
            container.LoadType.ToUpper() != LoadTypes.LOOSE_FREIGHT && 
            string.IsNullOrEmpty(requestDto.ImagesInBase64))
            {
                return Result<string>.Failure(Error<string>.Set(ERRORS.INVALID_SIGNATURE_IMAGE));
            } 

        if (!string.IsNullOrEmpty(container.LegType)) container.LegType = container.LegType.Trim().ToUpper();
        return Result<string>.Success("");
    }
    public Result<string> ValidateImage(DocumentSubmitRequestDto requestDto, DriverContainerModel container)
    {
        // IMAGE VALIDATION
        if (container.LoadType.ToUpper() != LoadTypes.LOOSE_FREIGHT  && 
            string.IsNullOrEmpty(requestDto.ImagesInBase64))
            {
                return Result<string>.Failure(Error<string>.Set(ERRORS.INVALID_IMAGE));
            } 

        if (!string.IsNullOrEmpty(container.LegType)) container.LegType = container.LegType.Trim().ToUpper();
        return Result<string>.Success("");
    }
}