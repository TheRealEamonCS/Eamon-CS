
// EamonMHViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Mobile.Models;

namespace Eamon.Mobile.ViewModels
{
	public class EamonMHViewModel : BaseViewModel
	{

		public List<BatchFile> BatchFiles { get; set; }

		public EamonMHViewModel()
		{
			Title = "EamonMH";

			BatchFiles = new List<BatchFile>()
			{
				new BatchFile()
				{
					Name = "EnterMainHallUsingAdventures",

					PluginArgs = new string[] { "-pfn", "EamonMH.dll", "-fsfn", "ADVENTURES.DAT" }
				},
				new BatchFile()
				{
					Name = "EnterMainHallUsingCatalog",

					PluginArgs = new string[] { "-pfn", "EamonMH.dll", "-fsfn", "CATALOG.DAT" }
				}
			};
		}

	}
}