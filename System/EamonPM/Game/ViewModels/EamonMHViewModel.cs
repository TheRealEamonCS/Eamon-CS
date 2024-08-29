
// EamonMHViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using EamonPM.Game.Primitive.Classes;

namespace EamonPM.Game.ViewModels
{
	public class EamonMHViewModel : ViewModelBase
	{
		public virtual List<BatchFile> BatchFileList { get; set; }

		public EamonMHViewModel()
		{
			BatchFileList = new List<BatchFile>() 
			{
				CreateBatchFile("EnterMainHallUsingAdventures", "-pfn", "EamonMH.dll", "-fsfn", "ADVENTURES.DAT"),
				CreateBatchFile("EnterMainHallUsingCatalog", "-pfn", "EamonMH.dll", "-fsfn", "CATALOG.DAT")
			};
		}
	}
}
