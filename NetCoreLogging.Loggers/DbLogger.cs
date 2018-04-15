using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace NetCoreLogging.Loggers
{
	public class DbLogger : LoggerBase
	{
		private readonly string _connectionString;

		public DbLogger(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
			{
				throw new ArgumentException($"{nameof(connectionString)} cannot be null or white-space", nameof(connectionString));
			}

			this._connectionString = connectionString;
		}

		public override bool IsEnabled(LogLevel logLevel) => true;

		public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var isEnabled = this.IsEnabled(logLevel);

			if (!isEnabled)
			{
				return;
			}

			var message = this.PrepareMessage(state, exception, formatter);

			this.LogInternal(logLevel, message);
		}

		private string PrepareMessage<TState>(TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var messageBuilder = new StringBuilder();

			if (formatter != null)
			{
				var formatted = formatter(state, exception);

				messageBuilder.Append(formatted);
			}
			else
			{
				messageBuilder.Append(state);
			}

			var message = messageBuilder.ToString();

			return message;
		}

		private void LogInternal(LogLevel logLevel, string message)
		{
			using (var connection = new SqlConnection(this._connectionString))
			using (var command = connection.CreateCommand())
			{
				if (connection.State != ConnectionState.Open)
				{
					connection.Open();
				}

				command.CommandText = "INSERT INTO [Log] ([Date],[Level],[Logger],[Message]) VALUES (@date, @level, @logger, @message)";
				command.CommandType = CommandType.Text;

				command.Parameters.AddWithValue("@date", DateTime.Now);
				command.Parameters.AddWithValue("@level", logLevel.ToString());
				command.Parameters.AddWithValue("@logger", this.Name);
				command.Parameters.AddWithValue("@message", message);

				command.ExecuteNonQuery();

				if (connection.State != ConnectionState.Closed)
				{
					connection.Close();
				}
			}
		}
	}
}
