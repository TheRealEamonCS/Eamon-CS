
// TextReader.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms;
using Eamon;
using Eamon.Framework.Portability;
using Eamon.Game.Extensions;
using Eamon.Mobile;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Portability
{
	public class TextReader : ITextReader
	{
		public virtual bool EnableInput { get; set; }

		public virtual StringBuilder Buf01 { get; set; }

		public virtual bool ReadLineMode { get; set; }

		public virtual RetCode ReadField(StringBuilder buf, long bufSize, char[] boxChars, char fillChar, char maskChar, bool emptyAllowed, string emptyVal, Func<char, char> modifyCharFunc, Func<char, bool> validCharFunc, Func<char, bool> termCharFunc)
		{
			RetCode rc;

			Debug.Assert(gEngine != null);

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

			App.InputBufSize = bufSize;

			App.InputBoxChars = boxChars;

			App.InputFillChar = fillChar;

			App.InputMaskChar = maskChar;

			App.InputEmptyAllowed = emptyAllowed;

			App.InputEmptyVal = emptyVal;

			App.InputModifyCharFunc = modifyCharFunc;

			App.InputValidCharFunc = validCharFunc;

			App.InputTermCharFunc = termCharFunc;

			var startColumn = 0L;

			try
			{
				App.OutputBufMutex.WaitOne();

				var lastIndex = App.OutputBuf.LastIndexOf(Environment.NewLine);

				startColumn = lastIndex > -1 ? App.OutputBuf.Length - (lastIndex + 1) : App.OutputBuf.Length;
			}
			catch (Exception ex)
			{
				// Do something
			}
			finally
			{
				App.OutputBufMutex.ReleaseMutex();
			}

			var defaultText = buf.ToString();

			Device.BeginInvokeOnMainThread(() =>
			{
				App.PluginLauncherPage.SetInputTextNoEvents(defaultText);
			});

			while (true)         // Xamarin Forms bug workaround ???
			{
				App.FinishInput.WaitOne();

				buf.SetFormat("{0}", App.PluginLauncherViewModel.InputText);

				App.FinishInputSet = false;

				if (buf.Length > 0 || (emptyAllowed && string.IsNullOrEmpty(emptyVal)))
				{
					break;
				}
			}

			Device.BeginInvokeOnMainThread(() =>
			{
				App.PluginLauncherPage.SetInputTextNoEvents("");
			});

			gEngine.LineWrap(buf.ToString(), Buf01, startColumn);

			if (gEngine.Out != null)
			{
				gEngine.Out.WriteLine("{0}", Buf01);
			}
			else if (gEngine.Error != null)
			{
				gEngine.Error.WriteLine("{0}", Buf01);
			}

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

				var bufSize = (gEngine.WindowWidth * 2);

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
				var buf = new StringBuilder(gEngine.BufSize);

				ReadField(buf, gEngine.BufSize02, null, ' ', '\0', true, null, null, null, (ch01) => true);

				ch = buf.Length > 0 ? buf[0] : '\0';
			}
			else
			{
				ch = '\0';
			}

			return ch;
		}

		public virtual void KeyPress(StringBuilder buf, bool prependNewLine = true)
		{
			Debug.Assert(gEngine != null);

			Debug.Assert(buf != null);

			if (EnableInput)
			{
				gEngine.Out.WriteLine("{0}{1}", prependNewLine ? Environment.NewLine : "", gEngine.LineSep);

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

			Buf01 = new StringBuilder(gEngine.BufSize);
		}
	}
}
