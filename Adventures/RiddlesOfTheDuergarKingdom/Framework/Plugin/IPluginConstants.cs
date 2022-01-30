
// IPluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace RiddlesOfTheDuergarKingdom.Framework.Plugin
{
	public interface IPluginConstants : EamonRT.Framework.Plugin.IPluginConstants
	{
		/// <summary></summary>
		long PoisonInjuryTurns { get; }

		/// <summary></summary>
		long IbexAbandonTurns { get; }
	}
}
