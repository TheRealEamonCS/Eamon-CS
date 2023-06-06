
// WindowRepainter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Runtime.InteropServices;

namespace Eamon.Game.Utilities
{
	/// <summary>
	/// </summary>
	/// <remarks>
	/// Full credit:  
	///     https://chat.openai.com/        "give me the C# code to repaint a window given its window handle"
	/// </remarks>
	public static class WindowRepainter
	{
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

		[DllImport("user32.dll")]
		static extern bool UpdateWindow(IntPtr hWnd);

		public static IntPtr GetConsoleWindow01()
		{
			return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? GetConsoleWindow() : IntPtr.Zero;
		}

		public static void RepaintWindow(IntPtr hWnd)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				InvalidateRect(hWnd, IntPtr.Zero, true);

				UpdateWindow(hWnd);
			}
		}
	}
}
