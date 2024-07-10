
// MainWindow.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Avalonia.Controls;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class MainWindow : Window
	{
		public void EamonPMMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var settingsViewModel = App.GetViewModel(typeof(SettingsViewModel)) as SettingsViewModel;

			if (sender is MainWindow mainWindow && DataContext is MainViewModel viewModel)
			{
				var newSize = e.NewSize;

				viewModel.EamonPMMainWindowSizeChanged(newSize.Width, newSize.Height);

				settingsViewModel.EamonPMMainWindowSizeChanged(newSize.Width, newSize.Height);
			}
		}

		public void EamonPMMainWindow_Closed(object sender, WindowClosingEventArgs e)
		{

		}

		public MainWindow()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(MainViewModel));

			var viewModel = DataContext as MainViewModel;

			viewModel.WindowWidth = 600;

			viewModel.WindowHeight = 800;
		}
	}
}
