using Microsoft.Extensions.Options;

namespace UseCases.SharedServices;

public class ProofOfDeliveryEmailService: IProofOfDeliveryEmailService
{

    private readonly IEmailService emailService;
    private readonly IConfiguration configuration;
    private readonly EmailConfiguration emailConfig;

    public ProofOfDeliveryEmailService(         
        IEmailService emailService,
        IOptions<EmailConfiguration> emailConfig,
        IConfiguration configuration
    )
    {
        this.emailService = emailService;
        this.configuration = configuration;
        this.emailConfig = emailConfig.Value;
    }

  
    public async Task<Result<string>> SendPODASEmail(DriverContainerModel container, string PODFilePath)
    {
        if( emailConfig.SendPODEmailNotification is false)
            return Result<string>.Failure(Error<string>.Set("Skipped due to SendPODEmailNotification is false in appsettings"));



        string  emailTo="";
        string emailSubject = "Proof of Delivery -  Pro# {container.Pro}, Container Code: {container.ContainerCode}";
        string emailBody = $"<b>Proof of Delivery</b> <br><br><br> Container Code: {container.ContainerCode}, Pro# {container.Pro}";
        bool IsBodyHtml = true;

        List<string> attachment = new List<string>();
        attachment.Add(PODFilePath);
        
        emailSubject = " Pro# " + container.Pro.ToString() + " / Container # " + container.Container.ToUpper().Trim();

        if (!string.IsNullOrEmpty(container.ConsigneePODEmail)) emailTo = container.ConsigneePODEmail;
        if (!string.IsNullOrEmpty(container.WarehousePODEmail)) emailTo = container.WarehousePODEmail;

        await emailService.Send(emailTo, emailSubject, emailBody, IsBodyHtml, attachment);

        attachment = null;
        return Result<string>.Success("");
    }



}