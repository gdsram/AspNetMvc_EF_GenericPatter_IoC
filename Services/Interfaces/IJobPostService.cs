using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IJobPostService
    {
        IEnumerable<JobPost> GetAll();
        JobPost GetById(int id);
        void Insert(JobPost jobPost);
        void Insert(JobPost jobPost, JobTag jobTag);
        void Update(JobPost jobPost);
        void Delete(JobPost jobPost);
        void Update(JobPost jobPost, List<int> jobTagsId);
    }
}
