
// SettingsView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using EamonPM.Game.Helpers;
using EamonPM.Game.ViewModels;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Views
{
	public partial class SettingsView : UserControl
	{
		public async void SaveSettingsButton_Clicked(object sender, RoutedEventArgs e)
		{
			var viewModel = DataContext as SettingsViewModel;

			var fileName = gEngine.Path.Combine(App.BasePath, "System", "Bin", "EAMONPM_SETTINGS.DAT");

			try
			{
				gEngine.SharpSerializer.Serialize(viewModel, fileName);
			}
			catch (Exception ex)
			{
				await App.ShowErrorMessage("Save Settings operation failed.", ex);
			}

			viewModel.SettingsChanged = false;
		}

		public async void AppThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel)
			{
				var appTheme = comboBox.SelectedItem as string;

				settingsViewModel.AppThemeComboBoxSelectionChanged(appTheme);

				// Weird hack to fix Save Settings button greyout

				if (!App.InitializeSettings)
				{
					await Task.Delay(25);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public async void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var fontFamily = (string)(((FontFamily)comboBox.SelectedItem).Name);

				settingsViewModel.FontFamilyComboBoxSelectionChanged(fontFamily);

				pluginLauncherViewModel.FontFamilyComboBoxSelectionChanged(fontFamily);

				// Weird hack to fix Save Settings button greyout

				if (!App.InitializeSettings)
				{
					await Task.Delay(25);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public async void FontWeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var fontWeight = (string)comboBox.SelectedItem.ToString();

				settingsViewModel.FontWeightComboBoxSelectionChanged(fontWeight);

				pluginLauncherViewModel.FontWeightComboBoxSelectionChanged(fontWeight);

				// Weird hack to fix Save Settings button greyout

				if (!App.InitializeSettings)
				{
					await Task.Delay(25);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public async void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var fontSize = Convert.ToDouble(comboBox.SelectedItem);

				settingsViewModel.FontSizeComboBoxSelectionChanged(fontSize);

				pluginLauncherViewModel.FontSizeComboBoxSelectionChanged(fontSize);

				// Weird hack to fix Save Settings button greyout

				if (!App.InitializeSettings)
				{
					await Task.Delay(25);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public async void OutputBufMaxSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var outputBufMaxSize = Convert.ToInt32(comboBox.SelectedItem);

				settingsViewModel.OutputBufMaxSizeComboBoxSelectionChanged(outputBufMaxSize);

				pluginLauncherViewModel.OutputBufMaxSizeComboBoxSelectionChanged(outputBufMaxSize);

				// Weird hack to fix Save Settings button greyout

				if (!App.InitializeSettings)
				{
					await Task.Delay(25);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public void ForegroundColorPicker_ColorChanged(object sender, ColorChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ColorPicker colorPicker && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var color = colorPicker.Color;

				settingsViewModel.ForegroundColorPickerColorChanged(color);

				pluginLauncherViewModel.ForegroundColorPickerColorChanged(color);
			}
		}

		public void BackgroundColorPicker_ColorChanged(object sender, ColorChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ColorPicker colorPicker && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var color = colorPicker.Color;

				settingsViewModel.BackgroundColorPickerColorChanged(color);

				pluginLauncherViewModel.BackgroundColorPickerColorChanged(color);
			}
		}

		public void KeepKeyboardVisibleToggleSwitch_Changed(object sender, RoutedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ToggleSwitch toggleSwitch && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var keepKeyboardVisible = (bool)toggleSwitch.IsChecked;

				settingsViewModel.KeepKeyboardVisibleToggleSwitchChanged(keepKeyboardVisible);

				pluginLauncherViewModel.KeepKeyboardVisibleToggleSwitchChanged(keepKeyboardVisible);
			}
		}


		public SettingsView()
		{
			InitializeComponent();

			App.InitializeSettings = true;

			DataContext = App.GetViewModel(typeof(SettingsViewModel));

			var viewModel = DataContext as SettingsViewModel;

			var appTheme = viewModel.AppThemeList.FirstOrDefault(at => at.Equals(viewModel.AppTheme, StringComparison.OrdinalIgnoreCase));

			AppThemeComboBox.SelectedIndex = appTheme != null ? viewModel.AppThemeList.IndexOf(appTheme) : -1;

			if (AppThemeComboBox.SelectedIndex < 0)
			{
				AppThemeComboBox.SelectedIndex = 0;
			}

			var fontFamily = viewModel.FontFamilyList.FirstOrDefault(ff => ff.Name.Equals(viewModel.FontFamily, StringComparison.OrdinalIgnoreCase));

			FontFamilyComboBox.SelectedIndex = fontFamily != null ? viewModel.FontFamilyList.IndexOf(fontFamily) : -1;

			if (FontFamilyComboBox.SelectedIndex < 0)
			{
				FontFamilyComboBox.SelectedIndex = 0;
			}

			var fontWeight = viewModel.FontWeightList.FirstOrDefault(fw => fw.ToString().Equals(viewModel.FontWeight, StringComparison.OrdinalIgnoreCase));

			FontWeightComboBox.SelectedIndex = viewModel.FontWeightList.IndexOf(fontWeight);

			if (FontWeightComboBox.SelectedIndex < 0)
			{
				FontWeightComboBox.SelectedIndex = 0;
			}

			FontSizeComboBox.SelectedIndex = viewModel.FontSizeList.IndexOf((int)viewModel.FontSize);

			if (FontSizeComboBox.SelectedIndex < 0)
			{
				FontSizeComboBox.SelectedIndex = 7;
			}

			OutputBufMaxSizeComboBox.SelectedIndex = viewModel.OutputBufMaxSizeList.IndexOf(viewModel.OutputBufMaxSize);

			if (OutputBufMaxSizeComboBox.SelectedIndex < 0)
			{
				OutputBufMaxSizeComboBox.SelectedIndex = 5;
			}

			ForegroundColorPicker.Color = ColorHelper.FromHexString(viewModel.ForegroundColor, Color.FromRgb(0, 0, 0));

			ForegroundColorPicker_ColorChanged(ForegroundColorPicker, null);

			BackgroundColorPicker.Color = ColorHelper.FromHexString(viewModel.BackgroundColor, Color.FromRgb(255, 255, 255));

			BackgroundColorPicker_ColorChanged(BackgroundColorPicker, null);

			KeepKeyboardVisibleToggleSwitch.IsChecked = viewModel.KeepKeyboardVisible;

			App.InitializeSettings = false;
		}
	}
}
