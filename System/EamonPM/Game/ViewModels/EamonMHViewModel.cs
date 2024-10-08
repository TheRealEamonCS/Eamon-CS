
// EamonMHViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			var scriptsFileName = gEngine.Path.Combine(App.BasePath, "System", "Bin", "EAMONMH_SCRIPTS.TXT");

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

			FileList = FileList.OrderBy(psf => psf.Name).ToList();
		}
	}
}
