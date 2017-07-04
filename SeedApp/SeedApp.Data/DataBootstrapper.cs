using Microsoft.Practices.Unity;
using SeedApp.Common.DB;
using SeedApp.Common.Interfaces;
using SeedApp.Common.Logging;
using SeedApp.Common.Providers;
using SeedApp.Data.Database;
using SeedApp.Data.Interfaces;
using SeedApp.Data.Repositories;
using SeedApp.Data.Repositories.Interfaces;

namespace SeedApp.Data
{
	public class DataBootstrapper
	{		
		public static void Initialize(IUnityContainer container)
		{
			// Interface and Class Registration goes here.
			container.RegisterType<ISqLiteConnectionFactory, SqLiteConnectionFactory>(new ContainerControlledLifetimeManager());
			container.RegisterType<ILogsInternalDatabase, LogsInternalDatabase>(new HierarchicalLifetimeManager());
			container.RegisterType<IAppInternalDatabase, ServiceClientInternalDatabase>(new HierarchicalLifetimeManager());
			container.RegisterType<IServiceClientInternalDatabase, ServiceClientInternalDatabase>(new HierarchicalLifetimeManager());
			container.RegisterType<IServiceClientDatabase, ServiceClientDatabase>(new HierarchicalLifetimeManager());
			container.RegisterType<ILoggingProvider, LogglyLocalDatabaseLoggingProvider>("LogglyLogger",new ContainerControlledLifetimeManager());
			container.RegisterType<ILogger, Logger>();
			container.RegisterType<IConnectivityHelper, ConnectivityHelper>(new ContainerControlledLifetimeManager());
			container.RegisterType<ILogsDatabase, LogsDatabase>(new HierarchicalLifetimeManager());

			// Repository Registration goes here.
			container.RegisterType<ITestRepository, TestRepository>(new TransientLifetimeManager());
		}
	}
}
