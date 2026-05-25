
// FocusCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// Defines the target selection strategy used by a Monster when repositioning in range-based combat.
	/// </summary>
	public enum FocusCode : long
	{
		/// <summary></summary>
		None = 0,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> ranges toward the closest valid target.
		/// This is the default behavior.
		/// </summary>
		Closest,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> ranges toward the farthest valid target,
		/// forcing it to cross the battlefield to reach its positioning goal.
		/// </summary>
		Farthest,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> ranges toward the target with the lowest
		/// <see cref="IMonster.Hardiness">Hardiness</see> — focuses on easier kills.
		/// </summary>
		Weakest,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> ranges toward the target with the highest
		/// <see cref="IMonster.Hardiness">Hardiness</see> — focuses on the biggest threat.
		/// </summary>
		Strongest,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> ranges toward the target with the highest
		/// injury percentage — finishes off wounded enemies.
		/// </summary>
		MostInjured,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> ranges toward the target with the lowest
		/// injury percentage — ignores wounded, targets healthy enemies.
		/// </summary>
		LeastInjured,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> randomly selects a target from all valid
		/// candidates each time a new target is chosen. Produces unpredictable behavior.
		/// </summary>
		Random,

		/// <summary></summary>
		User1,

		/// <summary></summary>
		User2,

		/// <summary></summary>
		User3
	}
}
