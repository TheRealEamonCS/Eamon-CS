
// QuickLaunchPage.xaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Xamarin.Forms;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class QuickLaunchPage : ContentPage
	{
		QuickLaunchViewModel viewModel;

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
				if (folder.Equals("EamonDD", StringComparison.OrdinalIgnoreCase))
				{
					await Navigation.PushAsync(new EamonDDPage());
				}
				else if (folder.Equals("EamonMH", StringComparison.OrdinalIgnoreCase))
				{
					await Navigation.PushAsync(new EamonMHPage());
				}
				else
				{
					await Navigation.PushAsync(new EamonRTPage());
				}
			}

			FoldersListView.IsEnabled = true;
		}

		public QuickLaunchPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new QuickLaunchViewModel();
		}
	}
}
