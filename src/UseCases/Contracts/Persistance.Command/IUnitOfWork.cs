using UseCases.Contracts.Persistance;

public interface IUnitOfWork: IDisposable
{
    void Commit();
    void Rollback();
    // IGenericRepository<T> GetRepository<T>() where T : class;

    public IDriverRepository DriverRepo{ get; }
    public INotificationCommandRepository NotificationRepo { get; }
    
}