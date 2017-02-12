using DAL;
using DTO;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Unity
{
    /// <summary>
    /// Bootstrapper
    /// </summary>
    public class ContainerBootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }
        /// <summary>
        /// Registering all the types
        /// </summary>
        /// <returns></returns>
        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // Database Contexts
            container.RegisterType<IDbContext, TeijonDbContext>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDbContext, ManyToManyDBContext>(new ContainerControlledLifetimeManager());

            // Repositories
            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IRepository<Job>, Repository<Job>>();
            container.RegisterType<IRepository<JobPost>, Repository<JobPost>>();
            container.RegisterType<IRepository<JobTag>, Repository<JobTag>>();
            container.RegisterType<IRepository<Employer>, Repository<Employer>>();

            container.RegisterType<IManyToManyRepository<JobPost, JobTag>, ManyToManyRepository<JobPost, JobTag>>();
            container.RegisterType<IManyToManyRepository<JobTag, JobPost>, ManyToManyRepository<JobTag, JobPost>>();

            // Services
            container.RegisterType<IPersonService, PersonService>();
            container.RegisterType<IJobService, JobService>();
            container.RegisterType<IJobPostService, JobPostService>();
            container.RegisterType<IJobTagService, JobTagService>();
            container.RegisterType<IEmployerService, EmployerService>();


            MvcUnityContainer.Container = container;
            return container;
        }
    }
}