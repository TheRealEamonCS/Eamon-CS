
// EamonRTViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using EamonPM.Game.Primitive.Classes;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.ViewModels
{
	public class EamonRTViewModel : ViewModelBase
	{
		public virtual List<BatchFile> BatchFileList { get; set; }

		public EamonRTViewModel()
		{
			BatchFileList = new List<BatchFile>();

			var adventureDirs = App.GetAdventureDirs();

			foreach (var dir in adventureDirs)
			{
				var pluginFileName = string.Format("{0}.dll", dir);

				BatchFileList.Add
				(
					CreateBatchFile(string.Format("Resume{0}", dir), "-pfn", App.PluginExists(pluginFileName) ? pluginFileName : "EamonRT.dll", "-wd", gEngine.Path.Combine("..", "..", "Adventures", dir))
				);
			}

			BatchFileList = BatchFileList.OrderBy(bf => bf.Name).ToList();
		}
	}
}
