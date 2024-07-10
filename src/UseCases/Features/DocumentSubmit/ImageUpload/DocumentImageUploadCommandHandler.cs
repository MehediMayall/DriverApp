namespace UseCases.Features;

public record DocumentImageUploadCommand(DocumentImageDto requestDto) : IRequest<Response<string>>{}
public class DocumentImageUploadCommandHandler: IRequestHandler<DocumentImageUploadCommand, Response<string>>
{
    private readonly IDirectoryService directoryService;
    private readonly Result<SessionUserDto> sessionUser;

    public DocumentImageUploadCommandHandler(
        IBaseService baseService, 
        IDirectoryService directoryService
        )
    {
        
        this.directoryService = directoryService;
        sessionUser = baseService.GetSessionUser();
    }

    public async Task<Response<string>> Handle(DocumentImageUploadCommand request, CancellationToken cancellationToken = default)
    {

        DocumentImageDto requestDto = request.requestDto;


        // LEG TYPE VALIDATION
        if( requestDto.LegType.ToUpper() != LegTypes.DELIVERY) 
            return Response<string>.Error(DocumentImageError<string>.InvalidLegType(requestDto.LegType), requestDto);


        // Validate Document
        var validator = new DocumentSubmitImageValidator();
        var validationResult = await validator.ValidateAsync(requestDto);
        if(validationResult.Errors.Count > 0) return Response<string>.ValidationError(validationResult);


        // GET POD IMAGES PATH
        var resultImage = await directoryService.GetPODImagesPath();
        string podImagesPath = resultImage.Value;


        // SAVE IMAGES
        await SaveImageForDelivery(requestDto, podImagesPath);
   

        return Response<string>.OK("Successfully Images Uploaded");
    }

    public async Task<Boolean> SaveImageForDelivery(DocumentImageDto requestDto, string ImagePath)
    {
        string parsedData;
        string ImagesInBase64 = requestDto.ImagesInBase64;
        string imageFilePath = Path.Combine(ImagePath, requestDto.ProNumber.ToString() + DateTime.Now.ToString("hhMMss")+ ".jpg");

        parsedData = ImagesInBase64.Contains(',')
            ? ImagesInBase64.Substring(ImagesInBase64.IndexOf(',') + 1)
            : ImagesInBase64;


        byte[] imageBytes = Convert.FromBase64String(parsedData);

        string tempImage = imageFilePath.Substring(0, imageFilePath.Length - 4) + "_deleted.jpg";

        File.WriteAllBytes(tempImage, imageBytes);

        Image image = Image.FromFile(tempImage);
        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        image.Save(imageFilePath);
        image.Dispose();

        File.Delete(tempImage);


        return true;

    }
}