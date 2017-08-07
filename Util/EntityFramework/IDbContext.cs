using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Util.EntityFramework
{
   public interface IDbContext
    {
        ISet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity t) where TEntity : BaseEntity;

        int SaveChanges();
    }
}
