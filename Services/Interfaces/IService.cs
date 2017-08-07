using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IService
    {
        IEnumerable<BaseEntity> List();
        BaseEntity GetById(int id);
        void Insert(BaseEntity entity);
        void Update(BaseEntity entity);
        void Delete(BaseEntity entity);
        void Update(BaseEntity entity, List<int> idList);
    }
}
