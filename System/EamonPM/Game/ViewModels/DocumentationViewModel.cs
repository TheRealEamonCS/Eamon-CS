
// DocumentationViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonPM.Game.ViewModels
{
	public class DocumentationViewModel : ViewModelBase
	{
		public virtual IList<string> VFileList { get; set; }

		public DocumentationViewModel()
		{
			VFileList = new List<string>()
			{
				"ViewDocumentationUsingBrowser.psh"
			};
		}
	}
}
