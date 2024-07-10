namespace UseCases.SharedServices;

public class ProofOfDeliveryService: IProofOfDeliveryService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IImageService imageService;
    private readonly IDirectoryService directoryService;
    private readonly IPDFService pDFService;
    private readonly IFileOperationService fileOperationService;


    public ProofOfDeliveryService(         
            IUnitOfWork unitOfWork,
            IImageService imageService, 
            IDirectoryService directoryService, 
            IPDFService pDFService,
            IFileOperationService fileOperationService
    ){
        this.unitOfWork = unitOfWork;
        this.imageService = imageService;
        this.directoryService = directoryService;
        this.pDFService = pDFService;
        this.fileOperationService = fileOperationService;
    }

    public async Task<Result<string>> SaveProofOfDeliveryPDF(
            DriverContainerModel container, 
            string PDFPath, 
            DocumentSubmitRequestDto document, 
            string ImagePath
    ){

        var podReporResult = await GetPODReportHTML(container);
        if (podReporResult.IsFailure) return podReporResult;

        string emailContent = podReporResult.Value;

        // POD Attached Images
        var imageResult = await imageService.GetImagesAsString( container.Pro.GetProNumber(), ImagePath);
        if(imageResult.IsFailure) return imageResult;
        string imagesInString = imageResult.Value;

        DateTime receiveDate = DateTime.Now;
        if (!string.IsNullOrEmpty(document.ReceiveDate.ToString())) receiveDate = DateTime.ParseExact(document.ReceiveDate.ToString(), "MM/dd/yyyy", null);

        emailContent = emailContent.Replace("[SIGNATURE]", document.ImagesInBase64);

        emailContent = emailContent.Replace("[RECEIVED_BY]", document.ReceivedBy);
        emailContent = emailContent.Replace("[RECEIVED_DATE]", receiveDate.ToString("ddd, dd-MMM-yyyy"));
        emailContent = emailContent.Replace("[IN_TIME]", document.InTime);
        emailContent = emailContent.Replace("[OUT_TIME]", document.OutTime);
        emailContent = emailContent.Replace("[MISC_NOTE]", document.MiscellaneousNote);

        emailContent = emailContent.Replace("[IMAGES]", imagesInString);
        emailContent = emailContent.Replace("[QUANTITY]", container.Quantity.ToString());


        // Delete Previous File
        fileOperationService.DeleteFile(PDFPath);
        // await imageService.DeletePODImages(container.Pro.Value, ImagePath);

        string pdfFilePath = await pDFService.GeneratePODPDF(emailContent, PDFPath);
        Log.Information($"POD Path: {pdfFilePath}");
        return Result<string>.Success(pdfFilePath);
    }

    public async Task<Result<OrderLogDoc>> SaveProDocument(
            SessionUserDto sessionUser, 
            OrderLogID orderLogID,  
            string FolderName, 
            string DocumentFileName, 
            DocumentID documentID
    ){
        OrderLogDoc newDoc = new OrderLogDoc();

        newDoc.DocId = documentID.Value.Value; // PROOF OF DELIVERY;
        newDoc.DocNamePro = fileOperationService.GetFileWithoutExtension(DocumentFileName);
        newDoc.OrderLogId = orderLogID.Value.Value;
        newDoc.CreationUserId = sessionUser.driverID.Value.Value.ToString();
        newDoc.CreationDateTime = DateTime.Now;
        newDoc.LastUpdateDateTime = DateTime.Now;
        newDoc.LastUpdateUserId = sessionUser.driverID.Value.Value.ToString();


        newDoc.SubFolder = FolderName;
        newDoc.CompanyId = sessionUser.companyID.Value.Value.ToString();

        var document = await unitOfWork.DriverRepo.SaveProDocument(newDoc);

        return Result<OrderLogDoc>.Success(document);
    }

   

    public async Task<Result<string>> GetPODReportHTML(DriverContainerModel container)
    {
        string PODTemplateFullPath = directoryService.GetPODTemplatePath(container.LoadType.GetLoadType());

        string html =  fileOperationService.ReadFile(PODTemplateFullPath);
        if (string.IsNullOrEmpty(html)) return Error<string>.Set(PODTemplatePathError.TemplateFileNotFound(PODTemplateFullPath));

        html = html.Replace("[PRO]", container.Pro.ToString());
        html = html.Replace("[LOAD_TYPE]", container.LoadType);
        html = html.Replace("[TERMINAL]", container.Terminal);
        html = html.Replace("[CARRIER_NAME]", container.COMPANYNAME);
        html = html.Replace("[CONTAINER]", container.Container);
        html = html.Replace("[CONTAINER_SIZE]", container.ContainerSize);
        html = html.Replace("[BL]", container.BillOrBooking);

        html = html.Replace("[WEIGHT]", container.Weight is not null ?  container.Weight.ToString() : "");
         
        html = html.Replace("[PO]", container.PO);
        html = html.Replace("[SEAL]", container.Seal);
        html = html.Replace("[OTHER_REF]", container.OtherRef);
        html = html.Replace("[CONTAINER_TYPE]", container.ContainerType);


        html = html.Replace("[SCHEDULE_DELIVERY_DATE]", container.ScheduledDeliveryDate);
        html = html.Replace("[APPOINTMENT_TIME]", container.AppointmentTime);
        html = html.Replace("[DRIVER_PICKUP]", container.DriverPickup.ToString());
        html = html.Replace("[DRIVER_DELIVERY]", container.DriverDelivery);

        html = html.Replace("[CHASSIS]", container.ChassisNumber);
        html = html.Replace("[VESSEL]", container.VesselName);
        html = html.Replace("[COMMODITY]", container.Commodity);
        html = html.Replace("[ENTRY]", container.EntryNo);
        html = html.Replace("[PU_RAIL_WAREHOUSE]", container.PURailOrWarehouse);
        html = html.Replace("[Steamshipline]", container.OceanCarrier);
        html = html.Replace("[QUANTITY]", container.Quantity.ToString());

        string ConsigneeOrWarehouse = string.IsNullOrEmpty(container.Warehouse)
                ? container.Consignee 
                : container.Consignee + " C/O " + container.Warehouse;

        html = html.Replace("[CONSIGNEE_WAREHOUSE]", ConsigneeOrWarehouse);

        return Result<string>.Success(html);
    }

    

}