
// App.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
using Avalonia.Media;
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

			- change the BuildGuid to upgrade the binary .apk file and the .DAT datafiles in the filesystem (but not CHARACTERS.DAT or EAMONPM_SETTINGS.DAT)
		*/

		public static IDictionary<Type, ViewModelBase> ViewModelDictionary { get; set; }

		public static IDictionary<Type, Control> ViewDictionary { get; set; }

		public static Thread GameThread { get; set; }

		public static StringBuilder OutputBuf { get; set; }

		public static Mutex OutputBufMutex { get; set; }

		public static AutoResetEvent FinishInput { get; set; }

		public static Action<string> StartBrowserFunc { get; set; }

		public static Action KillProcessFunc { get; set; }

		public static Action ShowKeyboardFunc { get; set; }

		public static Action HideKeyboardFunc { get; set; }

		public static Action<bool> RefreshStatusFunc { get; set; }

		public static Func<char, char> InputModifyCharFunc { get; set; }

		public static Func<char, bool> InputValidCharFunc { get; set; }

		public static Func<char, bool> InputTermCharFunc { get; set; }

		public static string BuildGuid = "FDEB8D58-A1C0-44C7-983B-118BA6B0C015";

		public static string ProgramName { get; set; }

		public static string OrigBasePath { get; set; }

		public static string BasePath { get; set; }

		public static string WorkDir { get; set; }

		public static string[] NextArgs { get; set; }

		public static string InputEmptyVal { get; set; }

		public static long InputBufSize { get; set; }

		public static char[] InputBoxChars { get; set; }

		public static char InputFillChar { get; set; }

		public static char InputMaskChar { get; set; }

		public static bool InitializeSettings { get; set; }

		public static bool InputEmptyAllowed { get; set; }

		public static bool FinishInputSet { get; set; }

		public override void Initialize()
		{
			AvaloniaXamlLoader.Load(this);

			ViewModelDictionary = new ConcurrentDictionary<Type, ViewModelBase>();

			ViewDictionary = new ConcurrentDictionary<Type, Control>();

			OutputBuf = new StringBuilder();

			OutputBufMutex = new Mutex();

			FinishInput = new AutoResetEvent(false);

			PushEngine();

			gEngine.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

			gEngine.ResolvePortabilityClassMappings();
		}

		public override async void OnFrameworkInitializationCompleted()
		{
			var settingsView = GetView(typeof(SettingsView)) as SettingsView;

			var settingsViewModel = GetViewModel(typeof(SettingsViewModel)) as SettingsViewModel;

			if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
			{
				Window splashScreen = null;

				if (settingsViewModel.DisplaySplashScreen)
				{
					splashScreen = GetView(typeof(SplashScreen)) as Window;

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
				}

				var mainWindow = GetView(typeof(MainWindow)) as Window;

				var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

				mainViewModel.WindowWidth = settingsViewModel.WindowWidth;

				mainViewModel.WindowHeight = settingsViewModel.WindowHeight;

				var eamonCSView = GetView(typeof(EamonCSView)) as EamonCSView;

				mainViewModel.NavigateTo(eamonCSView, "Eamon CS", false);

				desktop.MainWindow = mainWindow;

				mainWindow.Show();

				if (settingsViewModel.DisplaySplashScreen)
				{
					splashScreen.Close();
				}
			}
			else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
			{
				var mainView = GetView(typeof(MainView));

				var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

				var eamonCSView = GetView(typeof(EamonCSView)) as EamonCSView;

				mainViewModel.NavigateTo(eamonCSView, "Eamon CS", false);

				singleViewPlatform.MainView = mainView;
			}

			settingsViewModel.SettingsChanged = false;

			base.OnFrameworkInitializationCompleted();

			var separator = Regex.Escape(gEngine.Path.DirectorySeparatorChar.ToString());

			var pattern = string.Empty;

			if (ProgramName == "EamonPM.Desktop")
			{
				pattern = string.Format(@"(?-i).*{0}Eamon-CS[^{0}]*{0}System{0}Bin{0}?$", separator);
			}
			else if (ProgramName == "EamonPM.Android")
			{
				pattern = string.Format(@"(?-i).*{0}EamonPM.Android[^{0}]*{0}files{0}System{0}Bin{0}?$", separator);
			}
			else
			{
				Debug.Assert(1 == 0);
			}

			if (string.IsNullOrWhiteSpace(OrigBasePath) || !Regex.IsMatch(OrigBasePath, pattern))
			{
				await ShowErrorMessage("Program startup failed.", 
					new Exception(string.IsNullOrWhiteSpace(OrigBasePath) ? 
						"This program has an unknown base path." : 
						"This program must be located in a valid Eamon CS repository."));

				KillProcess();
			}

			WorkDir = gEngine.Path.Combine(BasePath, "System", "Bin");

			gEngine.Directory.SetCurrentDirectory(WorkDir);
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

		public static Assembly LoadAssembly(string assemblyPath)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(assemblyPath));

			var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

			Debug.Assert(assembly != null);

			foreach (var dependency in assembly.GetReferencedAssemblies())
			{
				var dependencyPath = gEngine.Path.Combine(WorkDir, dependency.Name + ".dll");

				if (gEngine.File.Exists(dependencyPath))
				{
					LoadAssembly(dependencyPath);
				}
			}

			return assembly;
		}

		public static void ExecutePlugin(string[] args, bool enableStdio = true)
		{
			Debug.Assert(args != null);

			Debug.Assert(args.Length > 1);

			Debug.Assert(args[0].Equals("-pfn", StringComparison.OrdinalIgnoreCase));

			var pluginLauncherView = GetView(typeof(PluginLauncherView)) as PluginLauncherView;

			DispatcherUIThreadPost(() =>
			{
				pluginLauncherView.CommandList.Clear();

				pluginLauncherView.CommandListIndex = -1;
			});

			var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

			var pluginFileName = gEngine.Path.GetFileNameWithoutExtension(args[1]);

			var mainTitle =
				Array.Exists(args, arg => arg.Equals("-dgs", StringComparison.OrdinalIgnoreCase) || arg.Equals("EamonMH.dll", StringComparison.OrdinalIgnoreCase)) ? "EamonMH" :
				Array.Exists(args, arg => arg.Equals("-rge", StringComparison.OrdinalIgnoreCase)) ? "EamonDD" :
				Array.Exists(args, arg => arg.Equals("EamonRT.dll", StringComparison.OrdinalIgnoreCase)) ? "EamonRT" :
				pluginFileName;

			mainViewModel.MainTitleStack.Pop();

			mainViewModel.MainTitleStack.Push(mainTitle);

			mainViewModel.MainTitle = mainTitle;

			var plugin = LoadAssembly(gEngine.Path.Combine(WorkDir, args[1]));

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

				var systemBinDir = string.Format("{0}System{0}Bin", gEngine.Path.DirectorySeparatorChar);

				var currWorkDir = gEngine.Directory.GetCurrentDirectory();

				// If current working directory invalid, bail out

				if (!currWorkDir.EndsWith(systemBinDir) || currWorkDir.Length <= systemBinDir.Length)
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

			gEngine.Thread.Sleep(750);

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

			Debug.Assert(pluginLauncherViewModel.PluginScriptFile != null);

			GameThread.Start(pluginLauncherViewModel.PluginScriptFile.PluginArgs);
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

		public static void DispatcherUIThreadPost(Action actionFunc)
		{
			Debug.Assert(actionFunc != null);

			Dispatcher.UIThread.Post(actionFunc);
		}

		public static void ChangeTheme(string themeName)
		{
			var app = Application.Current;

			Debug.Assert(app != null);

			var resources = app.Resources;

			Debug.Assert(resources != null);

			var mainViewModel = GetViewModel(typeof(MainViewModel)) as MainViewModel;

			mainViewModel.IsBackArrowActive = themeName != "Default Light";

			mainViewModel.IsBackArrowDarkActive = themeName == "Default Light";

			app.RequestedThemeVariant = ThemeVariant.Light;

			switch (themeName)
			{
				case "Default Light":

					resources["Primary"] = Color.Parse("#D1D1D1"); // Light Gray
					resources["PrimaryDark"] = Color.Parse("#A3A3A3"); // Darker Gray
					resources["Accent"] = Color.Parse("#949494"); // Medium Gray
					resources["TitleTabItemForegroundColor"] = Color.Parse("#292929"); // Dark Gray
					resources["GeneralForegroundColor"] = Color.Parse("#1A1A1A"); // Dark Gray
					resources["LightBackgroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["DarkBackgroundColor"] = Color.Parse("#EDEDED"); // Light Gray
					resources["GeneralBackgroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Color.Parse("#6E6E6E"); // Medium Gray
					resources["LightTextColor"] = Color.Parse("#808080"); // Light Gray
					resources["RowDivider"] = Color.Parse("#D6D6D6"); // Light Gray

					break;

				case "Default Dark":

					app.RequestedThemeVariant = ThemeVariant.Dark;

					resources["Primary"] = Color.Parse("#424242"); // Dark Gray
					resources["PrimaryDark"] = Color.Parse("#303030"); // Darker Gray
					resources["Accent"] = Color.Parse("#949494"); // Medium Gray
					resources["TitleTabItemForegroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Color.Parse("#E0E0E0"); // Light Gray
					resources["LightBackgroundColor"] = Color.Parse("#303030"); // Very Dark Gray
					resources["DarkBackgroundColor"] = Color.Parse("#212121"); // Near Black
					resources["GeneralBackgroundColor"] = Color.Parse("#121212"); // Black
					resources["MediumGrayTextColor"] = Color.Parse("#B0BEC5"); // Light Blue-Gray
					resources["LightTextColor"] = Color.Parse("#CFD8DC"); // Soft Blue-Gray
					resources["RowDivider"] = Color.Parse("#394952"); // Dark Blue-Gray

					break;

				case "Element Air":

					resources["Primary"] = Color.Parse("#1970DB"); // Darker Rich Sky Blue
					resources["PrimaryDark"] = Color.Parse("#12488F"); // Darker Deep Sky Blue
					resources["Accent"] = Color.Parse("#63B4F5"); // Vibrant Light Blue
					resources["TitleTabItemForegroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Color.Parse("#000000"); // Black
					resources["LightBackgroundColor"] = Color.Parse("#E3F2FD"); // Very Light Sky Blue
					resources["DarkBackgroundColor"] = Color.Parse("#90CAF9"); // Light Muted Blue
					resources["GeneralBackgroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Color.Parse("#3F51B5"); // Medium Sky Blue
					resources["LightTextColor"] = Color.Parse("#7986CB"); // Light Muted Blue
					resources["RowDivider"] = Color.Parse("#B3E5FC"); // Very Light Blue

					break;

				case "Element Earth":

					resources["Primary"] = Color.Parse("#217A3A"); // Deep Forest Green
					resources["PrimaryDark"] = Color.Parse("#165225"); // Very Dark Forest Green
					resources["Accent"] = Color.Parse("#E0A526"); // Goldenrod
					resources["TitleTabItemForegroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Color.Parse("#000000"); // Black
					resources["LightBackgroundColor"] = Color.Parse("#E8F5E9"); // Light Leaf Green
					resources["DarkBackgroundColor"] = Color.Parse("#4E6B58"); // Muted Olive Green
					resources["GeneralBackgroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Color.Parse("#2E3B33"); // Dark Olive Green
					resources["LightTextColor"] = Color.Parse("#73977B"); // Light Sage Green
					resources["RowDivider"] = Color.Parse("#C5E3C6"); // Very Light Mint Green

					break;

				case "Element Fire":

					resources["Primary"] = Color.Parse("#AB401A"); // Rich Ember Red
					resources["PrimaryDark"] = Color.Parse("#872B00"); // Deep Dark Red
					resources["Accent"] = Color.Parse("#E87F00"); // Dark Orange
					resources["TitleTabItemForegroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Color.Parse("#000000"); // Black
					resources["LightBackgroundColor"] = Color.Parse("#F7EDD9"); // Very Light Orange
					resources["DarkBackgroundColor"] = Color.Parse("#F7AB8D"); // Light Muted Red
					resources["GeneralBackgroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Color.Parse("#634023"); // Muted Warm Red Brown
					resources["LightTextColor"] = Color.Parse("#9E5329"); // Soft Light Red Brown
					resources["RowDivider"] = Color.Parse("#F5CBBC"); // Very Light Orangish Red

					break;

				case "Element Water":

					resources["Primary"] = Color.Parse("#0D47A1"); // Deep Blue
					resources["PrimaryDark"] = Color.Parse("#002171"); // Darker Deep Blue
					resources["Accent"] = Color.Parse("#00B4CC"); // Aqua Blue
					resources["TitleTabItemForegroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["GeneralForegroundColor"] = Color.Parse("#000000"); // Black
					resources["LightBackgroundColor"] = Color.Parse("#E0F7FA"); // Very Light Aqua
					resources["DarkBackgroundColor"] = Color.Parse("#B0BEC5"); // Light Grayish Blue
					resources["GeneralBackgroundColor"] = Color.Parse("#FFFFFF"); // White
					resources["MediumGrayTextColor"] = Color.Parse("#2C3B44"); // Darker Medium Grayish Blue
					resources["LightTextColor"] = Color.Parse("#71828F"); // Darker Light Grayish Blue
					resources["RowDivider"] = Color.Parse("#CCD5D9"); // Very Light Grayish Blue

					break;

				default:

					Debug.Assert(1 == 0);

					break;
			}

			RefreshStatusFunc?.Invoke(themeName != "Default Dark");
		}

		public static void ResetListBox(ListBox listBox)
		{
			Debug.Assert(listBox != null);

			listBox.SelectedItem = null;

			if (listBox.Items.Count > 0)
			{
				listBox.ScrollIntoView(listBox.Items[0]);
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

						if (nlIndex > startIndex)
						{
							startIndex = nlIndex + 1;
						}

						OutputBuf.SetFormat("{0}", OutputBuf.ToString().Substring(startIndex));
					}
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// Do something
					}
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

					var startIndex = OutputBuf.Length - pluginLauncherViewModel.OutputWindowMaxSize;

					if (startIndex > 0)
					{
						var nlIndex = OutputBuf.IndexOf(Environment.NewLine[Environment.NewLine.Length - 1], startIndex);

						if (nlIndex > startIndex)
						{
							startIndex = nlIndex + 1;
						}
					}
					else
					{
						startIndex = 0;
					}

					pluginLauncherViewModel.OutputText = OutputBuf.ToString().Substring(startIndex);
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// Do something
					}
				}
				finally
				{
					OutputBufMutex.ReleaseMutex();
				}
			});
		}

		public static Control GetView(Type type)
		{
			Debug.Assert(type != null);

			Control result = null;

			if (!ViewDictionary.TryGetValue(type, out result))
			{
				result = (Control)Activator.CreateInstance(type);

				ViewDictionary[type] = result;
			}

			return result;
		}

		public static ViewModelBase GetViewModel(Type type)
		{
			Debug.Assert(type != null);

			ViewModelBase result = null;

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

		public static bool PluginExists(string pluginFileName)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(pluginFileName));

			var pluginPath = gEngine.Path.Combine(BasePath, "System", "Bin", pluginFileName);

			var result = gEngine.File.Exists(pluginPath);

			return result;
		}

		public static async Task ShowErrorMessage(string message, Exception ex)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(message));

			Debug.Assert(ex != null);

			await Dispatcher.UIThread.InvokeAsync(async () =>
			{
				var errorMessage = string.Format("{0}{1}{1}Error Type: {2}{1}{1}Error Message: {3}", message, Environment.NewLine, ex.GetType().Name, ex.Message);

				if (ex.InnerException != null)
				{
					errorMessage += string.Format("{0}{0}Inner Error Type: {1}{0}{0}Inner Error Message: {2}", Environment.NewLine, ex.InnerException.GetType().Name, ex.InnerException.Message);
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
