namespace UseCases.Tests;

public class ContainerMockData: UserMockData
{
    public readonly OrderLogID orderLogID = new OrderLogID((NonNegative) 451200);
    public readonly ProNumber ProNumber = new ProNumber((NonNegative) 25655);
    public readonly ProNumber ProNumber_Pickup = new ProNumber((NonNegative) 25655);
    public readonly ProNumber ProNumber_Delivery = new ProNumber((NonNegative) 25655);
    public readonly DocumentID documentID = new DocumentID((NonNegative) 11);
    public readonly DocumentID documentID_Delivery = new DocumentID((NonNegative) 10);
    public readonly LegType LegType_Delivery = new LegType((NonEmptyString) LegTypes.DELIVERY);
  
    public readonly string ReceivedBy = "Mehedi Hasan";
    public readonly string ReceivedDate = DateTime.Now.ToString("MM/dd/yyyy");    
    public readonly string InTime = "12:30";
    public readonly string OutTime = "18:40";
    public readonly string MiscellaneousNote = "Some note goes here";
    public string ImagesInBase64 = "";

    public readonly string IMF_WEBSITE = "https://imficontainer.com/#/search?container=";
    public readonly string HWTIß_WEBSITE = "https://hwticontainer.com/#/search?container=";
    public readonly string DIVISION_FOLDER_NAME = "scanneddocs";
    public readonly string EMPTY_PARAMETER = "";
    public readonly string POD_PATH_IMPORT = "StaticContents\\pod_format\\IMF_POD_IMPORT.html";
    public readonly string POD_PATH_EXPORT = "StaticContents\\pod_format\\IMF_POD_EXPORT.html";
    public readonly string POD_PATH_LOOSE_FRIEGHT = "StaticContents\\pod_format\\IMF_POD_LOOSEFRIEGHT.html";
    public readonly string FILE_NAME_WITHOUT_EXT = "765_282396_2418211118_Outbound_TIR";
    public readonly string POD_ImagePath = "attachments\\POD_Images";

    public string Data_RootDir = "";
    public string API_Project_Dir = "";

    public ContainerMockData()
    {
        Data_RootDir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.ToString(),"MockData");
        Data_RootDir = Path.Combine(Data_RootDir,"Data");

        API_Project_Dir = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.ToString(),"src");
        API_Project_Dir = Path.Combine(API_Project_Dir,"API");
    }

    public string GetPODTemplatePath(LoadType loadType)
    {
        string rootPath = Path.Combine(API_Project_Dir,"StaticContents");
        rootPath = Path.Combine(rootPath,"pod_format");

        return loadType.Value.ToUpper() switch
        {
            "IMPORT" => Path.Combine(rootPath,"IMF_POD_IMPORT.html"),
            "EXPORT" => Path.Combine(rootPath,"IMF_POD_EXPORT.html"),
            _ => Path.Combine(rootPath,"IMF_POD_LOOSEFRIEGHT.html"),
        };

    }

    public string GetSamplePODPDFPath()
    {        
        return Path.Combine(Data_RootDir, "samplePOD.pdf");
    }

    public string GetPODTemplate(LoadType loadType)    
    {
        using(StreamReader sr = new StreamReader(GetPODTemplatePath(loadType)))
        {
            return sr.ReadToEnd();
        }
    }

    public string GetPickupMTContainer()
    {
        using(StreamReader sr = new StreamReader(Path.Combine(Data_RootDir, "Container_Pickup_MT.json")))
        {
            return sr.ReadToEnd();
        }
    }


    public string GetPickupContainer(){
        using(StreamReader sr = new StreamReader(Path.Combine(Data_RootDir, "Container_Pickup.json")))
        {
            return sr.ReadToEnd();
        }
    }

    public string GetDeliveryContainer(){
        using(StreamReader sr = new StreamReader(Path.Combine(Data_RootDir, "Container_Delivery.json")))
        {
            return sr.ReadToEnd();
        }
    }

    public string GetPODEmailContent(){
        using(StreamReader sr = new StreamReader(Path.Combine(Data_RootDir, "POD_EmailContent.txt")))
        {
            return sr.ReadToEnd();
        }
    }
    public string GetPODImageAsString(){
        using(StreamReader sr = new StreamReader(Path.Combine(Data_RootDir, "PODImage.txt")))
        {
            return sr.ReadToEnd();
        }
    }

    public CommonRequestDto GetEmptyCommonRequestDto()
    {
        return new CommonRequestDto( Parameters: "" );
    }
    public CommonRequestDto GetCommonRequestDto()
    {
        string Parameters = $"'ProNumber': {this.ProNumber.Value}, 'LegType': '{LegTypes.PICKUP_MT}'";
        return new CommonRequestDto(Parameters:Parameters );
    }

    public ContainerRequestDto GetContainerRequestDto()
    {
        return new ContainerRequestDto(
            proNumber : this.ProNumber,
            legType : LegTypes.PICKUP_MT.GetLegType(),
            driverID: driverID
        );
    }
    public ContainerRequest GetContainerRequest()
    {
        return new ContainerRequest(
            ProNumber : this.ProNumber.Value,
            LegType : LegTypes.PICKUP_MT
        );
    }


    public ContainerRequestDto GetContainerRequestDtoWithInvalidData()
    {
        return  new ContainerRequestDto(new DriverID((NonNegative) 0), new ProNumber((NonNegative) 0),  null);            
    }

    public DriverContainerModel GetPickupMTContainerModelDto()
    {
        return JsonSerializer.Deserialize<DriverContainerModel>(GetPickupMTContainer());
    }
    public Result<DriverContainerModel> GetContainer(LegType legType) 
    {
        var data = legType.Value.ToString() switch{
            "PICKUP" => GetPickupContainer(),
            "PICKUP MT" => GetPickupMTContainer(),
            "DELIVERY" => GetDeliveryContainer(),
            _ => GetPickupContainer(),
        }; 

        return Result<DriverContainerModel>.Success(JsonSerializer.Deserialize<DriverContainerModel>(data));
    }

    public StaticFileDto GetStaticFileDto()
    {
        return new StaticFileDto(        
            BasePath : "\\\\test+documentstorageaccount.file.core.windows.net\\fs-docs"
        );
    }
    
    public DocumentRequestDto GetPDFRequestDto()
    {
        return new DocumentRequestDto(
            companyID: this.companyID,
            proNumber: this.ProNumber,
            documentID: this.documentID
        );
    }


    public DocumentSubmitRequestDto GetDocumentSubmitRequestDto(int ProNumber, string LegType = LegTypes.PICKUP_MT)
    {
        this.ImagesInBase64 = GetPODImageAsString();
        return DocumentSubmitRequestDto.Get(new DocumentSubmitRequest(){
             DriverID = driverID.Value,
             ProNumber = ProNumber,
             LegType = LegType,
             ReceivedBy = this.ReceivedBy,
             ReceiveDate = this.ReceivedDate,
             InTime = this.InTime,
             OutTime = this.OutTime,
             MiscellaneousNote = this.MiscellaneousNote,
             ImagesInBase64 = this.ImagesInBase64
        });
    }
    public DocumentSubmitRequestDto GetDocumentSubmit_NoImageRequestDto(int ProNumber, string LegType = LegTypes.PICKUP_MT)
    {
        return DocumentSubmitRequestDto.Get(new DocumentSubmitRequest(){
             DriverID = driverID.Value,
             ProNumber = ProNumber,
             LegType = LegType,
             ReceivedBy = this.ReceivedBy,
             ReceiveDate = this.ReceivedDate,
             InTime = this.InTime,
             OutTime = this.OutTime,
             MiscellaneousNote = this.MiscellaneousNote,
             ImagesInBase64 = ""
        });
    }


    



    public Result<IReadOnlyList<DocumentNotifications>> GetNotifications()
    {
        IReadOnlyList<DocumentNotifications> notifications = new List<DocumentNotifications>()
        {
            new DocumentNotifications(){
                Id = 1,
                DriverId = driverID.Value,
                ProNumber = ProNumber.Value,
                Title = "TEST",
                Details = "Test Notification",
            },
            new DocumentNotifications(){
                Id = 2,
                DriverId = driverID.Value,
                ProNumber = ProNumber.Value,
                Title = "TEST 2",
                Details = "Test Notification 2",
            }
        };

        return Result<IReadOnlyList<DocumentNotifications>>.Success(notifications);
    }

    public Result<FileInfoDto> GetPDFInfo()
    {
        return Result<FileInfoDto>.Success(
            new FileInfoDto(
                FileParentFolder : "\\\\test+documentstorageaccount.file.core.windows.net\\fs-docs\\scanneddocsNJ\\DocsQueue\\",
                FileName : "765_282396_2418211118_Outbound_TIR.pdf",
                FileFullPath: "\\\\test+documentstorageaccount.file.core.windows.net\\fs-docs\\scanneddocsNJ\\DocsQueue\\/765_282396_2418211118_Outbound_TIR.pdf"
            )
        );
    }

    public Result<DriverDocuments> GetDocuments(string legType)
    {
        return Result<DriverDocuments>.Success(
            new DriverDocuments()
            {
                ContainerCode = "TEST_CONTAINER",
                DocumentId = documentID.Value,
                ProNumber = ProNumber.Value,
                LegType = legType,
                CompanyId = companyID.Value          
            }            
        );
    }

    public Result<OrderLogDocument> GetOrderLogDocument(int ProNumber, int documentID)
    {
        var fileinfo = GetPDFInfo().Value;
        var sessionUser = this.GetSessionData().Value;
        
        return Result<OrderLogDocument>.Success(
            new OrderLogDocument()
            {
                DocId = documentID, // PROOF OF DELIVERY;
                DocNamePro = FILE_NAME_WITHOUT_EXT,
                OrderLogId = ProNumber,
                SubFolder = fileinfo.FileParentFolder,
                CompanyId = companyID.Value
            }
        );
    }
    public Result<OrderLogDoc> GetOrderLogDoc(int ProNumber, int documentID)
    {
        var fileinfo = GetPDFInfo().Value;
        var sessionUser = this.GetSessionData().Value;
        
        return Result<OrderLogDoc>.Success(
            new OrderLogDoc()
            {
                DocId = documentID, // PROOF OF DELIVERY;
                DocNamePro = FILE_NAME_WITHOUT_EXT,
                OrderLogId = ProNumber,
                SubFolder = fileinfo.FileParentFolder,
                CompanyId = companyID.Value.ToString()
            }
        );
    }

    public Result<DriverMoves> GetDriverMove(ProNumber ProNumber, LegType legType)
    {
        return Result<DriverMoves>.Success(
            new DriverMoves()
            {
                ID = 1,
                BeginMovedOn = DateTime.Now,
                ProNumber = ProNumber.Value,
                DriverID = driverID.Value,
                LegType = legType.Value,
                ViewMovedOn = DateTime.Now,
                IsEmailSent = false,
                MoveCompletedOn = null,
                CompanyID = companyID.Value.ToString()
            }
        );
    }
    public Result<List<DriverMoves>> GetDriverMoves(ProNumber ProNumber, LegType legType)
    {
        var data  = new List<DriverMoves>(); 
        data.Add(GetDriverMove(ProNumber, legType).Value);
        return Result<List<DriverMoves>>.Success(data);
    }

    public Result<IReadOnlyList<PurchaseOrder>> GetPurchaseOrders()
    {
        var data = new List<PurchaseOrder> {
            new PurchaseOrder(),
            new PurchaseOrder()
        };
        return Result<IReadOnlyList<PurchaseOrder>>.Success(data);
    }

    public Result<IReadOnlyList<PurchaseOrder>> GetPurchaseOrdersErrorResult(ProNumber ProNumber)
    {
        return PurchaseOrderError<IReadOnlyList<PurchaseOrder>>.PurchaseOrderNotFound(ProNumber.Value);
    }

    public EmailConfiguration GetEmailConfiguration()
    {
        return new EmailConfiguration(){
            Email = "test@gmail.com",
            Password = "32434234",                        
        };
    }
}