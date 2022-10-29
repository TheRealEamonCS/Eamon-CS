
// HtmlFileViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Xamarin.Forms;
using Eamon.Mobile.Models;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Mobile.ViewModels
{
	public class HtmlFileViewModel : BaseViewModel
	{
		public HtmlWebViewSource _outputHtmlWebViewSource;

		public virtual HtmlWebViewSource OutputHtmlWebViewSource
		{
			get
			{
				return _outputHtmlWebViewSource;
			}
			set
			{
				SetProperty(ref _outputHtmlWebViewSource, value);
			}
		}

		public HtmlFileViewModel(BatchFile batchFile)
		{
			Debug.Assert(batchFile != null && batchFile.Name != null && batchFile.FileName != null);

			Title = batchFile.Name;

			OutputHtmlWebViewSource = new HtmlWebViewSource()
			{
				Html = gEngine.File.ReadAllText(batchFile.FileName)
			};
		}
	}
}