
// ICharacter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	/// <remarks></remarks>
	public interface ICharacter : IGameBase, IComparable<ICharacter>
	{
		#region Properties

		/// <summary>
		/// Gets or sets this <see cref="ICharacter">Character</see>'s gender.
		/// </summary>
		Gender Gender { get; set; }

		/// <summary></summary>
		Status Status { get; set; }

		/// <summary></summary>
		long[] Stats { get; set; }

		/// <summary></summary>
		long[] SpellAbilities { get; set; }

		/// <summary></summary>
		long[] WeaponAbilities { get; set; }

		/// <summary></summary>
		long ArmorExpertise { get; set; }

		/// <summary></summary>
		long HeldGold { get; set; }

		/// <summary></summary>
		long BankGold { get; set; }

		/// <summary></summary>
		Armor ArmorClass { get; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetStat(long index);

		/// <summary></summary>
		/// <param name="stat"></param>
		/// <returns></returns>
		long GetStat(Stat stat);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetSpellAbility(long index);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <returns></returns>
		long GetSpellAbility(Spell spell);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetWeaponAbility(long index);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <returns></returns>
		long GetWeaponAbility(Weapon weapon);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetSynonym(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetStat(long index, long value);

		/// <summary></summary>
		/// <param name="stat"></param>
		/// <param name="value"></param>
		void SetStat(Stat stat, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetSpellAbility(long index, long value);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <param name="value"></param>
		void SetSpellAbility(Spell spell, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetWeaponAbility(long index, long value);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <param name="value"></param>
		void SetWeaponAbility(Weapon weapon, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetSynonym(long index, string value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void ModStat(long index, long value);

		/// <summary></summary>
		/// <param name="stat"></param>
		/// <param name="value"></param>
		void ModStat(Stat stat, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void ModSpellAbility(long index, long value);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <param name="value"></param>
		void ModSpellAbility(Spell spell, long value);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void ModWeaponAbility(long index, long value);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <param name="value"></param>
		void ModWeaponAbility(Weapon weapon, long value);

		/// <summary></summary>
		/// <returns></returns>
		long GetWeightCarryableGronds();

		/// <summary></summary>
		/// <returns></returns>
		long GetWeightCarryableDos();

		/// <summary></summary>
		/// <returns></returns>
		long GetIntellectBonusPct();

		/// <summary></summary>
		/// <returns></returns>
		long GetCharmMonsterPct();

		/// <summary></summary>
		/// <returns></returns>
		long GetMerchantAdjustedCharisma();

		/// <summary>
		/// Evaluates this <see cref="ICharacter">Character</see>'s <see cref="Gender">Gender</see>, returning a value of type T.
		/// </summary>
		/// <param name="maleValue"></param>
		/// <param name="femaleValue"></param>
		/// <param name="neutralValue"></param>
		/// <returns></returns>
		T EvalGender<T>(T maleValue, T femaleValue, T neutralValue);

		/// <summary></summary>
		/// <param name="characterFindFunc"></param>
		/// <returns></returns>
		IList<IArtifact> GetCarriedList(Func<IArtifact, bool> characterFindFunc = null);

		/// <summary></summary>
		/// <param name="characterFindFunc"></param>
		/// <returns></returns>
		IList<IArtifact> GetWornList(Func<IArtifact, bool> characterFindFunc = null);

		/// <summary></summary>
		/// <param name="characterFindFunc"></param>
		/// <returns></returns>
		IList<IArtifact> GetContainedList(Func<IArtifact, bool> characterFindFunc = null);

		/// <summary></summary>
		/// <param name="weight"></param>
		/// <param name="characterFindFunc"></param>
		/// <returns></returns>
		RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> characterFindFunc = null);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <param name="baseOddsToHit"></param>
		/// <returns></returns>
		RetCode GetBaseOddsToHit(IArtifact weapon, ref long baseOddsToHit);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="capitalize"></param>
		/// <returns></returns>
		RetCode ListWeapons(StringBuilder buf, bool capitalize = true);

		/// <summary></summary>
		/// <returns></returns>
		bool StripUniqueCharsFromWeaponNames();

		/// <summary></summary>
		/// <returns></returns>
		bool AddUniqueCharsToWeaponNames();

		#endregion
	}
}
