namespace UseCases.Contracts.Infrastructure;

public interface IImageService
{
    Task<Result<string>> GetImagesAsString(ProNumber ProNumber, string ImagePath);
    Task<Result<string>> GetPortraitImage(string ImageInBase64,  ProNumber ProNumber);
    Task<ImageSize> GetImageSize(string ImagesInBase64);
    Task<Result<string>> DeletePODImages(ProNumber ProNumber, string ImagePath);
}