
// PluginLauncherViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using EamonPM.Game.Helpers;
using EamonPM.Game.Primitive.Classes;

namespace EamonPM.Game.ViewModels
{
	public class PluginLauncherViewModel : ViewModelBase
	{
		public Thickness _inputTextBoxMargin;

		public SolidColorBrush _foregroundColor;

		public SolidColorBrush _backgroundColor;

		public FontFamily _fontFamily;

		public FontWeight _fontWeight;

		public double _fontSize;

		public int _outputBufMaxSize;

		public int _outputWindowMaxSize;

		public bool _keepKeyboardVisible;

		public string _outputText;

		public string _oldInputText;

		public string _inputText;

		public BatchFile BatchFile { get; set; }

		public Thickness InputTextBoxMargin
		{
			get
			{
				return _inputTextBoxMargin;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _inputTextBoxMargin, value);
			}
		}

		public SolidColorBrush ForegroundColor
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

		public SolidColorBrush BackgroundColor
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

		public FontFamily FontFamily
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

		public FontWeight FontWeight
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

		public string OutputText
		{
			get
			{
				return _outputText;
			}

			set
			{
				this.RaiseAndSetIfChanged(ref _outputText, value);
			}
		}

		public string OldInputText
		{
			get
			{
				return _oldInputText;
			}
		}

		public string InputText
		{
			get
			{
				return _inputText;
			}

			set
			{
				_oldInputText = _inputText;

				this.RaiseAndSetIfChanged(ref _inputText, value);
			}
		}

		public void InputTextBoxMarginChanged(Thickness thickness)
		{
			InputTextBoxMargin = thickness;
		}

		public void ForegroundColorPickerColorChanged(Color color)
		{
			ForegroundColor = new SolidColorBrush(color);
		}

		public void BackgroundColorPickerColorChanged(Color color)
		{
			BackgroundColor = new SolidColorBrush(color);
		}

		public void FontFamilyComboBoxSelectionChanged(string fontFamily)
		{
			FontFamily = new FontFamily(fontFamily);
		}

		public void FontWeightComboBoxSelectionChanged(string fontWeight)
		{
			FontWeight = FontWeightHelper.FromString(fontWeight);
		}

		public void FontSizeComboBoxSelectionChanged(double fontSize)
		{
			FontSize = fontSize;
		}

		public void OutputBufMaxSizeComboBoxSelectionChanged(int outputBufMaxSize)
		{
			OutputBufMaxSize = outputBufMaxSize;
		}

		public void OutputWindowMaxSizeComboBoxSelectionChanged(int outputWindowMaxSize)
		{
			OutputWindowMaxSize = outputWindowMaxSize;
		}

		public void KeepKeyboardVisibleToggleSwitchChanged(bool keepKeyboardVisible)
		{
			KeepKeyboardVisible = keepKeyboardVisible;
		}

		public PluginLauncherViewModel()
		{
			InputTextBoxMargin = new Thickness(0, 0, 0, 10);
		}
	}
}
