using UseCases.Contracts.Persistance;

namespace Infrastructure.Persistance.Command;

public class GenericRepository<T>: IGenericRepository<T> where T: class
{
    private readonly DbContext dbContext;
    private DbSet<T> table;
    public GenericRepository(DbContext dbContext)
    {
        this.dbContext = dbContext;
        table = dbContext.Set<T>();
    }
    
    public virtual async Task<T> GetOneByID(int Id)
    {
        return await dbContext.Set<T>().FindAsync(Id);
    }
    public virtual async Task<T> GetOne(Expression<Func<T, bool>> predicate)
    {
        return await dbContext.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task<IReadOnlyList<T>> GetAll()
    {
        return await dbContext.Set<T>().ToListAsync();
    }
    public async Task<IReadOnlyList<T>> Get(Expression<Func<T, bool>> predicate)
    {
        return await dbContext.Set<T>().Where(predicate).ToListAsync();
    }
    public async Task<List<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return await dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T> Insert(T Entity)
    {
        await dbContext.Set<T>().AddAsync(Entity);
        return Entity;
    }

    public async Task Update(T Entity)
    {
        dbContext.Entry(Entity).State = EntityState.Modified;
    }

    public async Task<T> Update(T entity, Expression<Func<T, bool>> predicate)
    {
        T value = await table.FirstOrDefaultAsync(predicate);
        if(value is not null)
        {
            dbContext.Entry(value).CurrentValues.SetValues(entity);
        }
        
        return entity;
    }

    public async Task Delete(T Entity)
    {
        dbContext.Set<T>().Remove(Entity);
    }

    public async Task<T?> GetOneUsingSPAsync(string storeProcedure, SqlParameter[] parameters = null)
    {
        if (parameters == null)
        {
            var result = await table.FromSqlRaw<T>(storeProcedure).ToListAsync();
            return result.FirstOrDefault();
        }
        else
        {
            var result = await table.FromSqlRaw<T>(storeProcedure, parameters).ToListAsync();
            return result.FirstOrDefault();
        }
    }


    public async Task<IReadOnlyList<T>> FindUsingSPReadOnly(string storeProcedure, SqlParameter[] parameters = null)
    {
        if (parameters == null) return await table.FromSqlRaw<T>(storeProcedure).ToListAsync();
        else return await table.FromSqlRaw<T>(storeProcedure, parameters).ToListAsync();
    }
    public async Task<List<T>> FindUsingSPAsync(string storeProcedure, SqlParameter[] parameters = null)
    {
        if (parameters == null) return await table.FromSqlRaw<T>(storeProcedure).ToListAsync();
        else return await table.FromSqlRaw<T>(storeProcedure, parameters).ToListAsync();
    }

}