
// PluginScriptsView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Avalonia.Controls;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class PluginScriptsView : UserControl
	{
		public virtual void PluginListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var mainViewModel = App.GetViewModel(typeof(MainViewModel)) as MainViewModel;

			if (sender is ListBox listBox && listBox.SelectedItem != null && mainViewModel != null)
			{
				var selectedIndex = listBox.SelectedIndex;

				var currentView = App.GetView (
					selectedIndex == 0 ? typeof(EamonDDView) :
					selectedIndex == 1 ? typeof(EamonMHView) :
					typeof(EamonRTView)
				) as UserControl;

				mainViewModel.NavigateTo(currentView, listBox.SelectedItem.ToString().Replace("View", ""), true);

				App.ResetListBox(listBox);
			}
		}

		public PluginScriptsView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(PluginScriptsViewModel));
		}
	}
}
