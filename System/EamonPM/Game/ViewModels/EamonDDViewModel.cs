
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
		public virtual IList<PluginScriptVFile> VFileList { get; set; }

		public EamonDDViewModel()
		{
			VFileList = new List<PluginScriptVFile>()
			{
				CreatePluginScriptVFile("EditAdventures.psh", "-pfn", "EamonRT.dll", "-fsfn", "ADVENTURES.DAT", "-rge"),
				CreatePluginScriptVFile("EditCatalog.psh", "-pfn", "EamonRT.dll", "-fsfn", "CATALOG.DAT", "-rge"),
				CreatePluginScriptVFile("EditCharacters.psh", "-pfn", "EamonRT.dll", "-chrfn", "CHARACTERS.DAT", "-rge"),
				CreatePluginScriptVFile("EditContemporary.psh", "-pfn", "EamonRT.dll", "-fsfn", "CONTEMPORARY.DAT", "-rge"),
				CreatePluginScriptVFile("EditFantasy.psh", "-pfn", "EamonRT.dll", "-fsfn", "FANTASY.DAT", "-rge"),
				CreatePluginScriptVFile("EditHorror.psh", "-pfn", "EamonRT.dll", "-fsfn", "HORROR.DAT", "-rge"),
				CreatePluginScriptVFile("EditSciFi.psh", "-pfn", "EamonRT.dll", "-fsfn", "SCIFI.DAT", "-rge"),
				CreatePluginScriptVFile("EditTest.psh", "-pfn", "EamonRT.dll", "-fsfn", "TEST.DAT", "-rge"),
				CreatePluginScriptVFile("EditWorkbench.psh", "-pfn", "EamonRT.dll", "-fsfn", "WORKBENCH.DAT", "-rge"),
				CreatePluginScriptVFile("EditWorkInProgress.psh", "-pfn", "EamonRT.dll", "-fsfn", "WIP.DAT", "-rge"),
				CreatePluginScriptVFile("LoadAdventureSupportMenu.psh", "-pfn", "EamonRT.dll", "-rge")
			};

			var adventureDirs = App.GetAdventureDirs();

			foreach (var dir in adventureDirs)
			{
				var pluginFileName = string.Format("{0}.dll", dir);

				VFileList.Add
				(
					CreatePluginScriptVFile(string.Format("Edit{0}.psh", dir), "-pfn", App.PluginExists(pluginFileName) ? pluginFileName : "EamonRT.dll", "-wd", gEngine.Path.Combine("..", "..", "Adventures", dir), "-la", "-rge")
				);
			}

			VFileList = VFileList.OrderBy(psvf => psvf.Name).ToList();
		}
	}
}