
// EamonMHPage.xaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Xamarin.Forms;
using Eamon.Mobile.Models;
using Eamon.Mobile.ViewModels;

namespace Eamon.Mobile.Views
{
	public partial class EamonMHPage : ContentPage
	{
		EamonMHViewModel viewModel;

		/// <summary></summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		async public virtual void OnItemTapped(object sender, ItemTappedEventArgs args)
		{
			BatchFilesListView.IsEnabled = false;

			BatchFilesListView.SelectedItem = null;

			App.BatchFile = args.Item as BatchFile;

			await Navigation.PushAsync(new PluginLauncherPage(Title));

			while (Navigation.NavigationStack.Count > 1)
			{
				Navigation.RemovePage(Navigation.NavigationStack[0]);
			}

			BatchFilesListView.IsEnabled = true;
		}

		public EamonMHPage()
		{
			InitializeComponent();

			BindingContext = viewModel = new EamonMHViewModel();
		}
	}
}
