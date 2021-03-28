
// EamonCSPage.xaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Xamarin.Forms;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class EamonCSPage : ContentPage
	{
		EamonCSViewModel viewModel;

		/// <summary></summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		async public virtual void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			FoldersListView.IsEnabled = false;

			FoldersListView.SelectedItem = null;

			var folder = args.Item as string;

			if (!string.IsNullOrWhiteSpace(folder))
			{
				if (folder.Equals("Documentation", StringComparison.OrdinalIgnoreCase))
				{
					await Navigation.PushAsync(new DocumentationPage());
				}
				else
				{
					await Navigation.PushAsync(new QuickLaunchPage());
				}
			}

			FoldersListView.IsEnabled = true;
		}

		public EamonCSPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new EamonCSViewModel();
		}
	}
}
