using Microsoft.Extensions.Logging;
using NetCoreLogging.Extensions;
using NetCoreLogging.Loggers;
using System;

namespace NetCoreLogging.Services
{
	public class LogService : ILogService
	{
		private readonly ILoggerFactory _loggerFactory;

		public LogService(ILoggerFactory loggerFactory)
		{
			this._loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory), $"{nameof(loggerFactory)} cannot be null.");
		}

		public ILogger DbLogger => this._loggerFactory.CreateLogger(LoggerType.DbLogger.GetDescriptionFromValue());
		public ILogger FileLogger => this._loggerFactory.CreateLogger(LoggerType.FileLogger.GetDescriptionFromValue());
	}
}
