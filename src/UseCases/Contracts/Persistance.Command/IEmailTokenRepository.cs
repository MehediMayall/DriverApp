namespace UseCases.Contracts.Persistance.Command;


public interface IEmailTokeRepository : IAsyncRepository<EmailLoginToken>
{
    Task<EmailLoginToken> SaveEmailLoginToken(EmailLoginToken Entity);
}