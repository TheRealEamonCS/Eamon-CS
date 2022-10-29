
// Directory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Portability
{
	public class Directory : IDirectory
	{
		public virtual bool Exists(string path)
		{
			return System.IO.Directory.Exists(NormalizePath(path));
		}

		public virtual bool IsEamonCSDirectory(string path, ref string parentName)
		{
			var result = false;

			if (!string.IsNullOrWhiteSpace(path) && parentName != null)
			{
				parentName = "";

				var directoryInfo = new System.IO.DirectoryInfo(NormalizePath(path));

				while (directoryInfo != null && directoryInfo.Parent != null && directoryInfo.Root != null && directoryInfo.Parent.Name != directoryInfo.Root.Name)
				{
					if (directoryInfo.Parent.Name.Equals("Adventures") ||
							directoryInfo.Parent.Name.Equals("System"))
					{
						parentName = directoryInfo.Parent.Name;
					}

					if (parentName.Length > 0 &&
							directoryInfo.Name.Equals(parentName) &&
							directoryInfo.Parent.FullName.Contains("EamonPM.Android") &&
							System.IO.Directory.Exists(gEngine.Path.Combine(directoryInfo.Parent.FullName, "Adventures")) &&
							System.IO.Directory.Exists(gEngine.Path.Combine(directoryInfo.Parent.FullName, "System")))
					{
						result = true;

						break;
					}

					directoryInfo = directoryInfo.Parent;
				}
			}

			return result;
		}

		public virtual void Delete(string path, bool recursive)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				var fullPath = gEngine.Path.GetFullPath(NormalizePath(path));

				var parentName = "";

				if (IsEamonCSDirectory(path, ref parentName) && parentName != null && parentName.Equals("Adventures"))
				{
					System.IO.Directory.Delete(fullPath, recursive);
				}
			}
		}

		public virtual void DeleteEmptySubdirectories(string path, bool recursive)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				foreach (var directory in System.IO.Directory.GetDirectories(NormalizePath(path)))
				{
					if (recursive)
					{
						DeleteEmptySubdirectories(directory, recursive);
					}

					if (!System.IO.Directory.EnumerateFileSystemEntries(directory).Any())
					{
						var fullPath = gEngine.Path.GetFullPath(directory);

						var parentName = "";

						if (IsEamonCSDirectory(directory, ref parentName) && parentName != null && parentName.Equals("Adventures"))
						{
							System.IO.Directory.Delete(fullPath, false);
						}
					}
				}
			}
		}

		public virtual void CreateDirectory(string path)
		{
			System.IO.Directory.CreateDirectory(NormalizePath(path));
		}

		public virtual void SetCurrentDirectory(string path)
		{
			System.IO.Directory.SetCurrentDirectory(NormalizePath(path));
		}

		public virtual string GetCurrentDirectory()
		{
			return System.IO.Directory.GetCurrentDirectory();
		}

		public virtual string[] GetFiles(string path)
		{
			return System.IO.Directory.GetFiles(NormalizePath(path));
		}

		public virtual string[] GetDirectories(string path)
		{
			return System.IO.Directory.GetDirectories(NormalizePath(path));
		}

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public virtual string NormalizePath(string path)
		{
			return path != null ? path.Replace(gEngine.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', gEngine.Path.DirectorySeparatorChar) : null;
		}
	}
}
