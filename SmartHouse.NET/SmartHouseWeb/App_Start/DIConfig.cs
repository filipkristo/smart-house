using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using SmartHouseWebLib.DomainService;
using SmartHouseWebLib.DomainService.Interface;
using SmartHouseWebLib.Models;
using SmartHouseWebLib.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace SmartHouseWeb.App_Start
{
    public static class DIConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterType<SmartHouseContext>().As<IDbContext>().InstancePerRequest();

            builder.RegisterType<UserLocationService>().As<IUserLocationService>().InstancePerRequest();
            builder.RegisterType<RoomService>().As<IRoomService>().InstancePerRequest();
            builder.RegisterType<TelemetryDataService>().As<ITelemetryDataService>().InstancePerRequest();

            builder.RegisterFilterProvider();
            IContainer container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}