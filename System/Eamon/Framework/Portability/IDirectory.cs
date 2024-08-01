
// IDirectory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface IDirectory
	{
		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool Exists(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="adventureName"></param>
		/// <returns></returns>
		bool IsEamonCSAdventuresDirectory(string path, string adventureName);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="recursive"></param>
		void Delete(string path, bool recursive);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="recursive"></param>
		void DeleteEmptySubdirectories(string path, bool recursive);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="adventureName"></param>
		/// <param name="recursive"></param>
		void DeleteEmptySubdirectories01(string path, string adventureName, bool recursive);

		/// <summary></summary>
		/// <param name="path"></param>
		void CreateDirectory(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		void SetCurrentDirectory(string path);

		/// <summary></summary>
		/// <returns></returns>
		string GetCurrentDirectory();

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string[] GetFiles(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		string[] GetDirectories(string path);
	}
}
