
// IPath.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface IPath
	{
		/// <summary></summary>
		char DirectorySeparatorChar { get; }

		/// <summary></summary>
		/// <param name="path1"></param>
		/// <param name="path2"></param>
		/// <returns></returns>
		bool EqualPaths(string path1, string path2);

		/// <summary></summary>
		/// <param name="paths"></param>
		/// <returns></returns>
		string Combine(params string[] paths);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string GetDirectoryName(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string GetExtension(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string GetFileName(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string GetFileNameWithoutExtension(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string GetFullPath(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="extension"></param>
		/// <returns></returns>
		string ChangeExtension(string path, string extension);
	}
}
