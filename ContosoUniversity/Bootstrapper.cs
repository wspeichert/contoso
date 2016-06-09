using System.Web.Mvc;
using DataLayer;
using Microsoft.Practices.Unity;
using Unity.Mvc3;

namespace ContosoUniversity
{
    public static class Bootstrapper
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();            
            container.RegisterType<IDataContext, SchoolContext>();
            return container;
        }
    }
}