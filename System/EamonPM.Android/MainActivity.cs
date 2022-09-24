
// MainActivity.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Eamon;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using Eamon.Mobile;
using static Eamon.Game.Plugin.PluginContext;
using static Eamon.Game.Plugin.PluginContextStack;

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

			PushConstants();

			PushClassMappings();

			ClassMappings.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			ClassMappings.ResolvePortabilityClassMappings();

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

			PopClassMappings();

			PopConstants();

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

				ClassMappings.SharpSerializer.Serialize(App.SettingsViewModel, fileName);
			}
		}

		/// <summary></summary>
		/// <param name="obj"></param>
		public virtual void PluginLoop(object obj)
		{
			var args = (string[])obj;

			while (true)
			{
				if (args == null || args.Length < 2 || !args[0].Equals("-pfn", StringComparison.OrdinalIgnoreCase))
				{
					goto Cleanup;
				}

				var systemBinDir = string.Format("{0}System{0}Bin", Path.DirectorySeparatorChar);

				var currWorkDir = Directory.GetCurrentDirectory();

				// if current working directory invalid, bail out

				if (!currWorkDir.EndsWith(systemBinDir) || currWorkDir.Length <= systemBinDir.Length || !Directory.Exists(Constants.AdventuresDir.Replace('\\', Path.DirectorySeparatorChar)))
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
			IPluginGlobals pg = EamonMH.Game.Plugin.PluginContext.Globals;

			pg = EamonDD.Game.Plugin.PluginContext.Globals;

			pg = EamonRT.Game.Plugin.PluginContext.Globals;

			pg = ARuncibleCargo.Game.Plugin.PluginContext.Globals;

			pg = BeginnersForest.Game.Plugin.PluginContext.Globals;

			pg = StrongholdOfKahrDur.Game.Plugin.PluginContext.Globals;

			pg = TheBeginnersCave.Game.Plugin.PluginContext.Globals;

			pg = TheTempleOfNgurct.Game.Plugin.PluginContext.Globals;

			pg = TheTrainingGround.Game.Plugin.PluginContext.Globals;

			pg = WrenholdsSecretVigil.Game.Plugin.PluginContext.Globals;

			pg = LandOfTheMountainKing.Game.Plugin.PluginContext.Globals;
			
			pg = TheVileGrimoireOfJaldial.Game.Plugin.PluginContext.Globals;

			pg = RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext.Globals;

			pg = BeginnersCaveII.Game.Plugin.PluginContext.Globals;

			pg = AlternateBeginnersCave.Game.Plugin.PluginContext.Globals;

			pg = TheDeepCanyon.Game.Plugin.PluginContext.Globals;

			pg = SampleAdventure.Game.Plugin.PluginContext.Globals;
			
			pg = TheWayfarersInn.Game.Plugin.PluginContext.Globals;

			// Note: The Eamon CS datafile upgrade logic now auto-converts System.Private.CoreLib references
			// into mscorlib references; Xamarin.Forms appears to only work with the latter.
		}
	}
}