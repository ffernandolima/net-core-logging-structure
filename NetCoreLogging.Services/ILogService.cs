using Microsoft.Extensions.Logging;

namespace NetCoreLogging.Services
{
	public interface ILogService
	{
		ILogger DbLogger { get; }
		ILogger FileLogger { get; }
	}
}
