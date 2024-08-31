
// MainWindow.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Avalonia.Controls;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class MainWindow : Window
	{
		public virtual void EamonPMMainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			var settingsViewModel = App.GetViewModel(typeof(SettingsViewModel)) as SettingsViewModel;

			if (sender is MainWindow mainWindow && e != null && DataContext is MainViewModel mainViewModel && settingsViewModel != null)
			{
				var newSize = e.NewSize;

				mainViewModel.EamonPMMainWindowSizeChanged(newSize.Width, newSize.Height);

				settingsViewModel.EamonPMMainWindowSizeChanged(newSize.Width, newSize.Height);
			}
		}

		public virtual void EamonPMMainWindow_Closed(object sender, WindowClosingEventArgs e)
		{
			// Do nothing
		}

		public MainWindow()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(MainViewModel));

			var viewModel = DataContext as MainViewModel;

			Debug.Assert(viewModel != null);

			viewModel.WindowWidth = 600;

			viewModel.WindowHeight = 800;
		}
	}
}
