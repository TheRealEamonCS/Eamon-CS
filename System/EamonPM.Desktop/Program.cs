
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Threading;
using Avalonia;
using Avalonia.ReactiveUI;

namespace EamonPM.Desktop
{
    class Program
    {
        // MP NOTE: when changing this file regenerate both published executables and EamonPM.Desktop.dll (the former may be unchanged)

        // MP NOTE: when publishing you can safely delete the System\Bin\runtimes folder since it contains only duplicates (verified with KDiff3)

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
			App.ProgramName = "EamonPM.Desktop";

			App.OrigBasePath = AppContext.BaseDirectory;

			App.BasePath = AppContext.BaseDirectory.Replace("\\System\\Bin\\", "").Replace("/System/Bin/", "");

			App.KillProcessFunc = () =>
			{
				Thread.Sleep(150);

				Environment.Exit(0);
			};

			App.StartBrowserFunc = (url) =>
			{
				var proc = new Process();

				proc.StartInfo.UseShellExecute = true;

				proc.StartInfo.FileName = url;

				proc.Start();
			};

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace()
                .UseReactiveUI();
    }
}
