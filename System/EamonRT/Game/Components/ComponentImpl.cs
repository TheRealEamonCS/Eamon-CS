
// ComponentImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

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

		public virtual IArtifactCategory DobjArtAc { get; set; }

		public virtual bool OmitFinalNewLine { get; set; }

		public virtual void PrintAttack(IRoom room, IMonster actorMonster, IMonster dobjMonster, IArtifact weapon, long attackNumber, WeaponRevealType weaponRevealType)
		{
			Debug.Assert(room != null && actorMonster != null && dobjMonster != null /* && attackNumber > 0 && Enum.IsDefined(typeof(WeaponRevealType), weaponRevealType) */);

			var attackDesc = actorMonster.GetAttackDescString(room, weapon);

			var actorMonsterName = actorMonster.IsCharacterMonster() ? "You" :
					room.EvalLightLevel(attackNumber == 1 ? "An unseen offender" : "The unseen offender",
						actorMonster.InitGroupCount > 1 && attackNumber == 1 ? actorMonster.GetArticleName(true, true, false, true) : actorMonster.GetTheName(true, true, false, true));

			var dobjMonsterName = dobjMonster.IsCharacterMonster() ? "you" :
					room.EvalLightLevel("an unseen defender",
					dobjMonster.InitGroupCount > 1 ? dobjMonster.GetArticleName(groupCountOne: true) : dobjMonster.GetTheName(groupCountOne: true));

			gOut.Write("{0}{1} {2} {3}{4}.",
				Environment.NewLine,
				actorMonsterName,
				attackDesc,
				dobjMonsterName,
					weapon != null &&
					(weaponRevealType == WeaponRevealType.Always ||
					(weaponRevealType == WeaponRevealType.OnlyIfSeen && weapon.Seen)) ?
						" with " + weapon.GetArticleName() :
						"");
		}

		public virtual void PrintMiss(IMonster monster, IArtifact weapon)
		{
			Debug.Assert(monster != null);

			var missDesc = monster.GetMissDescString(weapon);

			gOut.Write("{0} --- {1}!", Environment.NewLine, missDesc);
		}

		public virtual void PrintFumble()
		{
			gOut.Write("{0} ... A fumble!", Environment.NewLine);
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
				room.EvalLightLevel("The offender", monster.GetTheName(true, true, false, true)),
				monster.IsCharacterMonster() ? "drop" : "drops",
				monster.IsCharacterMonster() || room.IsLit() ?
					(
						(weaponRevealType == WeaponRevealType.Never ||
						(weaponRevealType == WeaponRevealType.OnlyIfSeen && !weapon.Seen)) ?
							weapon.GetArticleName(buf: Globals.Buf01) :
							weapon.GetTheName(buf: Globals.Buf01)
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

			gOut.Write("{0} {1} ", Environment.NewLine, monster.IsCharacterMonster() ? "***" : "+++");
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
				var armorDesc = monster.GetArmorDescString();

				gOut.Write("{0}{1}Blow bounces off {2}!", Environment.NewLine, omitBboaPadding ? "" : "  ", armorDesc);
			}
			else
			{
				gOut.Write("{0}{1}Blow turned!", Environment.NewLine, omitBboaPadding ? "" : "  ");
			}
		}

		public virtual void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell)
		{
			Debug.Assert(room != null && dobjMonster != null);

			var dobjMonsterName = dobjMonster.IsCharacterMonster() ? "You" :
				blastSpell && dobjMonster.InitGroupCount > 1 ? room.EvalLightLevel(dobjMonster == actorMonster ? "An offender" : "A defender", dobjMonster.GetArticleName(true, true, false, true)) :
				room.EvalLightLevel(dobjMonster == actorMonster ? "The offender" : "The defender", dobjMonster.GetTheName(true, true, false, true, Globals.Buf01));

			Globals.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				dobjMonsterName,
				dobjMonster.IsCharacterMonster() ? "are" : "is");

			dobjMonster.AddHealthStatus(Globals.Buf, false);

			gOut.Write("{0}", Globals.Buf);
		}

		public virtual void PrintZapDirectHit()
		{
			gEngine.PrintZapDirectHit();
		}

		public virtual void PrintHackToBits(IArtifact artifact, IMonster monster, bool blastSpell)
		{
			gEngine.PrintHackToBits(artifact, monster, blastSpell);
		}

		public virtual void PrintAlreadyBrokeIt(IArtifact artifact)
		{
			gEngine.PrintAlreadyBrokeIt(artifact);
		}

		public virtual void PrintNothingHappens()
		{
			gEngine.PrintNothingHappens();
		}

		public virtual void PrintWhamHitObj(IArtifact artifact)
		{
			gEngine.PrintWhamHitObj(artifact);
		}

		public virtual void PrintSmashesToPieces(IRoom room, IArtifact artifact, bool contentsSpilled)
		{
			gEngine.PrintSmashesToPieces(room, artifact, contentsSpilled);
		}

		public ComponentImpl()
		{
			// Here we make an exception to the "always use Component" rule

		}
	}
}
