
// QuickLaunchViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonPM.Game.ViewModels
{
	public class QuickLaunchViewModel : ViewModelBase
	{
		public List<string> FolderList { get; set; }

		public QuickLaunchViewModel()
		{
			FolderList = new List<string>()
			{
				"EamonDD",
				"EamonMH",
				"EamonRT"
			};
		}
	}
}
