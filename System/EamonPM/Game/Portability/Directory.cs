
// Directory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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

		public virtual bool IsEamonCSAdventuresDirectory(string path, string adventureName)
		{
			var result = false;

			if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(adventureName))
			{
				var fullPath = gEngine.Path.GetFullPath(NormalizePath(path));

				var separator = Regex.Escape(gEngine.Path.DirectorySeparatorChar.ToString());

				var pattern = string.Format(@"(?-i).*{0}Eamon-CS[^{0}]*{0}Adventures{0}{1}({0}.*)?$", separator, adventureName);

				var isValidDirectory = System.IO.Directory.Exists(fullPath);

				if (isValidDirectory)
				{
					try
					{
						System.IO.Directory.EnumerateFileSystemEntries(fullPath).Any();
					}
					catch (IOException)
					{
						isValidDirectory = false;
					}
					catch (UnauthorizedAccessException)
					{
						isValidDirectory = false;
					}
				}

				result = isValidDirectory && Regex.IsMatch(fullPath, pattern);
			}

			return result;
		}

		public virtual void Delete(string path, bool recursive)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				var adventureName = gEngine.Path.GetFileName(path);

				var fullPath = gEngine.Path.GetFullPath(NormalizePath(path));

				if (IsEamonCSAdventuresDirectory(fullPath, adventureName))
				{
					System.IO.Directory.Delete(fullPath, recursive);
				}
			}
		}

		public virtual void DeleteEmptySubdirectories(string path, bool recursive)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				var adventureName = gEngine.Path.GetFileName(path);

				DeleteEmptySubdirectories01(path, adventureName, recursive);
			}
		}

		public virtual void DeleteEmptySubdirectories01(string path, string adventureName, bool recursive)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				foreach (var directory in System.IO.Directory.GetDirectories(NormalizePath(path)))
				{
					if (recursive)
					{
						DeleteEmptySubdirectories01(directory, adventureName, recursive);
					}

					if (!System.IO.Directory.EnumerateFileSystemEntries(directory).Any())
					{
						var fullPath = gEngine.Path.GetFullPath(directory);

						if (IsEamonCSAdventuresDirectory(fullPath, adventureName))
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
