
// PluginLauncherViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Avalonia;
using Avalonia.Media;
using ReactiveUI;
using EamonPM.Game.Helpers;
using EamonPM.Game.Primitive.Classes;

namespace EamonPM.Game.ViewModels
{
	public class PluginLauncherViewModel : ViewModelBase
	{
		public FontFamily _fontFamily;

		public double _fontSize;

		public FontWeight _fontWeight;

		public int _outputBufMaxSize;

		public int _outputWindowMaxSize;

		public SolidColorBrush _backgroundColor;

		public SolidColorBrush _foregroundColor;

		public bool _keepKeyboardVisible;

		public Thickness _inputTextBoxMargin;

		public string _outputText;

		public string _origInputText;

		public string _inputText;

		public virtual PluginScriptVFile PluginScriptVFile { get; set; }

		public virtual FontFamily FontFamily
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

		public virtual FontWeight FontWeight
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

		public virtual SolidColorBrush BackgroundColor
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

		public virtual SolidColorBrush ForegroundColor
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

		public virtual Thickness InputTextBoxMargin
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

		public virtual string OutputText
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

		public virtual string OrigInputText
		{
			get
			{
				return _origInputText;
			}
		}

		public virtual string InputText
		{
			get
			{
				return _inputText;
			}

			set
			{
				_origInputText = _inputText;

				this.RaiseAndSetIfChanged(ref _inputText, value);
			}
		}

		public virtual void FontFamilyComboBoxSelectionChanged(string fontFamily)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fontFamily));

			FontFamily = new FontFamily(fontFamily);
		}

		public virtual void FontSizeComboBoxSelectionChanged(double fontSize)
		{
			Debug.Assert(fontSize > 0);

			FontSize = fontSize;
		}

		public virtual void FontWeightComboBoxSelectionChanged(string fontWeight)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(fontWeight));

			FontWeight = FontWeightHelper.FromString(fontWeight);
		}

		public virtual void OutputBufMaxSizeComboBoxSelectionChanged(int outputBufMaxSize)
		{
			Debug.Assert(outputBufMaxSize > 0);

			OutputBufMaxSize = outputBufMaxSize;
		}

		public virtual void OutputWindowMaxSizeComboBoxSelectionChanged(int outputWindowMaxSize)
		{
			Debug.Assert(outputWindowMaxSize > 0);

			OutputWindowMaxSize = outputWindowMaxSize;
		}

		public virtual void BackgroundColorPickerColorChanged(Color color)
		{
			BackgroundColor = new SolidColorBrush(color);
		}

		public virtual void ForegroundColorPickerColorChanged(Color color)
		{
			ForegroundColor = new SolidColorBrush(color);
		}

		public virtual void KeepKeyboardVisibleToggleSwitchChanged(bool keepKeyboardVisible)
		{
			KeepKeyboardVisible = keepKeyboardVisible;
		}

		public virtual void InputTextBoxMarginChanged(Thickness thickness)
		{
			InputTextBoxMargin = thickness;
		}

		public PluginLauncherViewModel()
		{
			InputTextBoxMargin = new Thickness(0, 0, 0, 10);
		}
	}
}
