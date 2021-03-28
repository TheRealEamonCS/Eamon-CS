
// EamonRTViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Eamon.Mobile.Models;

namespace Eamon.Mobile.ViewModels
{
	public class EamonRTViewModel : BaseViewModel
	{
		public List<BatchFile> BatchFiles { get; set; }

		public EamonRTViewModel()
		{
			Title = "EamonRT";

			BatchFiles = new List<BatchFile>();

			var adventureDirs = App.GetAdventureDirs();

			foreach (var dir in adventureDirs)
			{
				var pluginFileName = string.Format("{0}.dll", dir);

				BatchFiles.Add(new BatchFile()
				{
					Name = string.Format("Resume{0}", dir),

					PluginArgs = new string[] { "-pfn", App.PluginExists(pluginFileName) ? pluginFileName : "EamonRT.dll", "-wd", string.Format(@"..\..\Adventures\{0}", dir) }
				});
			}

			BatchFiles = BatchFiles.OrderBy(bf => bf.Name).ToList();
		}
	}
}