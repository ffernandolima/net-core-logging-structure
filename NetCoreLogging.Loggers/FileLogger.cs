using Microsoft.Extensions.Logging;
using NetCoreLogging.Extensions;
using System;
using System.IO;
using System.Text;

namespace NetCoreLogging.Loggers
{
	public class FileLogger : LoggerBase
	{
		private readonly FileInfo _logFile;

		public FileLogger(string filePath, long maximumFileSize)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException($"{nameof(filePath)} cannot be null or white-space", nameof(filePath));
			}

			if (maximumFileSize <= 0)
			{
				throw new ArgumentException($"{nameof(maximumFileSize)} cannot be less than or equal to zero", nameof(maximumFileSize));
			}

			this._logFile = this.CreateLogFile(filePath, maximumFileSize);
		}

		public override bool IsEnabled(LogLevel logLevel) => true;

		public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var isEnabled = this.IsEnabled(logLevel);

			if (!isEnabled)
			{
				return;
			}

			var message = this.PrepareMessage(logLevel, state, exception, formatter);

			this.LogInternal(message);
		}

		private FileInfo CreateLogFile(string filePath, long maximumFileSize)
		{
			var logFile = new FileInfo(filePath);

			if (!logFile.Directory.Exists)
			{
				logFile.Directory.Create();
			}

			if (!logFile.Exists)
			{
				logFile.Create();
			}
			else if (logFile.Exists && logFile.Length >= maximumFileSize)
			{
				this.CreateBackupFile(logFile);

				logFile.ReCreate();
			}

			return logFile;
		}

		private void CreateBackupFile(FileInfo logFile)
		{
			var logfileName = Path.GetFileName(logFile.Name);
			var logfileNameWithoutExtension = Path.GetFileNameWithoutExtension(logfileName);

			var backupFileName = $"{logfileNameWithoutExtension}.bkp";
			var backupFilePath = logFile.FullName.Replace(logfileName, backupFileName);

			var backupFile = new FileInfo(backupFilePath);

			if (backupFile.Exists)
			{
				backupFile.ReCreate();
			}

			File.Copy(logFile.FullName, backupFile.FullName);
		}

		private string PrepareMessage<TState>(LogLevel logLevel, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var messageBuilder = new StringBuilder();

			messageBuilder.Append($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz")} ");
			messageBuilder.Append($"[{logLevel.ToString()}] ");
			messageBuilder.Append($"{this.Name} : ");

			if (formatter != null)
			{
				var formatted = formatter(state, exception);

				messageBuilder.Append(formatted);
			}
			else
			{
				messageBuilder.Append(state);
			}

			messageBuilder.Append(Environment.NewLine);

			var message = messageBuilder.ToString();

			return message;
		}

		private void LogInternal(string message)
		{
			using (var streamWriter = this._logFile.AppendText())
			{
				streamWriter.Write(message);
			}
		}
	}
}
