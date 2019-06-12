using Autofac;

namespace SeedApp.Common
{
    public class ContainerManager
    {
        private static IContainer _container;

        public static ContainerBuilder DependencyRegistrar { get; set; }

        public static IContainer Container => _container ?? (_container = DependencyRegistrar.Build());

        public static void InitializeDependencyRegistrar()
        {
            DependencyRegistrar = new ContainerBuilder();

            CommonBootstrapper.Initialize(DependencyRegistrar);
        }
    }
}