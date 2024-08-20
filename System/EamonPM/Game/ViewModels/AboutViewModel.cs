
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

		public string BuildGuidWithPeriod
		{
			get
			{
				return string.Format("{0}.", BuildGuid);
			}
		}

		public string ProgramName
		{
			get
			{
				return App.ProgramName;
			}
		}

		public string ProgramNameWithParens
		{
			get
			{
				return string.Format("({0})", ProgramName);
			}
		}

		public AboutViewModel()
		{

		}
	}
}
