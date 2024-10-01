
// DocumentationView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Avalonia.Controls;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class DocumentationView : UserControl
	{
		public virtual async void VFileListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ListBox listBox && listBox.SelectedItem != null)
			{
				var url = "https://TheRealEamonCS.github.io";

				App.ResetListBox(listBox);

				try
				{
					Debug.Assert(App.StartBrowserFunc != null);

					App.StartBrowserFunc(url);
				}
				catch (Exception ex)
				{
					await App.ShowErrorMessage("View Documentation operation failed.", ex);
				}
			}
		}

		public DocumentationView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(DocumentationViewModel));
		}
	}
}
