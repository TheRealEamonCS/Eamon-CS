
// SettingsViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

		public string _fontFamily;

		public double _fontSize;

		public string _fontWeight;

		public int _outputBufMaxSize;

		public int _outputWindowMaxSize;

		public string _backgroundColor;

		public string _foregroundColor;

		public bool _keepKeyboardVisible;

		public double _windowHeight;

		public double _windowWidth;

		public bool _settingsChanged;

		[ExcludeFromSerialization]
		public virtual string[] MonospaceFonts { get; set; }

		[ExcludeFromSerialization]
		public virtual IList<string> AppThemeList { get; set; }

		[ExcludeFromSerialization]
		public virtual IList<FontFamily> FontFamilyList { get; set; }

		[ExcludeFromSerialization]
		public virtual IList<int> FontSizeList { get; set; }

		[ExcludeFromSerialization]
		public virtual IList<FontWeight> FontWeightList { get; set; }

		[ExcludeFromSerialization]
		public virtual IList<int> OutputBufMaxSizeList { get; set; }

		[ExcludeFromSerialization]
		public virtual ObservableCollection<int> OutputWindowMaxSizeList { get; set; }

		public virtual string AppTheme
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

		public virtual string FontFamily
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

		public virtual double FontSize
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

		public virtual string FontWeight
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

		public virtual int OutputBufMaxSize
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

		public virtual int OutputWindowMaxSize
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

		public virtual string BackgroundColor
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

		public virtual string ForegroundColor
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

		public virtual bool KeepKeyboardVisible
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

		public virtual double WindowHeight
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

		public virtual double WindowWidth
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

		[ExcludeFromSerialization]
		public virtual bool SettingsChanged
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

		public virtual void AppThemeComboBoxSelectionChanged(string appTheme)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(appTheme));

			AppTheme = appTheme;

			App.ChangeTheme(appTheme);

			SettingsChanged = true;
		}

		public virtual void FontFamilyComboBoxSelectionChanged(string fontFamily)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fontFamily));

			FontFamily = fontFamily;

			SettingsChanged = true;
		}

		public virtual void FontSizeComboBoxSelectionChanged(double fontSize)
		{
			Debug.Assert(fontSize > 0);

			FontSize = fontSize;

			SettingsChanged = true;
		}

		public virtual void FontWeightComboBoxSelectionChanged(string fontWeight)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fontWeight));

			FontWeight = fontWeight;

			SettingsChanged = true;
		}

		public virtual void OutputBufMaxSizeComboBoxSelectionChanged(int outputBufMaxSize)
		{
			Debug.Assert(outputBufMaxSize > 0);

			OutputBufMaxSize = outputBufMaxSize;

			SettingsChanged = true;
		}

		public virtual void OutputWindowMaxSizeComboBoxSelectionChanged(int outputWindowMaxSize)
		{
			Debug.Assert(outputWindowMaxSize > 0);

			OutputWindowMaxSize = outputWindowMaxSize;

			SettingsChanged = true;
		}

		public virtual void BackgroundColorPickerColorChanged(Color color)
		{
			BackgroundColor = ColorHelper.ToHexString(color);

			SettingsChanged = true;
		}

		public virtual void ForegroundColorPickerColorChanged(Color color)
		{
			ForegroundColor = ColorHelper.ToHexString(color);

			SettingsChanged = true;
		}

		public virtual void KeepKeyboardVisibleToggleSwitchChanged(bool keepKeyboardVisible)
		{
			KeepKeyboardVisible = keepKeyboardVisible;

			SettingsChanged = true;
		}

		public virtual void EamonPMMainWindowSizeChanged(double windowWidth, double windowHeight)
		{
			Debug.Assert(windowWidth > 0 && windowHeight > 0);

			WindowWidth = windowWidth;

			WindowHeight = windowHeight;

			SettingsChanged = true;
		}

		public virtual void OutputWindowMaxSizeComboBoxSync()
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

		public virtual bool IsMonospaceFont(string fontFamily)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fontFamily));

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

			AppThemeList = new List<string> { "Default Light", "Default Dark", "Element Air", "Element Earth", "Element Fire", "Element Water" };

			FontFamilyList = FontManager.Current.SystemFonts.Where(f => IsMonospaceFont(f.Name)).OrderBy(f => f.Name).ToList();

			if (FontFamilyList.Count == 0)
			{
				FontFamilyList = FontManager.Current.SystemFonts.OrderBy(f => f.Name).ToList();
			}

			FontSizeList = new List<int> { 6, 7, 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

			FontWeightList = new List<FontWeight> { Avalonia.Media.FontWeight.Normal, Avalonia.Media.FontWeight.Bold, Avalonia.Media.FontWeight.Black };

			OutputBufMaxSizeList = new List<int> { 8192, 16384, 32768, 65536, 131072, 262144, 524288, 1048576 };

			OutputWindowMaxSizeList = new ObservableCollection<int> { 8192, 16384, 32768, 65536, 131072, 262144 };

			AppTheme = AppThemeList[0];

			FontFamily = FontFamilyList[0].Name;

			FontSize = FontSizeList[7];

			FontWeight = FontWeightList[0].ToString();

			OutputBufMaxSize = OutputBufMaxSizeList[5];

			OutputWindowMaxSize = OutputWindowMaxSizeList[2];

			BackgroundColor = ColorHelper.ToHexString(Color.FromRgb(255, 255, 255));

			ForegroundColor = ColorHelper.ToHexString(Color.FromRgb(0, 0, 0));

			KeepKeyboardVisible = false;

			WindowWidth = 600;

			WindowHeight = 800;
		}
	}
}
