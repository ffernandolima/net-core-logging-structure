using Microsoft.Extensions.Logging;
using NetCoreLogging.Settings;

namespace NetCoreLogging.Providers
{
	public static class LoggerFactoryExtensions
	{
		public static ILoggerFactory AddLoggerProvider(this ILoggerFactory loggerFactory, LoggingSettings settings)
		{
			var provider = new LoggerProvider(settings);

			loggerFactory.AddProvider(provider);

			return loggerFactory;
		}
	}
}
