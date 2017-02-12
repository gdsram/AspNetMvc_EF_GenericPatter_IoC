using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IJobTagService
    {
        IQueryable<JobTag> GetAll();
        JobTag GetById(int id);
        void Insert(JobTag entity);
        void Insert(JobTag jobTag, JobPost jobPost);
        void Update(JobTag entity);
        void Delete(JobTag entity);
    }
}
