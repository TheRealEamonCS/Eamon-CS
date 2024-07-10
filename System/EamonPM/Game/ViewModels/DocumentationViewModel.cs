
// DocumentationViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonPM.Game.ViewModels
{
	public class DocumentationViewModel : ViewModelBase
	{
		public List<string> BrowserList { get; set; }

		public DocumentationViewModel()
		{
			BrowserList = new List<string>()
			{
				"ViewDocumentationUsingBrowser"
			};
		}
	}
}
