namespace UseCases.Contracts.Infrastructure;

public interface IEmailService
{
    Task<bool> Send(string emailTo, string emailSubject, string emailBody,bool IsBodyHtml, List<string> attachments = null);
}
