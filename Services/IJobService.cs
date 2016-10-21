using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IJobService
    {
        IQueryable<Job> GetJobs();
        Job GetJob(long id);
        void InsertJob(Job job);
        void UpdateJob(Job job);
        void DeleteJob(Job job);
    }
}
