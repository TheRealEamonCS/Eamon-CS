
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
		public async void BrowserListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender is ListBox listBox && listBox.SelectedItem != null && DataContext is DocumentationViewModel viewModel)
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
