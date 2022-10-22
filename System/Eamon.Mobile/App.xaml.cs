
// App.xaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Eamon.Mobile.Models;
using Eamon.Mobile.Views;
using Eamon.Mobile.ViewModels;
using static Eamon.Game.Plugin.Globals;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Eamon.Mobile
{
	public partial class App : Application
	{
		public static RetCode Rc { get; set; }

		public static string BasePath { get; set; }

		public static string[] NextArgs { get; set; }

		public static BatchFile BatchFile { get; set; }

		public static StringBuilder OutputBuf { get; set; }

		public static Mutex OutputBufMutex { get; set; }

		public static long OutputBufStartIndex { get; set; }

		public static long InputBufSize { get; set; }
		
		public static char[] InputBoxChars { get; set; }

		public static char InputFillChar { get; set; }
		
		public static char InputMaskChar { get; set; }
		
		public static bool InputEmptyAllowed { get; set; }
		
		public static bool FinishInputSet { get; set; }

		public static string InputEmptyVal { get; set; }
		
		public static Func<char, char> InputModifyCharFunc { get; set; }
		
		public static Func<char, bool> InputValidCharFunc { get; set; }
		
		public static Func<char, bool> InputTermCharFunc { get; set; }

		public static AutoResetEvent FinishInput { get; set; }

		public static Func<string[]> GetAdventureDirs { get; set; }

		public static Func<string, bool> PluginExists { get; set; }

		public static Action<string[], bool> ExecutePlugin { get; set; }

		public static Action<object> PluginLoop { get; set; }

		public static Action StartGameThread { get; set; }

		public static PluginLauncherPage PluginLauncherPage { get; set; }

		public static PluginLauncherViewModel PluginLauncherViewModel { get; set; }

		public static SettingsPage SettingsPage { get; set; }

		public static SettingsViewModel SettingsViewModel { get; set; }

		public App()
		{
			InitializeComponent();

			OutputBuf = new StringBuilder();

			OutputBufMutex = new Mutex();

			OutputBufStartIndex = -1;

			FinishInput = new AutoResetEvent(false);

			SetMainPage();
		}

		public static bool ShouldStopApplicationOnBackPressed()
		{
			var tabbedPage = Current.MainPage as TabbedPage;

			Debug.Assert(tabbedPage != null);

			return tabbedPage.CurrentPage != tabbedPage.Children[0] || tabbedPage.CurrentPage.Navigation.NavigationStack.Count == 1;
		}

		public static void SetMainPage()
		{
			SettingsViewModel settingsViewModel;

			var path = Path.Combine(BasePath, Path.Combine("System", "Bin"));

			var fileName = Path.Combine(path, "MOBILE_SETTINGS.DAT");

			try
			{
				settingsViewModel = gEngine.SharpSerializer.Deserialize<SettingsViewModel>(fileName);
			}
			catch (Exception)
			{
				settingsViewModel = null;
			}

			if (settingsViewModel == null)
			{
				settingsViewModel = new SettingsViewModel();

				Debug.Assert(settingsViewModel != null);

				gEngine.SharpSerializer.Serialize(settingsViewModel, fileName);
			}

			Current.MainPage = new TabbedPage
			{
				Children =
				{
					new NavigationPage(new EamonCSPage())
					{
						Title = "Gameplay",
						Icon = Device.OnPlatform("tab_feed.png",null,null)
					},
					new NavigationPage(new SettingsPage(settingsViewModel))
					{
						Title = "Settings",
						Icon = Device.OnPlatform("tab_feed.png",null,null)
					},
					new NavigationPage(new AboutPage())
					{
						Title = "About",
						Icon = Device.OnPlatform("tab_about.png",null,null)
					},
				}
			};
		}
	}
}
