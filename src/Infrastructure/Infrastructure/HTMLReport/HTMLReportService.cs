
using Domain.Exceptions;
using Domain.Shared;
using Domain.ValueObjects;
using Infrastructure.Library;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.Infrastructure;

public class HTMLReportService: IHTMLReportService
{
    private readonly IDirectoryService directoryService;
    private readonly IFileOperationService fileOperationService;
    private readonly IPDFService pDFService;

    private readonly AttachmentDirectoriesDto attachmentDirectories;

    public HTMLReportService(
        IDirectoryService directoryService, 
        IFileOperationService fileOperationService, 
        IPDFService pDFService,
        IOptions<AttachmentDirectoriesDto> AppSettingAttachment
        )
    {
        this.directoryService = directoryService;
        this.fileOperationService = fileOperationService;
        this.pDFService = pDFService;
        attachmentDirectories = AppSettingAttachment.Value;
    }


    public async Task<Result<string>> GetPODReportHTML(DriverContainerModel container)
    {
        string PODTemplateFullPath = directoryService.GetPODTemplatePath(new LoadType(container.LoadType));
        string templateHTML =  fileOperationService.ReadFile(PODTemplateFullPath);
        return Result<string>.Success(GetHTMLReport(container, templateHTML));
    }

    public async Task<Result<FileInfoDto>> GetBillOfLadingReportHTML(DriverContainerModel container)
    {
        var result = await directoryService.GetBillofLadingTemplatePath(new LoadType(container.LoadType));
        if (result.IsFailure) return Result<FileInfoDto>.Failure(Error<FileInfoDto>.Set("No html found from GetBillofLadingTemplatePath"));

        string templateHTML =  fileOperationService.ReadFile(result.Value);
        var resultHTML = Result<string>.Success(GetHTMLReport(container, templateHTML));
        if (resultHTML.IsFailure) return Result<FileInfoDto>.Failure(Error<FileInfoDto>.Set(resultHTML.Error.Message));

        // string reportFileName = "billoflading_" + container.Consignee + "_"  + container.Pro.ToString() + "_" + DateTime.Now.ToString("mmss") + ".pdf";
        string reportFileName = "billoflading_"  + container.Pro.ToString() + "_" + DateTime.Now.ToString("mmss") + ".pdf";
        
 
        string pdfFilePath = Path.Combine(Environment.CurrentDirectory, Path.Combine(attachmentDirectories.BOL, reportFileName));


        await pDFService.GeneratePODPDF(resultHTML.Value, pdfFilePath);

        return Result<FileInfoDto>.Success(
            new FileInfoDto(
                FileName : reportFileName,
                FileParentFolder: "",
                FileFullPath: pdfFilePath
            ));
    }

    public async Task<Result<BeginMoveEmailContentDto>> GetBeginMoveReportHTML(DriverContainerModel container, string LINK, GlobalSetup DivisionInfo, EmailLoginTokenDto token)
    {
        var result = await directoryService.GetBeginMoveEmailTemplatePath();
        if (result.IsFailure) return Result<BeginMoveEmailContentDto>.Failure(Error<BeginMoveEmailContentDto>.Set("No html found from GetBillofLadingTemplatePath"));

        string templateHTML =  fileOperationService.ReadFile(result.Value);


        BeginMoveEmailContentDto emailContent = new BeginMoveEmailContentDto();
 
        var resultHTML = Result<string>.Success(GetHTMLReport(container, templateHTML));
        if (resultHTML.IsFailure) return Result<BeginMoveEmailContentDto>.Failure(Error<BeginMoveEmailContentDto>.Set(resultHTML.Error.Message));

     
        string consigneeContent = resultHTML.Value;
        string warehouseContent = resultHTML.Value;

        consigneeContent = consigneeContent.Replace("[PO]", container.PO);
        consigneeContent = consigneeContent.Replace("[CONTAINER_NUMBER]", container.Container);

        var linkParam = new JSONSerialize().EncodeBase64(new JSONSerialize().getJSONString(token));

        consigneeContent = consigneeContent.Replace("[LINK]", LINK + linkParam);
        consigneeContent = consigneeContent.Replace("[COMPANY_NAME]", DivisionInfo.CompanyName);



        // WAREHOUSE
        warehouseContent = warehouseContent.Replace("[PO]", container.PO);
        warehouseContent = warehouseContent.Replace("[CONTAINER_NUMBER]", container.Container);

        // token.is_warehouse = 1;
        linkParam = new JSONSerialize().EncodeBase64(new JSONSerialize().getJSONString(token));
        warehouseContent = warehouseContent.Replace("[LINK]", LINK + linkParam);
        warehouseContent = warehouseContent.Replace("[COMPANY_NAME]", DivisionInfo.CompanyName);

        emailContent.CONSIGNEE = consigneeContent;
        emailContent.WAREHOUSE = warehouseContent;

        return Result<BeginMoveEmailContentDto>.Success(emailContent);
    }


    private string GetHTMLReport(DriverContainerModel container, string html )
    {
        
        html = html.Replace("[PRO]", container.Pro.ToString());
        html = html.Replace("[LOAD_TYPE]", container.LoadType);
        html = html.Replace("[TERMINAL]", container.Terminal);
        html = html.Replace("[CARRIER_NAME]", container.COMPANYNAME);
        html = html.Replace("[CONTAINER]", container.Container);
        html = html.Replace("[CONTAINER_SIZE]", container.ContainerSize);
        html = html.Replace("[BL]", container.BillOrBooking);

        if (container.Weight != null) html = html.Replace("[WEIGHT]", container.Weight.ToString());
        else html = html.Replace("[WEIGHT]", "");

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

        string ConsigneeOrWarehouse = "";

        if (!string.IsNullOrEmpty(container.Warehouse)) ConsigneeOrWarehouse = container.Consignee + " C/O " + container.Warehouse;
        else ConsigneeOrWarehouse = container.Consignee;

        html = html.Replace("[CONSIGNEE_WAREHOUSE]", ConsigneeOrWarehouse);

        return html;
    }
}