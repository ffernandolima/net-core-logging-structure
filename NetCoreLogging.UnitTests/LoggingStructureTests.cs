using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetCoreLogging.Extensions;
using NetCoreLogging.Providers;
using NetCoreLogging.Services;
using NetCoreLogging.Settings;
using System;

namespace NetCoreLogging.UnitTests
{
	[TestClass]
	public class LoggingStructureTests
	{
		[TestMethod]
		public void GeneralTestMethod()
		{
			var configurationBuilder = new ConfigurationBuilder();
			var configuration = configurationBuilder.AddJsonFile("appsettings.json").Build();

			var loggerFactory = (ILoggerFactory)new LoggerFactory();

			var settings = new LoggingSettings
			{
				FilePath = configuration.GetConfiguration<string>("LoggingSettings:FilePath"),
				MaximumFileSize = configuration.GetConfiguration<long>("LoggingSettings:MaximumFileSize"),
				ConnectionString = configuration.GetConnectionString("Context")
			};

			loggerFactory.AddLoggerProvider(settings);

			var logService = new LogService(loggerFactory);

			var ex = new Exception("Testing exception");

			logService.DbLogger.LogError(ex.ToString());
			logService.FileLogger.LogError(ex.ToString());

		}
	}
}
