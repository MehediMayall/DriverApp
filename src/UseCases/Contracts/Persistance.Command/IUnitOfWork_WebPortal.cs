
public interface IUnitOfWork_WebPortal: IDisposable
{
    void Commit();
    void Rollback();
    // IGenericRepository<T> GetRepository<T>() where T : class;

    public IEmailTokeRepository EmailTokenRepo{ get; }
    
}