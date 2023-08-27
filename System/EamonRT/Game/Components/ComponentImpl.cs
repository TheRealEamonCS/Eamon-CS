
// ComponentImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Components
{
	[ClassMappings]
	public class ComponentImpl : IComponentImpl
	{
		public virtual IComponent Component { get; set; }

		public virtual Action<IState> SetNextStateFunc { get; set; }

		public virtual Action<ICommand> CopyCommandDataFunc { get; set; }

		public virtual IMonster ActorMonster { get; set; }

		public virtual IRoom ActorRoom { get; set; }

		public virtual IGameBase Dobj { get; set; }

		public virtual IArtifact DobjArtifact
		{
			get
			{
				return Component.Dobj as IArtifact;
			}
		}

		public virtual IMonster DobjMonster
		{
			get
			{
				return Component.Dobj as IMonster;
			}
		}

		public virtual IGameBase Iobj { get; set; }

		public virtual IArtifact IobjArtifact
		{
			get
			{
				return Component.Iobj as IArtifact;
			}
		}

		public virtual IMonster IobjMonster
		{
			get
			{
				return Component.Iobj as IMonster;
			}
		}

		public virtual ICommand RedirectCommand { get; set; }

		public virtual IArtifactCategory DobjArtAc { get; set; }

		public virtual bool OmitSkillGains { get; set; }

		public virtual bool OmitFinalNewLine { get; set; }

		/// <summary></summary>
		public virtual string ActorMonsterName { get; set; }

		/// <summary></summary>
		public virtual string DobjMonsterName { get; set; }

		/// <summary></summary>
		public virtual string AttackDesc { get; set; }

		/// <summary></summary>
		public virtual string MissDesc { get; set; }

		/// <summary></summary>
		public virtual string ArmorDesc { get; set; }

		public virtual void PrintAttack(IRoom room, IMonster actorMonster, IMonster dobjMonster, IArtifact weapon, long attackNumber, WeaponRevealType weaponRevealType)
		{
			Debug.Assert(room != null && actorMonster != null && dobjMonster != null /* && attackNumber > 0 && Enum.IsDefined(typeof(WeaponRevealType), weaponRevealType) */);

			AttackDesc = actorMonster.GetAttackDescString(room, weapon);

			ActorMonsterName = actorMonster.IsCharacterMonster() ? "You" :
					room.EvalLightLevel(attackNumber == 1 ? "An unseen offender" : "The unseen offender",
						actorMonster.InitGroupCount > 1 && attackNumber == 1 ? actorMonster.GetArticleName(true, true, false, false, true) : actorMonster.GetTheName(true, true, false, false, true));

			DobjMonsterName = dobjMonster.IsCharacterMonster() ? "you" :
					room.EvalLightLevel("an unseen defender",
					dobjMonster.InitGroupCount > 1 ? dobjMonster.GetArticleName(groupCountOne: true) : dobjMonster.GetTheName(groupCountOne: true));

			gOut.Write("{0}{1} {2} {3}{4}.",
				Environment.NewLine,
				ActorMonsterName,
				AttackDesc,
				DobjMonsterName,
					weapon != null &&
					(weaponRevealType == WeaponRevealType.Always ||
					(weaponRevealType == WeaponRevealType.OnlyIfSeen && weapon.Seen)) ?
						" with " + room.EvalLightLevel("a weapon", weapon.GetArticleName()) :
						"");
		}

		public virtual void PrintMiss(IMonster monster, IArtifact weapon)
		{
			Debug.Assert(monster != null);

			MissDesc = monster.GetMissDescString(weapon);

			gOut.Write("{0} {1} {2}!", Environment.NewLine, gEngine.EnableScreenReaderMode ? "" : "---", MissDesc);
		}

		public virtual void PrintFumble()
		{
			gOut.Write("{0} {1} A fumble!", Environment.NewLine, gEngine.EnableScreenReaderMode ? "" : "...");
		}

		public virtual void PrintRecovered()
		{
			gOut.Write("{0}  Recovered.", Environment.NewLine);
		}

		public virtual void PrintWeaponDropped(IRoom room, IMonster monster, IArtifact weapon, WeaponRevealType weaponRevealType)
		{
			Debug.Assert(room != null && monster != null && weapon != null /* && Enum.IsDefined(typeof(WeaponRevealType), weaponRevealType) */);

			gOut.Write("{0}  {1} {2} {3}!",
				Environment.NewLine,
				monster.IsCharacterMonster() ? "You" :
				room.EvalLightLevel("The offender", monster.GetTheName(true, true, false, false, true)),
				monster.IsCharacterMonster() ? "drop" : "drops",
				monster.IsCharacterMonster() || room.IsLit() ?
					(
						(weaponRevealType == WeaponRevealType.Never ||
						(weaponRevealType == WeaponRevealType.OnlyIfSeen && !weapon.Seen)) ?
							weapon.GetArticleName() :
							weapon.GetTheName()
					) :
					"a weapon");
		}

		public virtual void PrintWeaponHitsUser()
		{
			gOut.Write("{0}  Weapon hits user!", Environment.NewLine);
		}

		public virtual void PrintSparksFly(IRoom room, IMonster monster, IArtifact weapon, WeaponRevealType weaponRevealType)
		{
			Debug.Assert(room != null && monster != null && weapon != null /* && Enum.IsDefined(typeof(WeaponRevealType), weaponRevealType) */);

			gOut.Write("{0}  Sparks fly from {1}!",
				Environment.NewLine,
				monster.IsCharacterMonster() || room.IsLit() ?
					(
						(weaponRevealType == WeaponRevealType.Never ||
						(weaponRevealType == WeaponRevealType.OnlyIfSeen && !weapon.Seen)) ?
							weapon.GetArticleName() :
							weapon.GetTheName()
					) :
					"a weapon");
		}

		public virtual void PrintWeaponDamaged()
		{
			gOut.Write("{0}  Weapon damaged!", Environment.NewLine);
		}

		public virtual void PrintWeaponBroken()
		{
			gOut.Write("{0}  Weapon broken!", Environment.NewLine);
		}

		public virtual void PrintBrokenWeaponHitsUser()
		{
			gOut.Write("{0}  Broken weapon hits user!", Environment.NewLine);
		}

		public virtual void PrintStarPlus(IMonster monster)
		{
			Debug.Assert(monster != null);

			gOut.Write("{0} {1} ", Environment.NewLine, gEngine.EnableScreenReaderMode ? "" : monster.IsCharacterMonster() ? "***" : "+++");
		}

		public virtual void PrintHit()
		{
			gOut.Write("A hit!");
		}

		public virtual void PrintCriticalHit()
		{
			gOut.Write("A critical hit!");
		}

		public virtual void PrintBlowTurned(IMonster monster, bool omitBboaPadding)
		{
			Debug.Assert(monster != null);

			if (monster.Armor > 0)
			{
				ArmorDesc = monster.GetArmorDescString();

				gOut.Write("{0}{1}Blow bounces off {2}!", Environment.NewLine, omitBboaPadding ? "" : "  ", ArmorDesc);
			}
			else
			{
				gOut.Write("{0}{1}Blow turned!", Environment.NewLine, omitBboaPadding ? "" : "  ");
			}
		}

		public virtual void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell, bool nonCombat)
		{
			Debug.Assert(room != null && dobjMonster != null);

			DobjMonsterName = dobjMonster.IsCharacterMonster() ? "You" :
				blastSpell && dobjMonster.InitGroupCount > 1 ? room.EvalLightLevel(dobjMonster == actorMonster ? "An offender" : "A defender", dobjMonster.GetArticleName(true, true, false, false, true)) :
				room.EvalLightLevel(nonCombat ? "The entity" : dobjMonster == actorMonster ? "The offender" : "The defender", dobjMonster.GetTheName(true, true, false, false, true));

			gEngine.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				DobjMonsterName,
				dobjMonster.IsCharacterMonster() ? "are" : "is");

			dobjMonster.AddHealthStatus(gEngine.Buf, false);

			gOut.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintZapDirectHit()
		{
			gEngine.PrintZapDirectHit();
		}

		public virtual void PrintHackToBits(IArtifact artifact, IMonster monster, bool blastSpell)
		{
			Debug.Assert(artifact != null && monster != null);

			gOut.Print("You {0} {1} to bits!", blastSpell ? "blast" : monster.Weapon > 0 ? "hack" : "tear", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintAlreadyBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You already broke {0}!", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintNothingHappens()
		{
			gEngine.PrintNothingHappens();
		}

		public virtual void PrintWhamHitObj(IArtifact artifact)
		{
			gEngine.PrintWhamHitObj(artifact);
		}

		public virtual void PrintSmashesToPieces(IRoom room, IArtifact artifact, bool spillContents)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0}{1} {2}{3}!",
				Environment.NewLine,
				artifact.GetTheName(true),
				gEngine.IsRulesetVersion(62) ? 
					artifact.EvalPlural("shatters", "shatter") : 
					artifact.EvalPlural("smashes to pieces", "smash to pieces"),
				spillContents ?
					string.Format("; {0} contents spill {1}",
						artifact.EvalPlural("its", "their"), 
						room != null ? room.EvalRoomType("to the floor", "to the ground") : "forth") :
					"");
		}

		public virtual void PrintWeaponAbilityIncreases(Weapon w, IWeapon weapon)
		{
			Debug.Assert(Enum.IsDefined(typeof(Weapon), w) && weapon != null);

			gOut.Print("Your {0} ability increases!", weapon.Name);
		}

		public virtual void PrintArmorExpertiseIncreases()
		{
			gOut.Print("Your armor expertise increases!");
		}

		public virtual void PrintSpellOverloadsBrain(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s) && spell != null);

			gOut.Print("The strain of attempting to cast {0} overloads your brain and you forget it completely{1}.", 
				gEngine.IsRulesetVersion(5, 62) ? "this spell" : spell.Name, 
				gEngine.IsRulesetVersion(5, 62) ? "" : " for the rest of this adventure");
		}

		public virtual void PrintSpellAbilityIncreases(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s) && spell != null);

			gOut.Print("Your ability to cast {0} increases!", spell.Name);
		}

		public virtual void PrintSpellCastFailed(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s) && spell != null);

			gOut.Print("Nothing happens.");
		}

		public virtual void PrintHealthImproves(IMonster monster)
		{
			gEngine.PrintHealthImproves(monster);
		}

		public virtual void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters)
		{
			gEngine.PrintHealthStatus(monster, includeUninjuredGroupMonsters);
		}

		public virtual void PrintFeelNewAgility()
		{
			gOut.Print("You can feel the new agility flowing through you!");
		}

		public virtual void PrintSonicBoom(IRoom room)
		{
			Debug.Assert(room != null);

			if (gEngine.IsRulesetVersion(5, 62))
			{
				gOut.Print("You hear a very loud sonic boom that echoes through the {0}.", room.EvalRoomType("tunnels", "area"));
			}
			else
			{
				gOut.Print("You hear a loud sonic boom which echoes all around you!");
			}
		}

		public virtual void PrintFortuneCookie()
		{
			var rl = gEngine.RollDice(1, 100, 0);

			gOut.Print("A fortune cookie appears in mid-air and explodes!  The smoking paper left behind reads, \"{0}\"  How strange.",
				rl > 50 ?
				"THE SECTION OF TUNNEL YOU ARE IN COLLAPSES AND YOU DIE." :
				"YOU SUDDENLY FIND YOU CANNOT CARRY ALL OF THE ITEMS YOU ARE CARRYING, AND THEY ALL FALL TO THE GROUND.");
		}

		public virtual void PrintTunnelCollapses(IRoom room)
		{
			Debug.Assert(room != null);

			gOut.Print("The section of {0} collapses and you die.", room.EvalRoomType("tunnel you are in", "ground you are on"));
		}

		public virtual void PrintAllWoundsHealed()
		{
			gEngine.PrintAllWoundsHealed();
		}

		public virtual void PrintTeleportToRoom()
		{
			gOut.Print("There is a cloud of dust and a flash of light!");

			gOut.Print("You teleported somewhere!");
		}

		public virtual void PrintArmorThickens()
		{
			gOut.Print("Your armor thickens!");
		}

		public virtual void PrintMagicSkillsIncrease()
		{
			gOut.Print("Your magical prowess increases!");
		}

		public ComponentImpl()
		{
			// Here we make an exception to the "always use Component" rule

		}
	}
}
