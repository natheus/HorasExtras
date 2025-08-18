namespace OvertimeManagement.Domain.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetByIdAsync(int id);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task SaveAsync(TEntity entity);
    Task UpdateAsync(TEntity entity);
    Task RemoveAsync(int id);
}
