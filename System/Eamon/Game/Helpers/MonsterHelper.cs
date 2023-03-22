
// MonsterHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
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
	public class MonsterHelper : Helper<IMonster>, IMonsterHelper
	{
		#region Public Methods

		#region Interface IHelper

		public override bool ValidateRecordAfterDatabaseLoaded()
		{
			Clear();

			return ValidateFieldAfterDatabaseLoaded("Weapon");
		}

		public override void ListErrorField()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(ErrorFieldName));

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Uid"), null), Record.Uid);

			gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Name"), null), Record.Name);

			if (ErrorFieldName.Equals("Desc", StringComparison.OrdinalIgnoreCase) || ShowDesc)
			{
				gOut.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Desc"), null), Record.Desc);
			}

			if (ErrorFieldName.Equals("PluralType", StringComparison.OrdinalIgnoreCase))
			{
				gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '.', 0, GetPrintedName("PluralType"), null), (long)Record.PluralType);
			}
			else if (ErrorFieldName.Equals("Location", StringComparison.OrdinalIgnoreCase))
			{
				gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Location"), null), Record.Location);
			}
			else if (ErrorFieldName.Equals("Weapon", StringComparison.OrdinalIgnoreCase))
			{
				gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '.', 0, GetPrintedName("Weapon"), null), Record.Weapon);
			}
			else if (ErrorFieldName.Equals("DeadBody", StringComparison.OrdinalIgnoreCase))
			{
				gOut.Print("{0}{1}", gEngine.BuildPrompt(27, '.', 0, GetPrintedName("DeadBody"), null), Record.DeadBody);
			}
		}

		#region GetPrintedName Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameStateDesc()
		{
			return "State Description";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameIsListed()
		{
			return "Is Listed";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNamePluralType()
		{
			return "Plural Type";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameGroupCount()
		{
			return "Group Count";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameAttackCount()
		{
			return "Attack Count";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameCombatCode()
		{
			return "Combat Code";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameWeapon()
		{
			return "Weapon Uid";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameNwDice()
		{
			return "Natural Wpn Dice";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameNwSides()
		{
			return "Natural Wpn Sides";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameDeadBody()
		{
			return "Dead Body Uid";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameField1()
		{
			return "Field #1";
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual string GetPrintedNameField2()
		{
			return "Field #2";
		}

		#endregion

		#region GetName Methods

		// do nothing

		#endregion

		#region GetValue Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueSpellsSpell()
		{
			var i = Index;

			var monsterSpell = Record.Spells != null ? Record.Spells[i] : null;
			
			return monsterSpell != null ? monsterSpell.Spell : default(object);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueSpellsField1()
		{
			var i = Index;

			var monsterSpell = Record.Spells != null ? Record.Spells[i] : null;

			return monsterSpell != null ? monsterSpell.Field1 : default(object);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual object GetValueSpellsField2()
		{
			var i = Index;

			var monsterSpell = Record.Spells != null ? Record.Spells[i] : null;

			return monsterSpell != null ? monsterSpell.Field2 : default(object);
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

			var result = !string.IsNullOrWhiteSpace(Record.Name);

			if (result && Record.Name.Length > gEngine.MonNameLen)
			{
				for (var i = gEngine.MonNameLen; i < Record.Name.Length; i++)
				{
					if (Record.Name[i] != '%')
					{
						result = false;

						break;
					}
				}
			}

			if (result)
			{
				var recordName = string.Format(" {0} ", Record.Name.ToLower());

				result = !Regex.IsMatch(recordName, gEngine.CommandSepRegexPattern) && !Regex.IsMatch(recordName, gEngine.PronounRegexPattern);

				// TODO: might need to disallow verb name matches as well
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateStateDesc()
		{
			return Record.StateDesc != null && Record.StateDesc.Length <= gEngine.MonStateDescLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateDesc()
		{
			return string.IsNullOrWhiteSpace(Record.Desc) == false && Record.Desc.Length <= gEngine.MonDescLen;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidatePluralType()
		{
			return gEngine.IsValidPluralType(Record.PluralType);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArticleType()
		{
			return Enum.IsDefined(typeof(ArticleType), Record.ArticleType);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateHardiness()
		{
			return Record.Hardiness >= 0;           // 0=Must be calculated
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateAgility()
		{
			return Record.Agility >= 0;          // 0=Must be calculated
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateGroupCount()
		{
			return Record.GroupCount >= 0;          // 0=Must be calculated
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateAttackCount()
		{
			if (Record.AttackCount == 0)
			{
				Record.AttackCount = 1;
			}

			return Record.AttackCount > 0 || Record.AttackCount < -1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCourage()
		{
			return gEngine.IsValidMonsterCourage(Record.Courage);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCombatCode()
		{
			return Enum.IsDefined(typeof(CombatCode), Record.CombatCode);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateArmor()
		{
			return gEngine.IsValidMonsterArmor(Record.Armor);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNwDice()
		{
			return Record.NwDice >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNwSides()
		{
			return Record.NwSides >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateFriendliness()
		{
			return gEngine.IsValidMonsterFriendliness(Record.Friendliness);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateGender()
		{
			return Enum.IsDefined(typeof(Gender), Record.Gender);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCurrGroupCount()
		{
			if (Record.CurrGroupCount == 0)
			{
				Record.CurrGroupCount = Record.GroupCount;
			}

			return Record.CurrGroupCount >= 0 && Record.CurrGroupCount <= Record.GroupCount;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateReaction()
		{
			if (!Enum.IsDefined(typeof(Friendliness), Record.Reaction))
			{
				Record.ResolveReaction(10);
			}

			return Enum.IsDefined(typeof(Friendliness), Record.Reaction);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateDmgTaken()
		{
			return Record.DmgTaken >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSpells()
		{
			var result = true;

			if (Record.Spells != null)
			{
				for (Index = 0; Index < Record.Spells.Length; Index++)
				{
					result = ValidateField("SpellsSpell") &&
									ValidateField("SpellsField1") &&
									ValidateField("SpellsField2") &&
									/* ValidateField("SpellsFieldN") */ true;

					if (result == false)
					{
						break;
					}
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSpellsSpell()
		{
			var i = Index;

			var monsterSpell = Record.Spells[i];

			var spell = monsterSpell != null ? gEngine.GetSpell(monsterSpell.Spell) : null;

			return spell != null;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSpellsField1()
		{
			var i = Index;

			var monsterSpell = Record.Spells[i];

			return monsterSpell != null && Enum.IsDefined(typeof(SpellPolicy), (SpellPolicy)monsterSpell.Field1);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSpellsField2()
		{
			var result = true;

			var i = Index;

			var monsterSpell = Record.Spells[i];

			if (monsterSpell != null)
			{
				switch(monsterSpell.Spell)
				{
					case Spell.Speed:

						result = monsterSpell.Field2 >= 0;

						break;
				}
			}
			else
			{
				result = false;
			}

			return result;
		}

		#endregion

		#region ValidateAfterDatabaseLoaded Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateAfterDatabaseLoadedWeapon()
		{
			var result = true;

			var weapon = Record.Weapon > 0 ? gADB[Record.Weapon] : null;

			if (weapon != null)
			{
				var ac = weapon.GeneralWeapon;

				Debug.Assert(ac != null);

				if (ac.Field5 > 1)
				{
					var shield = Record.GetWornList().FirstOrDefault(a =>
					{
						var ac01 = a.Wearable;

						Debug.Assert(ac01 != null);

						return ac01.Field1 == 1;
					});

					if (shield != null)
					{
						result = false;
					}
				}
			}

			return result;
		}

		#endregion

		#region ValidateInterdependencies Methods

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesDesc()
		{
			var result = true;

			long invalidUid = 0;

			var rc = gEngine.ResolveUidMacros(Record.Desc, Buf, false, false, ref invalidUid);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (invalidUid > 0)
			{
				result = false;

				Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Desc"), "Effect", invalidUid, "which doesn't exist");

				ErrorMessage = Buf.ToString();

				RecordType = typeof(IEffect);

				NewRecordUid = invalidUid;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesPluralType()
		{
			var result = true;

			var effectUid = gEngine.GetPluralTypeEffectUid(Record.PluralType);

			if (effectUid > 0)
			{
				var effect = gEDB[effectUid];

				if (effect == null)
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("PluralType"), "Effect", effectUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IEffect);

					NewRecordUid = effectUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesLocation()
		{
			var result = true;

			var roomUid = Record.GetInRoomUid();

			if (roomUid > 0)
			{
				var room = gRDB[roomUid];

				if (room == null)
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Location"), "Room", roomUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IRoom);

					NewRecordUid = roomUid;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesWeapon()
		{
			var result = true;

			var artUid = Record.GetCarryingWeaponUid();

			if (artUid > 0)
			{
				var artifact = gADB[artUid];

				if (artifact == null)
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Weapon"), "Artifact", artUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					NewRecordUid = artUid;

					goto Cleanup;
				}
				else if (!artifact.IsReadyableByMonster(Record))
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Weapon"), "Artifact", artUid, "which should be a readyable weapon, but isn't");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					EditRecord = artifact;

					goto Cleanup;
				}
				else if ((Record.IsCharacterMonster() && !artifact.IsCarriedByCharacter() && !artifact.IsWornByCharacter()) || (!Record.IsCharacterMonster() && !artifact.IsCarriedByMonster(Record) && !artifact.IsWornByMonster(Record)))
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("Weapon"), "Artifact", artUid, "which should be carried/worn by this Monster, but isn't");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					EditRecord = artifact;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateInterdependenciesDeadBody()
		{
			var result = true;

			var artUid = Record.GetDeadBodyUid();

			if (artUid > 0)
			{
				var artifact = gADB[artUid];

				if (artifact == null)
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("DeadBody"), "Artifact", artUid, "which doesn't exist");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					NewRecordUid = artUid;

					goto Cleanup;
				}
				else if (artifact.Location > 0 && Record.Location > 0)
				{
					result = false;

					Buf.SetFormat(gEngine.RecIdepErrorFmtStr, GetPrintedName("DeadBody"), "Artifact", artUid, "which should have a location compatible with this Monster's, but doesn't");

					ErrorMessage = Buf.ToString();

					RecordType = typeof(IArtifact);

					EditRecord = artifact;

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		#endregion

		#region PrintDesc Methods

		/// <summary></summary>
		public virtual void PrintDescName()
		{
			var fullDesc = "Enter the name of the Monster." + Environment.NewLine + Environment.NewLine + "Monster names should always be in singular form and capitalized when appropriate.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescStateDesc()
		{
			var fullDesc = "Enter the state description of the Monster (will typically be empty).";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescDesc()
		{
			var fullDesc = "Enter a detailed description of the Monster.";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, null);
		}

		/// <summary></summary>
		public virtual void PrintDescSeen()
		{
			var fullDesc = "Enter whether the player has seen the Monster.";

			var briefDesc = "0=Not seen; 1=Seen";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescIsListed()
		{
			var fullDesc = "Enter whether the Monster is included in all appropriate content lists (Room, inventory, etc.)" + Environment.NewLine + Environment.NewLine + "Monsters should always be included in content lists except in rare circumstances (invisibility, vision impairment, mentioned in Room description, etc.); excluding them typically requires support from special (user-programmed) events.";

			var briefDesc = "0=Not listed; 1=Listed";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescPluralType()
		{
			var fullDesc = "Enter the plural type that converts the Monster's name from singular to plural.";

			var briefDesc = "0=No change; 1=Use 's'; 2=Use 'es'; 3=Use 'y' to 'ies'; (1000 + N)=Use Effect Uid N as plural name";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescArticleType()
		{
			var fullDesc = "Enter the article type that prefixes the Monster's name with an article.";

			var briefDesc = "0=No article; 1=Use 'a'; 2=Use 'an'; 3=Use 'some'; 4=Use 'the'";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescHardiness()
		{
			var fullDesc = "Enter the Hardiness of the Monster.";

			var briefDesc = "2-8=Weak Monster; 9-15=Medium Monster; 16-30=Tough Monster; 31-60=Exceptional Monster";

			if (FieldDesc == FieldDesc.Full)
			{
				Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, gEngine.ToughDesc, fullDesc, briefDesc);
			}
			else if (FieldDesc == FieldDesc.Brief)
			{
				Buf01.AppendPrint("{0}", briefDesc);
			}
		}

		/// <summary></summary>
		public virtual void PrintDescAgility()
		{
			var fullDesc = "Enter the Agility of the Monster.";

			var briefDesc = "5-9=Weak Monster; 10-16=Medium Monster; 17-24=Tough Monster; 25-30=Exceptional Monster";

			if (FieldDesc == FieldDesc.Full)
			{
				if (EditRec && EditField)
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, gEngine.ToughDesc, fullDesc, briefDesc);
				}
				else
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}", Environment.NewLine, fullDesc, briefDesc);
				}
			}
			else if (FieldDesc == FieldDesc.Brief)
			{
				Buf01.AppendPrint("{0}", briefDesc);
			}
		}

		/// <summary></summary>
		public virtual void PrintDescGroupCount()
		{
			var fullDesc = "Enter the number of members in the Monster's group." + (gEngine.IsRulesetVersion(5, 62) ? Environment.NewLine + Environment.NewLine + "For classic Eamon games this value should always be 1." : "");

			var briefDesc = "1=Single Monster; (GT 1)=Multiple Monsters";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescAttackCount()
		{
			var fullDesc = "Enter the number of attacks per round for each member of the Monster's group." + Environment.NewLine + Environment.NewLine + (gEngine.IsRulesetVersion(5, 62) ? "For classic Eamon games this value should always be 1." : "When the value is < -1, ABS(value) is attacks per round.");

			var briefDesc = "1=Single attack, single target; (GT 1)=Multiple attacks, single target;      (LT -1)=Multiple attacks, multiple targets";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescCourage()
		{
			var fullDesc = "Enter the courage of the Monster." + (gEngine.IsRulesetVersion(5, 62) ? Environment.NewLine + Environment.NewLine + "For classic Eamon games this value should always be between 1 and 105, inclusive." : "");

			var briefDesc = "80-90=Weak Monster; 95-100=Medium Monster; 200=Tough/Exceptional Monster";

			if (FieldDesc == FieldDesc.Full)
			{
				if (EditRec && EditField)
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}{0}{4}{0}", Environment.NewLine, gEngine.ToughDesc, gEngine.CourageDesc, fullDesc, briefDesc);
				}
				else
				{
					Buf01.AppendFormat("{0}{1}{0}{0}{2}{0}{0}{3}{0}", Environment.NewLine, gEngine.CourageDesc, fullDesc, briefDesc);
				}
			}
			else if (FieldDesc == FieldDesc.Brief)
			{
				Buf01.AppendPrint("{0}", briefDesc);
			}
		}

		/// <summary></summary>
		public virtual void PrintDescLocation()
		{
			var fullDesc = "Enter the location of the Monster.";

			var briefDesc = "(LE 0)=Limbo; (GT 0)=Room Uid";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescCombatCode()
		{
			var fullDesc = "Enter the combat code that describes the Monster's combat behavior.";

			var briefDesc = new StringBuilder(gEngine.BufSize);

			var combatCodeValues = EnumUtil.GetValues<CombatCode>();

			for (var j = 0; j < combatCodeValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)combatCodeValues[j], gEngine.GetCombatCodeDesc(combatCodeValues[j]));
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescArmor()
		{
			var fullDesc = "Enter the armor of the Monster." + Environment.NewLine + Environment.NewLine + "The Monster absorbs up to this many hit points of damage per strike before being injured in combat.";

			var briefDesc = "0=Weak Monster; 1=Medium Monster; 2-3=Tough Monster; 4-7+=Exceptional Monster";

			var briefDesc01 = new StringBuilder(gEngine.BufSize);

			var armorValues = EnumUtil.GetValues<Armor>(a => ((long)a) % 2 == 0);

			for (var j = 0; j < armorValues.Count; j++)
			{
				var armor = gEngine.GetArmor(armorValues[j]);

				Debug.Assert(armor != null);

				briefDesc01.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (((long)armorValues[j]) / 2) + (j >= 3 ? 2 : 0), armor.Name);

				if (j == 2)
				{
					briefDesc01.AppendFormat("{0}{1}={2}", "; ", 3, "Splint Mail");

					briefDesc01.AppendFormat("{0}{1}={2}", "; ", 4, "Splint Mail");
				}
			}

			briefDesc01.AppendFormat("{0}{0}{1}", Environment.NewLine, briefDesc);

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc01.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescWeapon()
		{
			var fullDesc = "Enter the weapon of the Monster.";

			var briefDesc = "(LT 0)=Weaponless; 0=Natural weapons; (GT 0)=Weapon Artifact Uid";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescNwDice()
		{
			var fullDesc = "Enter the Monster's natural weapon hit dice.";

			var briefDesc = "(GE 0)=Valid value";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescNwSides()
		{
			var fullDesc = "Enter the Monster's natural weapon hit dice sides.";

			var briefDesc = "(GE 0)=Valid value";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescDeadBody()
		{
			var fullDesc = "Enter the dead body of the Monster.";

			var briefDesc = "0=No dead body used; (GT 0)=Dead body Artifact Uid";

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc);
		}

		/// <summary></summary>
		public virtual void PrintDescFriendliness()
		{
			int j;

			var fullDesc = "Enter the friendliness of the Monster." + (gEngine.IsRulesetVersion(5, 62) ? Environment.NewLine + Environment.NewLine + "For classic Eamon games this value should always be between 100 and 200, inclusive." : "");

			var briefDesc = new StringBuilder(gEngine.BufSize);

			var friendlinessValues = EnumUtil.GetValues<Friendliness>();

			for (j = 0; j < friendlinessValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)friendlinessValues[j], gEngine.EvalFriendliness(friendlinessValues[j], "Enemy", "Neutral", "Friend"));
			}

			briefDesc.AppendFormat("{0}(100 + N)=N% chance of being friend", j != 0 ? "; " : "");

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
		}

		/// <summary></summary>
		public virtual void PrintDescGender()
		{
			var fullDesc = "Enter the gender of the Monster.";

			var briefDesc = new StringBuilder(gEngine.BufSize);

			var genderValues = EnumUtil.GetValues<Gender>();

			for (var j = 0; j < genderValues.Count; j++)
			{
				briefDesc.AppendFormat("{0}{1}={2}", j != 0 ? "; " : "", (long)genderValues[j], gEngine.EvalGender(genderValues[j], "Male", "Female", "Neutral"));
			}

			gEngine.AppendFieldDesc(FieldDesc, Buf01, fullDesc, briefDesc.ToString());
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
		public virtual void ListStateDesc()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (!string.IsNullOrWhiteSpace(Record.StateDesc))
				{
					Buf.Clear();

					Buf.Append(Record.StateDesc);

					gOut.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("StateDesc"), null), Buf);
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("StateDesc"), null), Record.StateDesc);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListDesc()
		{
			if (FullDetail && ShowDesc)
			{
				Buf.Clear();

				if (ResolveEffects)
				{
					var rc = gEngine.ResolveUidMacros(Record.Desc, Buf, true, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}
				else
				{
					Buf.Append(Record.Desc);
				}

				var listNum = NumberFields ? ListNum++ : 0;

				gOut.WriteLine("{0}{1}{0}{0}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Desc"), null), Buf);
			}
		}

		/// <summary></summary>
		public virtual void ListSeen()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Seen"), null), Convert.ToInt64(Record.Seen));
			}
		}

		/// <summary></summary>
		public virtual void ListIsListed()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("IsListed"), null), Convert.ToInt64(Record.IsListed));
			}
		}

		/// <summary></summary>
		public virtual void ListPluralType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					Buf.Clear();

					Buf01.Clear();

					var effectUid = gEngine.GetPluralTypeEffectUid(Record.PluralType);

					var effect = gEDB[effectUid];

					if (effect != null)
					{
						Buf01.Append(effect.Desc.Length > gEngine.MonNameLen - 6 ? effect.Desc.Substring(0, gEngine.MonNameLen - 9) + "..." : effect.Desc);

						Buf.AppendFormat("Use '{0}'", Buf01.ToString());
					}
					else
					{
						Buf.AppendFormat("Use Effect Uid {0}", effectUid);
					}

					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("PluralType"), null),
						gEngine.BuildValue(51, ' ', 8, (long)Record.PluralType, null,
						Record.PluralType == PluralType.None ? "No change" :
						Record.PluralType == PluralType.S ? "Use 's'" :
						Record.PluralType == PluralType.Es ? "Use 'es'" :
						Record.PluralType == PluralType.YIes ? "Use 'y' to 'ies'" :
						Buf.ToString()));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("PluralType"), null), (long)Record.PluralType);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListArticleType()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ArticleType"), null),
						gEngine.BuildValue(51, ' ', 8, (long)Record.ArticleType, null,
						Record.ArticleType == ArticleType.None ? "No article" :
						Record.ArticleType == ArticleType.A ? "Use 'a'" :
						Record.ArticleType == ArticleType.An ? "Use 'an'" :
						Record.ArticleType == ArticleType.Some ? "Use 'some'" :
						"Use 'the'"));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("ArticleType"), null), (long)Record.ArticleType);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListHardiness()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Hardiness"), null), Record.Hardiness);
			}
		}

		/// <summary></summary>
		public virtual void ListAgility()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Agility"), null), Record.Agility);
			}
		}

		/// <summary></summary>
		public virtual void ListGroupCount()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("GroupCount"), null), Record.GroupCount);
			}
		}

		/// <summary></summary>
		public virtual void ListAttackCount()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("AttackCount"), null), Record.AttackCount);
			}
		}

		/// <summary></summary>
		public virtual void ListCourage()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}%", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Courage"), null), Record.Courage);
			}
		}

		/// <summary></summary>
		public virtual void ListLocation()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg && Record.Location > 0)
				{
					var room = gRDB[Record.Location];

					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Location"), null),
						gEngine.BuildValue(51, ' ', 8, Record.Location, null, room != null ? gEngine.Capitalize(room.Name) : gEngine.UnknownName));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Location"), null), Record.Location);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListCombatCode()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CombatCode"), null),
						gEngine.BuildValue(51, ' ', 8, (long)Record.CombatCode, null, gEngine.GetCombatCodeDesc(Record.CombatCode)));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("CombatCode"), null), (long)Record.CombatCode);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListArmor()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					var armor = Record.Armor <= 2 ? gEngine.GetArmor((Armor)(Record.Armor * 2)) :
						Record.Armor <= 4 ? gEngine.CreateInstance<IArmor>(x => { x.Name = "Splint Mail"; }) :
						gEngine.GetArmor((Armor)((Record.Armor - 2) * 2));

					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Armor"), null),
						gEngine.BuildValue(51, ' ', 8, Record.Armor, null, armor != null ? armor.Name : "Magic/Exotic"));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Armor"), null), Record.Armor);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListWeapon()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					if (Record.Weapon > 0)
					{
						var artifact = gADB[Record.Weapon];

						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Weapon"), null),
							gEngine.BuildValue(51, ' ', 8, Record.Weapon, null, artifact != null ? gEngine.Capitalize(artifact.Name) : gEngine.UnknownName));
					}
					else
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Weapon"), null),
							gEngine.BuildValue(51, ' ', 8, Record.Weapon, null, Record.Weapon == 0 ? "Natural weapons" : "Weaponless"));
					}
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Weapon"), null), Record.Weapon);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListNwDice()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("NwDice"), null), Record.NwDice);
			}
		}

		/// <summary></summary>
		public virtual void ListNwSides()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("NwSides"), null), Record.NwSides);
			}
		}

		/// <summary></summary>
		public virtual void ListDeadBody()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					if (Record.DeadBody > 0)
					{
						var artifact = gADB[Record.DeadBody];

						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("DeadBody"), null),
							gEngine.BuildValue(51, ' ', 8, Record.DeadBody, null, artifact != null ? gEngine.Capitalize(artifact.Name) : gEngine.UnknownName));
					}
					else
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("DeadBody"), null),
							gEngine.BuildValue(51, ' ', 8, Record.DeadBody, null, "No dead body"));
					}
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("DeadBody"), null), Record.DeadBody);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListFriendliness()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					if (gEngine.IsValidMonsterFriendlinessPct(Record.Friendliness))
					{
						Buf.SetFormat("{0}% Chance", gEngine.GetMonsterFriendlinessPct(Record.Friendliness));

						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Friendliness"), null),
							gEngine.BuildValue(51, ' ', 8, (long)Record.Friendliness, null, Buf.ToString()));
					}
					else
					{
						gOut.Write("{0}{1}{2}",
							Environment.NewLine,
							gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Friendliness"), null),
							gEngine.BuildValue(51, ' ', 8, (long)Record.Friendliness, null, gEngine.EvalFriendliness(Record.Friendliness, "Enemy", "Neutral", "Friend")));
					}
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Friendliness"), null), (long)Record.Friendliness);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListGender()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				if (LookupMsg)
				{
					gOut.Write("{0}{1}{2}",
						Environment.NewLine,
						gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Gender"), null),
						gEngine.BuildValue(51, ' ', 8, (long)Record.Gender, null, Record.EvalGender("Male", "Female", "Neutral")));
				}
				else
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Gender"), null), (long)Record.Gender);
				}
			}
		}

		/// <summary></summary>
		public virtual void ListField1()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Field1"), null), Record.Field1);
			}
		}

		/// <summary></summary>
		public virtual void ListField2()
		{
			if (FullDetail)
			{
				var listNum = NumberFields ? ListNum++ : 0;

				gOut.Write("{0}{1}{2}", Environment.NewLine, gEngine.BuildPrompt(27, '.', listNum, GetPrintedName("Field2"), null), Record.Field2);
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

				var rc = gEngine.In.ReadField(Buf, gEngine.MonNameLen, null, '_', '\0', false, null, null, null, null);

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
		public virtual void InputStateDesc()
		{
			var fieldDesc = FieldDesc;

			var stateDesc = Record.StateDesc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", stateDesc);

				PrintFieldDesc("StateDesc", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("StateDesc"), null));

				gOut.WordWrap = false;

				var rc = gEngine.In.ReadField(Buf, gEngine.MonStateDescLen, null, '_', '\0', true, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				Record.StateDesc = Buf.TrimEnd().ToString();

				if (ValidateField("StateDesc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputDesc()
		{
			var fieldDesc = FieldDesc;

			var desc = Record.Desc;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", desc);

				PrintFieldDesc("Desc", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Desc"), null));

				gOut.WordWrap = false;

				var rc = gEngine.In.ReadField(Buf, gEngine.MonDescLen, null, '_', '\0', false, null, null, null, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.WordWrap = true;

				Record.Desc = Buf.Trim().ToString();

				if (ValidateField("Desc"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputSeen()
		{
			var fieldDesc = FieldDesc;

			var seen = Record.Seen;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(seen));

				PrintFieldDesc("Seen", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Seen"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Seen = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("Seen"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputIsListed()
		{
			var fieldDesc = FieldDesc;

			var isListed = Record.IsListed;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", Convert.ToInt64(isListed));

				PrintFieldDesc("IsListed", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("IsListed"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsChar0Or1, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.IsListed = Convert.ToInt64(Buf.Trim().ToString()) != 0 ? true : false;

				if (ValidateField("IsListed"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputPluralType()
		{
			var fieldDesc = FieldDesc;

			var pluralType = Record.PluralType;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)pluralType);

				PrintFieldDesc("PluralType", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("PluralType"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.PluralType = (PluralType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("PluralType"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputArticleType()
		{
			var fieldDesc = FieldDesc;

			var articleType = Record.ArticleType;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)articleType);

				PrintFieldDesc("ArticleType", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("ArticleType"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.ArticleType = (ArticleType)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("ArticleType"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputHardiness()
		{
			var fieldDesc = FieldDesc;

			var hardiness = Record.Hardiness;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", hardiness);

				PrintFieldDesc("Hardiness", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Hardiness"), "16"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "16", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Hardiness = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Hardiness"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Record.DmgTaken = 0;

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputAgility()
		{
			var fieldDesc = FieldDesc;

			var agility = Record.Agility;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", agility);

				PrintFieldDesc("Agility", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Agility"), "15"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "15", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Agility = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Agility"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputGroupCount()
		{
			var fieldDesc = FieldDesc;

			var groupCount = Record.GroupCount;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", groupCount);

				PrintFieldDesc("GroupCount", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("GroupCount"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.GroupCount = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("GroupCount"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Record.CurrGroupCount = Record.GroupCount;

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputAttackCount()
		{
			var fieldDesc = FieldDesc;

			var attackCount = Record.AttackCount;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", attackCount);

				PrintFieldDesc("AttackCount", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("AttackCount"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.AttackCount = Convert.ToInt64(Buf.Trim().ToString());

					if (Record.AttackCount == 0)
					{
						error = true;
					}
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("AttackCount"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputCourage()
		{
			var fieldDesc = FieldDesc;

			var courage = Record.Courage;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", courage);

				PrintFieldDesc("Courage", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Courage"), "100"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "100", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Courage = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Courage"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputLocation()
		{
			var fieldDesc = FieldDesc;

			var location = Record.Location;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", location);

				PrintFieldDesc("Location", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Location"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Location = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Location"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputCombatCode()
		{
			var fieldDesc = FieldDesc;

			var combatCode = Record.CombatCode;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)combatCode);

				PrintFieldDesc("CombatCode", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("CombatCode"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.CombatCode = (CombatCode)Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("CombatCode"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputArmor()
		{
			var fieldDesc = FieldDesc;

			var armor = Record.Armor;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", armor);

				PrintFieldDesc("Armor", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Armor"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Armor = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Armor"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputWeapon()
		{
			var fieldDesc = FieldDesc;

			var weapon = Record.Weapon;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", weapon);

				PrintFieldDesc("Weapon", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Weapon"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Weapon = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Weapon"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputNwDice()
		{
			var fieldDesc = FieldDesc;

			var nwDice = Record.NwDice;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", nwDice);

				PrintFieldDesc("NwDice", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("NwDice"), "1"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.NwDice = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("NwDice"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputNwSides()
		{
			var fieldDesc = FieldDesc;

			var nwSides = Record.NwSides;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", nwSides);

				PrintFieldDesc("NwSides", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("NwSides"), "4"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "4", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.NwSides = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("NwSides"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputDeadBody()
		{
			var fieldDesc = FieldDesc;

			var deadBody = Record.DeadBody;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", deadBody);

				PrintFieldDesc("DeadBody", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("DeadBody"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.DeadBody = Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("DeadBody"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputFriendliness()
		{
			var fieldDesc = FieldDesc;

			var friendliness = Record.Friendliness;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", (long)friendliness);

				PrintFieldDesc("Friendliness", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Friendliness"), "3"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "3", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				Record.Friendliness = (Friendliness)Convert.ToInt64(Buf.Trim().ToString());

				if (ValidateField("Friendliness"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			Record.ResolveReaction(10);

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
		public virtual void InputField1()
		{
			var fieldDesc = FieldDesc;

			var field1 = Record.Field1;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", field1);

				PrintFieldDesc("Field1", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Field1"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Field1 = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Field1"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		/// <summary></summary>
		public virtual void InputField2()
		{
			var fieldDesc = FieldDesc;

			var field2 = Record.Field2;

			while (true)
			{
				Buf.SetFormat(EditRec ? "{0}" : "", field2);

				PrintFieldDesc("Field2", EditRec, EditField, fieldDesc);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(27, '\0', 0, GetPrintedName("Field2"), "0"));

				var rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharPlusMinusDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var error = false;

				try
				{
					Record.Field2 = Convert.ToInt64(Buf.Trim().ToString());
				}
				catch (Exception)
				{
					error = true;
				}

				if (!error && ValidateField("Field2"))
				{
					break;
				}

				fieldDesc = FieldDesc.Brief;
			}

			gOut.Print("{0}", gEngine.LineSep);
		}

		#endregion

		#region BuildValue Methods

		// do nothing

		#endregion

		#endregion

		#region Class MonsterHelper

		public override void SetUidIfInvalid()
		{
			if (Record.Uid <= 0)
			{
				Record.Uid = gEngine.Database.GetMonsterUid();

				Record.IsUidRecycled = true;
			}
			else if (!EditRec)
			{
				Record.IsUidRecycled = false;
			}
		}

		public MonsterHelper()
		{

		}

		#endregion

		#endregion
	}
}
