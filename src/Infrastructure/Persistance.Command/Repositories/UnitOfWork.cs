
namespace Infrastructure.Persistance.Command;

public class UnitOfWork : IUnitOfWork
{
    private readonly DriverContext context;
    private readonly ICacheService cacheService;
    // private Dictionary<Type, object> repository;


    public UnitOfWork(DriverContext context, ICacheService cacheService)
    {
        this.context = context;
        this.cacheService = cacheService;
        // repository = new Dictionary<Type, object>();
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
    public IDriverRepository DriverRepo=> new DriverRepository(this.context, this.cacheService);
    public INotificationCommandRepository NotificationRepo=> new NotificationCommandRepository(this.context, this.cacheService);

    // public IGenericRepository<T> GetRepository<T>() where T : class
    // {
    //     if(repository.ContainsKey(typeof(T)))
    //     {
    //         return (IGenericRepository<T>) repository[typeof(T)];
    //     }

    //     var repo = new GenericRepository<T>(context);
    //     repository.Add(typeof(T), repo);
    //     return repo;
    // }




    public void Dispose()
    {
        context.Dispose();
    }
}