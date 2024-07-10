
// SplashScreen.axaml.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Avalonia.Controls;
using EamonPM.Game.ViewModels;

namespace EamonPM.Game.Views
{
	public partial class SplashScreen : Window
	{
		public SplashScreen()
		{
			InitializeComponent();

			DataContext = App.GetViewModel(typeof(SplashScreenViewModel));
		}
	}
}
