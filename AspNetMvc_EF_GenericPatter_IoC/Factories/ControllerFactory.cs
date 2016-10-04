using Microsoft.Practices.Unity;
using MVC.Unity;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC.Factories
{
    /// <summary>
    /// Controller Factory
    /// </summary>
    public class ControllerFactory : DefaultControllerFactory
    {
        /// <summary>
        /// Injecting types into the controller
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            try
            {
                if (controllerType == null)
                    throw new ArgumentNullException("controllerType");

                if (!typeof(IController).IsAssignableFrom(controllerType))
                    throw new ArgumentException(string.Format(
                        "Type requested is not a controller: {0}",
                        controllerType.Name),
                        "controllerType");

                return MvcUnityContainer.Container.Resolve(controllerType) as IController;
            }
            catch
            {
                return null;
            }

        }
    }
}