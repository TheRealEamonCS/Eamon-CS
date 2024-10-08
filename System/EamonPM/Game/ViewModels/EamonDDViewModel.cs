
// EamonDDViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			FileList = new List<PluginScriptFile>();

			var scriptsFileName = gEngine.Path.Combine(App.BasePath, "System", "Bin", "EAMONDD_SCRIPTS.TXT");

			try
			{
				foreach (var line in gEngine.File.ReadLines(scriptsFileName))
				{
					Debug.Assert(!string.IsNullOrWhiteSpace(line));

					var tokens = line.Split('|');

					Debug.Assert(tokens.Length > 1);

					FileList.Add
					(
						CreatePluginScriptFile(tokens[0], tokens.Skip(1).ToArray())
					);
				}
			}
			catch (Exception)
			{
				// Do nothing
			}

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
