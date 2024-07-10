
// AboutViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonPM.Game.ViewModels
{
	public class AboutViewModel : ViewModelBase
	{
		public string BuildGuid
		{
			get
			{
				return App.BuildGuid;
			}
		}

		public string ProgramName
		{
			get
			{
				return App.ProgramName;
			}
		}

		public AboutViewModel()
		{

		}
	}
}
