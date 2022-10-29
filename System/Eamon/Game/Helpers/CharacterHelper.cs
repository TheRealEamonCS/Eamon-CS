
// CharacterHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Helpers.Generic;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.Helpers
{
	[ClassMappings]
	public class CharacterHelper : Helper<ICharacter>, ICharacterHelper
	{
		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			return true;
		}

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameStatsElement()
		{
			var i = Index;

			var stat = gEngine.GetStat((Stat)i);

			Debug.Assert(stat != null);

			return stat.Name;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameSpellAbilitiesElement()
		{
			var i = Index;

			var spell = gEngine.GetSpell((Spell)i);

			Debug.Assert(spell != null);

			return string.Format("{0} Spell Ability", spell.Name);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = gEngine.GetWeapon((Weapon)i);

			Debug.Assert(weapon != null);

			return string.Format("{0} Wpn Ability", weapon.Name);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameArmorExpertise()
		{
			return "Armor Expertise";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameHeldGold()
		{
			return "Held Gold";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameBankGold()
		{
			return "Bank Gold";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameArmorClass()
		{
			return "Armor Class";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsName()
		{
			var i = Index;

			return string.Format("Wpn #{0} Name", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsIsPlural()
		{
			var i = Index;

			return string.Format("Wpn #{0} Is Plural", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsPluralType()
		{
			var i = Index;

			return string.Format("Wpn #{0} Plural Type", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsArticleType()
		{
			var i = Index;

			return string.Format("Wpn #{0} Article Type", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsField1()
		{
			var i = Index;

			return string.Format("Wpn #{0} Complexity", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsField2()
		{
			var i = Index;

			return string.Format("Wpn #{0} Type", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsField3()
		{
			var i = Index;

			return string.Format("Wpn #{0} Dice", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsField4()
		{
			var i = Index;

			return string.Format("Wpn #{0} Sides", i + 1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeaponsField5()
		{
			var i = Index;

			return string.Format("Wpn #{0} Number Of Hands", i + 1);
		}

		#endregion

		#region GetName Methods

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameStats(bool addToNameList)
		{
			var statValues = EnumUtil.GetValues<Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				GetName("StatsElement", addToNameList);
			}

			return "Stats";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameStatsElement(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Stats[{0}].Element", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameSpellAbilities(bool addToNameList)
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				GetName("SpellAbilitiesElement", addToNameList);
			}

			return "SpellAbilities";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameSpellAbilitiesElement(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("SpellAbilities[{0}].Element", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponAbilities(bool addToNameList)
		{
			var weaponValues = EnumUtil.GetValues<Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				GetName("WeaponAbilitiesElement", addToNameList);
			}

			return "WeaponAbilities";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponAbilitiesElement(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("WeaponAbilities[{0}].Element", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmor(bool addToNameList)
		{
			GetName("ArmorName", addToNameList);
			GetName("ArmorDesc", addToNameList);
			GetName("ArmorIsPlural", addToNameList);
			GetName("ArmorPluralType", addToNameList);
			GetName("ArmorArticleType", addToNameList);
			GetName("ArmorValue", addToNameList);
			GetName("ArmorWeight", addToNameList);
			GetName("ArmorType", addToNameList);
			GetName("ArmorField1", addToNameList);
			GetName("ArmorField2", addToNameList);

			return "Armor";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorName(bool addToNameList)
		{
			var result = "Armor.Name";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorDesc(bool addToNameList)
		{
			var result = "Armor.Desc";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorIsPlural(bool addToNameList)
		{
			var result = "Armor.IsPlural";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorPluralType(bool addToNameList)
		{
			var result = "Armor.PluralType";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorArticleType(bool addToNameList)
		{
			var result = "Armor.ArticleType";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorValue(bool addToNameList)
		{
			var result = "Armor.Value";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorWeight(bool addToNameList)
		{
			var result = "Armor.Weight";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorType(bool addToNameList)
		{
			var result = "Armor.Type";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorField1(bool addToNameList)
		{
			var result = "Armor.Field1";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameArmorField2(bool addToNameList)
		{
			var result = "Armor.Field2";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShield(bool addToNameList)
		{
			GetName("ShieldName", addToNameList);
			GetName("ShieldDesc", addToNameList);
			GetName("ShieldIsPlural", addToNameList);
			GetName("ShieldPluralType", addToNameList);
			GetName("ShieldArticleType", addToNameList);
			GetName("ShieldValue", addToNameList);
			GetName("ShieldWeight", addToNameList);
			GetName("ShieldType", addToNameList);
			GetName("ShieldField1", addToNameList);
			GetName("ShieldField2", addToNameList);

			return "Shield";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldName(bool addToNameList)
		{
			var result = "Shield.Name";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldDesc(bool addToNameList)
		{
			var result = "Shield.Desc";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldIsPlural(bool addToNameList)
		{
			var result = "Shield.IsPlural";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldPluralType(bool addToNameList)
		{
			var result = "Shield.PluralType";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldArticleType(bool addToNameList)
		{
			var result = "Shield.ArticleType";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldValue(bool addToNameList)
		{
			var result = "Shield.Value";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldWeight(bool addToNameList)
		{
			var result = "Shield.Weight";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldType(bool addToNameList)
		{
			var result = "Shield.Type";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldField1(bool addToNameList)
		{
			var result = "Shield.Field1";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameShieldField2(bool addToNameList)
		{
			var result = "Shield.Field2";

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeapons(bool addToNameList)
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				GetName("WeaponsName", addToNameList);
				GetName("WeaponsDesc", addToNameList);
				GetName("WeaponsIsPlural", addToNameList);
				GetName("WeaponsPluralType", addToNameList);
				GetName("WeaponsArticleType", addToNameList);
				GetName("WeaponsValue", addToNameList);
				GetName("WeaponsWeight", addToNameList);
				GetName("WeaponsType", addToNameList);
				GetName("WeaponsField1", addToNameList);
				GetName("WeaponsField2", addToNameList);
				GetName("WeaponsField3", addToNameList);
				GetName("WeaponsField4", addToNameList);
				GetName("WeaponsField5", addToNameList);
			}

			return "Weapons";
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsName(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Name", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsDesc(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Desc", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsIsPlural(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].IsPlural", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsPluralType(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].PluralType", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsArticleType(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].ArticleType", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsValue(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Value", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsWeight(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Weight", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsType(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Type", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsField1(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field1", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsField2(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field2", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsField3(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field3", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsField4(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field4", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		/// <summary></summary>
		/// <param name="addToNameList"></param>
		/// <returns></returns>
		public virtual string GetNameWeaponsField5(bool addToNameList)
		{
			var i = Index;

			var result = string.Format("Weapons[{0}].Field5", i);

			if (addToNameList)
			{
				NameList.Add(result);
			}

			return result;
		}

		#endregion

		#region GetValue Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueStatsElement()
		{
			var i = Index;

			return Record.GetStat(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueSpellAbilitiesElement()
		{
			var i = Index;

			return Record.GetSpellAbility(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponAbilitiesElement()
		{
			var i = Index;

			return Record.GetWeaponAbility(i);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorName()
		{
			return Record.Armor.Name;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorDesc()
		{
			return Record.Armor.Desc;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorIsPlural()
		{
			return Record.Armor.IsPlural;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorPluralType()
		{
			return Record.Armor.PluralType;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorArticleType()
		{
			return Record.Armor.ArticleType;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorValue()
		{
			return Record.Armor.Value;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorWeight()
		{
			return Record.Armor.Weight;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorType()
		{
			return Record.Armor.Type;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorField1()
		{
			return Record.Armor.Field1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueArmorField2()
		{
			return Record.Armor.Field2;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldName()
		{
			return Record.Shield.Name;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldDesc()
		{
			return Record.Shield.Desc;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldIsPlural()
		{
			return Record.Shield.IsPlural;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldPluralType()
		{
			return Record.Shield.PluralType;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldArticleType()
		{
			return Record.Shield.ArticleType;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldValue()
		{
			return Record.Shield.Value;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldWeight()
		{
			return Record.Shield.Weight;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldType()
		{
			return Record.Shield.Type;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldField1()
		{
			return Record.Shield.Field1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueShieldField2()
		{
			return Record.Shield.Field2;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsName()
		{
			var i = Index;

			return Record.GetWeapon(i).Name;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsDesc()
		{
			var i = Index;

			return Record.GetWeapon(i).Desc;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsIsPlural()
		{
			var i = Index;

			return Record.GetWeapon(i).IsPlural;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsPluralType()
		{
			var i = Index;

			return Record.GetWeapon(i).PluralType;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsArticleType()
		{
			var i = Index;

			return Record.GetWeapon(i).ArticleType;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsValue()
		{
			var i = Index;

			return Record.GetWeapon(i).Value;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsWeight()
		{
			var i = Index;

			return Record.GetWeapon(i).Weight;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsType()
		{
			var i = Index;

			return Record.GetWeapon(i).Type;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsField1()
		{
			var i = Index;

			return Record.GetWeapon(i).Field1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsField2()
		{
			var i = Index;

			return Record.GetWeapon(i).Field2;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsField3()
		{
			var i = Index;

			return Record.GetWeapon(i).Field3;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsField4()
		{
			var i = Index;

			return Record.GetWeapon(i).Field4;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueWeaponsField5()
		{
			var i = Index;

			return Record.GetWeapon(i).Field5;
		}

		#endregion

		#region Validate Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateUid()
		{
			return Record.Uid > 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateName()
		{
			if (Record.Name != null)
			{
				Record.Name = Regex.Replace(Record.Name, @"\s+", " ").Trim();
			}

			return string.IsNullOrWhiteSpace(Record.Name) == false && Record.Name.Length <= gEngine.CharNameLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArticleType()
		{
			return Record.ArticleType == ArticleType.None;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateGender()
		{
			return Enum.IsDefined(typeof(Gender), Record.Gender);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateStatus()
		{
			return Enum.IsDefined(typeof(Status), Record.Status);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateStats()
		{
			var result = true;

			var statValues = EnumUtil.GetValues<Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				result = ValidateField("StatsElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateStatsElement()
		{
			var i = Index;

			var stat = gEngine.GetStat((Stat)i);

			Debug.Assert(stat != null);

			return Record.GetStat(i) >= stat.MinValue && Record.GetStat(i) <= stat.MaxValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSpellAbilities()
		{
			var result = true;

			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				result = ValidateField("SpellAbilitiesElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSpellAbilitiesElement()
		{
			var i = Index;

			var spell = gEngine.GetSpell((Spell)i);

			Debug.Assert(spell != null);

			return Record.GetSpellAbility(i) >= spell.MinValue && Record.GetSpellAbility(i) <= spell.MaxValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponAbilities()
		{
			var result = true;

			var weaponValues = EnumUtil.GetValues<Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				result = ValidateField("WeaponAbilitiesElement");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = gEngine.GetWeapon((Weapon)i);			// TODO: should this be just GetWeapon(i) ???  Verify this and all other Validate methods like it

			Debug.Assert(weapon != null);

			return Record.GetWeaponAbility(i) >= weapon.MinValue && Record.GetWeaponAbility(i) <= weapon.MaxValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorExpertise()
		{
			return Record.ArmorExpertise >= 0 && Record.ArmorExpertise <= 79;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateHeldGold()
		{
			return Record.HeldGold >= gEngine.MinGoldValue && Record.HeldGold <= gEngine.MaxGoldValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateBankGold()
		{
			return Record.BankGold >= gEngine.MinGoldValue && Record.BankGold <= gEngine.MaxGoldValue;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorClass()
		{
			return Enum.IsDefined(typeof(Armor), Record.ArmorClass);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmor()
		{
			var result = true;

			result = ValidateField("ArmorName") &&
							ValidateField("ArmorDesc") &&
							ValidateField("ArmorIsPlural") &&
							ValidateField("ArmorPluralType") &&
							ValidateField("ArmorArticleType") &&
							ValidateField("ArmorValue") &&
							ValidateField("ArmorWeight") &&
							ValidateField("ArmorType") &&
							ValidateField("ArmorField1") &&
							ValidateField("ArmorField2");

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorName()
		{
			var result = true;

			if (Record.Armor.Name != null)
			{
				Record.Armor.Name = Regex.Replace(Record.Armor.Name, @"\s+", " ").Trim();
			}

			if (Record.IsArmorActive())
			{
				result = !string.IsNullOrWhiteSpace(Record.Armor.Name) && Record.Armor.Name.Length <= gEngine.CharArtNameLen;

				if (result)
				{
					var recordArmorName = string.Format(" {0} ", Record.Armor.Name.ToLower());

					result = Array.FindIndex(gEngine.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? recordArmorName.IndexOf(" " + token + " ") >= 0 : recordArmorName.IndexOf(token) >= 0) < 0;

					if (result)
					{
						result = Array.FindIndex(gEngine.PronounTokens, token => recordArmorName.IndexOf(" " + token + " ") >= 0) < 0;
					}

					// TODO: might need to disallow verb name matches as well
				}
			}
			else
			{
				result = Record.Armor.Name != null && (Record.Armor.Name == "" || Record.Armor.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase));
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorDesc()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = !string.IsNullOrWhiteSpace(Record.Armor.Desc) && Record.Armor.Desc.Length <= gEngine.CharArtDescLen;
			}
			else
			{
				result = Record.Armor.Desc == "";
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorIsPlural()
		{
			var result = true;

			if (!Record.IsArmorActive())
			{
				result = Record.Armor.IsPlural == false;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorPluralType()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Enum.IsDefined(typeof(PluralType), Record.Armor.PluralType);
			}
			else
			{
				result = Record.Armor.PluralType == PluralType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorArticleType()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Enum.IsDefined(typeof(ArticleType), Record.Armor.ArticleType);
			}
			else
			{
				result = Record.Armor.ArticleType == ArticleType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorValue()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Record.Armor.Value >= gEngine.MinGoldValue && Record.Armor.Value <= gEngine.MaxGoldValue;
			}
			else
			{
				result = Record.Armor.Value == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorWeight()
		{
			var result = true;

			if (!Record.IsArmorActive())
			{
				result = Record.Armor.Weight == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorType()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Record.Armor.Type == ArtifactType.Wearable;
			}
			else
			{
				result = Record.Armor.Type == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorField1()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = gEngine.IsValidArtifactArmor(Record.Armor.Field1);
			}
			else
			{
				result = Record.Armor.Field1 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmorField2()
		{
			var result = true;

			if (Record.IsArmorActive())
			{
				result = Enum.IsDefined(typeof(Clothing), Record.Armor.Field2);
			}
			else
			{
				result = Record.Armor.Field2 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShield()
		{
			var result = true;

			result = ValidateField("ShieldName") &&
							ValidateField("ShieldDesc") &&
							ValidateField("ShieldIsPlural") &&
							ValidateField("ShieldPluralType") &&
							ValidateField("ShieldArticleType") &&
							ValidateField("ShieldValue") &&
							ValidateField("ShieldWeight") &&
							ValidateField("ShieldType") &&
							ValidateField("ShieldField1") &&
							ValidateField("ShieldField2");

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldName()
		{
			var result = true;

			if (Record.Shield.Name != null)
			{
				Record.Shield.Name = Regex.Replace(Record.Shield.Name, @"\s+", " ").Trim();
			}

			if (Record.IsShieldActive())
			{
				result = !string.IsNullOrWhiteSpace(Record.Shield.Name) && Record.Shield.Name.Length <= gEngine.CharArtNameLen;

				if (result)
				{
					var recordShieldName = string.Format(" {0} ", Record.Shield.Name.ToLower());

					result = Array.FindIndex(gEngine.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? recordShieldName.IndexOf(" " + token + " ") >= 0 : recordShieldName.IndexOf(token) >= 0) < 0;

					if (result)
					{
						result = Array.FindIndex(gEngine.PronounTokens, token => recordShieldName.IndexOf(" " + token + " ") >= 0) < 0;
					}

					// TODO: might need to disallow verb name matches as well
				}
			}
			else
			{
				result = Record.Shield.Name != null && (Record.Shield.Name == "" || Record.Shield.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase));
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldDesc()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = !string.IsNullOrWhiteSpace(Record.Shield.Desc) && Record.Shield.Desc.Length <= gEngine.CharArtDescLen;
			}
			else
			{
				result = Record.Shield.Desc == "";
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldIsPlural()
		{
			var result = true;

			if (!Record.IsShieldActive())
			{
				result = Record.Shield.IsPlural == false;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldPluralType()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Enum.IsDefined(typeof(PluralType), Record.Shield.PluralType);
			}
			else
			{
				result = Record.Shield.PluralType == PluralType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldArticleType()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Enum.IsDefined(typeof(ArticleType), Record.Shield.ArticleType);
			}
			else
			{
				result = Record.Shield.ArticleType == ArticleType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldValue()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Record.Shield.Value >= gEngine.MinGoldValue && Record.Shield.Value <= gEngine.MaxGoldValue;
			}
			else
			{
				result = Record.Shield.Value == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldWeight()
		{
			var result = true;

			if (!Record.IsShieldActive())
			{
				result = Record.Shield.Weight == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldType()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Record.Shield.Type == ArtifactType.Wearable;
			}
			else
			{
				result = Record.Shield.Type == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldField1()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = gEngine.IsValidArtifactArmor(Record.Shield.Field1);
			}
			else
			{
				result = Record.Shield.Field1 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateShieldField2()
		{
			var result = true;

			if (Record.IsShieldActive())
			{
				result = Enum.IsDefined(typeof(Clothing), Record.Shield.Field2);
			}
			else
			{
				result = Record.Shield.Field2 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeapons()
		{
			var result = true;

			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				result = ValidateField("WeaponsName") &&
								ValidateField("WeaponsDesc") &&
								ValidateField("WeaponsIsPlural") &&
								ValidateField("WeaponsPluralType") &&
								ValidateField("WeaponsArticleType") &&
								ValidateField("WeaponsValue") &&
								ValidateField("WeaponsWeight") &&
								ValidateField("WeaponsType") &&
								ValidateField("WeaponsField1") &&
								ValidateField("WeaponsField2") &&
								ValidateField("WeaponsField3") &&
								ValidateField("WeaponsField4") &&
								ValidateField("WeaponsField5");

				if (result == false)
				{
					break;
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsName()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			if (Record.GetWeapon(i).Name != null)
			{
				Record.GetWeapon(i).Name = Regex.Replace(Record.GetWeapon(i).Name, @"\s+", " ").Trim();
			}

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				if (Record.GetWeapon(i).Name.Length > gEngine.CharArtNameLen)
				{
					for (var j = gEngine.CharArtNameLen; j < Record.GetWeapon(i).Name.Length; j++)
					{
						if (Record.GetWeapon(i).Name[j] != '#')
						{
							result = false;

							break;
						}
					}
				}

				if (result)
				{
					var recordWeaponName = string.Format(" {0} ", Record.GetWeapon(i).Name.ToLower());

					result = Array.FindIndex(gEngine.CommandSepTokens, token => !Char.IsPunctuation(token[0]) ? recordWeaponName.IndexOf(" " + token + " ") >= 0 : recordWeaponName.IndexOf(token) >= 0) < 0;

					if (result)
					{
						result = Array.FindIndex(gEngine.PronounTokens, token => recordWeaponName.IndexOf(" " + token + " ") >= 0) < 0;
					}

					// TODO: might need to disallow verb name matches as well
				}
			}
			else
			{
				result = Record.GetWeapon(i).Name != null && (Record.GetWeapon(i).Name == "" || Record.GetWeapon(i).Name.Equals("NONE", StringComparison.OrdinalIgnoreCase));
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsDesc()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapon(i).Desc == "" || (!string.IsNullOrWhiteSpace(Record.GetWeapon(i).Desc) && Record.GetWeapon(i).Desc.Length <= gEngine.CharArtDescLen);
			}
			else
			{
				result = Record.GetWeapon(i).Desc == "";
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsIsPlural()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (!activeWeapon)
			{
				result = Record.GetWeapon(i).IsPlural == false;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsPluralType()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(PluralType), Record.GetWeapon(i).PluralType);
			}
			else
			{
				result = Record.GetWeapon(i).PluralType == PluralType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsArticleType()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(ArticleType), Record.GetWeapon(i).ArticleType);
			}
			else
			{
				result = Record.GetWeapon(i).ArticleType == ArticleType.None;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsValue()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapon(i).Value >= gEngine.MinGoldValue && Record.GetWeapon(i).Value <= gEngine.MaxGoldValue;
			}
			else
			{
				result = Record.GetWeapon(i).Value == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsWeight()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (!activeWeapon)
			{
				result = Record.GetWeapon(i).Weight == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsType()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapon(i).Type == 0 || Record.GetWeapon(i).Type == ArtifactType.Weapon || Record.GetWeapon(i).Type == ArtifactType.MagicWeapon;
			}
			else
			{
				result = Record.GetWeapon(i).Type == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsField1()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapon(i).Field1 >= -50 && Record.GetWeapon(i).Field1 <= 50;
			}
			else
			{
				result = Record.GetWeapon(i).Field1 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsField2()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Enum.IsDefined(typeof(Weapon), Record.GetWeapon(i).Field2);
			}
			else
			{
				result = Record.GetWeapon(i).Field2 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsField3()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapon(i).Field3 >= 1 && Record.GetWeapon(i).Field3 <= 25;
			}
			else
			{
				result = Record.GetWeapon(i).Field3 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsField4()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				result = Record.GetWeapon(i).Field4 >= 1 && Record.GetWeapon(i).Field4 <= 25;
			}
			else
			{
				result = Record.GetWeapon(i).Field4 == 0;
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWeaponsField5()
		{
			var result = true;

			var activeWeapon = true;

			var i = Index;

			for (var h = 0; h <= i; h++)
			{
				if (!Record.IsWeaponActive(h))
				{
					activeWeapon = false;

					break;
				}
			}

			if (activeWeapon)
			{
				if (Record.GetWeapon(i).Field5 == 0)  // auto-upgrade old weapons
				{
					Record.GetWeapon(i).Field5 = Record.GetWeapon(i).Field2 == (long)Weapon.Bow ? 2 : 1;
				}

				result = Record.GetWeapon(i).Field5 >= 1 && Record.GetWeapon(i).Field5 <= 2;
			}
			else
			{
				result = Record.GetWeapon(i).Field5 == 0;
			}

			return result;
		}

		#endregion

		#region ValidateInterdependencies Methods

		// do nothing

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		public virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the Character.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescGender()
		{
			var fullDesc = "Enter the gender of the Character.";

			var briefDesc = new StringBuilder(gEngine.BufSize);

			var genderValues = EnumUtil.GetValues<Gender>();

			for (var j = 0; j < genderValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], gEngine.EvalGender(genderValues[j], "Male", "Female", "Neutral"));
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescStatus()
		{
			var fullDesc = "Enter the status of the Character.";

			var briefDesc = new StringBuilder(gEngine.BufSize);

			var statusValues = EnumUtil.GetValues<Status>();

			for (var j = 0; j < statusValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)statusValues[j], gEngine.GetStatusName(statusValues[j]));
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescStatsElement()
		{
			var i = Index;

			var stat = gEngine.GetStat((Stat)i);

			Debug.Assert(stat != null);

			var fullDesc = string.Format("Enter the {0} of the Character.", stat.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", stat.MinValue, stat.MaxValue);

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescSpellAbilitiesElement()
		{
			var i = Index;

			var spell = gEngine.GetSpell((Spell)i);

			Debug.Assert(spell != null);

			var fullDesc = string.Format("Enter the Character's {0} spell ability.", spell.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", spell.MinValue, spell.MaxValue);

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = gEngine.GetWeapon((Weapon)i);

			Debug.Assert(weapon != null);

			var fullDesc = string.Format("Enter the Character's {0} weapon ability.", weapon.Name);

			var briefDesc = string.Format("{0}-{1}=Valid value", weapon.MinValue, weapon.MaxValue);

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescArmorExpertise()
		{
			var fullDesc = "Enter the armor expertise of the Character.";

			var briefDesc = "0-79=Valid value";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescHeldGold()
		{
			var fullDesc = "Enter the Character's gold in hand.";

			var briefDesc = string.Format("{0}-{1}=Valid value", gEngine.MinGoldValue, gEngine.MaxGoldValue);

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescBankGold()
		{
			var fullDesc = "Enter the Character's gold in the bank.";

			var briefDesc = string.Format("{0}-{1}=Valid value", gEngine.MinGoldValue, gEngine.MaxGoldValue);

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescArmorClass()
		{
			var fullDesc = "Enter the armor class of the Character.";

			var briefDesc = new StringBuilder(gEngine.BufSize);

			var armorValues = EnumUtil.GetValues<Armor>();

			for (var j = 0; j < armorValues.Count; j++)
			{
				var armor = gEngine.GetArmor(armorValues[j]);

				Debug.Assert(armor != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)armorValues[j], armor.Name);
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsName()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the Character's weapon #{0} name.{1}{1}Weapon names should always be in singular form and capitalized when appropriate.", i + 1, Environment.NewLine);

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsIsPlural()
		{
			var i = Index;

			var fullDesc = string.Format("Enter whether the Character's weapon #{0} is singular or plural.", i + 1);

			var briefDesc = "0=Singular; 1=Plural";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsPluralType()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the plural type that converts the Character's weapon #{0} name from singular to plural.", i + 1);

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsArticleType()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the article type that prefixes the Character's weapon #{0} name with an article.", i + 1);

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsField1()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the Character's weapon #{0} complexity.", i + 1);

			var briefDesc = "-50-50=Valid value";          // TODO: eliminate hardcode

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsField2()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the Character's weapon #{0} type.", i + 1);

			var briefDesc = new StringBuilder(gEngine.BufSize);

			var weaponValues = EnumUtil.GetValues<Weapon>();

			for (var j = 0; j < weaponValues.Count; j++)
			{
				var weapon = gEngine.GetWeapon(weaponValues[j]);

				Debug.Assert(weapon != null);

				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)weaponValues[j], weapon.Name);
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsField3()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the Character's weapon #{0} hit dice.", i + 1);

			var briefDesc = "1-25=Valid value";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsField4()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the Character's weapon #{0} hit dice sides.", i + 1);

			var briefDesc = "1-25=Valid value";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescWeaponsField5()
		{
			var i = Index;

			var fullDesc = string.Format("Enter the Character's weapon #{0} number of hands required.", i + 1);

			var briefDesc = "1-2=Valid value";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		#endregion

		#region List Methods

		/// <summary></summary>
		public virtual void ListUid()
		{
			if (FullDetail)
			{
				if (!ExcludeROFields)
				{
					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Uid"), null), Record.Uid);
				}
			}
			else
			{
				gOut.Write("{0}{1,3}. {2}", Environment.NewLine, Record.Uid, gEngine.Capitalize(Record.Name));
			}
		}

		/// <summary></summary>
		public virtual void ListName()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Name"), null), Record.Name);
			}
		}

		/// <summary></summary>
		public virtual void ListGender()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Gender"), null), Record.EvalGender("Male", "Female", "Neutral"));
			}
		}

		/// <summary></summary>
		public virtual void ListStatus()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Status"), null), gEngine.GetStatusName(Record.Status));
			}
		}

		/// <summary></summary>
		public virtual void ListStats()
		{
			var statValues = EnumUtil.GetValues<Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				ListField("StatsElement");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		public virtual void ListStatsElement()
		{
			var i = Index;

			var sv = (Stat)i;

			if (FullDetail)
			{
				if (LookupMsg)
				{
					Buf.Clear();

					if (sv == Stat.Intellect)
					{
						var ibp = Record.GetIntellectBonusPct();

						Buf.AppendFormat("Learning: {0}{1}%", ibp > 0 ? "+" : "", ibp);
					}
					else if (sv == Stat.Hardiness)
					{
						Buf.AppendFormat("Weight Carryable: {0} G ({1} D)", Record.GetWeightCarryableGronds(), Record.GetWeightCarryableDos());
					}
					else if (sv == Stat.Charisma)
					{
						var cmp = Record.GetCharmMonsterPct();

						Buf.AppendFormat("Charm Monster: {0}{1}%", cmp > 0 ? "+" : "", cmp);
					}
				}

				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg && Buf.Length > 0)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("StatsElement"), null),
						gEngine.BuildValue(51, ' ', 8, Record.GetStat(i), null, Buf.ToString()));
				}
				else
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("StatsElement"), null),
						Record.GetStat(i));
				}
			}
		}

		/// <summary></summary>
		public virtual void ListSpellAbilities()
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				ListField("SpellAbilitiesElement");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		public virtual void ListSpellAbilitiesElement()
		{
			var i = Index;

			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}%",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("SpellAbilitiesElement"), null),
					Record.GetSpellAbility(i));
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponAbilities()
		{
			var weaponValues = EnumUtil.GetValues<Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				ListField("WeaponAbilitiesElement");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		public virtual void ListWeaponAbilitiesElement()
		{
			var i = Index;

			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}%",
					Environment.NewLine,
					gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponAbilitiesElement"), null),
					Record.GetWeaponAbility(i));
			}
		}

		/// <summary></summary>
		public virtual void ListArmorExpertise()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}%", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ArmorExpertise"), null), Record.ArmorExpertise);
			}
		}

		/// <summary></summary>
		public virtual void ListHeldGold()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("HeldGold"), null), Record.HeldGold);
			}
		}

		/// <summary></summary>
		public virtual void ListBankGold()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("BankGold"), null), Record.BankGold);
			}
		}

		/// <summary></summary>
		public virtual void ListArmorClass()
		{
			if (FullDetail)
			{
				var armor = gEngine.GetArmor(Record.ArmorClass);

				Debug.Assert(armor != null);

				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ArmorClass"), null), armor.Name);
			}
		}

		/// <summary></summary>
		public virtual void ListWeapons()
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				ListField("WeaponsName");
				ListField("WeaponsIsPlural");
				ListField("WeaponsPluralType");
				ListField("WeaponsArticleType");
				ListField("WeaponsField1");
				ListField("WeaponsField2");
				ListField("WeaponsField3");
				ListField("WeaponsField4");
				ListField("WeaponsField5");
			}

			AddToListedNames = false;
		}

		/// <summary></summary>
		public virtual void ListWeaponsName()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || i == 0 || Record.IsWeaponActive(i - 1))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsName"), null), Record.GetWeapon(i).Name);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponsIsPlural()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsIsPlural"), null), Convert.ToInt64(Record.GetWeapon(i).IsPlural));
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponsPluralType()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsPluralType"), null),
							gEngine.BuildValue(51, ' ', 8, (long)Record.GetWeapon(i).PluralType, null,
							Record.GetWeapon(i).PluralType == PluralType.None ? "No change" :
							Record.GetWeapon(i).PluralType == PluralType.S ? "Use 's'" :
							Record.GetWeapon(i).PluralType == PluralType.Es ? "Use 'es'" :
							Record.GetWeapon(i).PluralType == PluralType.YIes ? "Use 'y' to 'ies'" :
							"Invalid value"));
					}
					else
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsPluralType"), null), (long)Record.GetWeapon(i).PluralType);
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponsArticleType()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					if (LookupMsg)
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsArticleType"), null),
							gEngine.BuildValue(51, ' ', 8, (long)Record.GetWeapon(i).ArticleType, null,
							Record.GetWeapon(i).ArticleType == ArticleType.None ? "No article" :
							Record.GetWeapon(i).ArticleType == ArticleType.A ? "Use 'a'" :
							Record.GetWeapon(i).ArticleType == ArticleType.An ? "Use 'an'" :
							Record.GetWeapon(i).ArticleType == ArticleType.Some ? "Use 'some'" :
							Record.GetWeapon(i).ArticleType == ArticleType.The ? "Use 'the'" :
							"Invalid value"));
					}
					else
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsArticleType"), null), (long)Record.GetWeapon(i).ArticleType);
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponsField1()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}%", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField1"), null), Record.GetWeapon(i).Field1);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponsField2()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var weapon = gEngine.GetWeapon((Weapon)Record.GetWeapon(i).Field2);

					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField2"), null),
						weapon != null ? weapon.Name : "0");
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponsField3()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField3"), null), Record.GetWeapon(i).Field3);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponsField4()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField4"), null), Record.GetWeapon(i).Field4);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeaponsField5()
		{
			var i = Index;

			if (FullDetail)
			{
				if (!ExcludeROFields || Record.IsWeaponActive(i))
				{
					var listNum = NumberFields ? ListNum++ : 0;

					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("WeaponsField5"), null), Record.GetWeapon(i).Field5);
				}
			}
		}

		#endregion

		#region Input Methods

		/// <summary></summary>
		public virtual void InputUid()
		{
			gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Uid"), null), Record.Uid);

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputName()
		{
			var fieldDesc = FieldDesc;

			var name = Record.Name;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", name);

				PrintFieldDesc("Name", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Name"), null));

				var rc = gEngine.In.ReadField(Buf, gEngine.CharNameLen, null, '_', '\0', false, null, null, gEngine.IsCharAnyButDquoteCommaColon, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Name = Buf.ToString();

				if (ValidateField("Name"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputGender()
		{
			var fieldDesc = FieldDesc;

			var gender = Record.Gender;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)gender);

				PrintFieldDesc("Gender", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Gender"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0To2, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Gender = (Gender)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Gender"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputStatus()
		{
			var fieldDesc = FieldDesc;

			var status = Record.Status;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)status);

				PrintFieldDesc("Status", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Status"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsChar0To3, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Status = (Status)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Status"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputStats()
		{
			var statValues = EnumUtil.GetValues<Stat>();

			foreach (var sv in statValues)
			{
				Index = (long)sv;

				InputField("StatsElement");
			}
		}

		/// <summary></summary>
		public virtual void InputStatsElement()
		{
			var i = Index;

			var stat = gEngine.GetStat((Stat)i);

			Debug.Assert(stat != null);

			var fieldDesc = FieldDesc;

			var value = Record.GetStat(i);

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", value);

				PrintFieldDesc("StatsElement", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("StatsElement"), stat.EmptyVal));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, stat.EmptyVal, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.SetStat(i, Convert.ToInt64(Buf.Trim().ToString()));

				if (ValidateField("StatsElement"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputSpellAbilities()
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				Index = (long)sv;

				InputField("SpellAbilitiesElement");
			}
		}

		/// <summary></summary>
		public virtual void InputSpellAbilitiesElement()
		{
			var i = Index;

			var fieldDesc = FieldDesc;

			var value = Record.GetSpellAbility(i);

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", value);

				PrintFieldDesc("SpellAbilitiesElement", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("SpellAbilitiesElement"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.SetSpellAbility(i, Convert.ToInt64(Buf.Trim().ToString()));

				if (ValidateField("SpellAbilitiesElement"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputWeaponAbilities()
		{
			var weaponValues = EnumUtil.GetValues<Weapon>();

			foreach (var wv in weaponValues)
			{
				Index = (long)wv;

				InputField("WeaponAbilitiesElement");
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponAbilitiesElement()
		{
			var i = Index;

			var weapon = gEngine.GetWeapon((Weapon)i);

			Debug.Assert(weapon != null);

			var fieldDesc = FieldDesc;

			var value = Record.GetWeaponAbility(i);

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", value);

				PrintFieldDesc("WeaponAbilitiesElement", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponAbilitiesElement"), weapon.EmptyVal));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, weapon.EmptyVal, null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.SetWeaponAbility(i, Convert.ToInt64(Buf.Trim().ToString()));
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("WeaponAbilitiesElement"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputArmorExpertise()
		{
			var fieldDesc = FieldDesc;

			var armorExpertise = Record.ArmorExpertise;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", armorExpertise);

				PrintFieldDesc("ArmorExpertise", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ArmorExpertise"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ArmorExpertise = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArmorExpertise"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputHeldGold()
		{
			var fieldDesc = FieldDesc;

			var heldGold = Record.HeldGold;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", heldGold);

				PrintFieldDesc("HeldGold", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("HeldGold"), "200"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "200", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.HeldGold = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("HeldGold"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputBankGold()
		{
			var fieldDesc = FieldDesc;

			var bankGold = Record.BankGold;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", bankGold);

				PrintFieldDesc("BankGold", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("BankGold"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.BankGold = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("BankGold"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputArmorClass()
		{
			var fieldDesc = FieldDesc;

			var armorClass = Record.ArmorClass;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)armorClass);

				PrintFieldDesc("ArmorClass", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ArmorClass"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ArmorClass = (Armor)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArmorClass"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			if (Record.ArmorClass != armorClass)
			{
				Record.Armor = gEngine.CreateInstance<ICharacterArtifact>(x =>
				{
					x.Parent = Record;
				});

				Record.Shield = gEngine.CreateInstance<ICharacterArtifact>(x =>
				{
					x.Parent = Record;
				});
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputWeapons()
		{
			for (Index = 0; Index < Record.Weapons.Length; Index++)
			{
				InputField("WeaponsName");
				InputField("WeaponsIsPlural");
				InputField("WeaponsPluralType");
				InputField("WeaponsArticleType");
				InputField("WeaponsField1");
				InputField("WeaponsField2");
				InputField("WeaponsField3");
				InputField("WeaponsField4");
				InputField("WeaponsField5");
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsName()
		{
			var i = Index;

			if (i == 0 || Record.IsWeaponActive(i - 1))
			{
				var fieldDesc = FieldDesc;

				var name = Record.GetWeapon(i).Name;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", name);

					PrintFieldDesc("WeaponsName", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsName"), null));

					var rc = gEngine.In.ReadField(Buf, gEngine.CharArtNameLen, null, '_', '\0', true, null, null, null, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Record.GetWeapon(i).Name = Buf.ToString();

					if (ValidateField("WeaponsName"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.IsWeaponActive(i))
				{
					var clearExtraFields = !Record.GetWeapon(i).Name.Equals(name, StringComparison.OrdinalIgnoreCase);

					if (EditRec && (Record.GetWeapon(i).Field3 == 0 || Record.GetWeapon(i).Field4 == 0))
					{
						Record.GetWeapon(i).IsPlural = false;

						Record.GetWeapon(i).PluralType = PluralType.S;

						Record.GetWeapon(i).ArticleType = ArticleType.A;

						Record.GetWeapon(i).Field1 = 5;

						Record.GetWeapon(i).Field2 = (long)Weapon.Sword;

						Record.GetWeapon(i).Field3 = 1;

						Record.GetWeapon(i).Field4 = 6;

						Record.GetWeapon(i).Field5 = 1;

						clearExtraFields = true;
					}

					if (clearExtraFields)
					{
						Record.GetWeapon(i).ClearExtraFields();
					}
				}
				else
				{
					for (var k = i; k < Record.Weapons.Length; k++)
					{
						Record.GetWeapon(k).Name = "NONE";

						Record.GetWeapon(k).IsPlural = false;

						Record.GetWeapon(k).PluralType = 0;

						Record.GetWeapon(k).ArticleType = 0;

						Record.GetWeapon(k).Field1 = 0;

						Record.GetWeapon(k).Field2 = 0;

						Record.GetWeapon(k).Field3 = 0;

						Record.GetWeapon(k).Field4 = 0;

						Record.GetWeapon(k).Field5 = 0;

						Record.GetWeapon(k).ClearExtraFields();
					}
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).Name = "NONE";

				Record.GetWeapon(i).ClearExtraFields();
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsIsPlural()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var isPlural = Record.GetWeapon(i).IsPlural;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isPlural));

					PrintFieldDesc("WeaponsIsPlural", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsIsPlural"), "0"));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Record.GetWeapon(i).IsPlural = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

					if (ValidateField("WeaponsIsPlural"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapon(i).IsPlural != isPlural)
				{
					Record.GetWeapon(i).ClearExtraFields();
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).IsPlural = false;
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsPluralType()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var pluralType = Record.GetWeapon(i).PluralType;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", (long)pluralType);

					PrintFieldDesc("WeaponsPluralType", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsPluralType"), "1"));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapon(i).PluralType = (PluralType)Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsPluralType"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapon(i).PluralType != pluralType)
				{
					Record.GetWeapon(i).ClearExtraFields();
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).PluralType = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsArticleType()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var articleType = Record.GetWeapon(i).ArticleType;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", (long)articleType);

					PrintFieldDesc("WeaponsArticleType", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsArticleType"), "1"));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapon(i).ArticleType = (ArticleType)Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsArticleType"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapon(i).ArticleType != articleType)
				{
					Record.GetWeapon(i).ClearExtraFields();
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).ArticleType = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsField1()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field1 = Record.GetWeapon(i).Field1;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field1);

					PrintFieldDesc("WeaponsField1", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField1"), "5"));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "5", null, gEngine.IsCharPlusMinusDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					var error = false;

					try
					{
						Record.GetWeapon(i).Field1 = Convert.ToInt64(Buf.Trim().ToString());
					}
					catch (Exception)
					{
						error = true;
					}

					if (!error && ValidateField("WeaponsField1"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapon(i).Field1 != field1)
				{
					Record.GetWeapon(i).ClearExtraFields();
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).Field1 = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsField2()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field2 = Record.GetWeapon(i).Field2;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field2);

					PrintFieldDesc("WeaponsField2", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField2"), "5"));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "5", null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Record.GetWeapon(i).Field2 = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsField2"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapon(i).Field2 != field2)
				{
					Record.GetWeapon(i).ClearExtraFields();
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).Field2 = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsField3()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field3 = Record.GetWeapon(i).Field3;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field3);

					PrintFieldDesc("WeaponsField3", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField3"), "1"));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Record.GetWeapon(i).Field3 = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsField3"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapon(i).Field3 != field3)
				{
					Record.GetWeapon(i).ClearExtraFields();
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).Field3 = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsField4()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field4 = Record.GetWeapon(i).Field4;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field4);

					PrintFieldDesc("WeaponsField4", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField4"), "6"));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "6", null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Record.GetWeapon(i).Field4 = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsField4"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapon(i).Field4 != field4)
				{
					Record.GetWeapon(i).ClearExtraFields();
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).Field4 = 0;
			}
		}

		/// <summary></summary>
		public virtual void InputWeaponsField5()
		{
			var i = Index;

			if (Record.IsWeaponActive(i))
			{
				var fieldDesc = FieldDesc;

				var field5 = Record.GetWeapon(i).Field5;

				while (true)
				{
					Buf.SetFormat(EditRec ? "{0}" : "", field5);

					PrintFieldDesc("WeaponsField5", EditRec, EditField, fieldDesc);

					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("WeaponsField5"), "1"));

					var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Record.GetWeapon(i).Field5 = Convert.ToInt64(Buf.Trim().ToString());

					if (ValidateField("WeaponsField5"))
					{
						break;
					}

					fieldDesc = FieldDesc.Brief;
				}

				if (Record.GetWeapon(i).Field5 != field5)
				{
					Record.GetWeapon(i).ClearExtraFields();
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				Record.GetWeapon(i).Field5 = 0;
			}
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class CharacterHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = gEngine.Database.GetCharacterUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public CharacterHelper()
		{

		}

		#endregion

		#endregion
	}
}
