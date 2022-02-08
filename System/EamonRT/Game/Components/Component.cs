
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
using static EamonRT.Game.Plugin.PluginContext;

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

		public virtual void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell)
		{
			ComponentImpl.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell);
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

		public virtual void PrintSmashesToPieces(IRoom room, IArtifact artifact, bool contentsSpilled)
		{
			ComponentImpl.PrintSmashesToPieces(room, artifact, contentsSpilled);
		}

		public virtual void PrintWeaponAbilityIncreased(Weapon w, IWeapon weapon)
		{
			ComponentImpl.PrintWeaponAbilityIncreased(w, weapon);
		}

		public virtual void PrintArmorExpertiseIncreased()
		{
			ComponentImpl.PrintArmorExpertiseIncreased();
		}

		public virtual void PrintSpellOverloadsBrain(Spell s, ISpell spell)
		{
			ComponentImpl.PrintSpellOverloadsBrain(s, spell);
		}

		public virtual void PrintSpellAbilityIncreased(Spell s, ISpell spell)
		{
			ComponentImpl.PrintSpellAbilityIncreased(s, spell);
		}

		public virtual void PrintSpellCastFailed(Spell s, ISpell spell)
		{
			ComponentImpl.PrintSpellCastFailed(s, spell);
		}

		public Component()
		{
			ComponentImpl = Globals.CreateInstance<IComponentImpl>(x =>
			{
				x.Component = this;
			});
		}
	}
}
