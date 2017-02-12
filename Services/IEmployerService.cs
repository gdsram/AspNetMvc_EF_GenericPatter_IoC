using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IEmployerService
    {
        IQueryable<Employer> List();
        Employer GetById(int id);
        void Insert(Employer entity);
        void Update(Employer entity);
        void Delete(Employer entity);
    }
}
