
namespace Domain.Exceptions;

public static class  FileInfoError<FileInfoDto> where FileInfoDto : class
{     
    public static Error<FileInfoDto> GetDivisionFolderNameIsNull(CompanyID companyID)
    {
        return new($"Division folder root path is empty for company: {companyID.Value}", "DirectoryService.GetDivisionFolderName");
    }
    
}