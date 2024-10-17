
// EamonDDViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
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
			FileList = new List<PluginScriptFile>();

			var scriptsFileName = gEngine.Path.Combine(App.BasePath, "System", "Bin", "EAMONPM_SCRIPTS.TXT");

			try
			{
				var lines = gEngine.File.ReadAllLines(scriptsFileName);

				foreach (var line in lines)
				{
					if (!string.IsNullOrWhiteSpace(line))
					{
						var tokens = line.Split('|').Select(t => t.Trim()).ToArray();

						if (tokens.Length > 3 && tokens[0].Equals("EamonDD", StringComparison.OrdinalIgnoreCase))
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
