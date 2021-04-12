
// PluginContext.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace BeginnersCaveII.Game.Plugin
{
	public static class PluginContext
	{
		public static Framework.Plugin.IPluginGlobals Globals
		{
			get
			{
				return (Framework.Plugin.IPluginGlobals)EamonRT.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Globals = value;
			}
		}

		public static EamonRT.Framework.IEngine gEngine
		{
			get
			{
				return (EamonRT.Framework.IEngine)EamonRT.Game.Plugin.PluginContext.gEngine;
			}
		}
	}
}
