namespace DriverAPI.IntegrationTests;


public class FakeImageService : IImageService
{
    private readonly IFileOperationService fileOperationService;

    public FakeImageService(IFileOperationService fileOperationService)
    {
        this.fileOperationService = fileOperationService;
    }

    public async Task<Result<string>> GetImagesAsString(ProNumber ProNumber, string ImagePath)
    {
        try
        {
            if (!Directory.Exists(ImagePath)) Thread.Sleep(5000);

            string[] files = Directory.GetFiles(ImagePath, $"{ProNumber.Value.Value}*.*");
            string imageTag = "";

            if (files.Length == 0)
            {
                // Log.Warning($"No file found in the following path: {ImagePath}. Waiting for 5 seconds to check the folder again.");
                // wait for the images to be uploaded
                Thread.Sleep(5000);
                files = Directory.GetFiles(ImagePath, $"{ProNumber.Value.Value}*.*");
            }

            if (files.Length == 0) Result<string>.Failure(Error<string>.Set($"No file found in the following path: {ImagePath}"));
            // Log.Information($"GetImagesAsString: ImagePath:{ImagePath},  Count: {files.Length}");

            
            return Result<string>.Success(imageTag);
        }
        catch(Exception ex) 
        {
            return Result<string>.Failure(Error<string>.Set(ex.GetAllExceptions()));
        }
    }
    public async Task<Result<string>> DeletePODImages(ProNumber ProNumber, string ImagePath)
    {
        try
        {
            if (!Directory.Exists(ImagePath)) Thread.Sleep(5000);

            string[] files = Directory.GetFiles(ImagePath, $"{ProNumber.Value.Value}*.*");

            foreach (var item in files)
            {
                File.Delete(item);
            }
            return Result<string>.Success("");
        }
        catch(Exception ex) 
        {
            return Result<string>.Failure(Error<string>.Set(ex.GetAllExceptions()));
        }
    }


    public async Task<Result<string>> GetPortraitImage(string ImageInBase64, ProNumber ProNumber)
    {
        try
        {

            string FileName = Path.Combine(Directory.GetCurrentDirectory(), DateTime.Now.ToString("ddmmhhyyyy") + ProNumber.Value.Value + ".jpg");

     

            return Result<string>.Success(ImageInBase64);
        }
        catch(Exception ex) { return Result<string>.Failure(Error<string>.Set(ex.GetAllExceptions()));}
    }


    public async Task<ImageSize> GetImageSize(string ImagesInBase64)
    {

    
        return new ImageSize { Height = 700, Width = 1000 };
                 
    }
}