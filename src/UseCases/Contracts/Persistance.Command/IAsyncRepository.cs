namespace UseCases.Contracts.Persistance;

public interface IAsyncRepository<T> where T:class
{
    Task<T> GetOneByID(int Id);
    Task<IReadOnlyList<T>> GetAll();
    Task<T> Insert(T Entity);
    Task Update(T Entity);
    Task Delete(T Entity);
}