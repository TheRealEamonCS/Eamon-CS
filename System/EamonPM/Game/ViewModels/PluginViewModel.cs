
// PluginViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonPM.Game.ViewModels
{
	public class PluginViewModel : ViewModelBase
	{
		public virtual List<string> NodeList { get; set; }

		public PluginViewModel()
		{
			NodeList = new List<string>()
			{
				"EamonDD",
				"EamonMH",
				"EamonRT"
			};
		}
	}
}
