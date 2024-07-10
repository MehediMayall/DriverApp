
namespace Infrastructure.Persistance.Command;

public class UnitOfWork_WebPortal : IUnitOfWork_WebPortal
{
    private readonly WebPortalContext context;


    public UnitOfWork_WebPortal(WebPortalContext context)
    {
        this.context = context;
    }

    public void Commit()
    {
        context.SaveChanges();
    }
    public void Rollback()
    {
        throw new NotImplementedException();
    }

    // Repository
    public IEmailTokeRepository EmailTokenRepo => new EmailTokeRepository(this.context);
 




    public void Dispose()
    {
        context.Dispose();
    }
}