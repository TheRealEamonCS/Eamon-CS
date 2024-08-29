
// File.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Portability
{
	public class File : IFile
	{
		public virtual bool Exists(string path)
		{
			var normalizedPath = gEngine.Path.NormalizePath(path);

			if (gEngine.Path.GetExtension(normalizedPath?.ToUpper()) == ".DAT")
			{
				gEngine.ConvertDatafileFromXmlToDat(normalizedPath);
			}

			return System.IO.File.Exists(normalizedPath);
		}

		public virtual void Delete(string path)
		{
			var normalizedPath = gEngine.Path.NormalizePath(path);

			if (gEngine.Path.GetExtension(normalizedPath?.ToUpper()) == ".DAT")
			{
				gEngine.ConvertDatafileFromXmlToDat(normalizedPath);
			}

			System.IO.File.Delete(normalizedPath);
		}

		public virtual void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			var normalizedSourceFileName = gEngine.Path.NormalizePath(sourceFileName);

			var normalizedDestFileName = gEngine.Path.NormalizePath(destFileName);

			if (gEngine.Path.GetExtension(normalizedSourceFileName?.ToUpper()) == ".DAT")
			{
				gEngine.ConvertDatafileFromXmlToDat(normalizedSourceFileName);
			}

			System.IO.File.Copy(normalizedSourceFileName, normalizedDestFileName, overwrite);
		}

		public virtual string ReadFirstLine(string path, Encoding encoding = null)
		{
			var normalizedPath = gEngine.Path.NormalizePath(path);

			var firstLine = "";

			if (gEngine.Path.GetExtension(normalizedPath?.ToUpper()) == ".DAT")
			{
				gEngine.ConvertDatafileFromXmlToDat(normalizedPath);

				using (var fileStream = new System.IO.FileStream(normalizedPath, System.IO.FileMode.Open))
				{
					using (var gzipStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Decompress))
					{
						using (var streamReader = new System.IO.StreamReader(gzipStream, encoding ?? new UTF8Encoding(true)))
						{
							firstLine = streamReader.ReadLine();
						}
					}
				}
			}
			else
			{
				using (var streamReader = new System.IO.StreamReader(normalizedPath, encoding ?? new UTF8Encoding(true)))
				{
					firstLine = streamReader.ReadLine();
				}
			}

			return firstLine;
		}

		public virtual string ReadAllText(string path, Encoding encoding = null)
		{
			var normalizedPath = gEngine.Path.NormalizePath(path);

			var contents = "";

			if (gEngine.Path.GetExtension(normalizedPath?.ToUpper()) == ".DAT")
			{
				gEngine.ConvertDatafileFromXmlToDat(normalizedPath);

				using (var fileStream = new System.IO.FileStream(normalizedPath, System.IO.FileMode.Open))
				{
					using (var gzipStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Decompress))
					{
						using (var streamReader = new System.IO.StreamReader(gzipStream, encoding ?? new UTF8Encoding(true)))
						{
							contents = streamReader.ReadToEnd();
						}
					}
				}
			}
			else
			{
				contents = System.IO.File.ReadAllText(normalizedPath, encoding ?? new UTF8Encoding(true));
			}

			return contents;
		}

		public virtual System.IO.FileStream OpenRead(string path)
		{
			var normalizedPath = gEngine.Path.NormalizePath(path);

			return System.IO.File.OpenRead(normalizedPath);
		}

		public virtual void WriteAllText(string path, string contents, Encoding encoding = null)
		{
			var normalizedPath = gEngine.Path.NormalizePath(path);

			if (gEngine.Path.GetExtension(normalizedPath?.ToUpper()) == ".DAT")
			{
				using (var fileStream = new System.IO.FileStream(normalizedPath, System.IO.FileMode.Create))
				{
					using (var gzipStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Compress))
					{
						using (var streamWriter = new System.IO.StreamWriter(gzipStream, encoding ?? new UTF8Encoding(true)))
						{
							streamWriter.Write(contents);
						}
					}
				}
			}
			else
			{
				System.IO.File.WriteAllText(normalizedPath, contents, encoding ?? new UTF8Encoding(true));
			}
		}

		public virtual void AppendAllText(string path, string contents, Encoding encoding = null)
		{
			var normalizedPath = gEngine.Path.NormalizePath(path);

			if (gEngine.Path.GetExtension(normalizedPath?.ToUpper()) == ".DAT")
			{
				using (var fileStream = new System.IO.FileStream(normalizedPath, System.IO.FileMode.Append))
				{
					using (var gzipStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Compress))
					{
						using (var streamWriter = new System.IO.StreamWriter(gzipStream, encoding ?? new UTF8Encoding(true)))
						{
							streamWriter.Write(contents);
						}
					}
				}
			}
			else
			{
				System.IO.File.AppendAllText(normalizedPath, contents, encoding ?? new UTF8Encoding(true));
			}
		}
	}
}
