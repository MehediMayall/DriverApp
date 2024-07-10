
namespace Infrastructure.Persistance.Command;

public class DriverContext : DbContext
{
    public DriverContext(DbContextOptions<DriverContext> options): base(options)
    {
        
    }

    // Models
    public DbSet<OrderLogDoc> OrderLogDoc => Set<OrderLogDoc>();
    public DbSet<GlobalSetup> GlobalSetup => Set<GlobalSetup>();
    public DbSet<Driver> Driver => Set<Driver>();
    public DbSet<DriverMoves> DriverMoves => Set<DriverMoves>();
    public DbSet<DriverDocuments> DriverDocuments => Set<DriverDocuments>();
    public DbSet<OrderLog> OrderLog => Set<OrderLog>();
    public DbSet<PurchaseOrder> PurchaseOrder => Set<PurchaseOrder>();
    public DbSet<DocumentNotifications> DocumentNotifications => Set<DocumentNotifications>();

}
