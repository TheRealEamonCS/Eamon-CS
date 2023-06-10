
// MainActivity.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Eamon;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using Eamon.Mobile;
using static Eamon.Game.Plugin.ContextStack;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM
{
	// *** Note: don't forget to change BuildGuid in SplashActivity.cs if necessary! ***

	[Activity(Label = "@string/app_name", Theme = "@style/MyTheme", Icon = "@drawable/ten_sided_die", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		/// <summary></summary>
		public virtual Thread GameThread { get; set; }

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			ForcePluginLinkage();

			PushEngine();

			gEngine.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			gEngine.ResolvePortabilityClassMappings();

			var path = Path.Combine(App.BasePath, Path.Combine("System", "Bin"));

			Directory.SetCurrentDirectory(path);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			App.GetAdventureDirs = () =>
			{
				var fullDirs = Directory.GetDirectories(Path.Combine(App.BasePath, "Adventures"));

				var dirList = new List<string>();

				foreach (var fullDir in fullDirs)
				{
					dirList.Add(Path.GetFileNameWithoutExtension(fullDir));
				}

				return dirList.ToArray();
			};

			App.GetBuildGuid = () =>
			{
				return SplashActivity.BuildGuid;
			};
			
			App.PluginExists = (pluginFileName) =>
			{
				var pluginBaseName = Path.GetFileNameWithoutExtension(pluginFileName);

				var assemblyName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(an => an.Name == pluginBaseName).FirstOrDefault();

				return assemblyName != null;
			};

			App.ExecutePlugin = ExecutePlugin;

			App.PluginLoop = PluginLoop;

			App.StartGameThread = StartGameThread;

			LoadApplication(new App());
		}

		protected override void OnDestroy()
		{
			if (App.Rc != RetCode.Success)
			{
				// TODO
			}

			if (GameThread != null)
			{
				GameThread.Join(0);

				if (GameThread.IsAlive)
				{
					GameThread.Abort();
				}
			}

			PopEngine();

			base.OnDestroy();
		}

		public override void OnBackPressed()
		{
			if (App.ShouldStopApplicationOnBackPressed())
			{
				RunOnUiThread(SaveSettingsAndTerminate);
			}
			else
			{
				base.OnBackPressed();
			}
		}

		/// <summary></summary>
		/// <param name="classMappings"></param>
		public virtual void LoadPortabilityClassMappings(IDictionary<Type, Type> classMappings)
		{
			//Debug.Assert(classMappings != null);

			classMappings[typeof(ITextReader)] = typeof(Game.Portability.TextReader);

			classMappings[typeof(ITextWriter)] = typeof(Game.Portability.TextWriter);

			classMappings[typeof(IMutex)] = typeof(Game.Portability.Mutex);

			classMappings[typeof(ITransferProtocol)] = typeof(Game.Portability.TransferProtocol);

			classMappings[typeof(IDirectory)] = typeof(Game.Portability.Directory);

			classMappings[typeof(IFile)] = typeof(Game.Portability.File);

			classMappings[typeof(IPath)] = typeof(Game.Portability.Path);

			classMappings[typeof(ISharpSerializer)] = typeof(Game.Portability.SharpSerializer);

			classMappings[typeof(IThread)] = typeof(Game.Portability.Thread);
		}

		/// <summary></summary>
		/// <param name="args"></param>
		/// <param name="enableStdio"></param>
		public virtual void ExecutePlugin(string[] args, bool enableStdio = true)
		{
			//Debug.Assert(args != null);

			//Debug.Assert(args.Length > 1);

			//Debug.Assert(args[0].Equals("-pfn", StringComparison.OrdinalIgnoreCase));

			var pluginFileName = System.IO.Path.GetFileNameWithoutExtension(args[1]);

			var assemblyName = Assembly.GetExecutingAssembly().GetReferencedAssemblies().Where(an => an.Name == pluginFileName).FirstOrDefault();

			while (App.PluginLauncherViewModel == null)
			{
				Thread.Sleep(150);
			}

			App.PluginLauncherViewModel.Title =
				args.Contains("-dgs") || args.Contains("EamonMH.dll") ? "EamonMH" :
				args.Contains("-rge") ? "EamonDD" :
				args.Contains("EamonRT.dll") ? "EamonRT" :
				pluginFileName;

			//Debug.Assert(assemblyName != null);

			var plugin = Assembly.Load(assemblyName);

			//Debug.Assert(plugin != null);

			var typeName = string.Format("{0}.Program", pluginFileName);

			var type = plugin.GetType(typeName);

			//Debug.Assert(type != null);

			var program = (IProgram)Activator.CreateInstance(type);

			//Debug.Assert(program != null);

			program.EnableStdio = enableStdio;

			program.LineWrapUserInput = true;

			program.ConvertDatafileToMscorlib = true;

			program.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			program.Main(args.Skip(2).ToArray());
		}

		/// <summary></summary>
		public virtual void SaveSettings()
		{
			if (App.SettingsViewModel != null)
			{
				var path = Path.Combine(App.BasePath, Path.Combine("System", "Bin"));

				var fileName = Path.Combine(path, "MOBILE_SETTINGS.DAT");

				gEngine.SharpSerializer.Serialize(App.SettingsViewModel, fileName);
			}
		}

		/// <summary></summary>
		/// <param name="obj"></param>
		public virtual void PluginLoop(object obj)
		{
			var args = (string[])obj;

			var argsList = new List<string>();

			if (args != null)
			{
				argsList.AddRange(args);
			}

			var fileText = "";

			try
			{
				fileText = gEngine.File.ReadAllText(gEngine.GlobalLaunchParametersFile);
			}
			catch (Exception)
			{
				// do nothing
			}

			fileText = fileText.Replace("\r\n", " ").Replace("\n", " ");

			fileText = Regex.Replace(fileText, @"\s+", " ").Trim();

			var tokens = fileText.Split(' ', StringSplitOptions.RemoveEmptyEntries);

			if (tokens != null)
			{
				argsList.AddRange(tokens);
			}

			args = argsList.ToArray();

			while (true)
			{
				if (args == null || args.Length < 2 || !args[0].Equals("-pfn", StringComparison.OrdinalIgnoreCase))
				{
					goto Cleanup;
				}

				var systemBinDir = string.Format("{0}System{0}Bin", Path.DirectorySeparatorChar);

				var currWorkDir = Directory.GetCurrentDirectory();

				// if current working directory invalid, bail out

				if (!currWorkDir.EndsWith(systemBinDir) || currWorkDir.Length <= systemBinDir.Length || !Directory.Exists(gEngine.AdventuresDir.Replace('\\', Path.DirectorySeparatorChar)))
				{
					goto Cleanup;
				}

				try
				{
					App.ExecutePlugin(args, true);
				}
				catch (Exception e)
				{
					// print message box

					// goto Cleanup;
				}

				args = App.NextArgs;

				App.NextArgs = null;
			}

		Cleanup:

			if (App.SettingsViewModel.KeepKeyboardVisible)
			{
				App.PluginLauncherPage.InputEntry_Unfocus();
			}

			Thread.Sleep(750);

			RunOnUiThread(SaveSettingsAndTerminate);
		}

		/// <summary></summary>
		public virtual void StartGameThread()
		{
			var threadStart = new System.Threading.ParameterizedThreadStart(App.PluginLoop);

			GameThread = new System.Threading.Thread(threadStart);

			GameThread.Start(App.BatchFile.PluginArgs);
		}

		/// <summary></summary>
		public virtual void SaveSettingsAndTerminate()
		{
			SaveSettings();

			Finish();

			Device.BeginInvokeOnMainThread(() =>
			{
				Thread.Sleep(150);

				Process.KillProcess(Process.MyPid());
			});
		}

		/// <summary></summary>
		public virtual void ForcePluginLinkage()
		{
			IEngine e = EamonMH.Game.Plugin.Globals.gEngine;

			e = EamonDD.Game.Plugin.Globals.gEngine;

			e = EamonRT.Game.Plugin.Globals.gEngine;

			e = ARuncibleCargo.Game.Plugin.Globals.gEngine;

			e = BeginnersForest.Game.Plugin.Globals.gEngine;

			e = StrongholdOfKahrDur.Game.Plugin.Globals.gEngine;

			e = TheBeginnersCave.Game.Plugin.Globals.gEngine;

			e = TheTempleOfNgurct.Game.Plugin.Globals.gEngine;

			e = TheTrainingGround.Game.Plugin.Globals.gEngine;

			e = WrenholdsSecretVigil.Game.Plugin.Globals.gEngine;

			e = LandOfTheMountainKing.Game.Plugin.Globals.gEngine;
			
			e = TheVileGrimoireOfJaldial.Game.Plugin.Globals.gEngine;

			e = RiddlesOfTheDuergarKingdom.Game.Plugin.Globals.gEngine;

			e = BeginnersCaveII.Game.Plugin.Globals.gEngine;

			e = AlternateBeginnersCave.Game.Plugin.Globals.gEngine;

			e = TheDeepCanyon.Game.Plugin.Globals.gEngine;

			e = ThePyramidOfAnharos.Game.Plugin.Globals.gEngine;
			
			e = SampleAdventure.Game.Plugin.Globals.gEngine;
			
			e = TheWayfarersInn.Game.Plugin.Globals.gEngine;

			// Note: The Eamon CS datafile upgrade logic now auto-converts System.Private.CoreLib references
			// into mscorlib references; Xamarin.Forms appears to only work with the latter.
		}
	}
}