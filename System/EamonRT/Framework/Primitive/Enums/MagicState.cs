
// MagicState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Primitive.Enums
{
	/// <summary></summary>
	public enum MagicState : long
	{
		/// <summary></summary>
		None = 0,

		/// <summary></summary>
		BeginSpellBlast,

		/// <summary></summary>
		CheckAfterCastBlast,

		/// <summary></summary>
		AggravateMonster,

		/// <summary></summary>
		CheckAfterAggravateMonster,

		/// <summary></summary>
		AttackDobj,

		/// <summary></summary>
		BeginSpellHeal,

		/// <summary></summary>
		CheckAfterCastHeal,

		/// <summary></summary>
		HealInjury,

		/// <summary></summary>
		ShowHealthStatus,

		/// <summary></summary>
		BeginSpellSpeed,

		/// <summary></summary>
		CheckAfterCastSpeed,

		/// <summary></summary>
		BoostAgility,

		/// <summary></summary>
		CalculateSpeedTurns,

		/// <summary></summary>
		FeelEnergized,

		/// <summary></summary>
		BeginSpellPower,

		/// <summary></summary>
		CheckAfterCastPower,

		/// <summary></summary>
		SonicBoomFortuneCookie,

		/// <summary></summary>
		RaiseDeadVanishArtifacts,

		/// <summary></summary>
		TunnelCollapses,

		/// <summary></summary>
		SonicBoom,

		/// <summary></summary>
		AllWoundsHealed,

		/// <summary></summary>
		EndMagic,

		/// <summary></summary>
		User1,

		/// <summary></summary>
		User2,

		/// <summary></summary>
		User3
	}
}
