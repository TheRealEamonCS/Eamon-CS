
// ViewModelBase.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using ReactiveUI;
using EamonPM.Game.Primitive.Classes;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.ViewModels
{
	public class ViewModelBase : ReactiveObject
	{
		public virtual PluginScriptFile CreatePluginScriptFile(string name, params string[] pluginArgs)
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(name));

			var pluginArgsList = new List<string>();

			if (pluginArgs != null)
			{
				pluginArgsList.AddRange(pluginArgs);
			}

			if (gEngine.IgnoreMutex)
			{
				pluginArgsList.Add("-im");
			}

			if (gEngine.DisableValidation)
			{
				pluginArgsList.Add("-dv");
			}
			
			if (gEngine.EnableScreenReaderMode)
			{
				pluginArgsList.Add("-esrm");
			}

			return new PluginScriptFile()
			{
				Name = name,

				PluginArgs = pluginArgsList.ToArray()
			};
		}
	}
}
