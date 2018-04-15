using Microsoft.Extensions.Logging;
using NetCoreLogging.Extensions;
using NetCoreLogging.Loggers;
using NetCoreLogging.Settings;
using System;

namespace NetCoreLogging.Providers
{
	public class LoggerProvider : ILoggerProvider
	{
		private static readonly LoggerBase _loggerBase = new LoggerBase();

		private readonly LoggingSettings _settings;

		public LoggerProvider(LoggingSettings settings)
		{
			this._settings = settings ?? throw new ArgumentNullException(nameof(settings), $"{nameof(settings)} cannot be null.");
		}

		public ILogger CreateLogger(string categoryName)
		{
			ILogger logger = this.CreateLoggerInternal(categoryName);

			return logger;
		}

		private ILogger CreateLoggerInternal(string categoryName)
		{
			ILogger logger = null;

			var loggerType = categoryName.GetValueFromDescription<LoggerType>();

			switch (loggerType)
			{
				case LoggerType.DbLogger:
					{
						logger = new DbLogger(this._settings.ConnectionString);
					}
					break;
				case LoggerType.FileLogger:
					{
						logger = new FileLogger(this._settings.FilePath, this._settings.MaximumFileSize);
					}
					break;
				case LoggerType.Unknown:
				default:
					{
						logger = _loggerBase;
					}
					break;
			}

			return logger;
		}

		#region IDisposable Members

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
				}
			}

			this._disposed = true;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion IDisposable Members
	}
}
