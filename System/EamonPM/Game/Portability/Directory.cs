
// Directory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
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
			return System.IO.Directory.Exists(gEngine.Path.NormalizePath(path));
		}

		public virtual bool IsEamonCSAdventuresDirectory(string path, string adventureName)
		{
			var result = false;

			if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(adventureName))
			{
				var fullPath = gEngine.Path.GetFullPath(path);

				var separator = Regex.Escape(gEngine.Path.DirectorySeparatorChar.ToString());

				var pattern = string.Empty;

				if (App.ProgramName == "EamonPM.Desktop")
				{
					pattern = string.Format(@"(?-i).*{0}Eamon-CS[^{0}]*{0}Adventures{0}{1}({0}.*)?$", separator, adventureName);
				}
				else if (App.ProgramName == "EamonPM.Android")
				{
					pattern = string.Format(@"(?-i).*{0}EamonPM.Android[^{0}]*{0}files{0}Adventures{0}{1}({0}.*)?$", separator, adventureName);
				}
				else
				{
					Debug.Assert(1 == 0);
				}

				result = System.IO.Directory.Exists(fullPath);

				if (result)
				{
					try
					{
						System.IO.Directory.EnumerateFileSystemEntries(fullPath).Any();
					}
					catch (IOException)
					{
						result = false;
					}
					catch (UnauthorizedAccessException)
					{
						result = false;
					}
				}

				if (result)
				{
					result = Regex.IsMatch(fullPath, pattern);
				}
			}

			return result;
		}

		public virtual void Delete(string path, bool recursive)
		{
			if (!string.IsNullOrWhiteSpace(path))
			{
				var adventureName = gEngine.Path.GetFileName(path);

				var fullPath = gEngine.Path.GetFullPath(path);

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
			if (!string.IsNullOrWhiteSpace(path) && !string.IsNullOrWhiteSpace(adventureName))
			{
				foreach (var directory in System.IO.Directory.GetDirectories(gEngine.Path.NormalizePath(path)))
				{
					if (recursive)
					{
						DeleteEmptySubdirectories01(directory, adventureName, recursive);
					}

					if (!System.IO.Directory.EnumerateFileSystemEntries(gEngine.Path.NormalizePath(directory)).Any())
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
			System.IO.Directory.CreateDirectory(gEngine.Path.NormalizePath(path));
		}

		public virtual void SetCurrentDirectory(string path)
		{
			System.IO.Directory.SetCurrentDirectory(gEngine.Path.NormalizePath(path));
		}

		public virtual string GetCurrentDirectory()
		{
			return System.IO.Directory.GetCurrentDirectory();
		}

		public virtual string[] GetFiles(string path)
		{
			return System.IO.Directory.GetFiles(gEngine.Path.NormalizePath(path));
		}

		public virtual string[] GetDirectories(string path)
		{
			return System.IO.Directory.GetDirectories(gEngine.Path.NormalizePath(path));
		}
	}
}
