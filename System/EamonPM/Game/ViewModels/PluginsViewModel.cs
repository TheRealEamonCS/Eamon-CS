
// PluginsViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonPM.Game.ViewModels
{
	public class PluginsViewModel : ViewModelBase
	{
		public virtual List<string> PluginList { get; set; }

		public PluginsViewModel()
		{
			PluginList = new List<string>()
			{
				"EamonDD",
				"EamonMH",
				"EamonRT"
			};
		}
	}
}
