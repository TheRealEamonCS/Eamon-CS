
// EamonDDViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using EamonPM.Game.Primitive.Classes;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.ViewModels
{
	public class EamonDDViewModel : ViewModelBase
	{
		public virtual List<PluginScript> NodeList { get; set; }

		public EamonDDViewModel()
		{
			NodeList = new List<PluginScript>()
			{
				CreatePluginScript("EditAdventures", "-pfn", "EamonRT.dll", "-fsfn", "ADVENTURES.DAT", "-rge"),
				CreatePluginScript("EditCatalog", "-pfn", "EamonRT.dll", "-fsfn", "CATALOG.DAT", "-rge"),
				CreatePluginScript("EditCharacters", "-pfn", "EamonRT.dll", "-chrfn", "CHARACTERS.DAT", "-rge"),
				CreatePluginScript("EditContemporary", "-pfn", "EamonRT.dll", "-fsfn", "CONTEMPORARY.DAT", "-rge"),
				CreatePluginScript("EditFantasy", "-pfn", "EamonRT.dll", "-fsfn", "FANTASY.DAT", "-rge"),
				CreatePluginScript("EditHorror", "-pfn", "EamonRT.dll", "-fsfn", "HORROR.DAT", "-rge"),
				CreatePluginScript("EditSciFi", "-pfn", "EamonRT.dll", "-fsfn", "SCIFI.DAT", "-rge"),
				CreatePluginScript("EditTest", "-pfn", "EamonRT.dll", "-fsfn", "TEST.DAT", "-rge"),
				CreatePluginScript("EditWorkbench", "-pfn", "EamonRT.dll", "-fsfn", "WORKBENCH.DAT", "-rge"),
				CreatePluginScript("EditWorkInProgress", "-pfn", "EamonRT.dll", "-fsfn", "WIP.DAT", "-rge"),
				CreatePluginScript("LoadAdventureSupportMenu", "-pfn", "EamonRT.dll", "-rge")
			};

			var adventureDirs = App.GetAdventureDirs();

			foreach (var dir in adventureDirs)
			{
				var pluginFileName = string.Format("{0}.dll", dir);

				NodeList.Add
				(
					CreatePluginScript(string.Format("Edit{0}", dir), "-pfn", App.PluginExists(pluginFileName) ? pluginFileName : "EamonRT.dll", "-wd", gEngine.Path.Combine("..", "..", "Adventures", dir), "-la", "-rge")
				);
			}

			NodeList = NodeList.OrderBy(ps => ps.Name).ToList();
		}
	}
}
