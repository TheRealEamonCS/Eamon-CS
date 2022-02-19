
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace WrenholdsSecretVigil.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{
		public virtual Action MonsterCurseFunc { get; set; }

		public virtual bool MonsterCurses { get; set; }

		public virtual bool DeviceOpened { get; set; }
	}
}
