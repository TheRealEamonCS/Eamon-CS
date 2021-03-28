
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheVileGrimoireOfJaldial.Framework.Plugin
{
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		bool EncounterSurprises { get; set; }

		/// <summary></summary>
		bool CarrionCrawlerFlails { get; set; }

		/// <summary></summary>
		long EventRoll { get; set; }

		/// <summary></summary>
		long FrequencyRoll { get; set; }

		/// <summary></summary>
		long InitiativeMonsterUid { get; set; }
	}
}
