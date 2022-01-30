﻿
// CombatState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Primitive.Enums
{
	/// <summary></summary>
	public enum CombatState : long
	{
		/// <summary></summary>
		None = 0,

		/// <summary></summary>
		BeginAttack,

		/// <summary></summary>
		AttackMiss,

		/// <summary></summary>
		AttackFumble,

		/// <summary></summary>
		AttackHit,

		/// <summary></summary>
		CalculateDamage,

		/// <summary></summary>
		CheckArmor,

		/// <summary></summary>
		CheckMonsterStatus,

		/// <summary></summary>
		CheckDisguisedMonster,

		/// <summary></summary>
		CheckDeadBody,

		/// <summary></summary>
		CheckAlreadyBroken,

		/// <summary></summary>
		CheckNotBreakable,

		/// <summary></summary>
		AttackHitArtifact,

		/// <summary></summary>
		CalculateDamageArtifact,

		/// <summary></summary>
		CheckArtifactStatus,

		/// <summary></summary>
		CheckContentsSpilled,

		/// <summary></summary>
		EndAttack,

		/// <summary></summary>
		User1,

		/// <summary></summary>
		User2,

		/// <summary></summary>
		User3
	}
}
