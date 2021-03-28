
// DocumentationViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Eamon.Mobile.Models;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Mobile.ViewModels
{
	public class DocumentationViewModel : BaseViewModel
	{
		public List<BatchFile> BatchFiles { get; set; }

		public DocumentationViewModel()
		{
			Title = "Documentation";

			BatchFiles = new List<BatchFile>();

			BatchFiles.Add(new BatchFile()
			{
				Name = "ViewDocumentationUsingBrowser",

				FileName = "https://TheRealEamonCS.github.io"
			});

			BatchFiles = BatchFiles.OrderBy(bf => bf.Name).ToList();
		}

	}
}