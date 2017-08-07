using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DTO;
using AspNetMvc_EF_GenericPattern_IoC.Models;
using Services.Interfaces;

namespace AspNetMvc_EF_GenericPattern_IoC.Controllers
{
    public class JobPostsController : Controller
    {
        private readonly IJobPostService jobPostService = null;
        private readonly IJobTagService jobTagService = null;
        private readonly IEmployerService employerService = null;

        public JobPostsController(IJobPostService jobPostService, IJobTagService jobTagService, IEmployerService employerService)
        {
            this.jobPostService = jobPostService;
            this.jobTagService = jobTagService;
            this.employerService = employerService;
        }

        // GET: JobPosts
        public ActionResult Index()
        {
            var posts = jobPostService.GetAll();

            // var jobPosts = db.JobPosts.Include(j => j.Employer);
            return View(posts/*obPosts.ToList()*/);
        }

        // GET: JobPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobPost jobPost = jobPostService.GetById(id.Value);
            if (jobPost == null)
            {
                return HttpNotFound();
            }
            return View(jobPost);
        }

        // GET: JobPosts/Create
        public ActionResult Create()
        {
            ViewBag.EmployerID = new SelectList(employerService.List(), "Id", "FullName");

            var jobPostViewModel = new JobPostViewModel
            {
                AllJobTags = jobTagService.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Tag,
                    Value = x.Id.ToString()
                })
            };

            return View(jobPostViewModel);
        }

        // POST: JobPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobPostViewModel jobPostViewModel)
        {
            if (jobPostService == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var jobPost = new JobPost { Id = jobPostViewModel.JobPost.Id };

            if (TryUpdateModel(jobPost, "JobPost", new string[] { "Title", "EmployerID" }))
            {
                var allTags = jobTagService.GetAll().ToList();
                jobPost.JobTags = allTags.Where(x => jobPostViewModel.SelectedJobTags.Contains(x.Id)).ToList();
                jobPostService.Insert(jobPost);
                
                return RedirectToAction("Index");
            }

            ViewBag.EmployerID = new SelectList(employerService.List(), "Id", "FullName", jobPost.EmployerID);
            return View(jobPostViewModel);
        }

        public ActionResult CreateFromScratch()
        {
            var jobPostViewModel = new JobPostFromScratchViewModel();

            return View(jobPostViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateFromScratch(JobPostFromScratchViewModel model)
        {
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var jobPost = new JobPost();
            var jobTag = new List<JobTag>();
            var employer = new Employer();

            if (TryUpdateModel(jobPost, "JobPost", new string[] { "Title" }) 
                || TryUpdateModel(employer, "Employer", new string[] { "FullName" }))
            {
                var allTags = jobTagService.GetAll().ToList();
                //jobPost.JobTags = allTags.Where(x => jobPostViewModel.SelectedJobTags.Contains(x.Id)).ToList();
                jobPostService.Insert(jobPost);

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        // GET: JobPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var jobPostViewModel = new JobPostViewModel
            {
                JobPost = jobPostService.GetById(id.Value)
            };

            if (jobPostViewModel.JobPost == null)
            {
                return HttpNotFound();
            }

            jobPostViewModel.AllJobTags = jobTagService.GetAll().Select(list => new SelectListItem
            {
                Text = list.Tag,
                Value = list.Id.ToString()
            });

            ViewBag.EmployerID = new SelectList(employerService.List(), "Id", "FullName", jobPostViewModel.JobPost.EmployerID);
            return View(jobPostViewModel);
        }

        // POST: JobPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobPostViewModel jobPostViewModel)
        {
            if (jobPostViewModel == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var jobToUpdate = jobPostService.GetById(jobPostViewModel.JobPost.Id);
            if (TryUpdateModel(jobToUpdate, "JobPost", new string[] {"Id", "Title", "EmployerID" }))
            {
                var allTags = jobTagService.GetAll().ToList();
                jobToUpdate.JobTags = allTags.Where(x => jobPostViewModel.SelectedJobTags.Contains(x.Id)).ToList();
                jobPostService.Update(jobToUpdate);

                return RedirectToAction("Index");
            }

            ViewBag.EmployerID = new SelectList(employerService.List(), "Id", "FullName", jobToUpdate.EmployerID);
            jobPostViewModel.JobPost = jobToUpdate;
            return View(jobPostViewModel);
        }

        // GET: JobPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            JobPost jobPost = jobPostService.GetById(id.Value);
            if (jobPost == null)
            {
                return HttpNotFound();
            }

            return View(jobPost);
        }

        // POST: JobPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobPost jobPost = jobPostService.GetById(id);
            jobPostService.Delete(jobPost);

            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
