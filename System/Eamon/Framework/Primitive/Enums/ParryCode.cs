
// ParryCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	// Note: this enum must be synchronized with the following:
	//	1. Monster.cs - GetParryAdjustment method, switch statement
	//	2. MonsterHelper.cs - InputParryTurns method, parryCodeTurns array
	//	3. Engine.cs - Engine constructor, ParryCodeDescs array

	/// <summary></summary>
	public enum ParryCode : long
	{
		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting never varies.
		/// </summary>
		NeverVaries = 0,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting is randomly selected periodically.
		/// </summary>
		Random,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting trends to offensive when uninjured and defensive when injured.
		/// </summary>
		OffenseToDefense,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting trends to defensive when uninjured and offensive when injured.
		/// </summary>
		DefenseToOffense,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting trends to natural (initial) preferences.
		/// </summary>
		TrendToPreferred,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting mirrors the player's Parry setting.
		/// </summary>
		MirrorPlayer,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting counters the player's Parry setting.
		/// </summary>
		CounterPlayer,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting alternates between extreme offensive and defensive states.
		/// </summary>
		Alternating,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting becomes more defensive with multiple enemies, more offensive against single targets.
		/// </summary>
		CrowdAware,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting becomes more offensive at close range, more defensive at long range.
		/// </summary>
		RangeDependent,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting becomes increasingly offensive as combat progresses.
		/// </summary>
		ProgressivelyAggressive,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting has opposite values to nearby allied monsters.
		/// </summary>
		CoordinatedTeam,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting changes based on special ability availability.
		/// </summary>
		AbilityDependent,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting varies based on environmental conditions.
		/// </summary>
		EnvironmentDependent,

		/// <summary>
		/// The <see cref="IMonster">Monster</see>'s Parry setting copies the strongest nearby allied monster.
		/// </summary>
		PackMentality,

		/// <summary></summary>
		User1,

		/// <summary></summary>
		User2,

		/// <summary></summary>
		User3
	}
}
