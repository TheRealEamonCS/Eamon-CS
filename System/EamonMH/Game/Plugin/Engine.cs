
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Menus;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus;
using EamonMH.Framework.Plugin;
//using static EamonMH.Game.Plugin.Globals;

namespace EamonMH.Game.Plugin
{
	public class Engine : Eamon.Game.Plugin.Engine, IEngine
	{
		#region Public Properties

		public virtual string[] Argv { get; set; }

		public virtual long WordWrapCurrColumn { get; set; }

		public virtual char WordWrapLastChar { get; set; }

		public virtual string ConfigFileName { get; set; }

		public virtual string CharacterName { get; set; }

		public virtual IMhMenu MhMenu { get; set; }

		public virtual IMenu Menu { get; set; }

		public virtual IFileset Fileset { get; set; }

		public virtual ICharacter Character { get; set; }

		public virtual IConfig Config { get; set; }

		public virtual bool GoOnAdventure { get; set; }

		public virtual bool ConfigsModified { get; set; }

		public virtual bool FilesetsModified { get; set; }

		public virtual bool CharactersModified { get; set; }

		public virtual bool EffectsModified { get; set; }

		#endregion

		#region Public Methods

		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

		Cleanup:

			return rc;
		}

		public override void InitSystem()
		{
			base.InitSystem();

			ConfigFileName = "";

			CharacterName = "";

			MhMenu = CreateInstance<IMhMenu>();

			Config = CreateInstance<IConfig>();
		}

		public override void ResetProperties(PropertyResetCode resetCode)
		{
			base.ResetProperties(resetCode);
		}

		public virtual bool IsCharDOrM(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'D' || ch == 'M';
		}

		public virtual bool IsCharROrT(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'R' || ch == 'T';
		}

		public virtual bool IsCharDOrIOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'D' || ch == 'I' || ch == 'X';
		}

		public virtual bool IsCharDOrWOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'D' || ch == 'W' || ch == 'X';
		}

		public virtual bool IsCharUOrCOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'U' || ch == 'C' || ch == 'X';
		}

		public virtual bool IsChar1OrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == '1' || ch == 'X';
		}

		public virtual bool IsChar1Or2OrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == '1' || ch == '2' || ch == 'X';
		}

		public virtual bool IsCharTOrL(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'T' || ch == 'L';
		}

		public virtual bool IsCharBOrSOrAOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'B' || ch == 'S' || ch == 'A' || ch == 'X';
		}

		public virtual bool IsCharGOrFOrPOrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'G' || ch == 'F' || ch == 'P' || ch == 'X';
		}

		public virtual bool IsCharWpnType(char ch)
		{
			var weaponValues = EnumUtil.GetValues<Weapon>();

			ch = Char.ToUpper(ch);

			return ch >= ('0' + (long)weaponValues[0]) && ch <= ('0' + (long)weaponValues[weaponValues.Count - 1]);
		}

		public virtual bool IsCharWpnTypeOrX(char ch)
		{
			var weaponValues = EnumUtil.GetValues<Weapon>();

			ch = Char.ToUpper(ch);

			return (ch >= ('0' + (long)weaponValues[0]) && ch <= ('0' + (long)weaponValues[weaponValues.Count - 1])) || ch == 'X';
		}

		public virtual bool IsCharSpellType(char ch)
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			ch = Char.ToUpper(ch);

			return ch >= ('0' + (long)spellValues[0]) && ch <= ('0' + (long)spellValues[spellValues.Count - 1]);
		}

		public virtual bool IsCharSpellTypeOrX(char ch)
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			ch = Char.ToUpper(ch);

			return (ch >= ('0' + (long)spellValues[0]) && ch <= ('0' + (long)spellValues[spellValues.Count - 1])) || ch == 'X';
		}

		public virtual bool IsCharMarcosNumOrX(char ch)
		{
			var i = GetMaxArmorMarcosNum();

			ch = Char.ToUpper(ch);

			return (ch >= '1' && ch <= ('1' + (i - 1))) || ch == 'X';
		}

		public virtual bool IsCharWpnNumOrX(char ch)
		{
			long i = 0;

			var rc = Character.GetWeaponCount(ref i);

			Debug.Assert(IsSuccess(rc));

			ch = Char.ToUpper(ch);

			return (ch >= '1' && ch <= ('1' + (i - 1))) || ch == 'X';
		}

		public virtual bool IsCharStat(char ch)
		{
			var statValues = EnumUtil.GetValues<Stat>();

			ch = Char.ToUpper(ch);

			return ch >= ('0' + (long)statValues[0]) && ch <= ('0' + (long)statValues[statValues.Count - 1]);
		}

		public virtual long GetMaxArmorMarcosNum()
		{
			long rc = 0;

			var armorValues = EnumUtil.GetValues<Armor>();

			for (var i = 0; i < armorValues.Count; i++)
			{
				var armor = GetArmor(armorValues[i]);

				Debug.Assert(armor != null);

				if (rc < armor.MarcosNum)
				{
					rc = armor.MarcosNum;
				}
			}

			return rc;
		}

		public virtual IArmor GetArmorByMarcosNum(long marcosNum)
		{
			IArmor rc = null;

			var armorValues = EnumUtil.GetValues<Armor>();

			for (var i = 0; i < armorValues.Count; i++)
			{
				var armor = GetArmor(armorValues[i]);

				Debug.Assert(armor != null);

				if (armor.MarcosNum == marcosNum)
				{
					rc = armor;

					break;
				}
			}

			return rc;
		}

		public virtual void MhProcessArgv(bool secondPass, ref bool nlFlag)
		{
			long i;

			for (i = 0; i < Argv.Length; i++)
			{
				if (Argv[i].Equals("--workingDirectory", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						// do nothing
					}
				}
				else if (Argv[i].Equals("--filePrefix", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						// do nothing
					}
				}
				else if (Argv[i].Equals("--configFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && !secondPass)
					{
						ConfigFileName = Argv[i].Trim();
					}
				}
				else if (Argv[i].Equals("--filesetFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.MhFilesetFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--characterFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.MhCharacterFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--effectFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.MhEffectFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--characterName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-chrnm", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						CharacterName = Argv[i].Trim();
					}
				}
				else if (Argv[i].Equals("--enableScreenReaderMode", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-esrm", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Argv[i].Equals("--ignoreMutex", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-im", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Argv[i].Equals("--disableValidation", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-dv", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						Out.Print("{0}", LineSep);
					}

					Out.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Argv[i]);

					nlFlag = true;
				}
			}
		}

		#endregion
	}
}
