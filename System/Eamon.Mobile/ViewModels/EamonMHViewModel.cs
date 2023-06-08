
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
				CreateBatchFile("EnterMainHallUsingAdventures", "-pfn", "EamonMH.dll", "-fsfn", "ADVENTURES.DAT"),
				CreateBatchFile("EnterMainHallUsingCatalog", "-pfn", "EamonMH.dll", "-fsfn", "CATALOG.DAT")
			};
		}
	}
}
