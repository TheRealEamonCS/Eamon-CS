
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
		public virtual List<BatchFile> BatchFileList { get; set; }

		public EamonDDViewModel()
		{
			BatchFileList = new List<BatchFile>()
			{
				CreateBatchFile("EditAdventures", "-pfn", "EamonRT.dll", "-fsfn", "ADVENTURES.DAT", "-rge"),
				CreateBatchFile("EditCatalog", "-pfn", "EamonRT.dll", "-fsfn", "CATALOG.DAT", "-rge"),
				CreateBatchFile("EditCharacters", "-pfn", "EamonRT.dll", "-chrfn", "CHARACTERS.DAT", "-rge"),
				CreateBatchFile("EditContemporary", "-pfn", "EamonRT.dll", "-fsfn", "CONTEMPORARY.DAT", "-rge"),
				CreateBatchFile("EditFantasy", "-pfn", "EamonRT.dll", "-fsfn", "FANTASY.DAT", "-rge"),
				CreateBatchFile("EditHorror", "-pfn", "EamonRT.dll", "-fsfn", "HORROR.DAT", "-rge"),
				CreateBatchFile("EditSciFi", "-pfn", "EamonRT.dll", "-fsfn", "SCIFI.DAT", "-rge"),
				CreateBatchFile("EditTest", "-pfn", "EamonRT.dll", "-fsfn", "TEST.DAT", "-rge"),
				CreateBatchFile("EditWorkbench", "-pfn", "EamonRT.dll", "-fsfn", "WORKBENCH.DAT", "-rge"),
				CreateBatchFile("EditWorkInProgress", "-pfn", "EamonRT.dll", "-fsfn", "WIP.DAT", "-rge"),
				CreateBatchFile("LoadAdventureSupportMenu", "-pfn", "EamonRT.dll", "-rge")
			};

			var adventureDirs = App.GetAdventureDirs();

			foreach (var dir in adventureDirs)
			{
				var pluginFileName = string.Format("{0}.dll", dir);

				BatchFileList.Add
				(
					CreateBatchFile(string.Format("Edit{0}", dir), "-pfn", App.PluginExists(pluginFileName) ? pluginFileName : "EamonRT.dll", "-wd", gEngine.Path.Combine("..", "..", "Adventures", dir), "-la", "-rge")
				);
			}

			BatchFileList = BatchFileList.OrderBy(bf => bf.Name).ToList();
		}
	}
}
