
// ISharpSerializer.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.IO;

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface ISharpSerializer
	{
		/// <summary></summary>
		/// <param name="data"></param>
		/// <param name="fileName"></param>
		/// <param name="binaryMode"></param>
		void Serialize<T>(T data, string fileName, bool binaryMode = false) where T : class;

		/// <summary></summary>
		/// <param name="data"></param>
		/// <param name="stream"></param>
		/// <param name="binaryMode"></param>
		void Serialize<T>(T data, Stream stream, bool binaryMode = false) where T : class;

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="binaryMode"></param>
		/// <returns></returns>
		T Deserialize<T>(string fileName, bool binaryMode = false) where T : class;

		/// <summary></summary>
		/// <param name="stream"></param>
		/// <param name="binaryMode"></param>
		/// <returns></returns>
		T Deserialize<T>(Stream stream, bool binaryMode = false) where T : class;
	}
}
