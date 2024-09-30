
// EamonMHViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using EamonPM.Game.Primitive.Classes;

namespace EamonPM.Game.ViewModels
{
	public class EamonMHViewModel : ViewModelBase
	{
		public virtual List<PluginScript> NodeList { get; set; }

		public EamonMHViewModel()
		{
			NodeList = new List<PluginScript>() 
			{
				CreatePluginScript("EnterMainHallUsingAdventures", "-pfn", "EamonMH.dll", "-fsfn", "ADVENTURES.DAT"),
				CreatePluginScript("EnterMainHallUsingCatalog", "-pfn", "EamonMH.dll", "-fsfn", "CATALOG.DAT")
			};
		}
	}
}
