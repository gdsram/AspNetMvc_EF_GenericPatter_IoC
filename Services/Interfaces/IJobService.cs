using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IJobService
    {
        IEnumerable<Job> GetJobs();
        Job GetJob(long id);
        void InsertJob(Job job);
        void UpdateJob(Job job);
        void DeleteJob(Job job);
    }
}
