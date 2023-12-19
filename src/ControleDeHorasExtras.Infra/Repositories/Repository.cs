using ControleDeHorasExtras.Domain.Interfaces;
using ControleDeHorasExtras.Domain.Models;
using ControleDeHorasExtras.Infra.Context;

namespace ControleDeHorasExtras.Infra.Repositories
{
    public class Repository<TEntity>(AppDbContext context) : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context = context;

        public virtual TEntity GetById(int id)
        {
            var query = _context.Set<TEntity>().Where(e => e.Id == id);

            if (query.Any())
                return query.FirstOrDefault();

            return null;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var query = _context.Set<TEntity>();

            if (query.Any())
                return query.ToList();

            return new List<TEntity>();
        }

        public virtual void Save(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }
    }
}
