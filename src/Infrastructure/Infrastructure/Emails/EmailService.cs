
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Options;
using Serilog;

namespace DriverApp.Infrastructure.Emails;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration emailConfig;

    public EmailService(IOptions<EmailConfiguration> AppSettingsOption)
    {
        emailConfig = AppSettingsOption.Value;
    }

    public async Task<bool> Send(string emailTo, string emailSubject, string emailBody,bool IsBodyHtml, List<string> attachments = null)
    {       
        // Validation
        if(emailTo.Split(";").Length<=0) throw new Exception($"Send email failed due to empty value of Email To address. emailTo: {emailTo}");
        if(string.IsNullOrEmpty(emailSubject)) throw new Exception($"Send email failed due to empty email subject . emailSubject: {emailSubject}");
        if(string.IsNullOrEmpty(emailConfig.Email)) throw new Exception($"Send email failed due to empty send email address . emailConfig.Email: {emailConfig.Email}");
        if(string.IsNullOrEmpty(emailConfig.Password)) throw new Exception($"Send email failed due to empty send email password . emailConfig.Password: {emailConfig.Password}");

        MailAddress mailTo = new MailAddress(emailConfig.Email);


        MailAddress mailFrom = new MailAddress(emailConfig.Email);
        MailMessage mailMessage = new MailMessage(mailFrom, mailTo);

        if (!String.IsNullOrEmpty(emailSubject)) mailMessage.Subject = emailSubject;        
        else throw new Exception($"Send email failed due to Email Subject is empty. emailSubject: {emailSubject}");

        if (!String.IsNullOrEmpty(emailBody)) mailMessage.Body = emailBody;
        else throw new Exception($"Send email failed due to Email Body is empty. emailBody: {emailBody}");


        mailMessage.IsBodyHtml = IsBodyHtml;

        if (emailTo.Contains(";"))
        {
            String[] addr = emailTo.Split(';');
            for (int i = 0; i < addr.Length; i++)
            {
                if (!string.IsNullOrEmpty(addr[i])) mailMessage.To.Add(addr[i]);
            }
        }


        // EMAIL CC
        if (!String.IsNullOrEmpty(emailConfig.EmailCC))
        {
            if (emailConfig.EmailCC.Contains(";"))
            {
                String[] addr = emailConfig.EmailCC.Split(';');
                foreach(string email in addr) if (!string.IsNullOrEmpty(email)) mailMessage.CC.Add(email);
            }
            else
            {
                mailMessage.CC.Add(emailConfig.EmailCC);
            }
        }


        // EMAIL BCC
        if (!String.IsNullOrEmpty(emailConfig.EmailBCC))
        {
            if (emailConfig.EmailBCC.Contains(";"))
            {
                String[] addr = emailConfig.EmailBCC.Split(';');
                for (int i = 0; i < addr.Length; i++) if (!string.IsNullOrEmpty(addr[i])) mailMessage.Bcc.Add(addr[i]);
            }
            else
            {
                mailMessage.CC.Add(emailConfig.EmailBCC);
            }
        }


        string strSMTPServer = emailConfig.SMTPServer;
        if(string.IsNullOrEmpty(strSMTPServer)) strSMTPServer =  "smtp.office365.com";


        if (attachments != null)
        {
            foreach (string item in attachments)
            {
                FileInfo file = new FileInfo(item);

                if (file.Exists)
                {
                    mailMessage.Attachments.Add(new Attachment(file.FullName));
                }
            }
        }

        using (SmtpClient smtpClient = new SmtpClient(strSMTPServer, 587))
        {
            smtpClient.Credentials = new NetworkCredential(emailConfig.Email, emailConfig.Password);
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

            if (mailMessage != null)
            {
                smtpClient.Send(mailMessage);
                Log.Information($"Email successfully has been sent. Email to:{emailTo} {emailConfig.EmailCC}, Subject: {emailSubject}");
                smtpClient.Dispose();
                mailMessage.Dispose();
            }
            else throw new Exception($"Email send failed due to mailMessage is null . Email to:{emailTo}, Subject: {emailSubject}");
        }

        return true;   
    }

    protected string GetErrorMessage(Exception ex)
    {
        try
        {
            var (Source, Message) = (ex?.Source ?? string.Empty, ex?.Message ?? string.Empty);
            StringBuilder ErrorDetail = new();
            ErrorDetail.Append($"Source: {Source} ==> {Message}.");


            // Inner Exception level 1
            if (ex!.InnerException != null)
            {
                var iEx = ex.InnerException;
                ErrorDetail.Append($"Source: {iEx!.Source ?? string.Empty} ==> {iEx!.Message ?? string.Empty}");

                // Inner Exception level 2
                if (iEx!.InnerException != null)
                {
                    var iEx2 = iEx.InnerException;
                    ErrorDetail.Append($"Source: {iEx2!.Source ?? string.Empty} ==> {iEx2!.Message ?? string.Empty}");
                }
            }
            return ErrorDetail.ToString();
        }
        catch(Exception exception){ 
            return "Unexpted error occured in GetErrorMessage. Exception detail: " + exception.Message; 
        }
    }

    
}
