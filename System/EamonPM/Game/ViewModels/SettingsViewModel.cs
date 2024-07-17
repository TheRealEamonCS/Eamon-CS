
// SettingsViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Media;
using ReactiveUI;
using Polenter.Serialization;
using EamonPM.Game.Helpers;

namespace EamonPM.Game.ViewModels
{
	public class SettingsViewModel : ViewModelBase
	{
		public string _appTheme;

		public string _foregroundColor;

		public string _backgroundColor;

		public string _fontFamily;

		public string _fontWeight;

		public double _fontSize;

		public int _outputBufMaxSize;

		public int _outputWindowMaxSize;

		public bool _keepKeyboardVisible;

		public bool _settingsChanged;

		public double _windowWidth;

		public double _windowHeight;

		[ExcludeFromSerialization]
		public string[] MonospaceFonts { get; set; }

		[ExcludeFromSerialization]
		public List<string> AppThemeList { get; set; }

		[ExcludeFromSerialization]
		public List<FontFamily> FontFamilyList { get; set; }

		[ExcludeFromSerialization]
		public List<FontWeight> FontWeightList { get; set; }

		[ExcludeFromSerialization]
		public List<int> FontSizeList { get; set; }

		[ExcludeFromSerialization]
		public List<int> OutputBufMaxSizeList { get; set; }

		[ExcludeFromSerialization]
		public ObservableCollection<int> OutputWindowMaxSizeList { get; set; }

		public string AppTheme
		{
			get
			{
				return _appTheme;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _appTheme, value);
			}
		}

		public string ForegroundColor
		{
			get
			{
				return _foregroundColor;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _foregroundColor, value);
			}
		}

		public string BackgroundColor
		{
			get
			{
				return _backgroundColor;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _backgroundColor, value);
			}
		}

		public string FontFamily
		{
			get
			{
				return _fontFamily;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _fontFamily, value);
			}
		}

		public string FontWeight
		{
			get
			{
				return _fontWeight;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _fontWeight, value);
			}
		}

		public double FontSize
		{
			get
			{
				return _fontSize;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _fontSize, value);
			}
		}

		public int OutputBufMaxSize
		{
			get
			{
				return _outputBufMaxSize;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _outputBufMaxSize, value);
			}
		}

		public int OutputWindowMaxSize
		{
			get
			{
				return _outputWindowMaxSize;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _outputWindowMaxSize, value);
			}
		}

		public bool KeepKeyboardVisible
		{
			get
			{
				return _keepKeyboardVisible;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _keepKeyboardVisible, value);
			}
		}

		[ExcludeFromSerialization]
		public bool SettingsChanged
		{
			get
			{
				return _settingsChanged;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _settingsChanged, value);
			}
		}

		public double WindowWidth
		{
			get
			{
				return _windowWidth;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _windowWidth, value);
			}
		}

		public double WindowHeight
		{
			get
			{
				return _windowHeight;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _windowHeight, value);
			}
		}

		public void AppThemeComboBoxSelectionChanged(string appTheme)
		{
			AppTheme = appTheme;

			App.ChangeTheme(appTheme);

			SettingsChanged = true;
		}

		public void ForegroundColorPickerColorChanged(Color color)
		{
			ForegroundColor = ColorHelper.ToHexString(color);

			SettingsChanged = true;
		}

		public void BackgroundColorPickerColorChanged(Color color)
		{
			BackgroundColor = ColorHelper.ToHexString(color);

			SettingsChanged = true;
		}

		public void FontFamilyComboBoxSelectionChanged(string fontFamily)
		{
			FontFamily = fontFamily;

			SettingsChanged = true;
		}

		public void FontWeightComboBoxSelectionChanged(string fontWeight)
		{
			FontWeight = fontWeight;

			SettingsChanged = true;
		}

		public void FontSizeComboBoxSelectionChanged(double fontSize)
		{
			FontSize = fontSize;

			SettingsChanged = true;
		}

		public void OutputBufMaxSizeComboBoxSelectionChanged(int outputBufMaxSize)
		{
			OutputBufMaxSize = outputBufMaxSize;

			SettingsChanged = true;
		}

		public void OutputWindowMaxSizeComboBoxSelectionChanged(int outputWindowMaxSize)
		{
			OutputWindowMaxSize = outputWindowMaxSize;

			SettingsChanged = true;
		}

		public void OutputWindowMaxSizeComboBoxSync()
		{
			OutputWindowMaxSizeList.Clear();

			for (var i = 0; i < OutputBufMaxSizeList.Count; i++)
			{
				if (OutputBufMaxSize >= OutputBufMaxSizeList[i])
				{
					OutputWindowMaxSizeList.Add(OutputBufMaxSizeList[i]);
				}
				else
				{
					break;
				}
			}
		}

		public void KeepKeyboardVisibleToggleSwitchChanged(bool keepKeyboardVisible)
		{
			KeepKeyboardVisible = keepKeyboardVisible;

			SettingsChanged = true;
		}

		public void EamonPMMainWindowSizeChanged(double windowWidth, double windowHeight)
		{
			WindowWidth = windowWidth;

			WindowHeight = windowHeight;

			SettingsChanged = true;
		}

		public bool IsMonospaceFont(string fontFamily)
		{
			return MonospaceFonts.FirstOrDefault(ff => ff.Equals(fontFamily, StringComparison.OrdinalIgnoreCase)) != null;
		}

		public SettingsViewModel()
		{
			MonospaceFonts = new[]
			{
				// Windows

				"Ms Gothic", "Ms Pgothic", "Ms Ui Gothic", "Nsimsun", "Simsun", "Simsun-Extb",

				// Linux

				"Nimbus Mono Ps", "Noto Sans Mono", "Noto Sans Mono Cjk Hk", "Noto Sans Mono Cjk Jp", "Noto Sans Mono Cjk Kr", "Noto Sans Mono Cjk Sc", "Noto Sans Mono Cjk Tc",
				"Tlwg Mono", "Tlwg Typewriter", "Tlwg Typist", "Tlwg Typo", 

				// MacOS

				"Andale Mono", 

				// Android

				"Monospace",

				// General

				"Anonymous Pro", "Bitstream Vera Sans Mono", "Cascadia Code", "Cascadia Mono", "Century Schoolbook Monospace", "Comic Mono", "Computer Modern Mono/Typewriter",
				"Consolas", "Courier", "Courier New", "Cousine", "Dejavu Sans Mono", "Droid Sans Mono", "Envy Code R", "Everson Mono", "Fantasque Sans", "Fira Code", "Fira Mono",
				"Fixed", "Fixedsys", "Go Mono", "Hack", "Hyperfont", "Ibm Mda", "Ibm Plex Mono", "Inconsolata", "Input", "Iosevka", "Jetbrains Mono", "Juliamono", "Letter Gothic",
				"Liberation Mono", "Lucida Console", "Menlo", "Monaco", "Monofur", "Monospace (Unicode)", "Nimbus Mono L", "Nk57 Monospace", "Noto Mono", "Ocr-A", "Ocr-B",
				"Operator Mono", "Overpass Mono", "Oxygen Mono", "Pragmatapro", "Prestige Elite", "Profont", "Pt Mono", "Recursive Mono", "Roboto Mono", "Sf Mono", 
				"Source Code Pro", "Spleen", "Terminus", "Tex Gyre Cursor", "Ubuntu Mono", "Victor Mono", "Wumpus Mono"
			};

			AppThemeList = new List<string> { "Air", "Earth", "Fire", "Water" };

			FontFamilyList = FontManager.Current.SystemFonts.Where(f => IsMonospaceFont(f.Name)).OrderBy(f => f.Name).ToList();

			if (FontFamilyList.Count == 0)
			{
				FontFamilyList = FontManager.Current.SystemFonts.OrderBy(f => f.Name).ToList();
			}

			FontWeightList = new List<FontWeight> { Avalonia.Media.FontWeight.Normal, Avalonia.Media.FontWeight.Bold, Avalonia.Media.FontWeight.Black };

			FontSizeList = new List<int> { 6, 7, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

			OutputBufMaxSizeList = new List<int> { 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576 };

			OutputWindowMaxSizeList = new ObservableCollection<int> { 8192, 16384, 32768, 65536, 131072, 262144 };

			ForegroundColor = ColorHelper.ToHexString(Color.FromRgb(0, 0, 0));

			BackgroundColor = ColorHelper.ToHexString(Color.FromRgb(255, 255, 255));

			AppTheme = AppThemeList[0];

			FontFamily = FontFamilyList[0].Name;

			FontWeight = FontWeightList[0].ToString();

			FontSize = FontSizeList[7];

			OutputBufMaxSize = OutputBufMaxSizeList[5];

			OutputWindowMaxSize = OutputWindowMaxSizeList[2];

			KeepKeyboardVisible = false;

			WindowWidth = 600;

			WindowHeight = 800;
		}
	}
}
