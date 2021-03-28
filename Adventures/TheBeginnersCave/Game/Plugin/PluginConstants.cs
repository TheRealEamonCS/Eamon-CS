
// PluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheBeginnersCave.Game.Plugin
{
	public class PluginConstants : EamonRT.Game.Plugin.PluginConstants, Framework.Plugin.IPluginConstants
	{
		public virtual string AlightDesc { get; protected set; } = "(alight)";
	}
}
