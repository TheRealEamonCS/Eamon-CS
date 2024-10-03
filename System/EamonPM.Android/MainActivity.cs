
// MainActivity.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.IO;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Avalonia;
using Avalonia.Android;
using Avalonia.ReactiveUI;
using Android2 = Android;

namespace EamonPM.Android
{
	[Activity(
		Label = "EamonPM.Android",
		Theme = "@style/MyTheme.NoActionBar",
		Icon = "@drawable/ten_sided_die",
		MainLauncher = true,
		ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
	public class MainActivity : AvaloniaMainActivity<App>
	{
		public override void OnBackPressed()
		{
			if (App.ShouldStopApplicationOnBackPressed())
			{
				App.KillProcess();
			}
		}

		protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
		{
			App.ProgramName = "EamonPM.Android";

			App.OrigBasePath = Path.Combine(FilesDir.Path, "System", "Bin");

			App.BasePath = FilesDir.Path;

			App.KillProcessFunc = () =>
			{
				Finish();

				Thread.Sleep(150);

				Process.KillProcess(Process.MyPid());
			};

			App.StartBrowserFunc = url =>
			{
				var intent = new Intent(Intent.ActionView, Android2.Net.Uri.Parse(url));
				intent.SetFlags(ActivityFlags.NewTask);
				Android2.App.Application.Context.StartActivity(intent);
			};

			App.ShowKeyboardFunc = () =>
			{
				var inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
				inputMethodManager.ToggleSoftInput(ShowFlags.Forced, HideSoftInputFlags.ImplicitOnly);
			};

			App.HideKeyboardFunc = () =>
			{
				var activity = (Activity)this;
				var view = activity.CurrentFocus;
				if (view != null)
				{
					var inputMethodManager = (InputMethodManager)GetSystemService(Context.InputMethodService);
					inputMethodManager.HideSoftInputFromWindow(view.WindowToken, 0);
					view.ClearFocus();
				}
			};

			App.RefreshStatusFunc = isLightTheme =>
			{
				if (Build.VERSION.SdkInt >= BuildVersionCodes.O) // Android 8.0 (API level 26) and above
				{
					var window = Window;
					var decorView = window?.DecorView;
					if (decorView != null)
					{
						var flags = decorView.SystemUiFlags;

						if (isLightTheme)
						{
							flags |= (SystemUiFlags)SystemUiFlags.LightStatusBar;
							window.SetStatusBarColor(Android2.Graphics.Color.White);
						}
						else
						{
							flags &= ~(SystemUiFlags)SystemUiFlags.LightStatusBar;
							window.SetStatusBarColor(Android2.Graphics.Color.Black);
						}

						decorView.SystemUiFlags = flags;
					}
				}
			};

			MirrorAssets();

			return base.CustomizeAppBuilder(builder)
				.WithInterFont()
				.UseReactiveUI();
		}

		public virtual void MirrorAssets()
		{
			var path = Path.Combine(App.BasePath, "System", "Bin");

			Directory.CreateDirectory(path);

			var copyFiles = true;

			var guidFile = Path.Combine(path, "BUILDGUID.TXT");

			if (File.Exists(guidFile))
			{
				var savedGuid = File.ReadAllText(guidFile);

				if (App.BuildGuid.Equals(savedGuid, StringComparison.OrdinalIgnoreCase))
				{
					copyFiles = false;
				}
			}

			File.WriteAllText(guidFile, App.BuildGuid);

			if (copyFiles)
			{
				var binFiles = Assets.List(Path.Combine("System", "Bin"));

				foreach (var file in binFiles)
				{
					var fileName = Path.Combine(path, file);

					if ((!fileName.EndsWith("CHARACTERS.DAT", StringComparison.OrdinalIgnoreCase) && !fileName.EndsWith("EAMONPM_SETTINGS.DAT", StringComparison.OrdinalIgnoreCase)) || !File.Exists(fileName))
					{
						fileName = Path.Combine("System", "Bin", file);

						using (var streamReader = new StreamReader(Assets.Open(fileName)))
						{
							var fileBytes = default(byte[]);

							using (var memoryStream = new MemoryStream())
							{
								streamReader.BaseStream.CopyTo(memoryStream);

								fileBytes = memoryStream.ToArray();

								fileName = Path.Combine(path, file);

								File.WriteAllBytes(fileName, fileBytes);
							}
						}
					}
				}

				var adventureDirs = Assets.List("Adventures");

				foreach (var dir in adventureDirs)
				{
					path = Path.Combine(App.BasePath, "Adventures", dir);

					Directory.CreateDirectory(path);

					var adventureFiles = Assets.List(Path.Combine("Adventures", dir));

					foreach (var file in adventureFiles)
					{
						var fileName = Path.Combine("Adventures", dir, file);

						using (var streamReader = new StreamReader(Assets.Open(fileName)))
						{
							var fileBytes = default(byte[]);

							using (var memoryStream = new MemoryStream())
							{
								streamReader.BaseStream.CopyTo(memoryStream);

								fileBytes = memoryStream.ToArray();

								fileName = Path.Combine(path, file);

								File.WriteAllBytes(fileName, fileBytes);
							}
						}
					}
				}
			}
		}
	}
}
