
// Path.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.Portability;

namespace EamonPM.Game.Portability
{
	public class Path : IPath
	{
		public virtual char DirectorySeparatorChar
		{
			get
			{
				return System.IO.Path.DirectorySeparatorChar;
			}
		}

		public virtual bool EqualPaths(string path1, string path2)
		{
			return !string.IsNullOrWhiteSpace(path1) && !string.IsNullOrWhiteSpace(path2) ? NormalizePath(path1).Equals(NormalizePath(path2), System.IO.Path.DirectorySeparatorChar == '\\' ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal) : false;
		}

		public virtual string NormalizePath(string path)
		{
			return !string.IsNullOrWhiteSpace(path) ? path.Replace(System.IO.Path.DirectorySeparatorChar == '\\' ? '/' : '\\', System.IO.Path.DirectorySeparatorChar) : path;
		}

		public virtual string Combine(params string[] paths)
		{
			string[] normalizedPaths = null;

			if (paths != null)
			{
				normalizedPaths = new string[paths.Length];

				for (var i = 0; i < paths.Length; i++)
				{
					normalizedPaths[i] = NormalizePath(paths[i]);
				}
			}

			return System.IO.Path.Combine(normalizedPaths);
		}

		public virtual string GetDirectoryName(string path)
		{
			return System.IO.Path.GetDirectoryName(NormalizePath(path));
		}

		public virtual string GetExtension(string path)
		{
			return System.IO.Path.GetExtension(NormalizePath(path));
		}

		public virtual string GetFileName(string path)
		{
			return System.IO.Path.GetFileName(NormalizePath(path));
		}

		public virtual string GetFileNameWithoutExtension(string path)
		{
			return System.IO.Path.GetFileNameWithoutExtension(NormalizePath(path));
		}

		public virtual string GetFullPath(string path)
		{
			return System.IO.Path.GetFullPath(NormalizePath(path));
		}

		public virtual string ChangeExtension(string path, string extension)
		{
			return System.IO.Path.ChangeExtension(NormalizePath(path), extension);
		}
	}
}
