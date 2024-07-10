namespace UseCases.SharedServices;

public interface IDirectoryService
{
    Task<Result<FileInfoDto>> GetDocsQueueDirectoryAndFilename(DocumentSubmitRequestDto document, SessionUserDto sessionUser);
    string GetPODTemplatePath(LoadType loadType);
    Task<Result<FileInfoDto>> GetPODDirectoryAndFilename(DriverContainerModel container, SessionUserDto sessionUser);

    Task<string> GetDivisionFolderName(CompanyID companyID);

    Task<Result<string>> GetBillofLadingTemplatePath(LoadType loadType);

    Task<Result<string>> GetBeginMoveEmailTemplatePath();

    Task<Result<string>> GetPODImagesPath();
}

