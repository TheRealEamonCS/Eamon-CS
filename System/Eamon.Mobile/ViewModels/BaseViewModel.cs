
// BaseViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Mobile.Helpers;
using Eamon.Mobile.Models;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Mobile.ViewModels
{
	public class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// Private backing field to hold the title
		/// </summary>
		string title = string.Empty;
		/// <summary>
		/// Public property to set and get the title of the item
		/// </summary>
		public string Title
		{
			get { return title; }
			set { SetProperty(ref title, value); }
		}

		public virtual BatchFile CreateBatchFile(string name, params string[] pluginArgs)
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
			
			if (gEngine.RepaintWindow)
			{
				pluginArgsList.Add("-rw");
			}

			if (gEngine.EnableScreenReaderMode)
			{
				pluginArgsList.Add("-esrm");
			}

			return new BatchFile()
			{
				Name = name,

				PluginArgs = pluginArgsList.ToArray()
			};
		}
	}
}
