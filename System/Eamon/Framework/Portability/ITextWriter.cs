
// ITextWriter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public class Coord
	{
		/// <summary></summary>
		public int X { get; set; }

		/// <summary></summary>
		public int Y { get; set; }
	}

	/// <summary></summary>
	public interface ITextWriter
	{
		/// <summary></summary>
		bool EnableOutput { get; set; }

		/// <summary></summary>
		bool ResolveUidMacros { get; set; }

		/// <summary></summary>
		bool WordWrap { get; set; }

		/// <summary></summary>
		bool SuppressNewLines { get; set; }

		/// <summary></summary>
		bool Stdout { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether text being output has one or two spaces after punctuation, or if this behavior is disabled.
		/// </summary>
		PunctSpaceCode PunctSpaceCode { get; set; }

		/// <summary></summary>
		Encoding Encoding { get; }

		/// <summary></summary>
		bool CursorVisible { get; set; }

		/// <summary></summary>
		/// <param name="coord"></param>
		void SetCursorPosition(Coord coord);

		/// <summary></summary>
		/// <param name="title"></param>
		void SetWindowTitle(string title);

		/// <summary></summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		void SetWindowSize(long width, long height);

		/// <summary></summary>
		/// <param name="width"></param>
		/// <param name="height"></param>
		void SetBufferSize(long width, long height);

		/// <summary></summary>
		/// <returns></returns>
		Coord GetCursorPosition();

		/// <summary></summary>
		/// <returns></returns>
		long GetLargestWindowWidth();

		/// <summary></summary>
		/// <returns></returns>
		long GetLargestWindowHeight();

		/// <summary></summary>
		/// <returns></returns>
		long GetWindowHeight();

		/// <summary></summary>
		/// <returns></returns>
		long GetBufferHeight();

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg"></param>
		void Print(string format, params object[] arg);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(object value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(string value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(decimal value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(double value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(float value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(long value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(uint value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(int value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(bool value);

		/// <summary></summary>
		/// <param name="buffer"></param>
		void Write(char[] buffer);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(char value);

		/// <summary></summary>
		/// <param name="value"></param>
		void Write(ulong value);

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		void Write(string format, object arg0);

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg"></param>
		void Write(string format, params object[] arg);

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <param name="arg1"></param>
		void Write(string format, object arg0, object arg1);

		/// <summary></summary>
		/// <param name="buffer"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		void Write(char[] buffer, int index, int count);

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		void Write(string format, object arg0, object arg1, object arg2);

		/// <summary></summary>
		void WriteLine();

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(object value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(string value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(decimal value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(float value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(ulong value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(double value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(uint value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(int value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(bool value);

		/// <summary></summary>
		/// <param name="buffer"></param>
		void WriteLine(char[] buffer);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(char value);

		/// <summary></summary>
		/// <param name="value"></param>
		void WriteLine(long value);

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		void WriteLine(string format, object arg0);

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg"></param>
		void WriteLine(string format, params object[] arg);

		/// <summary></summary>
		/// <param name="buffer"></param>
		/// <param name="index"></param>
		/// <param name="count"></param>
		void WriteLine(char[] buffer, int index, int count);

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <param name="arg1"></param>
		void WriteLine(string format, object arg0, object arg1);

		/// <summary></summary>
		/// <param name="format"></param>
		/// <param name="arg0"></param>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		void WriteLine(string format, object arg0, object arg1, object arg2);
	}
}
