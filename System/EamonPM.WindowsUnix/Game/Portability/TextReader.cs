
// TextReader.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework.Portability;
using Eamon.Game.Extensions;
using static Eamon.Game.Plugin.PluginContext;

namespace EamonPM.Game.Portability
{
	public class TextReader : ITextReader
	{
		public virtual bool EnableInput { get; set; }

		public virtual bool ReadLineMode { get; set; }

		public virtual RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, char fillChar, char maskChar, bool emptyAllowed, string emptyVal, Func<char, char> modifyCharFunc, Func<char, bool> validCharFunc, Func<char, bool> termCharFunc)
		{
			RetCode rc;
			bool validChar;
			bool termChar;
			int rows;
			int h;
			int i;
			char ch;

			if (buf == null || bufSize < 1 || (boxChars != null && (boxChars[0] == '\0' || boxChars[1] == '\0')) || (emptyVal != null && emptyVal[0] == '\0'))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (!EnableInput)
			{
				buf.Clear();

				goto Cleanup;
			}

			if (fillChar == '\0')
			{
				fillChar = ' ';
			}

			var inputCh0Pos = Globals.Out.GetCursorPosition();

			rows = (int)bufSize / Constants.WindowWidth;

			if (bufSize == Constants.BufSize03)
			{
				rows += 2;
			}
			else if (!ReadLineMode)
			{
				rows++;
			}

			for (i = 0; i < rows; i++)
			{
				var cursorPosition = Globals.Out.GetCursorPosition();

				if (cursorPosition.Y + 1 >= Globals.Out.GetBufferHeight())
				{
					inputCh0Pos.Y--;
				}

				Globals.Out.WriteLine();
			}

			Globals.Out.SetCursorPosition(inputCh0Pos);

			if (boxChars != null)
			{
				Globals.Out.Write(boxChars[0]);
			}

			inputCh0Pos = Globals.Out.GetCursorPosition();

			Globals.Out.Write(new string(fillChar, (int)bufSize));

			if (boxChars != null)
			{
				Globals.Out.Write(boxChars[1]);
			}

			Globals.Out.SetCursorPosition(inputCh0Pos);

			var charPos = new Coord[bufSize + 1];

			if (buf.Length > bufSize)
			{
				buf.Length = (int)bufSize;
			}

			i = buf.Length;

			for (h = 0; h < i; h++)
			{
				charPos[h] = Globals.Out.GetCursorPosition();

				Globals.Out.Write(maskChar != '\0' ? maskChar : buf[h]);
			}

			if (buf.Length < bufSize)
			{
				buf.Length = (int)bufSize;
			}

			Console.CursorVisible = true;

			while (true)
			{
				charPos[i] = Globals.Out.GetCursorPosition();

				ch = Console.ReadKey(true).KeyChar;

				if (ch == '\r' || ch == '\n' || ch == '\t')
				{
					Globals.Out.SetCursorPosition(inputCh0Pos);

					if (i > 0 || emptyAllowed)
					{
						goto ExitLoop;
					}
				}
				else if (ch == 0x1B)
				{
					Globals.Out.SetCursorPosition(inputCh0Pos);

					Globals.Out.Write(new string(fillChar, (int)bufSize));

					if (boxChars != null)
					{
						Globals.Out.Write(boxChars[1]);
					}

					Globals.Out.SetCursorPosition(inputCh0Pos);

					Array.Clear(charPos, 0, (int)bufSize + 1);

					i = 0;
				}
				else if (ch == '\b' || ch == 0x7F)
				{
					if (i > 0)
					{
						Globals.Out.SetCursorPosition(charPos[i - 1]);

						Globals.Out.Write(fillChar);

						Globals.Out.SetCursorPosition(charPos[i - 1]);

						charPos[i] = null;

						i--;
					}
				}
				else if (i < bufSize)
				{
					if (ch != 0x00 && ch != 0xE0)
					{
						if (modifyCharFunc != null)
						{
							ch = modifyCharFunc(ch);
						}

						validChar = true;

						if (validCharFunc != null)
						{
							validChar = validCharFunc(ch);
						}
					}
					else
					{
						validChar = false;
					}

					if (validChar)
					{
						if (ch != '\0')
						{
							buf[i++] = ch;

							Globals.Out.Write(maskChar != '\0' ? maskChar : ch);
						}

						termChar = false;

						if (termCharFunc != null)
						{
							termChar = termCharFunc(ch);
						}

						if (termChar)
						{
							goto ExitLoop;
						}
					}
				}
			}

		ExitLoop:

			Console.CursorVisible = false;

			buf.Length = i;

			if (buf.Length == 0 && emptyVal != null)
			{
				buf.SetFormat("{0}", emptyVal);
			}

			Globals.Out.SetCursorPosition(inputCh0Pos);

			i = buf.Length;

			if (maskChar != '\0')
			{
				Globals.Out.Write(new string(maskChar, i));
			}
			else
			{
				Globals.Out.Write("{0}", buf);
			}

			Globals.Out.Write(new string(' ', (int)bufSize - i));

			if (boxChars == null)
			{
				Globals.Out.SetCursorPosition(inputCh0Pos);

				if (maskChar != '\0')
				{
					Globals.Out.Write(new string(maskChar, i));
				}
				else
				{
					Globals.Out.Write("{0}", buf);
				}
			}
			else
			{
				Globals.Out.Write(boxChars[1]);
			}

			Globals.Out.Write(Environment.NewLine);

		Cleanup:

			return rc;
		}

		public virtual RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, string emptyVal, Func<char, bool> validCharFunc)
		{
			return ReadField(buf, bufSize, boxChars, ' ', '\0', true, emptyVal, null, validCharFunc, null);
		}

		public virtual RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, string emptyVal)
		{
			return ReadField(buf, bufSize, boxChars, ' ', '\0', true, emptyVal, null, null, null);
		}

		public virtual string ReadLine()
		{
			if (EnableInput)
			{
				var cursorPosition = Globals.Out.GetCursorPosition();

				var bufSize = (Constants.WindowWidth * 2) - (cursorPosition.X + 1);

				var buf = new StringBuilder(bufSize);

				ReadLineMode = true;

				Globals.Out.WordWrap = false;

				var suppressNewLines = Globals.Out.SuppressNewLines;

				Globals.Out.SuppressNewLines = false;

				var rc = ReadField(buf, bufSize, null, ' ', '\0', true, null, null, null, null);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Out.WordWrap = true;

				Globals.Out.SuppressNewLines = suppressNewLines;

				ReadLineMode = false;

				return buf.ToString();
			}
			else
			{
				return string.Empty;
			}
		}

		public virtual char ReadKey(bool intercept)
		{
			char ch;

			if (EnableInput)
			{
				Console.CursorVisible = true;

				ch = Console.ReadKey(intercept).KeyChar;

				Console.CursorVisible = false;
			}
			else
			{
				ch = '\0';
			}

			return ch;
		}

		public virtual void KeyPress(StringBuilder buf, bool initialNewLine = true)
		{
			Debug.Assert(buf != null);

			Debug.Assert(Globals.Engine != null);

			if (EnableInput)
			{
				Globals.Out.WriteLine("{0}{1}", initialNewLine ? Environment.NewLine : "", Globals.LineSep);

				Globals.Out.Write("{0}Press any key to continue: ", Environment.NewLine);

				buf.Clear();

				var rc = ReadField(buf, Constants.BufSize02, null, ' ', '\0', true, null, Globals.Engine.ModifyCharToNull, null, Globals.Engine.IsCharAny);

				Debug.Assert(Globals.Engine.IsSuccess(rc));

				Globals.Thread.Sleep(150);
			}
		}

		public TextReader()
		{
			EnableInput = true;
		}
	}
}
