
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace RiddlesOfTheDuergarKingdom.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{
		public virtual bool BlackSpiderJumps { get; set; }

		public virtual bool PlayerAttacksBlackSpider { get; set; }

		public virtual bool EnemyInExitedDarkRoom { get; set; }
	}
}
