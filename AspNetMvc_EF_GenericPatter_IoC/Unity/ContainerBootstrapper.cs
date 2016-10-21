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
            container.RegisterType<IDbContext, TeijonDbContext>(new ContainerControlledLifetimeManager());
            container.RegisterType<IRepository<Person>, Repository<Person>>();
            container.RegisterType<IRepository<Job>, Repository<Job>>();
            container.RegisterType<IPersonService, PersonService>();
            container.RegisterType<IJobService, JobService>();

            MvcUnityContainer.Container = container;
            return container;
        }
    }
}