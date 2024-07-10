using Microsoft.Extensions.Options;

namespace UseCases.SharedServices;

public class BeginMoveEmailService: IBeginMoveEmailService
{
    private readonly IContainerRepository containerRepo;
    private readonly IUnitOfWork_WebPortal unitOfWork;
    private readonly IHTMLReportService hTMLReportService;
    private readonly IEmailService emailService;
    private readonly WebPortalURLDto webPortalURL;
    private readonly EmailConfiguration emailConfig;

    public BeginMoveEmailService(
        IContainerRepository containerRepo,
        IUnitOfWork_WebPortal unitOfWork, 
        IHTMLReportService hTMLReportService,
        IEmailService emailService,
        IOptions<WebPortalURLDto> AppSettingWebPortal,
        IOptions<EmailConfiguration> AppSettingsEmail
        )
    {
        this.containerRepo = containerRepo;
        this.unitOfWork = unitOfWork;
        this.hTMLReportService = hTMLReportService;
        this.emailService = emailService;
        webPortalURL = AppSettingWebPortal.Value;
        emailConfig = AppSettingsEmail.Value;
    }
    public async Task<Result<string>> SendBeginMoveEmail(DriverContainerModel container, DriverMoves driverMove)
    {

        if (driverMove.LegType == LegTypes.DELIVERY) 
            return Result<string>.Success("Skipped due to Delivery Leg");

        CompanyID companyID = driverMove.CompanyID.GetCompanyID();


        if(emailConfig.SendBeginMoveEmailNotification is false)
            return Result<string>.Failure(Error<string>.Set("Skipped due to SendBeginMoveEmailNotification is false in appsettings"));
       

        var result = await containerRepo.GetDivisionInfo(companyID);
        if (result.IsFailure) 
            return Result<string>.Failure(Error<string>.Set(result.Error.Message));

        GlobalSetup divisionInfo = result.Value; 

 
 
        string COMPANY_WEBSITE_LINK = companyID == CompanyIDs.IMF ? webPortalURL.IMF: webPortalURL.HWTI;


        EmailLoginTokenDto token = new EmailLoginTokenDto();
        token.container = container.Container.ToString().Trim();
        token.state = divisionInfo.State.Trim();
        token.is_warehouse = 0;

        string title = "Container On Move. ";
        title += " Pro# " + container.Pro.ToString() + " / Container# " + container.Container.ToString() + " / PO# " + container.PO.ToString();

        string titleConsignee = "", titleWarehouse = "";

        titleConsignee = title + " : CONSIGNEE";
        titleWarehouse = title + " : WAREHOUSE";

        // email.SMTPServer = emailConfig.SMTPServer;

        var resultHTML = await hTMLReportService.GetBeginMoveReportHTML(container, COMPANY_WEBSITE_LINK, divisionInfo, token);
        if(resultHTML.IsFailure) 
            return Result<string>.Failure(Error<string>.Set(resultHTML.Error.Message));

     

        // For Testing
    //    await emailService.Send("mehedi.sun@gmail.com", titleConsignee, resultHTML.Value.CONSIGNEE, true, null);
    //    await emailService.Send("mehedi@bsinfotechbd.com", titleWarehouse, resultHTML.Value.WAREHOUSE, true, null);

        Boolean isEmailSent = true;

        // For Production
        if (!string.IsNullOrEmpty(container.ConsigneePODEmail)) 
            await emailService.Send(container.ConsigneePODEmail,title, resultHTML.Value.CONSIGNEE, true, null);

        if (!string.IsNullOrEmpty(container.WarehousePODEmail)) 
            await emailService.Send(container.WarehousePODEmail, title, resultHTML.Value.WAREHOUSE, true, null);

        // SET AUTO LOGIN TOKEN
        if (isEmailSent) await SaveLoginToken(container, driverMove);

        return Result<string>.Success("Sent!");
    }

    private async Task<EmailLoginToken> SaveLoginToken(DriverContainerModel Container, DriverMoves Document)
    {
        EmailLoginToken eToken = new EmailLoginToken();

        eToken.ContainerCode = Container.Container;
        eToken.DriverId = Document.DriverID.Value;
        eToken.CreatedOn = DateTime.Now;
        eToken.ValidTill = DateTime.Now.AddDays(3);
        eToken.CompanyId = int.Parse(Document.CompanyID);

        var token = await unitOfWork.EmailTokenRepo.SaveEmailLoginToken(eToken);
        unitOfWork.Commit();
        return token;
    }
}