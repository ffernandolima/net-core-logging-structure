using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel;

namespace NetCoreLogging.Extensions
{
	public static class ConfigurationExtensions
	{
		public static T GetConfiguration<T>(this IConfiguration configuration, string key, bool? throwExceptionWhenNotFound = false)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException(nameof(configuration), $"{nameof(configuration)} cannot be null.");
			}

			var configValue = configuration[key];

			if (configValue == null && throwExceptionWhenNotFound.GetValueOrDefault())
			{
				throw new ArgumentException("Configuration was not found.", nameof(configValue));
			}

			var value = ConvertFrom<T>(configValue);

			return value;
		}

		private static T ConvertFrom<T>(string value)
		{
			if (typeof(T) == typeof(string))
			{
				return (T)(object)value;
			}

			if (!string.IsNullOrWhiteSpace(value))
			{
				var converter = TypeDescriptor.GetConverter(typeof(T));

				if (converter.CanConvertFrom(typeof(string)))
				{
					return (T)converter.ConvertFrom(value);
				}
			}

			return default(T);
		}
	}
}
