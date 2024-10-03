
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
		public virtual IList<PluginScriptFile> FileList { get; set; }

		public EamonDDViewModel()
		{
			FileList = new List<PluginScriptFile>()
			{
				CreatePluginScriptFile("EditAdventures.psh", "-pfn", "EamonRT.dll", "-fsfn", "ADVENTURES.DAT", "-rge"),
				CreatePluginScriptFile("EditCatalog.psh", "-pfn", "EamonRT.dll", "-fsfn", "CATALOG.DAT", "-rge"),
				CreatePluginScriptFile("EditCharacters.psh", "-pfn", "EamonRT.dll", "-chrfn", "CHARACTERS.DAT", "-rge"),
				CreatePluginScriptFile("EditContemporary.psh", "-pfn", "EamonRT.dll", "-fsfn", "CONTEMPORARY.DAT", "-rge"),
				CreatePluginScriptFile("EditFantasy.psh", "-pfn", "EamonRT.dll", "-fsfn", "FANTASY.DAT", "-rge"),
				CreatePluginScriptFile("EditHorror.psh", "-pfn", "EamonRT.dll", "-fsfn", "HORROR.DAT", "-rge"),
				CreatePluginScriptFile("EditSciFi.psh", "-pfn", "EamonRT.dll", "-fsfn", "SCIFI.DAT", "-rge"),
				CreatePluginScriptFile("EditTest.psh", "-pfn", "EamonRT.dll", "-fsfn", "TEST.DAT", "-rge"),
				CreatePluginScriptFile("EditWorkbench.psh", "-pfn", "EamonRT.dll", "-fsfn", "WORKBENCH.DAT", "-rge"),
				CreatePluginScriptFile("EditWorkInProgress.psh", "-pfn", "EamonRT.dll", "-fsfn", "WIP.DAT", "-rge"),
				CreatePluginScriptFile("LoadAdventureSupportMenu.psh", "-pfn", "EamonRT.dll", "-rge")
			};

			var adventureDirs = App.GetAdventureDirs();

			foreach (var dir in adventureDirs)
			{
				var pluginFileName = string.Format("{0}.dll", dir);

				FileList.Add
				(
					CreatePluginScriptFile(string.Format("Edit{0}.psh", dir), "-pfn", App.PluginExists(pluginFileName) ? pluginFileName : "EamonRT.dll", "-wd", gEngine.Path.Combine("..", "..", "Adventures", dir), "-la", "-rge")
				);
			}

			FileList = FileList.OrderBy(psf => psf.Name).ToList();
		}
	}
}
