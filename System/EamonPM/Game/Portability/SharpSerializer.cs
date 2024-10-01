
// SharpSerializer.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.IO;
using System.IO.Compression;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Portability
{
	public class SharpSerializer : ISharpSerializer
	{
		public virtual void Serialize<T>(T data, string fileName, bool binaryMode = false) where T : class
		{
			using (var fileStream = new FileStream(gEngine.Path.NormalizePath(fileName), FileMode.Create))
			{
				using (var gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
				{
					Serialize(data, gzipStream, binaryMode);
				}
			}
		}

		public virtual void Serialize<T>(T data, Stream stream, bool binaryMode = false) where T : class
		{
			var sharpSerializer = new Polenter.Serialization.SharpSerializer(binaryMode);

			try
			{
				gEngine.MutatePropertyCounter--;

				sharpSerializer.Serialize(data, stream);
			}
			finally
			{
				gEngine.MutatePropertyCounter++;
			}
		}

		public virtual T Deserialize<T>(string fileName, bool binaryMode = false) where T : class
		{
			using (var fileStream = new FileStream(gEngine.Path.NormalizePath(fileName), FileMode.Open))
			{
				using (var gzipStream = new GZipStream(fileStream, CompressionMode.Decompress))
				{
					return Deserialize<T>(gzipStream, binaryMode);
				}
			}
		}

		public virtual T Deserialize<T>(Stream stream, bool binaryMode = false) where T : class
		{
			var sharpSerializer = new Polenter.Serialization.SharpSerializer(binaryMode);

			T result = default(T);

			try
			{
				gEngine.MutatePropertyCounter--;

				result = sharpSerializer.Deserialize(stream) as T;
			}
			finally
			{
				gEngine.MutatePropertyCounter++;
			}

			return result;
		}
	}
}
