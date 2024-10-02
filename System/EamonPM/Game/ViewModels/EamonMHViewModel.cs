
// EamonMHViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using EamonPM.Game.Primitive.Classes;

namespace EamonPM.Game.ViewModels
{
	public class EamonMHViewModel : ViewModelBase
	{
		public virtual IList<PluginScriptVFile> VFileList { get; set; }

		public EamonMHViewModel()
		{
			VFileList = new List<PluginScriptVFile>() 
			{
				CreatePluginScriptVFile("EnterMainHallUsingAdventures.psh", "-pfn", "EamonMH.dll", "-fsfn", "ADVENTURES.DAT"),
				CreatePluginScriptVFile("EnterMainHallUsingCatalog.psh", "-pfn", "EamonMH.dll", "-fsfn", "CATALOG.DAT")
			};
		}
	}
}
