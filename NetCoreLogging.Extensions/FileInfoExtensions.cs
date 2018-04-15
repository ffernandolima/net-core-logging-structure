using System;
using System.IO;

namespace NetCoreLogging.Extensions
{
	public static class FileInfoExtensions
	{
		public static FileStream ReCreate(this FileInfo fileInfo)
		{
			if (fileInfo == null)
			{
				throw new ArgumentNullException(nameof(fileInfo), $"{nameof(fileInfo)} cannot be null.");
			}

			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}

			var fileStream = fileInfo.Create();

			fileInfo.Refresh();

			return fileStream;
		}
	}
}
