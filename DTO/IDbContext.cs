using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace DTO
{
   public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity t) where TEntity : BaseEntity;

        int SaveChanges();
    }
}
