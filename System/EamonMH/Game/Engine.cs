
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonMH.Framework;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game
{
	[ClassMappings(typeof(Eamon.Framework.IEngine))]
	public class Engine : Eamon.Game.Engine, IEngine
	{
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

			var rc = Globals.Character.GetWeaponCount(ref i);

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
				var armor = GetArmors(armorValues[i]);

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
				var armor = GetArmors(armorValues[i]);

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

			for (i = 0; i < Globals.Argv.Length; i++)
			{
				if (Globals.Argv[i].Equals("--workingDirectory", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (Globals.Argv[i].Equals("--filePrefix", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (Globals.Argv[i].Equals("--ignoreMutex", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-im", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Globals.Argv[i].Equals("--configFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && !secondPass)
					{
						Globals.ConfigFileName = Globals.Argv[i].Trim();
					}
				}
				else if (Globals.Argv[i].Equals("--filesetFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.MhFilesetFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--characterFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.MhCharacterFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--effectFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.MhEffectFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--characterName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-chrnm", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.CharacterName = Globals.Argv[i].Trim();
					}
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						gOut.Print("{0}", Globals.LineSep);
					}

					gOut.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Globals.Argv[i]);

					nlFlag = true;
				}
			}
		}
	}
}
