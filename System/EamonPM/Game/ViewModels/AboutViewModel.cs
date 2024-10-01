
// AboutViewModel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonPM.Game.ViewModels
{
	public class AboutViewModel : ViewModelBase
	{
		public virtual string BuildGuidWithPeriod
		{
			get
			{
				return string.Format("{0}.", App.BuildGuid);
			}
		}

		public virtual string ProgramNameWithParens
		{
			get
			{
				return string.Format("({0})", App.ProgramName);
			}
		}

		public AboutViewModel()
		{

		}
	}
}
