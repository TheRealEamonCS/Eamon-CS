
// IPluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheVileGrimoireOfJaldial.Framework.Plugin
{
	public interface IPluginConstants : EamonRT.Framework.Plugin.IPluginConstants
	{
		long StartHour { get; set; }

		long StartMinute { get; set; }

		long[] NonEmotingMonsterUids { get; set; }
	}
}
