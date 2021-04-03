using Autofac;
using System;
using System.Linq;

namespace RestaurantApp.BusinessEntities.Infrastructure
{
    public class BusinessEntitiesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            string[] namespaces = new[]
            {
                "RestaurantApp.BusinessEntities.Implementation",
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
