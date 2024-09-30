
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
		public virtual IList<PluginScript> NodeList { get; set; }

		public EamonRTViewModel()
		{
			NodeList = new List<PluginScript>();

			var adventureDirs = App.GetAdventureDirs();

			foreach (var dir in adventureDirs)
			{
				var pluginFileName = string.Format("{0}.dll", dir);

				NodeList.Add
				(
					CreatePluginScript(string.Format("Resume{0}", dir), "-pfn", App.PluginExists(pluginFileName) ? pluginFileName : "EamonRT.dll", "-wd", gEngine.Path.Combine("..", "..", "Adventures", dir))
				);
			}

			NodeList = NodeList.OrderBy(ps => ps.Name).ToList();
		}
	}
}
