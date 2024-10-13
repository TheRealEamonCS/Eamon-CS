
// EamonMHViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using EamonPM.Game.Primitive.Classes;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.ViewModels
{
	public class EamonMHViewModel : ViewModelBase
	{
		public virtual IList<PluginScriptFile> FileList { get; set; }

		public EamonMHViewModel()
		{
			FileList = new List<PluginScriptFile>();

			var scriptsFileName = gEngine.Path.Combine(App.BasePath, "System", "Bin", "EAMONPM_SCRIPTS.TXT");

			try
			{
				foreach (var line in gEngine.File.ReadLines(scriptsFileName))
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						var tokens = line.Split('|').Select(t => t.Trim()).ToArray();

						if (tokens.Length > 3 && tokens[0].Equals("EamonMH", StringComparison.OrdinalIgnoreCase))
						{
							FileList.Add
							(
								CreatePluginScriptFile(tokens[1], tokens.Skip(2).ToArray())
							);
						}
					}
				}
			}
			catch (Exception)
			{
				// Do nothing
			}

			FileList = FileList.OrderBy(psf => psf.Name).ToList();
		}
	}
}
