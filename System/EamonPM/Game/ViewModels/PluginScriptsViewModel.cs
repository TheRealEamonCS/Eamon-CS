
// PluginScriptsViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonPM.Game.ViewModels
{
	public class PluginScriptsViewModel : ViewModelBase
	{
		public virtual List<string> PluginList { get; set; }

		public PluginScriptsViewModel()
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
