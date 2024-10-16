﻿
// TextWriter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework.Portability;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Portability
{
	public class TextWriter : ITextWriter
	{
		/// <summary></summary>
		public bool _suppressNewLines;

		public virtual bool EnableOutput { get; set; }

		public virtual bool ResolveUidMacros { get; set; }

		public virtual bool WordWrap { get; set; }

		public virtual bool SuppressNewLines
		{
			get
			{
				return _suppressNewLines;
			}

			set
			{
				_suppressNewLines = value;

				if (_suppressNewLines)
				{
					NumNewLines = 0;
				}
			}
		}

		public virtual bool Stdout { get; set; }

		public virtual PunctSpaceCode PunctSpaceCode { get; set; }

		/// <summary></summary>
		public virtual StringBuilder Buf { get; set; }

		/// <summary></summary>
		public virtual StringBuilder Buf01 { get; set; }

		/// <summary></summary>
		public virtual Regex SingleSpaceRegex { get; set; }

		/// <summary></summary>
		public virtual Regex DoubleSpaceRegex { get; set; }

		/// <summary></summary>
		public virtual string ThreeNewLines { get; set; }

		/// <summary></summary>
		public virtual string TwoNewLines { get; set; }

		/// <summary></summary>
		public virtual long NumNewLines { get; set; }

		public virtual Encoding Encoding
		{
			get
			{
				// TODO

				return Encoding.Unicode;
			}
		}

		public virtual bool CursorVisible
		{
			get
			{
				// TODO

				return true;
			}

			set
			{
				// TODO
			}
		}

		public virtual void SetCursorPosition(Coord coord)
		{
			Debug.Assert(coord != null);

			try
			{
				App.OutputBufMutex.WaitOne();

				App.OutputBuf.Length = coord.X;
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// Do something
				}
			}
			finally
			{
				App.OutputBufMutex.ReleaseMutex();
			}

			App.EnforceOutputBufMaxSize();

			App.RefreshOutputWindowText();
		}

		public virtual void SetWindowTitle(string title)
		{
			Debug.Assert(title != null);

			// TODO
		}

		public virtual void SetWindowSize(long width, long height)
		{
			// TODO
		}

		public virtual void SetBufferSize(long width, long height)
		{
			// TODO
		}

		public virtual Coord GetCursorPosition()
		{
			var coord = new Coord();

			try
			{
				App.OutputBufMutex.WaitOne();

				coord.X = App.OutputBuf.Length;

				coord.Y = -1;
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// Do something
				}
			}
			finally
			{
				App.OutputBufMutex.ReleaseMutex();
			}

			return coord;
		}

		public virtual long GetLargestWindowWidth()
		{
			// TODO

			return 0;
		}

		public virtual long GetLargestWindowHeight()
		{
			// TODO

			return 0;
		}

		public virtual long GetWindowHeight()
		{
			// TODO

			return 0;
		}

		public virtual long GetBufferHeight()
		{
			// TODO

			return 0;
		}

		public virtual void Print(string format, params object[] arg)
		{
			Debug.Assert(format != null);

			Write(Environment.NewLine + format + Environment.NewLine, arg);
		}

		public virtual void Write(object value)
		{
			Write("{0}", value);
		}

		public virtual void Write(string value)
		{
			Write("{0}", value);
		}

		public virtual void Write(decimal value)
		{
			Write("{0}", value);
		}

		public virtual void Write(double value)
		{
			Write("{0}", value);
		}

		public virtual void Write(float value)
		{
			Write("{0}", value);
		}

		public virtual void Write(long value)
		{
			Write("{0}", value);
		}

		public virtual void Write(uint value)
		{
			Write("{0}", value);
		}

		public virtual void Write(int value)
		{
			Write("{0}", value);
		}

		public virtual void Write(bool value)
		{
			Write("{0}", value);
		}

		public virtual void Write(char[] buffer)
		{
			Debug.Assert(buffer != null);

			Write("{0}", buffer.ToString());
		}

		public virtual void Write(char value)
		{
			Write("{0}", value);
		}

		public virtual void Write(ulong value)
		{
			Write("{0}", value);
		}

		public virtual void Write(string format, object arg0)
		{
			Write(format, new object[] { arg0 });
		}

		public virtual void Write(string format, params object[] arg)
		{
			Debug.Assert(gEngine != null);

			Debug.Assert(format != null);

			Buf.SetFormat(format, arg);

			if (ResolveUidMacros)
			{
				Buf01.Clear();

				var rc = gEngine.ResolveUidMacros(Buf.ToString(), Buf01, true, true);

				Debug.Assert(gEngine.IsSuccess(rc));

				Buf.SetFormat("{0}", Buf01);
			}

			if (PunctSpaceCode != PunctSpaceCode.None)
			{
				Buf.SetFormat("{0}", PunctSpaceCode == PunctSpaceCode.Single ? SingleSpaceRegex.Replace(Buf.ToString(), @"$1 $3") : DoubleSpaceRegex.Replace(Buf.ToString(), @"$1  $3"));
			}

			if (WordWrap)
			{
				gEngine.WordWrap(Buf.ToString(), Buf);
			}

			if (SuppressNewLines)
			{
				while (Buf.IndexOf(ThreeNewLines) >= 0)
				{
					Buf.Replace(ThreeNewLines, TwoNewLines);
				}

				NumNewLines += (Buf.StartsWith(TwoNewLines) ? 2 : Buf.StartsWith(Environment.NewLine) ? 1 : 0);

				while (NumNewLines > 2 && Buf.StartsWith(Environment.NewLine))
				{
					Buf.Remove(0, Environment.NewLine.Length);

					NumNewLines--;
				}

				var s = Buf.ToString();

				if (Buf.Length > 0 && !s.Equals(Environment.NewLine) && !s.Equals(TwoNewLines))
				{
					NumNewLines = Buf.EndsWith(TwoNewLines) ? 2 : Buf.EndsWith(Environment.NewLine) ? 1 : 0;
				}
			}

			if (EnableOutput)
			{
				try
				{
					App.OutputBufMutex.WaitOne();

					if (Stdout)
					{
						App.OutputBuf.Append(Buf);
					}
					else
					{
						App.OutputBuf.Append(Buf);
					}
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// Do something
					}
				}
				finally
				{
					App.OutputBufMutex.ReleaseMutex();
				}

				App.EnforceOutputBufMaxSize();

				App.RefreshOutputWindowText();
			}
		}

		public virtual void Write(string format, object arg0, object arg1)
		{
			Write(format, new object[] { arg0, arg1 });
		}

		public virtual void Write(char[] buffer, int index, int count)
		{
			Debug.Assert(buffer != null);

			Write("{0}", buffer.ToString().Substring(index, count));
		}

		public virtual void Write(string format, object arg0, object arg1, object arg2)
		{
			Write(format, new object[] { arg0, arg1, arg2 });
		}

		public virtual void WriteLine()
		{
			WriteLine("{0}", "");
		}

		public virtual void WriteLine(object value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(string value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(decimal value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(float value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(ulong value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(double value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(uint value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(int value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(bool value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(char[] buffer)
		{
			Debug.Assert(buffer != null);

			WriteLine("{0}", buffer.ToString());
		}

		public virtual void WriteLine(char value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(long value)
		{
			WriteLine("{0}", value);
		}

		public virtual void WriteLine(string format, object arg0)
		{
			WriteLine(format, new object[] { arg0 });
		}

		public virtual void WriteLine(string format, params object[] arg)
		{
			Debug.Assert(format != null);

			Write(format + Environment.NewLine, arg);
		}

		public virtual void WriteLine(char[] buffer, int index, int count)
		{
			Debug.Assert(buffer != null);

			WriteLine("{0}", buffer.ToString().Substring(index, count));
		}

		public virtual void WriteLine(string format, object arg0, object arg1)
		{
			WriteLine(format, new object[] { arg0, arg1 });
		}

		public virtual void WriteLine(string format, object arg0, object arg1, object arg2)
		{
			WriteLine(format, new object[] { arg0, arg1, arg2 });
		}

		/// <summary></summary>
		public virtual void BackpatchLastCommand(string inputStr)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(inputStr));

			try
			{
				App.OutputBufMutex.WaitOne();

				if (App.OutputBuf.EndsWith(Environment.NewLine))
				{
					App.OutputBuf.Length -= Environment.NewLine.Length;
				}
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// Do something
				}
			}
			finally
			{
				App.OutputBufMutex.ReleaseMutex();
			}

			gOut.WriteLine(inputStr);
		}

		public TextWriter()
		{
			EnableOutput = true;

			ResolveUidMacros = true;

			WordWrap = true;

			SuppressNewLines = true;

			Stdout = true;

			Buf = new StringBuilder(gEngine.BufSize);

			Buf01 = new StringBuilder(gEngine.BufSize);

			SingleSpaceRegex = new Regex(@"([.?!:])(  )([^ ]|$)");

			DoubleSpaceRegex = new Regex(@"([.?!:])( )([^ ]|$)");

			ThreeNewLines = string.Format("{0}{0}{0}", Environment.NewLine);

			TwoNewLines = string.Format("{0}{0}", Environment.NewLine);
		}
	}
}
