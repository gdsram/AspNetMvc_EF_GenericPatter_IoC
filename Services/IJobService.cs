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
        IQueryable<job> GetJobs();
        job GetJob(long id);
        void InsertJob(job job);
        void UpdateJob(job job);
        void DeleteJob(job job);
    }
}
