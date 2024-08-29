
// EamonCSView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Avalonia.Controls;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class EamonCSView : UserControl
	{
		public virtual void FolderListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var mainViewModel = App.GetViewModel(typeof(MainViewModel)) as MainViewModel;

			if (sender is ListBox listBox && listBox.SelectedItem != null && mainViewModel != null)
			{
				var selectedIndex = listBox.SelectedIndex;

				var currentView = App.GetView(
					selectedIndex == 0 ? typeof(DocumentationView) : 
					typeof(QuickLaunchView)
				) as UserControl;

				mainViewModel.NavigateTo(currentView, listBox.SelectedItem.ToString().Replace("View", ""), true);

				App.ResetListBox(listBox);
			}
		}

		public EamonCSView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(EamonCSViewModel));
		}
	}
}
