using DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
public interface IBaseRepository<T>: IDisposable
  {
    IDbContext Context { get; }
    T Find<TId>(TId id, bool traking = true);
    void Delete<TId>(TId id);
    void InsertOrUpdate(params T[] entities);  
    bool Any(params Expression<Func<T, bool>>[] expressions);
  }
}
