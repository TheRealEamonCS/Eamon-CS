
// EamonDDView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Avalonia.Controls;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class EamonDDView : UserControl
	{
		public virtual void PluginScriptListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var mainViewModel = App.GetViewModel(typeof(MainViewModel)) as MainViewModel;

			if (sender is ListBox listBox && listBox.SelectedItem != null && DataContext is EamonDDViewModel eamonDDViewModel && mainViewModel != null)
			{
				var selectedIndex = listBox.SelectedIndex;

				var currentView = App.GetView(typeof(PluginLauncherView)) as UserControl;

				var currentViewModel = App.GetViewModel(typeof(PluginLauncherViewModel)) as PluginLauncherViewModel;

				Debug.Assert(currentViewModel != null);

				currentViewModel.PluginScript = eamonDDViewModel.PluginScriptList[selectedIndex];

				Debug.Assert(currentViewModel.PluginScript != null);

				mainViewModel.NavigateTo(currentView, "EamonDD", false);

				App.ResetListBox(listBox);
			}
		}

		public EamonDDView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(EamonDDViewModel));
		}
	}
}
