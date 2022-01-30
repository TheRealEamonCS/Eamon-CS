
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheDeepCanyon.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{
		public virtual long ResurrectMonsterUid { get; set; }
	}
}
