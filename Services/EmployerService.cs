using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace Services
{
    public class EmployerService : IEmployerService
    {
        private readonly IRepository<Employer> employerRepository = null;

        public EmployerService(IRepository<Employer> employerRepository)
        {
            this.employerRepository = employerRepository;
        }

        public void Delete(Employer entity)
        {
            employerRepository.Delete(entity);
        }

        public Employer GetById(int id)
        {
            return employerRepository.GetById(id);
        }

        public void Insert(Employer entity)
        {
            employerRepository.Insert(entity);
        }

        public IQueryable<Employer> List()
        {
            return employerRepository.Table;
        }

        public void Update(Employer entity)
        {
            employerRepository.Update(entity);
        }
    }
}
