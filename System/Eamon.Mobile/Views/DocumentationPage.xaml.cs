
// DocumentationPage.xaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Xamarin.Forms;
using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class DocumentationPage : ContentPage
	{
		DocumentationViewModel viewModel;

		/// <summary></summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public virtual void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			BatchFilesListView.IsEnabled = false;

			BatchFilesListView.SelectedItem = null;

			var batchFile = args.Item as BatchFile;

			if (batchFile != null)
			{
				if (batchFile.FileName.ToLower().StartsWith("https://"))
				{
					Device.OpenUri(new Uri(batchFile.FileName));
				}
				else
				{
					// unknown file type
				}
			}

			BatchFilesListView.IsEnabled = true;
		}

		public DocumentationPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new DocumentationViewModel();
		}
	}
}
