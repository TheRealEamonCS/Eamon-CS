
// ICombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;

namespace EamonRT.Framework.Combat
{
	/// <summary></summary>
	public interface ICombatSystem
	{
		/// <summary>
		/// Gets or sets the function used to set the <see cref="IStateSignatures.NextState">NextState</see> property of the Eamon CS
		/// game engine's current <see cref="IState">State</see> or <see cref="ICommand">Command</see>.
		/// </summary>
		Action<IState> SetNextStateFunc { get; set; }

		/// <summary>
		/// Gets or sets the offending (attacking) <see cref="IMonster">Monster</see> for this <see cref="ICombatSystem">CombatSystem</see>.
		/// </summary>
		IMonster OfMonster { get; set; }

		/// <summary>
		/// Gets or sets the defending <see cref="IMonster">Monster</see> for this <see cref="ICombatSystem">CombatSystem</see>.
		/// </summary>
		IMonster DfMonster { get; set; }

		/// <summary>
		/// Gets or sets which member of <see cref="OfMonster">OfMonster</see>'s group is attacking (always 1 for single <see cref="IMonster">Monster</see>s).
		/// </summary>
		long MemberNumber { get; set; }

		/// <summary>
		/// Gets or sets which attack number the <see cref="OfMonster">OfMonster</see>'s <see cref="MemberNumber">group member</see> is performing (may
		/// be >= 1 based on <see cref="IMonster.AttackCount">AttackCount</see>).
		/// </summary>
		long AttackNumber { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ICombatSystem">CombatSystem</see> was created to inflict <see cref="Spell.Blast">Blast</see>
		/// spell damage.
		/// </summary>
		bool BlastSpell { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ICombatSystem">CombatSystem</see> should use "attacks" in its combat description.
		/// </summary>
		bool UseAttacks { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ICombatSystem">CombatSystem</see> always calculates initial damage done to be the maximum allowed.
		/// </summary>
		/// <remarks>
		/// When this is <c>true</c> initial damage done is set to (D*S) where D=Dice and S=Sides.
		/// </remarks>
		bool MaxDamage { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether <see cref="DfMonster">DfMonster</see>'s armor should be omitted from damage calculations.
		/// </summary>
		bool OmitArmor { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ICombatSystem">CombatSystem</see> should omit skill gains if the player
		/// character's attack is successful.
		/// </summary>
		bool OmitSkillGains { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ICombatSystem">CombatSystem</see> should omit printing <see cref="DfMonster">DfMonster</see>'s
		/// health status after damage is calculated.
		/// </summary>
		bool OmitMonsterStatus { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ICombatSystem">CombatSystem</see> should print a final newline after
		/// processing completes.
		/// </summary>
		bool OmitFinalNewLine { get; set; }

		/// <summary></summary>
		AttackResult FixedResult { get; set; }

		/// <summary></summary>
		WeaponRevealType WeaponRevealType { get; set; }

		/// <summary></summary>
		/// <param name="numDice"></param>
		/// <param name="numSides"></param>
		/// <param name="mod"></param>
		void ExecuteCalculateDamage(long numDice, long numSides, long mod = 0);

		/// <summary></summary>
		void ExecuteCheckMonsterStatus();

		/// <summary></summary>
		void ExecuteAttack();
	}
}
