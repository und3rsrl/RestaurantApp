using Autofac;
using RestaurantApp.BusinessEntities.Infrastructure;
using RestaurantApp.DataServices.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.BusinessService.Infrastructure
{
    public class BusinessServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DataServicesModule());
            builder.RegisterModule(new BusinessEntitiesModule());
            string[] namespaces = new[]
            {
                "RestaurantApp.BusinessService.Implementation",
            };
            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => namespaces.Contains(x.Namespace))
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency();
        }
    }
}
