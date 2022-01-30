
// PluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace RiddlesOfTheDuergarKingdom.Game.Plugin
{
	public class PluginConstants : EamonRT.Game.Plugin.PluginConstants, Framework.Plugin.IPluginConstants
	{
		public virtual long PoisonInjuryTurns { get; protected set; } = 7;

		public virtual long IbexAbandonTurns { get; protected set; } = 15;
	}
}
