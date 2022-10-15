
// ICharacter.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Text;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
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
		Armor ArmorClass { get; set; }

		/// <summary></summary>
		ICharacterArtifact Armor { get; set; }

		/// <summary></summary>
		ICharacterArtifact Shield { get; set; }

		/// <summary></summary>
		ICharacterArtifact[] Weapons { get; set; }

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
		ICharacterArtifact GetWeapon(long index);

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
		void SetWeapon(long index, ICharacterArtifact value);

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

		/// <summary></summary>
		/// <returns></returns>
		bool IsArmorActive();

		/// <summary></summary>
		/// <returns></returns>
		bool IsShieldActive();

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool IsWeaponActive(long index);

		/// <summary>
		/// Evaluates this <see cref="ICharacter">Character</see>'s <see cref="Gender">Gender</see>, returning a value of type T.
		/// </summary>
		/// <param name="maleValue"></param>
		/// <param name="femaleValue"></param>
		/// <param name="neutralValue"></param>
		/// <returns></returns>
		T EvalGender<T>(T maleValue, T femaleValue, T neutralValue);

		/// <summary></summary>
		/// <param name="weight"></param>
		/// <param name="characterFindFunc"></param>
		/// <param name="artifactFindFunc"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> characterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <param name="baseOddsToHit"></param>
		/// <returns></returns>
		RetCode GetBaseOddsToHit(ICharacterArtifact weapon, ref long baseOddsToHit);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="baseOddsToHit"></param>
		/// <returns></returns>
		RetCode GetBaseOddsToHit(long index, ref long baseOddsToHit);

		/// <summary></summary>
		/// <param name="count"></param>
		/// <returns></returns>
		RetCode GetWeaponCount(ref long count);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="capitalize"></param>
		/// <returns></returns>
		RetCode ListWeapons(StringBuilder buf, bool capitalize = true);

		/// <summary></summary>
		void StripUniqueCharsFromWeaponNames();

		/// <summary></summary>
		void AddUniqueCharsToWeaponNames();

		/// <summary></summary>
		/// <param name="character"></param>
		/// <returns></returns>
		RetCode CopyProperties(ICharacter character);

		#endregion
	}
}
