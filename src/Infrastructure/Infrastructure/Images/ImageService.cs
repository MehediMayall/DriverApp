using Domain.Exceptions;
using Domain.Shared;
using Domain.ValueObjects;

namespace Infrastructure.Infrastructure;

public class ImageService : IImageService
{
    private readonly IFileOperationService fileOperationService;

    public ImageService(IFileOperationService fileOperationService)
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

            foreach (var item in files)
            {
                using (Image image = Image.FromFile(item))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        // Convert byte[] to Base64 String
                        string base64String = Convert.ToBase64String(imageBytes);
                        imageTag += "<img style='max-height:1000px;' src='data:image/png;base64," + base64String + "' /><br/><br/><br/>";
                    }
                }
            }
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

            byte[] imageBytes = Convert.FromBase64String(ImageInBase64);

            File.WriteAllBytes(FileName, imageBytes);


            Image image = Image.FromFile(FileName);
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            image.Save(FileName);
            image.Dispose();

            byte[] imageArray = System.IO.File.ReadAllBytes(FileName);
            string base64 = Convert.ToBase64String(imageArray);
            fileOperationService.DeleteFile(FileName);

            return Result<string>.Success(base64);
        }
        catch(Exception ex) { return Result<string>.Failure(Error<string>.Set(ex.GetAllExceptions()));}
    }


    public async Task<ImageSize> GetImageSize(string ImagesInBase64)
    {

        string parsedData;

        parsedData = ImagesInBase64.Contains(',')
            ? ImagesInBase64.Substring(ImagesInBase64.IndexOf(',') + 1)
            : ImagesInBase64;

        byte[] imageBytes = Convert.FromBase64String(parsedData);

        using (MemoryStream memoryStream = new MemoryStream(imageBytes))
        {
            using (Image image = Image.FromStream(memoryStream))
            {
                var imgSize = new ImageSize { Height = image.Height, Width = image.Width };
                return imgSize;
            }
        }

    }
}