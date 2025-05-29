
// Character.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game
{
	[ClassMappings]
	public class Character : GameBase, ICharacter
	{
		#region Public Fields

		public long _heldGold;

		public long _bankGold;

		#endregion

		#region Public Properties

		#region Interface ICharacter

		[FieldName(620)]
		public virtual Gender Gender { get; set; }

		[FieldName(640)]
		public virtual Status Status { get; set; }

		[FieldName(660)]
		public virtual long[] Stats { get; set; }

		[FieldName(680)]
		public virtual long[] SpellAbilities { get; set; }

		[FieldName(700)]
		public virtual long[] WeaponAbilities { get; set; }

		[FieldName(720)]
		public virtual long ArmorExpertise { get; set; }

		[FieldName(740)]
		public virtual long HeldGold
		{
			get
			{
				return _heldGold;
			}

			set
			{
				_heldGold = value;

				NormalizeGoldValues();
			}
		}

		[FieldName(760)]
		public virtual long BankGold
		{
			get
			{
				return _bankGold;
			}

			set
			{
				_bankGold = value;

				NormalizeGoldValues();
			}
		}

		[ExcludeFromSerialization]
		[FieldName(780)]
		public virtual Armor ArmorClass
		{
			get
			{
				var armorArtifact = GetWornList().FirstOrDefault(a => a.Wearable.Field1 >= 2);

				var shieldArtifact = GetWornList().FirstOrDefault(a => a.Wearable.Field1 == 1);

				var ar = armorArtifact != null ? armorArtifact.Wearable.Field1 : 0;

				var sh = shieldArtifact != null ? shieldArtifact.Wearable.Field1 : 0;

				return (Armor)(ar + sh);
			}
		}

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Get rid of managed resources
			}

			if (Uid > 0)
			{
				gDatabase.FreeCharacterUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IGameBase

		public override string GetPluralName(string fieldName)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			var buf = new StringBuilder(gEngine.BufSize);

			buf.Append(Name);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool showContents = false, bool groupCountOne = false)
		{
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			var buf = new StringBuilder(gEngine.BufSize);

			buf.Append(Name);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName, bool showVerboseName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var name = GetArticleName(true);

			if (showName)
			{
				buf.AppendFormat("{0}[{1}]",
					Environment.NewLine,
					name);
			}

			buf.AppendPrint("You are the {0} {1}.",
				EvalGender("Mighty", "Fair", "Androgynous"),
				name);

		Cleanup:

			return rc;
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(ICharacter character)
		{
			return this.Uid.CompareTo(character.Uid);
		}

		#endregion

		#region Interface ICharacter

		public virtual long GetStat(long index)
		{
			return Stats[index];
		}

		public virtual long GetStat(Stat stat)
		{
			return GetStat((long)stat);
		}

		public virtual long GetSpellAbility(long index)
		{
			return SpellAbilities[index];
		}

		public virtual long GetSpellAbility(Spell spell)
		{
			return GetSpellAbility((long)spell);
		}

		public virtual long GetWeaponAbility(long index)
		{
			return WeaponAbilities[index];
		}

		public virtual long GetWeaponAbility(Weapon weapon)
		{
			return GetWeaponAbility((long)weapon);
		}

		public virtual string GetSynonym(long index)
		{
			return Synonyms[index];
		}

		public virtual void SetStat(long index, long value)
		{
			Stats[index] = value;
		}

		public virtual void SetStat(Stat stat, long value)
		{
			SetStat((long)stat, value);
		}

		public virtual void SetSpellAbility(long index, long value)
		{
			SpellAbilities[index] = value;
		}

		public virtual void SetSpellAbility(Spell spell, long value)
		{
			SetSpellAbility((long)spell, value);
		}

		public virtual void SetWeaponAbility(long index, long value)
		{
			WeaponAbilities[index] = value;
		}

		public virtual void SetWeaponAbility(Weapon weapon, long value)
		{
			SetWeaponAbility((long)weapon, value);
		}

		public virtual void SetSynonym(long index, string value)
		{
			Synonyms[index] = value;
		}

		public virtual void ModStat(long index, long value)
		{
			Stats[index] += value;
		}

		public virtual void ModStat(Stat stat, long value)
		{
			ModStat((long)stat, value);
		}

		public virtual void ModSpellAbility(long index, long value)
		{
			SpellAbilities[index] += value;
		}

		public virtual void ModSpellAbility(Spell spell, long value)
		{
			ModSpellAbility((long)spell, value);
		}

		public virtual void ModWeaponAbility(long index, long value)
		{
			WeaponAbilities[index] += value;
		}

		public virtual void ModWeaponAbility(Weapon weapon, long value)
		{
			ModWeaponAbility((long)weapon, value);
		}

		public virtual long GetWeightCarryableGronds()
		{
			return gEngine.GetWeightCarryableGronds(GetStat(Stat.Hardiness));
		}

		public virtual long GetWeightCarryableDos()
		{
			return gEngine.GetWeightCarryableDos(GetStat(Stat.Hardiness));
		}

		public virtual long GetIntellectBonusPct()
		{
			return gEngine.GetIntellectBonusPct(GetStat(Stat.Intellect));
		}

		public virtual long GetCharmMonsterPct()
		{
			return gEngine.GetCharmMonsterPct(GetStat(Stat.Charisma));
		}

		public virtual long GetMerchantAdjustedCharisma()
		{
			return gEngine.GetMerchantAdjustedCharisma(GetStat(Stat.Charisma));
		}

		public virtual T EvalGender<T>(T maleValue, T femaleValue, T neutralValue)
		{
			return gEngine.EvalGender(Gender, maleValue, femaleValue, neutralValue);
		}

		public virtual IList<IArtifact> GetCarriedList(Func<IArtifact, bool> characterFindFunc = null)
		{
			if (characterFindFunc == null)
			{
				characterFindFunc = a => a.IsCarriedByCharacter(this);
			}

			IList<IArtifact> artifactList = null;

			gDatabase.ExecuteOnArtifactTable(ArtifactTableType.CharArt, () =>
			{
				artifactList = gEngine.GetArtifactList(a => characterFindFunc(a));
			});

			return artifactList;
		}

		public virtual IList<IArtifact> GetWornList(Func<IArtifact, bool> characterFindFunc = null)
		{
			if (characterFindFunc == null)
			{
				characterFindFunc = a => a.IsWornByCharacter(this);
			}

			IList<IArtifact> artifactList = null;

			gDatabase.ExecuteOnArtifactTable(ArtifactTableType.CharArt, () =>
			{
				artifactList = gEngine.GetArtifactList(a => characterFindFunc(a));
			});

			return artifactList;
		}

		public virtual IList<IArtifact> GetContainedList(Func<IArtifact, bool> characterFindFunc = null)
		{
			if (characterFindFunc == null)
			{
				characterFindFunc = a => a.IsCarriedByCharacter(this) || a.IsWornByCharacter(this);
			}

			IList<IArtifact> artifactList = null;

			gDatabase.ExecuteOnArtifactTable(ArtifactTableType.CharArt, () =>
			{
				artifactList = gEngine.GetArtifactList(a => characterFindFunc(a));
			});

			return artifactList;
		}

		public virtual RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> characterFindFunc = null)
		{
			RetCode rc;

			rc = RetCode.Success;

			var artifactList = GetContainedList(characterFindFunc);

			foreach (var a in artifactList)
			{
				if (!a.IsUnmovable01())
				{
					weight += a.Weight;
				}
			}

			return rc;
		}

		public virtual RetCode GetBaseOddsToHit(IArtifact weapon, ref long baseOddsToHit)
		{
			long ar1, sh1, af, x, a, d, f, odds;
			RetCode rc;

			if (weapon == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (weapon.GeneralWeapon == null)
			{
				baseOddsToHit = 0;

				goto Cleanup;
			}

			/* 
				Full Credit:  Derived wholly from Frank Black's Eamon Deluxe

				File: MAINHALL.BAS
				SUB	: Examine.Character
			*/

			/* COMPUTE ARMOR FACTOR */

			ar1 = (long)ArmorClass / 2;

			sh1 = (long)ArmorClass % 2;

			f = ar1;

			if (f > 3)
			{
				f = 3;
			}

			af = (-5 * sh1) - f * 10;

			if (f == 3)
			{
				af -= 30;
			}

			/* COMPUTE BASE ODDS TO HIT */

			x = GetStat(Stat.Agility);

			a = GetWeaponAbility((Weapon)weapon.Field2);

			d = weapon.Field1;

			if (x > 30)
			{
				x = 30;
			}

			if (a > 122)
			{
				a = 122;
			}

			if (d > 50)
			{
				d = 50;
			}

			odds = 50 + 2 * (x - (ar1 + sh1));

			odds = (long)Math.Round((double)odds + ((double)d / 2.0));

			odds += ((af + ArmorExpertise) * (-af > ArmorExpertise ? 1 : 0));

			odds = (long)Math.Round((double)odds + ((double)a / 4.0));

			/*
			if (odds > 100)
			{
				odds = 100;
			}
			*/

			baseOddsToHit = odds;

		Cleanup:

			return rc;
		}

		public virtual RetCode ListWeapons(StringBuilder buf, bool capitalize = true)
		{
			RetCode rc;
			long i;

			IWeapon weapon;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var artifactList = GetCarriedList().Where(a => a.GeneralWeapon != null).OrderBy(a => a.Uid).ToList();

			for (i = 0; i < artifactList.Count; i++)
			{
				var artifact = artifactList[(int)i];

				Debug.Assert(artifact != null);

				weapon = gEngine.GetWeapon((Weapon)artifact.Field2);

				Debug.Assert(weapon != null);

				buf.AppendFormat("{0}{1,2}. {2} ({3,-8}/{4,3}%/{5,2}D{6,-2}/{7}H)",
					Environment.NewLine,
					i + 1,
					capitalize ? gEngine.Capitalize(artifact.Name.PadTRight(31, ' ')) : artifact.Name.PadTRight(31, ' '),
					weapon.Name,
					artifact.Field1,
					artifact.Field3,
					artifact.Field4,
					artifact.Field5);
			}

		Cleanup:

			return rc;
		}

		public virtual bool StripUniqueCharsFromWeaponNames()
		{
			var artifactList = GetCarriedList().Where(a => a.GeneralWeapon != null).ToList();

			return gEngine.StripUniqueCharsFromRecordNames(artifactList.Cast<IGameBase>().ToList());
		}

		public virtual bool AddUniqueCharsToWeaponNames()
		{
			var artifactList = GetCarriedList().Where(a => a.GeneralWeapon != null).ToList();

			return gEngine.AddUniqueCharsToRecordNames(artifactList.Cast<IGameBase>().ToList());
		}

		#endregion

		#region Class Character

		public virtual void NormalizeGoldValues()
		{
			if (gEngine.EnableMutateProperties)
			{
				// Extinguish HeldGold debt with BankGold assets

				if (_heldGold < 0 && _bankGold > 0)
				{
					if (_heldGold + _bankGold >= 0)
					{
						_bankGold += _heldGold;

						_heldGold = 0;
					}
					else
					{
						_heldGold += _bankGold;

						_bankGold = 0;
					}
				}

				// Extinguish BankGold debt with HeldGold assets

				if (_bankGold < 0 && _heldGold > 0)
				{
					if (_bankGold + _heldGold >= 0)
					{
						_heldGold += _bankGold;

						_bankGold = 0;
					}
					else
					{
						_bankGold += _heldGold;

						_heldGold = 0;
					}
				}

				// Move remaining debt to HeldGold if possible

				if (_bankGold < 0)
				{
					_heldGold += _bankGold;

					if (_heldGold < gEngine.MinGoldValue)
					{
						_bankGold = _heldGold - gEngine.MinGoldValue;

						_heldGold = gEngine.MinGoldValue;
					}
					else
					{
						_bankGold = 0;
					}
				}
			}

			// Force values into valid range

			if (_heldGold < gEngine.MinGoldValue)
			{
				_heldGold = gEngine.MinGoldValue;
			}
			else if (_heldGold > gEngine.MaxGoldValue)
			{
				_heldGold = gEngine.MaxGoldValue;
			}

			if (_bankGold < gEngine.MinGoldValue)
			{
				_bankGold = gEngine.MinGoldValue;
			}
			else if (_bankGold > gEngine.MaxGoldValue)
			{
				_bankGold = gEngine.MaxGoldValue;
			}
		}

		public Character()
		{
			Stats = new long[(long)EnumUtil.GetLastValue<Stat>() + 1];

			SpellAbilities = new long[(long)EnumUtil.GetLastValue<Spell>() + 1];

			WeaponAbilities = new long[(long)EnumUtil.GetLastValue<Weapon>() + 1];
		}

		#endregion

		#endregion
	}
}
