namespace UseCases.SharedServices;

public class DirectoryService : IDirectoryService
{
    private readonly ICacheService cacheService;
    // private readonly ICacheServiceRedis cacheServiceRedis;
    private readonly IContainerRepository repo;
    private readonly IFileOperationService fileOperationService;
    private GlobalSetup? company;
    private readonly string AppRootPath;
    private readonly AttachmentDirectoriesDto attachmentDirectories;
    private readonly TemplatePathsDto templatePaths;
    private readonly StaticFileDto staticFileDto;
    
    public DirectoryService(IWebHostEnvironment webHostEnvironment, 
        ICacheService cacheService, 
        IContainerRepository repo,
        IFileOperationService fileOperationService,
        IOptions<AttachmentDirectoriesDto> AppSettingAttachment,
        IOptions<TemplatePathsDto> AppSettingTemplatePath,
        IOptions<StaticFileDto> AppSettingStaticFiles
        )
    {
        this.cacheService = cacheService;
        this.repo = repo;
        this.fileOperationService = fileOperationService;
        this.AppRootPath = webHostEnvironment.WebRootPath;
        attachmentDirectories = AppSettingAttachment.Value;
        templatePaths = AppSettingTemplatePath.Value;
        staticFileDto = AppSettingStaticFiles.Value;

        if (string.IsNullOrEmpty(this.AppRootPath)) this.AppRootPath = webHostEnvironment.ContentRootPath;
    }
    
    private async Task<Result<GlobalSetup>> GetCompany(CompanyID companyID)
    {
        // company =  await cacheService.GetAsync<GlobalSetup>("Company", async() =>
        // {
        //     return await repo.GetDivisionInfo(CompanyID);
        // });
        // return company;


        // company =  await cacheService.GetOrCreateAsync<GlobalSetup>("Company", async() =>
        // {
        //     return (await repo.GetDivisionInfo(companyID)).Value;
        // });

        company =  await cacheService.GetOrCreateAsync<GlobalSetup>("Company",null);
        if(company is null) company = (await repo.GetDivisionInfo(companyID))?.Value;


        return Result<GlobalSetup>.Success(company);
    }

    public async Task<Result<FileInfoDto>> GetDocsQueueDirectoryAndFilename(DocumentSubmitRequestDto document, SessionUserDto sessionUser)
    {
         
        if(company is null) await GetCompany(sessionUser.companyID);        
        string DocumentTypeName = "Inbound TIR";

        if (document.LegType.Value != LegTypes.PICKUP) DocumentTypeName = "Outbound_TIR";
        
        string ImageFileName = sessionUser.driverID.Value.Value.ToString() + "_" + document.ProNumber.Value.Value.ToString() + "_" + DateTime.Now.ToString("yymmddhhmm") + "_" + DocumentTypeName + ".pdf";

        string folderName = await GetDivisionFolderName(sessionUser.companyID);
        if(string.IsNullOrEmpty(folderName)) 
            return FileInfoError<FileInfoDto>.GetDivisionFolderNameIsNull(sessionUser.companyID);

        string ImageLocation = $"{staticFileDto?.BasePath}\\{folderName}{company.State}\\DocsQueue\\";
        string ImagePath = Path.Combine(ImageLocation, ImageFileName);

        return Result<FileInfoDto>.Success(new FileInfoDto(ImageLocation, ImageFileName, ImagePath));
    }

    public async Task<string> GetDivisionFolderName(CompanyID companyID)
    {        
        if(company is null) await GetCompany(companyID);

        string folderName = company?.ScannedDocPath;
        if (string.IsNullOrEmpty(folderName)) return "";
        folderName = folderName.Replace("\\", "").Replace("X:", "").Replace("T:", "");
        return folderName;
    }
    
    public string GetPODTemplatePath(LoadType loadType)
    {
        string rootPath = Path.Combine(AppRootPath,"StaticContents");
        rootPath = Path.Combine(rootPath,"pod_format");

        
        return loadType.Value.ToUpper() switch
        {            
             "IMPORT" => Path.Combine(rootPath, "IMF_POD_IMPORT.html"),
             "EXPORT" => Path.Combine(rootPath, "IMF_POD_EXPORT.html"),
             _ => Path.Combine(rootPath, "IMF_POD_LOOSEFRIEGHT.html")
        };
    }

    public async Task<Result<FileInfoDto>> GetPODDirectoryAndFilename(DriverContainerModel container, SessionUserDto sessionUser)
    {
        // Document File Name
        string folderName = "";
        if(company is null) await GetCompany(sessionUser.companyID);        

        if (container != null) folderName = Convert.ToDateTime(container.OrderEntryDate).ToString("yyyyMM");


        var rootFolder = await GetDivisionFolderName(sessionUser.companyID);
        if(string.IsNullOrEmpty(rootFolder)) return FileInfoError<FileInfoDto>.GetDivisionFolderNameIsNull(sessionUser.companyID);
        


        string State = company.State.ToLower();
        string pdfFileNamePOD = container.Pro.ToString() + "-PROOF OF DELIVERY.pdf";
        string pdfFileNameBL = container.Pro.ToString() + "-BILL OF LADING.pdf";


        string pdfLocation = $"{staticFileDto.BasePath}\\{rootFolder}{State}\\{folderName}";
        string pdfFullPathPOD = Path.Combine(pdfLocation, pdfFileNamePOD);
        // string pdfFullPathBL = Path.Combine(pdfLocation, pdfFileNameBL);

        return Result<FileInfoDto>.Success(new FileInfoDto(pdfLocation, pdfFileNamePOD, pdfFullPathPOD, folderName));
    }



    // public async Task<Result<FileInfoDto>> GetBillOfLadingPath(DriverContainerModel container, SessionUserDto sessionUser)
    // {
    //     // Document File Name
    //     string folderName = "";
    //     var templatePaths = configuration.GetSection("TemplatePaths").Get<TemplatePathsDto>();
    //     if(company is null) await GetCompany(sessionUser.companyID);        

    //     string siteRoot =  configuration.GetSection("API_Domain_Address").Value?.ToString();
    //     string templatePath = Path.Combine(siteRoot, templatePaths.BOL);

    //     if (container != null) folderName = Convert.ToDateTime(container.OrderEntryDate).ToString("yyyyMM");

    //     var rootFolder = await GetDivisionFolderName(sessionUser.companyID);
    //     if(string.IsNullOrEmpty(rootFolder)) return FileInfoError<FileInfoDto>.GetDivisionFolderNameIsNull(sessionUser.companyID);


    //     string pdfFileNamePOD = container.Pro.ToString() + "-PROOF OF DELIVERY.pdf";
    //     string pdfFileNameBL = container.Pro.ToString() + "-BILL OF LADING.pdf";


    //     string pdfLocation = staticFiles.Directory + "\\" + rootFolder + "\\" + folderName;
    //     string pdfFullPathPOD = Path.Combine(pdfLocation, pdfFileNamePOD);
    //     // string pdfFullPathBL = Path.Combine(pdfLocation, pdfFileNameBL);

    //     return Result<FileInfoDto>.Success(new FileInfoDto(pdfLocation, pdfFileNamePOD, pdfFullPathPOD));
    // }

    public async Task<Result<string>> GetBillofLadingTemplatePath(LoadType loadType)
    {
        // Document File Name
        string siteRoot =  Environment.CurrentDirectory;
        string templatePath = Path.Combine(siteRoot, templatePaths.BOL);

        string templateFilePath = loadType.Value.ToUpper() switch 
        {
            LoadTypes.IMPORT => Path.Combine(templatePath, "IMPORT.html"),
            LoadTypes.EXPORT => Path.Combine(templatePath, "EXPORT.html"),
            LoadTypes.LOOSE_FREIGHT => Path.Combine(templatePath, "LOOSEFRIEGHT.html"),
            _ => Path.Combine(templatePath, "IMPORT.html")
        };

        return Result<string>.Success(templateFilePath);
    }

    public async Task<Result<string>> GetBeginMoveEmailTemplatePath()
    {

        string siteRoot =  Environment.CurrentDirectory;
        string templatePath = Path.Combine(siteRoot, templatePaths.BeginMove);

        return Result<string>.Success(Path.Combine(templatePath, "begin_move_email_format.html"));
    }

    public async Task<Result<string>> GetPODImagesPath()
    {     
        string siteRoot =  Environment.CurrentDirectory;
        string attachmentPath = Path.Combine(siteRoot, attachmentDirectories.ATTACHMENT);
        string podImagesPath = Path.Combine(attachmentPath, "POD_Images");
        fileOperationService.CheckAndCreateDir(podImagesPath);

        return Result<string>.Success(podImagesPath);
    }


}