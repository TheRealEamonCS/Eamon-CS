
// PluginLauncherViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Xamarin.Forms;

namespace Eamon.Mobile.ViewModels
{
	public class PluginLauncherViewModel : BaseViewModel
	{
		public Color _foregroundColor;

		public Color _backgroundColor;

		public string _foregroundColorName;

		public string _backgroundColorName;

		public string _fontFamily;

		public long _fontSize;

		public string _inputText;

		public string _outputText;

		public ColorTypeConverter ColorTypeConverter { get; set; }

		public virtual Color ForegroundColor
		{
			get
			{
				return _foregroundColor;
			}
			set
			{
				SetProperty(ref _foregroundColor, value);
			}
		}

		public virtual Color BackgroundColor
		{
			get
			{
				return _backgroundColor;
			}
			set
			{
				SetProperty(ref _backgroundColor, value);
			}
		}

		public virtual string ForegroundColorName
		{
			get
			{
				return _foregroundColorName;
			}
			set
			{
				SetProperty(ref _foregroundColorName, value);

				ForegroundColor = !string.IsNullOrWhiteSpace(_foregroundColorName) ? (Color)ColorTypeConverter.ConvertFromInvariantString(_foregroundColorName) : Color.Default;
			}
		}

		public virtual string BackgroundColorName
		{
			get
			{
				return _backgroundColorName;
			}
			set
			{
				SetProperty(ref _backgroundColorName, value);

				BackgroundColor = !string.IsNullOrWhiteSpace(_backgroundColorName) ? (Color)ColorTypeConverter.ConvertFromInvariantString(_backgroundColorName) : Color.Default;
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
				SetProperty(ref _fontFamily, value);
			}
		}

		public virtual long FontSize
		{
			get
			{
				return _fontSize;
			}
			set
			{
				SetProperty(ref _fontSize, value);
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
				SetProperty(ref _inputText, value);
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
				SetProperty(ref _outputText, value);
			}
		}

		public virtual void SettingsChangedHandler(object sender, EventArgs args)
		{
			ForegroundColorName = App.SettingsViewModel.ForegroundColorName;

			BackgroundColorName = App.SettingsViewModel.BackgroundColorName;

			FontFamily = App.SettingsViewModel.FontFamily;

			FontSize = App.SettingsViewModel.FontSize;

			App.PluginLauncherPage.RedrawPluginLauncherPageControls();

			App.PluginLauncherPage.ScrollToBottomOfPluginScrollView();
		}

		public PluginLauncherViewModel(string title)
		{
			Title = title ?? "Plugin Launcher";

			ColorTypeConverter = new ColorTypeConverter();

			ForegroundColorName = App.SettingsViewModel.ForegroundColorName;

			BackgroundColorName = App.SettingsViewModel.BackgroundColorName;

			FontFamily = App.SettingsViewModel.FontFamily;

			FontSize = App.SettingsViewModel.FontSize;

			InputText = "";

			OutputText = "";

			App.SettingsViewModel.SettingsChanged += SettingsChangedHandler;

			App.PluginLauncherViewModel = this;
		}
	}
}