
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
using static Eamon.Game.Plugin.PluginContext;

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
				// do something
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

			if (Globals != null)
			{
				if (Globals.Engine != null)
				{
					Globals.Engine.LineWrap(buf.ToString(), Buf01, startColumn);
				}

				if (Globals.Out != null)
				{
					Globals.Out.WriteLine("{0}", Buf01);
				}
				else if (Globals.Error != null)
				{
					Globals.Error.WriteLine("{0}", Buf01);
				}
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
				var cursorPosition = Globals.Out.GetCursorPosition();

				var bufSize = (Constants.WindowWidth * 2);

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
				var buf = new StringBuilder(Constants.BufSize);

				ReadField(buf, Constants.BufSize02, null, ' ', '\0', true, null, null, null, (ch01) => true);

				ch = buf.Length > 0 ? buf[0] : '\0';
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

			Buf01 = new StringBuilder(Constants.BufSize);
		}
	}
}
