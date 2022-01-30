﻿
// IComponentSignatures.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;

namespace EamonRT.Framework.Components
{
	/// <summary></summary>
	public interface IComponentSignatures
	{
		/// <summary>
		/// Gets or sets the function used to set the <see cref="IStateSignatures.NextState">NextState</see> property of the Eamon CS
		/// game engine's current <see cref="IState">State</see> or <see cref="ICommand">Command</see>.
		/// </summary>
		Action<IState> SetNextStateFunc { get; set; }

		/// <summary>
		/// Gets or sets the function used to copy the data of the Eamon CS game engine's current <see cref="ICommand">Command</see>.
		/// </summary>
		Action<ICommand> CopyCommandDataFunc { get; set; }

		/// <summary></summary>
		IMonster ActorMonster { get; set; }

		/// <summary></summary>
		IRoom ActorRoom { get; set; }

		/// <summary></summary>
		IGameBase Dobj { get; set; }

		/// <summary></summary>
		IArtifact DobjArtifact { get; }

		/// <summary></summary>
		IMonster DobjMonster { get; }

		/// <summary></summary>
		IGameBase Iobj { get; set; }

		/// <summary></summary>
		IArtifact IobjArtifact { get; }

		/// <summary></summary>
		IMonster IobjMonster { get; }

		/// <summary></summary>
		IArtifactCategory DobjArtAc { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="IComponent">Component</see> should print a final newline after
		/// processing completes.
		/// </summary>
		bool OmitFinalNewLine { get; set; }

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="actorMonster"></param>
		/// <param name="dobjMonster"></param>
		/// <param name="weapon"></param>
		/// <param name="attackNumber"></param>
		/// <param name="weaponRevealType"></param>
		void PrintAttack(IRoom room, IMonster actorMonster, IMonster dobjMonster, IArtifact weapon, long attackNumber, WeaponRevealType weaponRevealType);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="weapon"></param>
		void PrintMiss(IMonster monster, IArtifact weapon);

		/// <summary></summary>
		void PrintFumble();

		/// <summary></summary>
		void PrintRecovered();

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="weapon"></param>
		/// <param name="weaponRevealType"></param>
		void PrintWeaponDropped(IRoom room, IMonster monster, IArtifact weapon, WeaponRevealType weaponRevealType);

		/// <summary></summary>
		void PrintWeaponHitsUser();

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="weapon"></param>
		/// <param name="weaponRevealType"></param>
		void PrintSparksFly(IRoom room, IMonster monster, IArtifact weapon, WeaponRevealType weaponRevealType);

		/// <summary></summary>
		void PrintWeaponDamaged();

		/// <summary></summary>
		void PrintWeaponBroken();

		/// <summary></summary>
		void PrintBrokenWeaponHitsUser();

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintStarPlus(IMonster monster);

		/// <summary></summary>
		void PrintHit();

		/// <summary></summary>
		void PrintCriticalHit();

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="omitBboaPadding"></param>
		void PrintBlowTurned(IMonster monster, bool omitBboaPadding);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="actorMonster"></param>
		/// <param name="dobjMonster"></param>
		/// <param name="blastSpell"></param>
		void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell);

		/// <summary></summary>
		void PrintZapDirectHit();

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="monster"></param>
		/// <param name="blastSpell"></param>
		void PrintHackToBits(IArtifact artifact, IMonster monster, bool blastSpell);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintAlreadyBrokeIt(IArtifact artifact);

		/// <summary></summary>
		void PrintNothingHappens();

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWhamHitObj(IArtifact artifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		/// <param name="contentsSpilled"></param>
		void PrintSmashesToPieces(IRoom room, IArtifact artifact, bool contentsSpilled);
	}
}
