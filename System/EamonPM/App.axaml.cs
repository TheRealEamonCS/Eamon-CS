
// App.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using Avalonia.Threading;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using Eamon.Framework.Portability;
using Eamon.Game.Extensions;
using EamonPM.Game.Views;
using EamonPM.Game.ViewModels;
using static Eamon.Game.Plugin.ContextStack;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM
{
	public partial class App : Application
	{
		/*
			- leave the BuildGuid alone to upgrade only the binary .apk file

			- change the BuildGuid to upgrade the binary .apk file and the .DAT datafiles in the filesystem (but not CHARACTERS.DAT)
		*/

		public static string BuildGuid = "4C9208E5-07FE-4BA7-90A2-AE05060D2435";

		public static ConcurrentDictionary<Type, ViewModelBase> ViewModelDictionary { get; set; }

		public static ConcurrentDictionary<Type, Control> ViewDictionary { get; set; }

		public static string ProgramName { get; set; }

		public static string BasePath { get; set; }

		public static Action KillProcessFunc { get; set; }

		public static Action<string> StartBrowserFunc { get; set; }

		public static Action ShowKeyboardFunc { get; set; }

		public static Action HideKeyboardFunc { get; set; }

		public static bool InitializeSettings { get; set; }



		public static Thread GameThread { get; set; }

		public static string WorkDir { get; set; }

		public static string[] NextArgs { get; set; }

		public static StringBuilder OutputBuf { get; set; }

		public static Mutex OutputBufMutex { get; set; }

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




		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);

			ViewModelDictionary = new ConcurrentDictionary<Type, ViewModelBase>();

			ViewDictionary = new ConcurrentDictionary<Type, Control>();

			OutputBuf = new StringBuilder();

			OutputBufMutex = new Mutex();

			FinishInput = new AutoResetEvent(false);

			WorkDir = Path.Combine(BasePath, "System", "Bin");

			PushEngine();

			gEngine.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			gEngine.ResolvePortabilityClassMappings();

			Directory.SetCurrentDirectory(WorkDir);
		}

		public override async void OnFrameworkInitializationCompleted()
		{
			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				var settingsView = GetView(typeof(SettingsView)) as SettingsView;

				var settingsViewModel = GetViewModel(typeof(SettingsViewModel)) as SettingsViewModel;

				var splashScreen = GetView(typeof(SplashScreen)) as Window;

				desktop.MainWindow = splashScreen;

				splashScreen.Show();

				try
				{
					await Task.Delay(3000);
				}
				catch (Exception)
				{
					splashScreen.Close();
					return;
				}


				var mainWindow = GetView(typeof(MainWindow)) as Window;

				var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

				mainViewModel.WindowWidth = settingsViewModel.WindowWidth;

				mainViewModel.WindowHeight = settingsViewModel.WindowHeight;

				var eamonCSView = GetView(typeof(EamonCSView)) as EamonCSView;

				mainViewModel.NavigateTo(eamonCSView, "Eamon CS", false);

				desktop.MainWindow = mainWindow;

				mainWindow.Show();

				splashScreen.Close();

				settingsViewModel.SettingsChanged = false;
			}
			else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
			{
				var settingsView = GetView(typeof(SettingsView)) as SettingsView;

				var settingsViewModel = GetViewModel(typeof(SettingsViewModel)) as SettingsViewModel;

				var mainView = GetView(typeof(MainView));

				var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

				var eamonCSView = GetView(typeof(EamonCSView)) as EamonCSView;

				mainViewModel.NavigateTo(eamonCSView, "Eamon CS", false);

				singleViewPlatform.MainView = mainView;

				settingsViewModel.SettingsChanged = false;
			}

			base.OnFrameworkInitializationCompleted();
		}

		public static void ExecutePlugin(string[] args, bool enableStdio = true)
		{
			Debug.Assert(args != null);

			Debug.Assert(args.Length > 1);

			Debug.Assert(args[0].Equals("-pfn", StringComparison.OrdinalIgnoreCase));

			var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

			var pluginFileName = Path.GetFileNameWithoutExtension(args[1]);

			var mainTitle =
				args.Contains("-dgs") || args.Contains("EamonMH.dll") ? "EamonMH" :
				args.Contains("-rge") ? "EamonDD" :
				args.Contains("EamonRT.dll") ? "EamonRT" :
				pluginFileName;

			mainViewModel.MainTitleStack.Pop();

			mainViewModel.MainTitleStack.Push(mainTitle);

			mainViewModel.MainTitle = mainTitle;

			var plugin = LoadAssembly(Path.Combine(WorkDir, args[1]));

			Debug.Assert(plugin != null);

			var typeName = string.Format("{0}.Program", pluginFileName);

			var type = plugin.GetType(typeName);

			Debug.Assert(type != null);

			var program = (IProgram)Activator.CreateInstance(type);

			Debug.Assert(program != null);

			program.EnableStdio = enableStdio;

			program.LineWrapUserInput = true;

			program.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			program.Main(args.Skip(2).ToArray());
		}

		public static Assembly LoadAssembly(string assemblyPath)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(assemblyPath));

			var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

			Debug.Assert(assembly != null);

			foreach (var dependency in assembly.GetReferencedAssemblies())
			{
				var dependencyPath = Path.Combine(WorkDir, dependency.Name + ".dll");

				if (File.Exists(dependencyPath))
				{
					LoadAssembly(dependencyPath);
				}
			}

			return assembly;
		}

		public static void LoadPortabilityClassMappings(IDictionary<Type, Type> classMappings)
		{
			Debug.Assert(classMappings != null);

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

		public static async void PluginLoop(object obj)
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
				// Do nothing
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

				// If current working directory invalid, bail out

				if (!currWorkDir.EndsWith(systemBinDir) || currWorkDir.Length <= systemBinDir.Length || !Directory.Exists(gEngine.AdventuresDir.Replace('\\', Path.DirectorySeparatorChar)))
				{
					goto Cleanup;
				}

				try
				{
					ExecutePlugin(args, true);
				}
				catch (Exception ex)
				{
					await ShowErrorMessage("Execute Plugin operation failed.", ex);

					goto Cleanup;
				}

				args = NextArgs;

				NextArgs = null;
			}

		Cleanup:

			var pluginLauncherView = GetView(typeof(PluginLauncherView)) as PluginLauncherView;

			DispatcherUIThreadPost(() =>
			{
				InputValidCharFunc = (ch) => false;

				pluginLauncherView.InputTextBoxLoseFocus();
			});

			Thread.Sleep(750);

			DispatcherUIThreadPost(() =>
			{
				KillProcess();
			});
		}

		public static void StartGameThread()
		{
			var pluginLauncherViewModel = GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			var threadStart = new ParameterizedThreadStart(PluginLoop);

			GameThread = new Thread(threadStart) 
			{ 
				IsBackground = true 
			};

			GameThread.Start(pluginLauncherViewModel.BatchFile.PluginArgs);
		}

		public static bool ShouldStopApplicationOnBackPressed()
		{
			var mainView = GetView(typeof(MainView)) as MainView;

			var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

			if (mainView.MainTabControl.SelectedIndex == 0 && mainViewModel.ViewStack.Count > 1 && mainViewModel.ViewStack.Count < 4)
			{
				mainView.BackButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

				return false;
			}
			else
			{
				return true;
			}
		}

		public static void KillProcess()
		{
			if (GameThread != null)
			{
				GameThread.Join(0);

				if (GameThread.IsAlive)
				{
					//GameThread.Abort();
				}
			}

			PopEngine();

			KillProcessFunc?.Invoke();
		}

		public static void DispatcherUIThreadPost(Action actionFunc)
		{
			Debug.Assert(actionFunc != null);

			Dispatcher.UIThread.Post(actionFunc);
		}

		public static void ResetListBox(ListBox listBox)
		{
			Debug.Assert(listBox != null);

			listBox.SelectedItem = null;

			listBox.ScrollIntoView(listBox.Items[0]);
		}

		public static void ChangeTheme(string themeName)
		{
			var app = Application.Current;
			if (app == null)
				return;

			var resources = app.Resources;

			var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

			mainViewModel.IsBackArrowActive = themeName != "Default Light";

			mainViewModel.IsBackArrowDarkActive = themeName == "Default Light";

			switch (themeName)
			{
				case "Default Light":

					app.RequestedThemeVariant = ThemeVariant.Light;

					resources["Primary"] = Avalonia.Media.Color.Parse("#DCDCDC"); // Light Gray
					resources["PrimaryDark"] = Avalonia.Media.Color.Parse("#ABABAB"); // Darker Gray
					resources["Accent"] = Avalonia.Media.Color.Parse("#9B9B9B"); // Medium Gray
					resources["TitleTabItemForegroundColor"] = Avalonia.Media.Color.Parse("#313131"); // Dark Gray
					resources["GeneralForegroundColor"] = Avalonia.Media.Color.Parse("#212121"); // Dark Gray
					resources["LightBackgroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["DarkBackgroundColor"] = Avalonia.Media.Color.Parse("#F5F5F5"); // Light Gray
					resources["GeneralBackgroundColor"] = Avalonia.Media.Color.Parse("#FAFAFA"); // Very Light Gray
					resources["MediumGrayTextColor"] = Avalonia.Media.Color.Parse("#757575"); // Medium Gray
					resources["LightTextColor"] = Avalonia.Media.Color.Parse("#878787"); // Light Gray
					resources["RowDivider"] = Avalonia.Media.Color.Parse("#D6D6D6"); // Light Gray

					break;

				case "Default Dark":

					app.RequestedThemeVariant = ThemeVariant.Dark;

					resources["Primary"] = Avalonia.Media.Color.Parse("#424242"); // Dark Gray
					resources["PrimaryDark"] = Avalonia.Media.Color.Parse("#303030"); // Darker Gray
					resources["Accent"] = Avalonia.Media.Color.Parse("#6F6F6F"); // Lighter Gray
					resources["TitleTabItemForegroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Avalonia.Media.Color.Parse("#E0E0E0"); // Light Gray
					resources["LightBackgroundColor"] = Avalonia.Media.Color.Parse("#303030"); // Very Dark Gray
					resources["DarkBackgroundColor"] = Avalonia.Media.Color.Parse("#212121"); // Near Black
					resources["GeneralBackgroundColor"] = Avalonia.Media.Color.Parse("#121212"); // Black
					resources["MediumGrayTextColor"] = Avalonia.Media.Color.Parse("#B0BEC5"); // Light Blue-Gray
					resources["LightTextColor"] = Avalonia.Media.Color.Parse("#CFD8DC"); // Soft Blue-Gray
					resources["RowDivider"] = Avalonia.Media.Color.Parse("#37474F"); // Dark Blue-Gray

					break;

				case "Element Air":

					app.RequestedThemeVariant = ThemeVariant.Light;

					resources["Primary"] = Avalonia.Media.Color.Parse("#1970DB"); // Darker Rich Sky Blue
					resources["PrimaryDark"] = Avalonia.Media.Color.Parse("#12488F"); // Darker Deep Sky Blue
					resources["Accent"] = Avalonia.Media.Color.Parse("#64B5F6"); // Vibrant Light Blue
					resources["TitleTabItemForegroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Avalonia.Media.Color.Parse("#000000"); // Black
					resources["LightBackgroundColor"] = Avalonia.Media.Color.Parse("#E3F2FD"); // Very Light Sky Blue
					resources["DarkBackgroundColor"] = Avalonia.Media.Color.Parse("#90CAF9"); // Light Muted Blue
					resources["GeneralBackgroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Avalonia.Media.Color.Parse("#3F51B5"); // Medium Sky Blue
					resources["LightTextColor"] = Avalonia.Media.Color.Parse("#7986CB"); // Light Muted Blue
					resources["RowDivider"] = Avalonia.Media.Color.Parse("#B3E5FC"); // Very Light Blue

					break;
				
				case "Element Earth":

					app.RequestedThemeVariant = ThemeVariant.Light;

					resources["Primary"] = Avalonia.Media.Color.Parse("#217A3A"); // Deep Forest Green
					resources["PrimaryDark"] = Avalonia.Media.Color.Parse("#165225"); // Very Dark Forest Green
					resources["Accent"] = Avalonia.Media.Color.Parse("#D09358"); // Honey Bronze
					resources["TitleTabItemForegroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Avalonia.Media.Color.Parse("#000000"); // Black
					resources["LightBackgroundColor"] = Avalonia.Media.Color.Parse("#E8F5E9"); // Light Leaf Green
					resources["DarkBackgroundColor"] = Avalonia.Media.Color.Parse("#4E6B58"); // Muted Olive Green
					resources["GeneralBackgroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Avalonia.Media.Color.Parse("#2E3B33"); // Dark Olive Green
					resources["LightTextColor"] = Avalonia.Media.Color.Parse("#73877B"); // Light Sage Green
					resources["RowDivider"] = Avalonia.Media.Color.Parse("#C8E6C9"); // Very Light Mint Green

					break;

				case "Element Fire":

					app.RequestedThemeVariant = ThemeVariant.Light;

					resources["Primary"] = Avalonia.Media.Color.Parse("#B33B1B"); // Rich Ember Red
					resources["PrimaryDark"] = Avalonia.Media.Color.Parse("#8F2500"); // Deep Dark Red
					resources["Accent"] = Avalonia.Media.Color.Parse("#F57C00"); // Dark Orange
					resources["TitleTabItemForegroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Avalonia.Media.Color.Parse("#000000"); // Black
					resources["LightBackgroundColor"] = Avalonia.Media.Color.Parse("#FFF3E0"); // Very Light Orange
					resources["DarkBackgroundColor"] = Avalonia.Media.Color.Parse("#FFAB91"); // Light Muted Red
					resources["GeneralBackgroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Avalonia.Media.Color.Parse("#6B4226"); // Muted Warm Red Brown
					resources["LightTextColor"] = Avalonia.Media.Color.Parse("#A0522D"); // Soft Light Red Brown
					resources["RowDivider"] = Avalonia.Media.Color.Parse("#FFD1C4"); // Very Light Orangish Red

					break;

				case "Element Water":

					app.RequestedThemeVariant = ThemeVariant.Light;

					resources["Primary"] = Avalonia.Media.Color.Parse("#0D47A1"); // Deep Blue
					resources["PrimaryDark"] = Avalonia.Media.Color.Parse("#002171"); // Darker Deep Blue
					resources["Accent"] = Avalonia.Media.Color.Parse("#00BCD4"); // Aqua Blue
					resources["TitleTabItemForegroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Avalonia.Media.Color.Parse("#000000"); // Black
					resources["LightBackgroundColor"] = Avalonia.Media.Color.Parse("#E0F7FA"); // Very Light Aqua
					resources["DarkBackgroundColor"] = Avalonia.Media.Color.Parse("#B0BEC5"); // Light Grayish Blue
					resources["GeneralBackgroundColor"] = Avalonia.Media.Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Avalonia.Media.Color.Parse("#2C3B44"); // Darker Medium Grayish Blue
					resources["LightTextColor"] = Avalonia.Media.Color.Parse("#788997"); // Darker Light Grayish Blue
					resources["RowDivider"] = Avalonia.Media.Color.Parse("#CFD8DC"); // Very Light Grayish Blue

					break;

				default:

					Debug.Assert(1 == 0);

					break;
			}
		}

		public static void EnforceOutputBufMaxSize()
		{
			DispatcherUIThreadPost(() =>
			{
				var pluginLauncherViewModel = GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

				try
				{
					OutputBufMutex.WaitOne();

					if (OutputBuf.Length > pluginLauncherViewModel.OutputBufMaxSize)
					{
						var startIndex = OutputBuf.Length - pluginLauncherViewModel.OutputBufMaxSize;

						var nlIndex = OutputBuf.IndexOf(Environment.NewLine[Environment.NewLine.Length - 1], startIndex);

						if (nlIndex >= 0)
						{
							startIndex = nlIndex + 1;
						}

						OutputBuf.SetFormat("{0}", OutputBuf.ToString().Substring(startIndex));
					}
				}
				catch (Exception ex)
				{
					// Do something
				}
				finally
				{
					OutputBufMutex.ReleaseMutex();
				}
			});
		}

		public static void RefreshOutputWindowText()
		{
			DispatcherUIThreadPost(() =>
			{
				var pluginLauncherViewModel = GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

				try
				{
					OutputBufMutex.WaitOne();

					var startIndex = Math.Max(OutputBuf.Length - pluginLauncherViewModel.OutputWindowMaxSize, 0);

					if (startIndex > 0)
					{
						var nlIndex = OutputBuf.IndexOf(Environment.NewLine[Environment.NewLine.Length - 1], startIndex);

						if (nlIndex >= 0)
						{
							startIndex = nlIndex + 1;
						}
					}

					var length = Math.Min(OutputBuf.Length - startIndex, pluginLauncherViewModel.OutputWindowMaxSize);

					pluginLauncherViewModel.OutputText = OutputBuf.ToString(startIndex, length);
				}
				catch (Exception ex)
				{
					// Do something
				}
				finally
				{
					OutputBufMutex.ReleaseMutex();
				}
			});
		}

		public static ViewModelBase GetViewModel(Type type)
		{
			Debug.Assert(type != null);

			ViewModelBase result;

			if (!ViewModelDictionary.TryGetValue(type, out result))
			{
				if (type == typeof(SettingsViewModel))
				{
					var fileName = gEngine.Path.Combine(BasePath, "System", "Bin", "EAMONPM_SETTINGS.DAT");

					try
					{
						result = gEngine.SharpSerializer.Deserialize<SettingsViewModel>(fileName);
					}
					catch (Exception)
					{
						result = (ViewModelBase)Activator.CreateInstance(type);

						try
						{
							gEngine.SharpSerializer.Serialize(result, fileName);
						}
						catch (Exception)
						{
							// Do nothing
						}
					}
				}
				else
				{
					result = (ViewModelBase)Activator.CreateInstance(type);
				}

				ViewModelDictionary[type] = result;
			}

			return result;
		}

		public static Control GetView(Type type)
		{
			Debug.Assert(type != null);

			Control result;

			if (!ViewDictionary.TryGetValue(type, out result))
			{
				result = (Control)Activator.CreateInstance(type);

				ViewDictionary[type] = result;
			}

			return result;
		}

		public static string[] GetAdventureDirs()
		{
			var fullDirs = gEngine.Directory.GetDirectories(gEngine.Path.Combine(BasePath, "Adventures"));

			var dirList = new List<string>();

			foreach (var fullDir in fullDirs)
			{
				dirList.Add(gEngine.Path.GetFileNameWithoutExtension(fullDir));
			}

			return dirList.ToArray();
		}

		public static bool PluginExists(string pluginFileName)
		{
			var pluginPath = gEngine.Path.Combine(BasePath, "System", "Bin", pluginFileName);

			var result = gEngine.File.Exists(pluginPath);

			return result;
		}

		public static async Task ShowErrorMessage(string message, Exception ex)
		{
			Debug.Assert(message != null);

			Debug.Assert(ex != null);

			await Dispatcher.UIThread.InvokeAsync(async () =>
			{
				var errorMessage = $"{message}\n\nError Type: {ex.GetType().Name}\n\nError Message: {ex.Message}";

				if (ex.InnerException != null)
				{
					errorMessage += $"\n\nInner Error Type: {ex.InnerException.GetType().Name}\n\nInner Error Message: {ex.InnerException.Message}";
				}

				var messageBoxCustom = MessageBoxManager.GetMessageBoxCustom(
							new MessageBoxCustomParams
							{
								ButtonDefinitions = new List<ButtonDefinition>
								{
									new ButtonDefinition { Name = "OK" }
								},
								ContentTitle = ProgramName,
								ContentMessage = errorMessage,
								Icon = Icon.Error,
								WindowStartupLocation = WindowStartupLocation.CenterOwner,
								CanResize = false,
								MinWidth = 400,
								MaxWidth = 600,
								SizeToContent = SizeToContent.WidthAndHeight,
								ShowInCenter = true,
								Topmost = true,
							});

				if (ProgramName == "EamonPM.Desktop")
				{
					var mainWindow = GetView(typeof(MainWindow)) as Window;

					await messageBoxCustom.ShowWindowDialogAsync(mainWindow);
				}
				else if (ProgramName == "EamonPM.Android")
				{
					var mainView = GetView(typeof(MainView)) as ContentControl;

					await messageBoxCustom.ShowAsPopupAsync(mainView);
				}
				else
				{
					Debug.Assert(1 == 0);
				}
			});
		}
	}
}
