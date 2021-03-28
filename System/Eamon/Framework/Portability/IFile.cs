
// IFile.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface IFile
	{
		/// <summary></summary>
		/// <param name="path"></param>
		/// <returns></returns>
		bool Exists(string path);

		/// <summary></summary>
		/// <param name="path"></param>
		void Delete(string path);

		/// <summary></summary>
		/// <param name="sourceFileName"></param>
		/// <param name="destFileName"></param>
		/// <param name="overwrite"></param>
		void Copy(string sourceFileName, string destFileName, bool overwrite);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		string ReadFirstLine(string path, Encoding encoding = null);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		string ReadAllText(string path, Encoding encoding = null);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="contents"></param>
		/// <param name="encoding"></param>
		void WriteAllText(string path, string contents, Encoding encoding = null);

		/// <summary></summary>
		/// <param name="path"></param>
		/// <param name="contents"></param>
		/// <param name="encoding"></param>
		void AppendAllText(string path, string contents, Encoding encoding = null);
	}
}
