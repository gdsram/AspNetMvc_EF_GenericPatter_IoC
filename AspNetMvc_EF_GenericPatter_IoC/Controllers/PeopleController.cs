using DTO;
using Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Services.Interfaces;

namespace MVC.Controllers
{
    /// <summary>
    /// People Controller
    /// </summary>
    public class PeopleController : Controller
    {
        /// <summary>
        /// Generic Unit Of Work class that will be injected
        /// </summary>
        private readonly IPersonService personService = null;
        private readonly IJobService jobService = null;
        private readonly IDbContext dbContext = null;

        public PeopleController (IPersonService personService, IJobService jobService)
        {
            this.personService = personService;
            this.jobService = jobService;
        }

        // GET: People
        public ActionResult Index()
        {
            var people = personService.GetPeople().ToList();
            return View(people);
        }

        // GET: People/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person person = personService.GetPerson(id ?? 0);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: People/Create
        public ActionResult Create()
        {
            ViewBag.id_job = new SelectList(jobService.GetJobs().ToList(), "id", "name");
            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,sex,developer,description,id_job")] Person person)
        {
            if (ModelState.IsValid)
            {
                personService.InsertPerson(person);
                
                return RedirectToAction("Index");
            }

            ViewBag.id_job = new SelectList(jobService.GetJobs().ToList(), "id", "name", person.id_job);
            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Person person = personService.GetPerson(id ?? 0);
            if (person == null)
            {
                return HttpNotFound();
            }

            ViewBag.id_job = new SelectList(jobService.GetJobs().ToList(), "id", "name", person.id_job);
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,sex,developer,description,id_job")] Person person)
        {
            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    personService.UpdatePerson(person);

                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            ViewBag.id_job = new SelectList(jobService.GetJobs().ToList(), "id", "name", person.id_job);
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = personService.GetPerson(id ?? 0);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = personService.GetPerson(id);
            personService.DeletePerson(person);

            return RedirectToAction("Index");
        }
    }
}
