namespace UseCases.Dtos;
public class EmailConfiguration
{
    public string Email {get;set;}
    public string Password {get;set;}
    public string SMTPServer {get;set;}
    public string EmailCC {get;set;}
    public string EmailBCC {get;set;}
    public string POD_Email_Title {get;set;}
    public bool SendBeginMoveEmailNotification {get;set;}
    public bool SendPODEmailNotification {get;set;}
};
