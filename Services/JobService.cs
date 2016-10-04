using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class JobService : IJobService
    {
        private IRepository<job> jobRepository;

        public JobService(IRepository<job> jobRepository)
        {
            this.jobRepository = jobRepository;
        }

        public IQueryable<job> GetJobs()
        {
            return jobRepository.Table;
        }

        public job GetJob(long id)
        {
            return jobRepository.GetById(id);
        }

        public void InsertJob(job person)
        {
            jobRepository.Insert(person);
        }

        public void UpdateJob(job person)
        {
            jobRepository.Update(person);
        }

        public void DeleteJob(job job)
        {
            jobRepository.Delete(job);
        }

    }
}
