
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheDeepCanyon.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		long ResurrectMonsterUid { get; set; }
	}
}
