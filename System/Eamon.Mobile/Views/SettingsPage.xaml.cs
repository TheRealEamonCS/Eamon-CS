
// SettingsPage.xaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Xamarin.Forms;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class SettingsPage : ContentPage
	{
		SettingsViewModel viewModel;

		public void BrowseSettingsPage_SizeChanged(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				ForegroundColorPicker.Unfocus();

				BackgroundColorPicker.Unfocus();

				FontFamilyPicker.Unfocus();

				FontSizePicker.Unfocus();

				ScrollbackBufferSizePicker.Unfocus();
			});

			Device.BeginInvokeOnMainThread(() =>
			{
				ForegroundColorLabel.IsVisible = false;

				ForegroundColorLabel.IsVisible = true;

				ForegroundColorPicker.IsVisible = false;

				ForegroundColorPicker.IsVisible = true;

				Separator0.IsVisible = false;

				Separator0.IsVisible = true;
				
				BackgroundColorLabel.IsVisible = false;

				BackgroundColorLabel.IsVisible = true;

				BackgroundColorPicker.IsVisible = false;

				BackgroundColorPicker.IsVisible = true;
				
				Separator1.IsVisible = false;

				Separator1.IsVisible = true;

				FontFamilyLabel.IsVisible = false;

				FontFamilyLabel.IsVisible = true;

				FontFamilyPicker.IsVisible = false;

				FontFamilyPicker.IsVisible = true;

				Separator2.IsVisible = false;

				Separator2.IsVisible = true;

				FontSizeLabel.IsVisible = false;

				FontSizeLabel.IsVisible = true;

				FontSizePicker.IsVisible = false;

				FontSizePicker.IsVisible = true;

				Separator3.IsVisible = false;

				Separator3.IsVisible = true;

				ScrollbackBufferSizeLabel.IsVisible = false;

				ScrollbackBufferSizeLabel.IsVisible = true;

				ScrollbackBufferSizePicker.IsVisible = false;

				ScrollbackBufferSizePicker.IsVisible = true;

				Separator4.IsVisible = false;

				Separator4.IsVisible = true;
				
				KeepKeyboardVisibleLabel.IsVisible = false;

				KeepKeyboardVisibleLabel.IsVisible = true;

				KeepKeyboardVisibleSwitch.IsVisible = false;

				KeepKeyboardVisibleSwitch.IsVisible = true;
				
				Separator5.IsVisible = false;

				Separator5.IsVisible = true;
			});
		}

		public SettingsPage(SettingsViewModel settingsViewModel)
		{
			InitializeComponent();

			BindingContext = viewModel = settingsViewModel;

			App.SettingsPage = this;
		}
	}
}
