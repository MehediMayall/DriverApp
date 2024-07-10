namespace Infrastructure.Persistance.Command;

public class EmailTokeRepository: GenericRepository<EmailLoginToken>, IEmailTokeRepository
{
    private readonly WebPortalContext dbContext;

    public EmailTokeRepository(WebPortalContext dbContext) : base(dbContext)
    {
        this.dbContext = dbContext;
    }

   
    public async Task<EmailLoginToken> SaveEmailLoginToken(EmailLoginToken Entity)
    {
        return await new GenericRepository<EmailLoginToken>(dbContext).Insert(Entity);
    }
 

}