
// AboutViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Eamon.Mobile.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		public string BuildGuid
		{
			get
			{
				return App.GetBuildGuid();
			}
		}

		public AboutViewModel()
		{
			Title = "About";
		}
	}
}
