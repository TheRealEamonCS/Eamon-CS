
// EamonMHViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using EamonPM.Game.Primitive.Classes;

namespace EamonPM.Game.ViewModels
{
	public class EamonMHViewModel : ViewModelBase
	{
		public virtual IList<PluginScriptFile> FileList { get; set; }

		public EamonMHViewModel()
		{
			FileList = new List<PluginScriptFile>() 
			{
				CreatePluginScriptFile("EnterMainHallUsingAdventures.psh", "-pfn", "EamonMH.dll", "-fsfn", "ADVENTURES.DAT"),
				CreatePluginScriptFile("EnterMainHallUsingCatalog.psh", "-pfn", "EamonMH.dll", "-fsfn", "CATALOG.DAT")
			};
		}
	}
}
