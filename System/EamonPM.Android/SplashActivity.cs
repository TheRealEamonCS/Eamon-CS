
// SplashActivity.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Eamon;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using Eamon.Mobile;
using static Eamon.Game.Plugin.PluginContext;
using static Eamon.Game.Plugin.PluginContextStack;

namespace EamonPM
{
	[Activity(Theme = "@style/MyTheme.Splash", ScreenOrientation = ScreenOrientation.Locked, NoHistory = true, MainLauncher = true)]
    public class SplashActivity : AppCompatActivity
    {
			/*
				- leave the BuildGuid alone to upgrade only the binary .apk file

				- change the BuildGuid to upgrade the binary .apk file and the .DAT datafiles in the filesystem (but not CHARACTERS.DAT)
			*/

			static readonly string BuildGuid = "9BE6A65E-252C-44EB-BF3F-9E9F05894053";

			static readonly string TAG = "X:" + typeof (SplashActivity).Name;

		// Launches the startup task
		protected override void OnResume()
		{
			base.OnResume();

			App.Rc = RetCode.Success;

			App.BasePath = Application.Context.FilesDir.Path;

			MirrorAssets();

			StartActivity(new Intent(this, typeof(MainActivity)));         // StartActivity(new Intent(Application.Context, typeof(MainActivity)));

			Finish();
		}

		// Launches the startup task
		public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
		{
			base.OnCreate(savedInstanceState, persistentState);
			Log.Debug(TAG, "SplashActivity.OnCreate");
		}

		// Prevent the back button from canceling the startup process
		public override void OnBackPressed() 
		{ 
		
		}

		/// <summary></summary>
		public virtual void MirrorAssets()
		{
			var path = Path.Combine(App.BasePath, Path.Combine("System", "Bin"));

			Directory.CreateDirectory(path);

			var copyFiles = true;

			var guidFile = Path.Combine(path, "BUILDGUID.TXT");

			if (File.Exists(guidFile))
			{
				var savedGuid = File.ReadAllText(guidFile);

				if (BuildGuid.Equals(savedGuid, StringComparison.OrdinalIgnoreCase))
				{
					copyFiles = false;
				}
			}

			File.WriteAllText(guidFile, BuildGuid);

			if (copyFiles)
			{
				var binFiles = Assets.List(Path.Combine("System", "Bin"));

				foreach (var file in binFiles)
				{
					var fileName = Path.Combine(path, file);

					if (!fileName.EndsWith("CHARACTERS.DAT", StringComparison.OrdinalIgnoreCase) || !File.Exists(fileName))
					{
						fileName = Path.Combine(Path.Combine("System", "Bin"), file);

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
					path = Path.Combine(App.BasePath, Path.Combine("Adventures", dir));

					Directory.CreateDirectory(path);

					var adventureFiles = Assets.List(Path.Combine("Adventures", dir));

					foreach (var file in adventureFiles)
					{
						var fileName = Path.Combine(Path.Combine("Adventures", dir), file);

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