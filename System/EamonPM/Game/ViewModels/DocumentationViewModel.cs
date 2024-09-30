
// DocumentationViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonPM.Game.ViewModels
{
	public class DocumentationViewModel : ViewModelBase
	{
		public virtual List<string> NodeList { get; set; }

		public DocumentationViewModel()
		{
			NodeList = new List<string>()
			{
				"ViewDocumentationUsingBrowser"
			};
		}
	}
}
