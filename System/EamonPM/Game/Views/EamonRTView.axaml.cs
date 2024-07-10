
// EamonRTView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Avalonia.Controls;
using EamonPM.Game.ViewModels;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Views
{
	public partial class EamonRTView : UserControl
	{
		public void BatchFileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var mainViewModel = App.GetViewModel(typeof(MainViewModel)) as MainViewModel;

			if (sender is ListBox listBox && listBox.SelectedItem != null && DataContext is EamonRTViewModel viewModel && mainViewModel != null)
			{
				var selectedIndex = listBox.SelectedIndex;

				var currentView = App.GetView(typeof(PluginLauncherView)) as UserControl;

				var currentViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

				currentViewModel.BatchFile = viewModel.BatchFileList[selectedIndex];

				Debug.Assert(currentViewModel.BatchFile != null);

				var mainTitle = gEngine.Path.GetFileNameWithoutExtension(currentViewModel.BatchFile.PluginArgs[1]);

				mainViewModel.NavigateTo(currentView, mainTitle, false);

				App.ResetListBox(listBox);
			}
		}

		public EamonRTView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(EamonRTViewModel));
		}
	}
}
