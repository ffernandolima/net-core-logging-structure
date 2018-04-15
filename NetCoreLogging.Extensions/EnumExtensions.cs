using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NetCoreLogging.Extensions
{
	public static class EnumExtensions
	{
		public static TEnum GetValueFromDescription<TEnum>(this string description) where TEnum : struct
		{
			var defaultValue = default(TEnum);

			if (string.IsNullOrWhiteSpace(description))
			{
				return defaultValue;
			}

			var type = typeof(TEnum);

			if (type.IsEnum)
			{
				var fields = type.GetFields();

				foreach (var field in fields)
				{
					var attribute = (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute);

					if (attribute != null)
					{
						if (attribute.Description.Equals(description, StringComparison.OrdinalIgnoreCase))
						{
							var value = (TEnum)field.GetValue(null);

							return value;
						}
					}
					else
					{
						if (field.Name.Equals(description, StringComparison.OrdinalIgnoreCase))
						{
							var value = (TEnum)field.GetValue(null);

							return value;
						}
					}
				}
			}

			return defaultValue;
		}

		public static string GetDescriptionFromValue<TEnum>(this TEnum tenum) where TEnum : struct
		{
			var type = typeof(TEnum);

			if (type.IsEnum)
			{
				var name = tenum.ToString();

				var memberInfos = type.GetMember(name);

				if (memberInfos != null && memberInfos.Length > 0)
				{
					var memberInfo = memberInfos[0];

					var attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

					if (attributes != null && attributes.Length > 0)
					{
						var attribute = (DescriptionAttribute)attributes[0];

						var description = attribute.Description;

						return description;
					}
				}
			}

			var defaultDescription = tenum.ToString();

			return defaultDescription;
		}

		public static IEnumerable<string> GetAllDescriptions<TEnum>() where TEnum : struct
		{
			var descriptions = new List<string>();

			var type = typeof(TEnum);

			if (type.IsEnum)
			{
				var values = Enum.GetValues(typeof(TEnum));

				foreach (var value in values)
				{
					var name = value.ToString();

					var memberInfos = type.GetMember(name);

					if (memberInfos != null && memberInfos.Length > 0)
					{
						var memberInfo = memberInfos[0];

						var attributes = memberInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

						if (attributes != null && attributes.Length > 0)
						{
							var attribute = (DescriptionAttribute)attributes[0];

							var description = attribute.Description;

							descriptions.Add(description);
						}
					}
					else
					{
						var description = value.ToString();

						descriptions.Add(description);
					}
				}
			}

			return descriptions;
		}
	}
}
