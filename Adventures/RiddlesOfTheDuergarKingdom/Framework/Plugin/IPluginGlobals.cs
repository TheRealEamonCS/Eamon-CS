
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace RiddlesOfTheDuergarKingdom.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		bool BlackSpiderJumps { get; set; }

		/// <summary></summary>
		bool PlayerAttacksBlackSpider { get; set; }

		/// <summary></summary>
		bool EnemyInExitedDarkRoom { get; set; }
	}
}
