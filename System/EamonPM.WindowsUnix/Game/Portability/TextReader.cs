
// TextReader.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework.Portability;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.Globals;

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

			Debug.Assert(gEngine != null);

			if (buf == null || bufSize < 1 || (boxChars != null && (boxChars[0] == '\0' || boxChars[1] == '\0')) || (emptyVal != null && emptyVal[0] == '\0'))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

            WindowRepainter.RepaintWindow(gEngine.ConsoleHandle);

            if (!EnableInput)
			{
				buf.Clear();

				goto Cleanup;
			}

			if (gEngine.EnableScreenReaderMode || fillChar == '\0')
			{
				fillChar = ' ';
			}

			var inputCh0Pos = gEngine.Out.GetCursorPosition();

			rows = (int)(inputCh0Pos.X + bufSize) / gEngine.WindowWidth;

			if (!ReadLineMode)
			{
				rows++;
			}

			for (i = 0; i < rows; i++)
			{
				var cursorPosition = gEngine.Out.GetCursorPosition();

				if (cursorPosition.Y + 1 >= gEngine.Out.GetBufferHeight())
				{
					inputCh0Pos.Y--;
				}

				gEngine.Out.WriteLine();
			}

			gEngine.Out.SetCursorPosition(inputCh0Pos);

			if (boxChars != null)
			{
				gEngine.Out.Write(boxChars[0]);
			}

			inputCh0Pos = gEngine.Out.GetCursorPosition();

			gEngine.Out.Write(new string(fillChar, (int)bufSize));

			if (boxChars != null)
			{
				gEngine.Out.Write(boxChars[1]);
			}

			gEngine.Out.SetCursorPosition(inputCh0Pos);

			var charPos = new Coord[bufSize + 1];

			if (buf.Length > bufSize)
			{
				buf.Length = (int)bufSize;
			}

			i = buf.Length;

			for (h = 0; h < i; h++)
			{
				charPos[h] = gEngine.Out.GetCursorPosition();

				gEngine.Out.Write(maskChar != '\0' ? maskChar : buf[h]);
			}

			if (buf.Length < bufSize)
			{
				buf.Length = (int)bufSize;
			}

			Console.CursorVisible = true;

			while (true)
			{
				charPos[i] = gEngine.Out.GetCursorPosition();

				ch = Console.ReadKey(true).KeyChar;

				if (ch == '\r' || ch == '\n' || ch == '\t')
				{
					gEngine.Out.SetCursorPosition(inputCh0Pos);

					if (i > 0 || emptyAllowed)
					{
						goto ExitLoop;
					}
				}
				else if (ch == 0x1B)
				{
					gEngine.Out.SetCursorPosition(inputCh0Pos);

					gEngine.Out.Write(new string(fillChar, (int)bufSize));

					if (boxChars != null)
					{
						gEngine.Out.Write(boxChars[1]);
					}

					gEngine.Out.SetCursorPosition(inputCh0Pos);

					Array.Clear(charPos, 0, (int)bufSize + 1);

					i = 0;
				}
				else if (ch == '\b' || ch == 0x7F)
				{
					if (i > 0)
					{
						gEngine.Out.SetCursorPosition(charPos[i - 1]);

						gEngine.Out.Write(fillChar);

						gEngine.Out.SetCursorPosition(charPos[i - 1]);

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

							gEngine.Out.Write(maskChar != '\0' ? maskChar : ch);
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

			if (!gEngine.EnableScreenReaderMode)
			{
				if (buf.Length == 0 && emptyVal != null)
				{
					buf.SetFormat("{0}", emptyVal);
				}

				gEngine.Out.SetCursorPosition(inputCh0Pos);

				i = buf.Length;

				if (maskChar != '\0')
				{
					gEngine.Out.Write(new string(maskChar, i));
				}
				else
				{
					gEngine.Out.Write("{0}", buf);
				}

				gEngine.Out.Write(new string(' ', (int)bufSize - i));

				if (boxChars == null)
				{
					gEngine.Out.SetCursorPosition(inputCh0Pos);

					if (maskChar != '\0')
					{
						gEngine.Out.Write(new string(maskChar, i));
					}
					else
					{
						gEngine.Out.Write("{0}", buf);
					}
				}
				else
				{
					gEngine.Out.Write(boxChars[1]);
				}
			}
			else
			{
				if (buf.Length == 0 && emptyVal != null)
				{
					buf.SetFormat("{0}", emptyVal);

					gEngine.Out.Write("{0}", buf);
				}
			}

			gEngine.Out.Write(Environment.NewLine);

            WindowRepainter.RepaintWindow(gEngine.ConsoleHandle);

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
				var cursorPosition = gEngine.Out.GetCursorPosition();

				var bufSize = (gEngine.WindowWidth * 2) - (cursorPosition.X + 1);

				var buf = new StringBuilder(bufSize);

				ReadLineMode = true;

				gEngine.Out.WordWrap = false;

				var suppressNewLines = gEngine.Out.SuppressNewLines;

				gEngine.Out.SuppressNewLines = false;

				var rc = ReadField(buf, bufSize, null, ' ', '\0', true, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Out.WordWrap = true;

				gEngine.Out.SuppressNewLines = suppressNewLines;

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
                WindowRepainter.RepaintWindow(gEngine.ConsoleHandle);

                Console.CursorVisible = true;

				ch = Console.ReadKey(intercept).KeyChar;

				Console.CursorVisible = false;

                WindowRepainter.RepaintWindow(gEngine.ConsoleHandle);
            }
            else
			{
				ch = '\0';
			}

			return ch;
		}

		public virtual void KeyPress(StringBuilder buf, bool initialNewLine = true)
		{
			Debug.Assert(gEngine != null);

			Debug.Assert(buf != null);

			if (EnableInput)
			{
				gEngine.Out.WriteLine("{0}{1}", initialNewLine ? Environment.NewLine : "", gEngine.LineSep);

				gEngine.Out.Write("{0}Press any key to continue: ", Environment.NewLine);

				buf.Clear();

				var rc = ReadField(buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNull, null, gEngine.IsCharAny);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Thread.Sleep(150);
			}
		}

		public TextReader()
		{
			EnableInput = true;
		}
	}
}
