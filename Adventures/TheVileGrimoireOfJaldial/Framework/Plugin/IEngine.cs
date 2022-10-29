
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheVileGrimoireOfJaldial.Framework.Plugin
{
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		long StartHour { get; set; }

		long StartMinute { get; set; }

		long[] NonEmotingMonsterUids { get; set; }

		/// <summary></summary>
		long EventRoll { get; set; }

		/// <summary></summary>
		long FrequencyRoll { get; set; }

		/// <summary></summary>
		long InitiativeMonsterUid { get; set; }

		/// <summary></summary>
		bool EncounterSurprises { get; set; }

		/// <summary></summary>
		bool CarrionCrawlerFlails { get; set; }
	}
}
