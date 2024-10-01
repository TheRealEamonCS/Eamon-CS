
// AboutView.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Avalonia.Controls;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class AboutView : UserControl
	{
		public virtual void WelcomeMessageTextBlock_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (WelcomeMessageTextBlock.Bounds.Width > 0)
			{
				AdventureDescriptionTextBlock.MaxWidth = WelcomeMessageTextBlock.Bounds.Width;
			}
		}

		public AboutView()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(AboutViewModel));
		}
	}
}
