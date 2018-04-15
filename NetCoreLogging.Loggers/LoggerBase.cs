using Microsoft.Extensions.Logging;
using System;

namespace NetCoreLogging.Loggers
{
	public class LoggerBase : ILogger
	{
		protected string Name { get; private set; }

		public LoggerBase()
		{
			this.Name = this.LoggerName();
		}

		public virtual IDisposable BeginScope<TState>(TState state) => null;

		public virtual bool IsEnabled(LogLevel logLevel) => false;

		public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }

		private string LoggerName()
		{
			var loggerType = this.GetType();
			var loggerName = loggerType.Name;

			return loggerName;
		}
	}
}
