
// EamonCSViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonPM.Game.ViewModels
{
	public class EamonCSViewModel : ViewModelBase
	{
		public virtual IList<string> FolderList { get; set; }

		public EamonCSViewModel()
		{
			FolderList = new List<string>()
			{
				"Documentation",
				"QuickLaunch"
			};
		}
	}
}
