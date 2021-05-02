
// FixedLengthReader.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.IO;
using System.Reflection;
using Eamon.Game.Attributes;

namespace Eamon.Game.Utilities
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// Full credit:  https://stackoverflow.com/questions/26060441/reading-data-from-fixed-length-file-into-class-objects
	/// </remarks>
	public class FixedLengthReader
	{
		public Stream _stream;

		public bool _utf8Strings;

		public byte[] _buffer;

		public FixedLengthReader(Stream stream, long origin, bool utf8Strings)
		{
			_stream = stream;

			_utf8Strings = utf8Strings;

			_buffer = new byte[4];

			stream.Seek(origin, SeekOrigin.Begin);
		}

		public virtual void read<T>(T data)
		{
			foreach (FieldInfo fi in typeof(T).GetFields())
			{
				foreach (object attr in fi.GetCustomAttributes())
				{
					if (attr is LayoutAttribute)
					{
						LayoutAttribute la = (LayoutAttribute)attr;
						if (_buffer.Length < la.length) _buffer = new byte[la.length];
						_stream.Read(_buffer, 0, la.length);

						if (fi.FieldType.Equals(typeof(int)))
						{
							fi.SetValue(data, BitConverter.ToInt32(_buffer, 0));
						}
						else if (fi.FieldType.Equals(typeof(bool)))
						{
							fi.SetValue(data, BitConverter.ToBoolean(_buffer, 0));
						}
						else if (fi.FieldType.Equals(typeof(string)))
						{
							if (_utf8Strings)
							{
								// --- If string was written using UTF8 ---
								byte[] tmp = new byte[la.length];
								Array.Copy(_buffer, tmp, tmp.Length);
								fi.SetValue(data, System.Text.Encoding.UTF8.GetString(tmp));
							}
							else
							{
								// --- ALTERNATIVE: Chars were written to file ---
								char[] tmp = new char[la.length - 1];
								for (int i = 0; i < la.length; i++)
								{
								    tmp[i] = BitConverter.ToChar(_buffer, i * sizeof(char));
								}
								fi.SetValue(data, new string(tmp));
							}
						}
						else if (fi.FieldType.Equals(typeof(double)))
						{
							fi.SetValue(data, BitConverter.ToDouble(_buffer, 0));
						}
						else if (fi.FieldType.Equals(typeof(short)))
						{
							fi.SetValue(data, BitConverter.ToInt16(_buffer, 0));
						}
						else if (fi.FieldType.Equals(typeof(long)))
						{
							fi.SetValue(data, BitConverter.ToInt64(_buffer, 0));
						}
						else if (fi.FieldType.Equals(typeof(float)))
						{
							fi.SetValue(data, BitConverter.ToSingle(_buffer, 0));
						}
						else if (fi.FieldType.Equals(typeof(ushort)))
						{
							fi.SetValue(data, BitConverter.ToUInt16(_buffer, 0));
						}
						else if (fi.FieldType.Equals(typeof(uint)))
						{
							fi.SetValue(data, BitConverter.ToUInt32(_buffer, 0));
						}
						else if (fi.FieldType.Equals(typeof(ulong)))
						{
							fi.SetValue(data, BitConverter.ToUInt64(_buffer, 0));
						}
					}
				}
			}
		}
	}
}
