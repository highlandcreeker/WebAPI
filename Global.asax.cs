using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using Ninject;

using FBPortal.API.Infrastructure;
using FBPortal.Domain.Concrete;
using FBPortal.Domain.Abstract;


namespace FBPortal.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            IKernel kernel = new StandardKernel();
            RegisterServices(kernel);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
        }

        private static void RegisterServices(IKernel kernel) {
            kernel.Bind<IInvoiceRepository>().To<InvoiceRepository>();
        }
    }
}
