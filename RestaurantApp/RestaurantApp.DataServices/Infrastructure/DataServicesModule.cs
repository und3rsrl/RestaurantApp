using Autofac;
using System;
using System.Linq;

namespace RestaurantApp.DataServices.Infrastructure
{
    public class DataServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            string[] namespaces = new[]
            {
                "RestaurantApp.DataServices.Implementation"
            };

            builder
                .RegisterAssemblyTypes(ThisAssembly)
                .Where(x => namespaces.Contains(x.Namespace))
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency();
            builder.RegisterType<Entities>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
