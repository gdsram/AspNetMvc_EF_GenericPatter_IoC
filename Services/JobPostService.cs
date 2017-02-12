using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using DAL;

namespace Services
{
    public class JobPostService : IJobPostService
    {
        private readonly IManyToManyRepository<JobPost, JobTag> jobPostTagRepository = null;
        private readonly IRepository<JobPost> jobPostRepository = null;
        private const string navigationProperty = "JobTags";

        public JobPostService(IRepository<JobPost> jobPostRepository, IManyToManyRepository<JobPost, JobTag> jobPostTagRepository)
        {
            this.jobPostRepository = jobPostRepository;
            this.jobPostTagRepository = jobPostTagRepository;
        }

        public void Delete(JobPost jobPost)
        {
            jobPostTagRepository.DeleteAllRelationship(jobPost.Id, navigationProperty);
            jobPostRepository.Delete(jobPost);
        }

        public JobPost GetById(int id)
        {
            return jobPostRepository.GetById(id);
        }

        public void Insert(JobPost jobPost, JobTag jobTag)
        {
            jobPostTagRepository.InsertWithoutData(jobPost, jobTag, navigationProperty);
            // Insert(jobPost);
        }

        /// <summary>
        /// Returns the list of all JobPost
        /// </summary>
        /// <returns></returns>
        public IQueryable<JobPost> GetAll()
        {
            return jobPostRepository.Table;
        }

        public void Update(JobPost jobPost, List<int> jobTagsId)
        {
            jobPostTagRepository.UpdateRelationship(jobPost.Id, jobPost.JobTags.Select(x => x.Id).ToList(), jobTagsId, navigationProperty);
            jobPostRepository.Update(jobPost);
        }

        public void Insert(JobPost jobPost)
        {
            jobPostRepository.Insert(jobPost);
            jobPostTagRepository.InsertWithData(jobPost.Id, jobPost.JobTags.Select(x => x.Id).ToList(), navigationProperty);
        }

        public void Update(JobPost jobPost)
        {
            jobPostTagRepository.UpdateRelationship(jobPost.Id, jobPost.JobTags.Select(x => x.Id).ToList(), navigationProperty);
            jobPostRepository.Update(jobPost);
        }
    }
}
