﻿
// Component.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Components
{
	public abstract class Component : IComponent
	{
		private IComponentImpl ComponentImpl { get; set; }

		public virtual Action<IState> SetNextStateFunc
		{
			get
			{
				return ComponentImpl.SetNextStateFunc;
			}

			set
			{
				ComponentImpl.SetNextStateFunc = value;
			}
		}

		public virtual Action<ICommand> CopyCommandDataFunc
		{
			get
			{
				return ComponentImpl.CopyCommandDataFunc;
			}

			set
			{
				ComponentImpl.CopyCommandDataFunc = value;
			}
		}

		public virtual IMonster ActorMonster
		{
			get
			{
				return ComponentImpl.ActorMonster;
			}

			set
			{
				ComponentImpl.ActorMonster = value;
			}
		}

		public virtual IRoom ActorRoom
		{
			get
			{
				return ComponentImpl.ActorRoom;
			}

			set
			{
				ComponentImpl.ActorRoom = value;
			}
		}

		public virtual IGameBase Dobj
		{
			get
			{
				return ComponentImpl.Dobj;
			}

			set
			{
				ComponentImpl.Dobj = value;
			}
		}

		public virtual IArtifact DobjArtifact
		{
			get
			{
				return ComponentImpl.DobjArtifact;
			}
		}

		public virtual IMonster DobjMonster
		{
			get
			{
				return ComponentImpl.DobjMonster;
			}
		}

		public virtual IGameBase Iobj
		{
			get
			{
				return ComponentImpl.Iobj;
			}

			set
			{
				ComponentImpl.Iobj = value;
			}
		}

		public virtual IArtifact IobjArtifact
		{
			get
			{
				return ComponentImpl.IobjArtifact;
			}
		}

		public virtual IMonster IobjMonster
		{
			get
			{
				return ComponentImpl.IobjMonster;
			}
		}

		public virtual ICommand RedirectCommand
		{
			get
			{
				return ComponentImpl.RedirectCommand;
			}

			set
			{
				ComponentImpl.RedirectCommand = value;
			}
		}

		public virtual IArtifactCategory DobjArtAc
		{
			get
			{
				return ComponentImpl.DobjArtAc;
			}

			set
			{
				ComponentImpl.DobjArtAc = value;
			}
		}

		public virtual bool OmitSkillGains
		{
			get
			{
				return ComponentImpl.OmitSkillGains;
			}

			set
			{
				ComponentImpl.OmitSkillGains = value;
			}
		}

		public virtual bool OmitFinalNewLine
		{
			get
			{
				return ComponentImpl.OmitFinalNewLine;
			}

			set
			{
				ComponentImpl.OmitFinalNewLine = value;
			}
		}

		public virtual void PrintAttack(IRoom room, IMonster actorMonster, IMonster dobjMonster, IArtifact weapon, long attackNumber, WeaponRevealType weaponRevealType)
		{
			ComponentImpl.PrintAttack(room, actorMonster, dobjMonster, weapon, attackNumber, weaponRevealType);
		}

		public virtual void PrintMiss(IMonster monster, IArtifact weapon)
		{
			ComponentImpl.PrintMiss(monster, weapon);
		}

		public virtual void PrintFumble()
		{
			ComponentImpl.PrintFumble();
		}

		public virtual void PrintRecovered()
		{
			ComponentImpl.PrintRecovered();
		}

		public virtual void PrintWeaponDropped(IRoom room, IMonster monster, IArtifact weapon, WeaponRevealType weaponRevealType)
		{
			ComponentImpl.PrintWeaponDropped(room, monster, weapon, weaponRevealType);
		}

		public virtual void PrintWeaponHitsUser()
		{
			ComponentImpl.PrintWeaponHitsUser();
		}

		public virtual void PrintSparksFly(IRoom room, IMonster monster, IArtifact weapon, WeaponRevealType weaponRevealType)
		{
			ComponentImpl.PrintSparksFly(room, monster, weapon, weaponRevealType);
		}

		public virtual void PrintWeaponDamaged()
		{
			ComponentImpl.PrintWeaponDamaged();
		}

		public virtual void PrintWeaponBroken()
		{
			ComponentImpl.PrintWeaponBroken();
		}

		public virtual void PrintBrokenWeaponHitsUser()
		{
			ComponentImpl.PrintBrokenWeaponHitsUser();
		}

		public virtual void PrintStarPlus(IMonster monster)
		{
			ComponentImpl.PrintStarPlus(monster);
		}

		public virtual void PrintHit()
		{
			ComponentImpl.PrintHit();
		}

		public virtual void PrintCriticalHit()
		{
			ComponentImpl.PrintCriticalHit();
		}

		public virtual void PrintBlowTurned(IMonster monster, bool omitBboaPadding)
		{
			ComponentImpl.PrintBlowTurned(monster, omitBboaPadding);
		}

		public virtual void PrintBlowDoesDamage(bool useCurlyBraces, bool omitBboaPadding, long damage, bool appendNewLine)
		{
			ComponentImpl.PrintBlowDoesDamage(useCurlyBraces, omitBboaPadding, damage, appendNewLine);
		}

		public virtual void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell, bool nonCombat)
		{
			ComponentImpl.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell, nonCombat);
		}

		public virtual void PrintZapDirectHit()
		{
			ComponentImpl.PrintZapDirectHit();
		}

		public virtual void PrintHackToBits(IArtifact artifact, IMonster monster, bool blastSpell)
		{
			ComponentImpl.PrintHackToBits(artifact, monster, blastSpell);
		}

		public virtual void PrintAlreadyBrokeIt(IArtifact artifact)
		{
			ComponentImpl.PrintAlreadyBrokeIt(artifact);
		}

		public virtual void PrintNothingHappens()
		{
			ComponentImpl.PrintNothingHappens();
		}

		public virtual void PrintWhamHitObj(IArtifact artifact)
		{
			ComponentImpl.PrintWhamHitObj(artifact);
		}

		public virtual void PrintSmashesToPieces(IRoom room, IArtifact artifact, bool spillContents)
		{
			ComponentImpl.PrintSmashesToPieces(room, artifact, spillContents);
		}

		public virtual void PrintWeaponAbilityIncreases(Weapon w, IWeapon weapon)
		{
			ComponentImpl.PrintWeaponAbilityIncreases(w, weapon);
		}

		public virtual void PrintArmorExpertiseIncreases()
		{
			ComponentImpl.PrintArmorExpertiseIncreases();
		}

		public virtual void PrintSpellOverloadsBrain(Spell s, ISpell spell)
		{
			ComponentImpl.PrintSpellOverloadsBrain(s, spell);
		}

		public virtual void PrintSpellAbilityIncreases(Spell s, ISpell spell)
		{
			ComponentImpl.PrintSpellAbilityIncreases(s, spell);
		}

		public virtual void PrintSpellCastFailed(Spell s, ISpell spell)
		{
			ComponentImpl.PrintSpellCastFailed(s, spell);
		}

		public virtual void PrintHealthImproves(IMonster monster)
		{
			ComponentImpl.PrintHealthImproves(monster);
		}

		public virtual void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters)
		{
			ComponentImpl.PrintHealthStatus(monster, includeUninjuredGroupMonsters);
		}

		public virtual void PrintFeelNewAgility()
		{
			ComponentImpl.PrintFeelNewAgility();
		}

		public virtual void PrintSonicBoom(IRoom room)
		{
			ComponentImpl.PrintSonicBoom(room);
		}

		public virtual void PrintFortuneCookie()
		{
			ComponentImpl.PrintFortuneCookie();
		}

		public virtual void PrintTunnelCollapses(IRoom room)
		{
			ComponentImpl.PrintTunnelCollapses(room);
		}

		public virtual void PrintAllWoundsHealed()
		{
			ComponentImpl.PrintAllWoundsHealed();
		}

		public virtual void PrintTeleportToRoom()
		{
			ComponentImpl.PrintTeleportToRoom();
		}

		public virtual void PrintArmorThickens()
		{
			ComponentImpl.PrintArmorThickens();
		}

		public virtual void PrintMagicSkillsIncrease()
		{
			ComponentImpl.PrintMagicSkillsIncrease();
		}

		public Component()
		{
			ComponentImpl = gEngine.CreateInstance<IComponentImpl>(x =>
			{
				x.Component = this;
			});
		}
	}
}
