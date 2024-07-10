using System.Linq.Expressions;
using UseCases.Contracts.Persistance;

namespace Infrastructure.Persistance.Query;

public class GenericRepository<T>: IAsyncRepository<T> where T: class
{
    public readonly IDbConnection db;
    public readonly IDatabase cacheDB;
    public GenericRepository(IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("Default");
        db = new SqlConnection(connectionString);

        // var redis = ConnectionMultiplexer.Connect("localhost:6379");
        // cacheDB = redis.GetDatabase();
    }
    
    public virtual async Task<T> GetOneByID(int Id)
    {
        throw new NotImplementedException();
    }
    public virtual async Task<T> GetOne(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<T>> GetAll()
    {
        throw new NotImplementedException();
    }
    public async Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<T> Insert(T Entity)
    {
        throw new NotImplementedException();
    }

    public async Task Update(T Entity)
    {
        throw new NotImplementedException();
    }

    public async Task<T> Update(T entity, Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(T Entity)
    {
        throw new NotImplementedException();
    }

    public async Task<T?> GetOneUsingSPAsync(string storeProcedure, SqlParameter[] parameters = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyList<T>> FindUsingSPReadOnly(string storeProcedure, SqlParameter[] parameters = null)
    {
       throw new NotImplementedException();
    }

    public async Task<List<T>> FindUsingSPAsync(string storeProcedure, SqlParameter[] parameters = null)
    {
        throw new NotImplementedException();
    }
}