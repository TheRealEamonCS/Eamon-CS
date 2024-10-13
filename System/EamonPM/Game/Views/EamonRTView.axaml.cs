
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
		public virtual void FileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var mainViewModel = App.GetViewModel(typeof(MainViewModel)) as MainViewModel;

			if (sender is ListBox listBox && listBox.SelectedItem != null && DataContext is EamonRTViewModel eamonRTViewModel && mainViewModel != null)
			{
				var selectedIndex = listBox.SelectedIndex;

				var currentView = App.GetView(typeof(PluginLauncherView)) as UserControl;

				var currentViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

				Debug.Assert(currentViewModel != null);

				currentViewModel.PluginScriptFile = eamonRTViewModel.FileList[selectedIndex];

				Debug.Assert(currentViewModel.PluginScriptFile != null);

				var mainTitle = gEngine.Path.GetFileNameWithoutExtension(currentViewModel.PluginScriptFile.PluginArgs[1]);

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
