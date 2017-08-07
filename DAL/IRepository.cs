using System.Linq;
using DTO;
using System.Collections.Generic;

namespace DAL
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll { get; }
    }
}
