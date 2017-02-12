using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace Services
{
    public class JobTagService : IJobTagService
    {
        private readonly IRepository<JobTag> jobTagRepository = null;
        private readonly IManyToManyRepository<JobTag, JobPost> jobTagPostRepository = null;
        private const string navigationProperty = "JobPosts";

        public JobTagService(IRepository<JobTag> jobTagRepository, IManyToManyRepository<JobTag, JobPost> jobTagPostRepository)
        {
            this.jobTagRepository = jobTagRepository;
            this.jobTagPostRepository = jobTagPostRepository;
        }

        public void Delete(JobTag entity)
        {
            jobTagPostRepository.DeleteRelationship(entity.Id, entity.JobPosts.Select(x => x.Id).ToList(), navigationProperty);
            jobTagRepository.Delete(entity);
        }

        public JobTag GetById(int id)
        {
            return jobTagRepository.GetById(id);
        }

        public void Insert(JobTag entity)
        {
            jobTagRepository.Insert(entity);
        }

        public IQueryable<JobTag> GetAll()
        {
            return jobTagRepository.Table;
        }

        public void Update(JobTag entity)
        {
            jobTagRepository.Update(entity);
        }

        public void Update(JobTag entity, List<int> idList)
        {
            jobTagPostRepository.UpdateRelationship(entity.Id, entity.JobPosts.Select(x => x.Id).ToList(), idList, navigationProperty);
            jobTagRepository.Update(entity);
        }

        public void Insert(JobTag jobTag, JobPost jobPost)
        {
            jobTagPostRepository.InsertWithoutData(jobTag, jobPost, navigationProperty);
            jobTagRepository.Insert(jobTag);
        }
    }
}
