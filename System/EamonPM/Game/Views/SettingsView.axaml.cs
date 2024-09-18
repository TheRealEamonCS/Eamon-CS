
// SettingsView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
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
		public virtual async void AppThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel viewModel)
			{
				var appTheme = comboBox.SelectedItem as string;

				viewModel.AppThemeComboBoxSelectionChanged(appTheme);

				// Weird hack to fix Save Settings button greyout

				if (!App.InitializeSettings)
				{
					await Task.Delay(50);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public virtual async void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var fontFamily = (string)(((FontFamily)comboBox.SelectedItem).Name);

				settingsViewModel.FontFamilyComboBoxSelectionChanged(fontFamily);

				pluginLauncherViewModel.FontFamilyComboBoxSelectionChanged(fontFamily);

				if (!App.InitializeSettings)
				{
					await Task.Delay(50);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public virtual async void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var fontSize = Convert.ToDouble(comboBox.SelectedItem);

				settingsViewModel.FontSizeComboBoxSelectionChanged(fontSize);

				pluginLauncherViewModel.FontSizeComboBoxSelectionChanged(fontSize);

				if (!App.InitializeSettings)
				{
					await Task.Delay(50);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public virtual async void FontWeightComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var fontWeight = (string)comboBox.SelectedItem.ToString();

				settingsViewModel.FontWeightComboBoxSelectionChanged(fontWeight);

				pluginLauncherViewModel.FontWeightComboBoxSelectionChanged(fontWeight);

				if (!App.InitializeSettings)
				{
					await Task.Delay(50);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public virtual async void OutputBufMaxSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherView = App.GetView(typeof(PluginLauncherView)) as PluginLauncherView;

			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null && pluginLauncherView != null)
			{
				var outputBufMaxSize = Convert.ToInt32(comboBox.SelectedItem);

				settingsViewModel.OutputBufMaxSizeComboBoxSelectionChanged(outputBufMaxSize);

				pluginLauncherViewModel.OutputBufMaxSizeComboBoxSelectionChanged(outputBufMaxSize);

				var outputWindowSelectedIndex = OutputWindowMaxSizeComboBox.SelectedIndex;

				settingsViewModel.OutputWindowMaxSizeComboBoxSync();

				OutputWindowMaxSizeComboBox.SelectedIndex = outputWindowSelectedIndex > settingsViewModel.OutputWindowMaxSizeList.Count - 1 ? settingsViewModel.OutputWindowMaxSizeList.Count - 1 : outputWindowSelectedIndex;

				App.EnforceOutputBufMaxSize();

				App.RefreshOutputWindowText();

				pluginLauncherView.OutputScrollViewerScrollToEnd();

				if (!App.InitializeSettings)
				{
					await Task.Delay(50);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public virtual async void OutputWindowMaxSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var pluginLauncherView = App.GetView(typeof(PluginLauncherView)) as PluginLauncherView;

			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ComboBox comboBox && comboBox.SelectedItem != null && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null && pluginLauncherView != null)
			{
				var outputWindowMaxSize = Convert.ToInt32(comboBox.SelectedItem);

				settingsViewModel.OutputWindowMaxSizeComboBoxSelectionChanged(outputWindowMaxSize);

				pluginLauncherViewModel.OutputWindowMaxSizeComboBoxSelectionChanged(outputWindowMaxSize);

				App.RefreshOutputWindowText();

				pluginLauncherView.OutputScrollViewerScrollToEnd();

				if (!App.InitializeSettings)
				{
					await Task.Delay(50);

					SaveSettingsButton.IsEnabled = false;

					SaveSettingsButton.IsEnabled = true;
				}
			}
		}

		public virtual void BackgroundColorPicker_ColorChanged(object sender, ColorChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ColorPicker colorPicker && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var color = colorPicker.Color;

				settingsViewModel.BackgroundColorPickerColorChanged(color);

				pluginLauncherViewModel.BackgroundColorPickerColorChanged(color);
			}
		}

		public virtual void ForegroundColorPicker_ColorChanged(object sender, ColorChangedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ColorPicker colorPicker && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var color = colorPicker.Color;

				settingsViewModel.ForegroundColorPickerColorChanged(color);

				pluginLauncherViewModel.ForegroundColorPickerColorChanged(color);
			}
		}

		public virtual void KeepKeyboardVisibleToggleSwitch_Changed(object sender, RoutedEventArgs e)
		{
			var pluginLauncherViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

			if (sender is ToggleSwitch toggleSwitch && DataContext is SettingsViewModel settingsViewModel && pluginLauncherViewModel != null)
			{
				var keepKeyboardVisible = (bool)toggleSwitch.IsChecked;

				settingsViewModel.KeepKeyboardVisibleToggleSwitchChanged(keepKeyboardVisible);

				pluginLauncherViewModel.KeepKeyboardVisibleToggleSwitchChanged(keepKeyboardVisible);
			}
		}

		public virtual async void SaveSettingsButton_Clicked(object sender, RoutedEventArgs e)
		{
			if (DataContext is SettingsViewModel viewModel)
			{
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
		}

		public SettingsView()
		{
			InitializeComponent();

			App.InitializeSettings = true;

			DataContext = App.GetViewModel(typeof(SettingsViewModel));

			var viewModel = DataContext as SettingsViewModel;

			Debug.Assert(viewModel != null);

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

			FontSizeComboBox.SelectedIndex = viewModel.FontSizeList.IndexOf((int)viewModel.FontSize);

			if (FontSizeComboBox.SelectedIndex < 0)
			{
				FontSizeComboBox.SelectedIndex = 7;
			}

			var fontWeight = viewModel.FontWeightList.FirstOrDefault(fw => fw.ToString().Equals(viewModel.FontWeight, StringComparison.OrdinalIgnoreCase));

			FontWeightComboBox.SelectedIndex = viewModel.FontWeightList.IndexOf(fontWeight);

			if (FontWeightComboBox.SelectedIndex < 0)
			{
				FontWeightComboBox.SelectedIndex = 0;
			}

			OutputBufMaxSizeComboBox.SelectedIndex = viewModel.OutputBufMaxSizeList.IndexOf(viewModel.OutputBufMaxSize);

			if (OutputBufMaxSizeComboBox.SelectedIndex < 0)
			{
				OutputBufMaxSizeComboBox.SelectedIndex = 5;
			}

			OutputWindowMaxSizeComboBox.SelectedIndex = viewModel.OutputWindowMaxSizeList.IndexOf(viewModel.OutputWindowMaxSize);

			if (OutputWindowMaxSizeComboBox.SelectedIndex < 0)
			{
				OutputWindowMaxSizeComboBox.SelectedIndex = Math.Min(viewModel.OutputWindowMaxSizeList.Count - 1, 2);
			}

			BackgroundColorPicker.Color = ColorHelper.FromHexString(viewModel.BackgroundColor, Color.FromRgb(255, 255, 255));

			BackgroundColorPicker_ColorChanged(BackgroundColorPicker, null);

			ForegroundColorPicker.Color = ColorHelper.FromHexString(viewModel.ForegroundColor, Color.FromRgb(0, 0, 0));

			ForegroundColorPicker_ColorChanged(ForegroundColorPicker, null);

			KeepKeyboardVisibleToggleSwitch.IsChecked = viewModel.KeepKeyboardVisible;

			App.InitializeSettings = false;
		}
	}
}
