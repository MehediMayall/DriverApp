
namespace Infrastructure.Persistance.Command;

public class WebPortalContext : DbContext
{
    public WebPortalContext(DbContextOptions<WebPortalContext> options): base(options)
    {
        
    }

    // Models
    public DbSet<EmailLoginToken> EmailLoginToken => Set<EmailLoginToken>();


}
