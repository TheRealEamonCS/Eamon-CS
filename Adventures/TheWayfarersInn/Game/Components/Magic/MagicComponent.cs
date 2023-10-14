
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override void CheckAfterCastBlast()
		{
			// BLAST unseen apparition

			if (DobjMonster != null && DobjMonster.Uid == 2)
			{
				gEngine.UnseenApparitionAttacks = 1;

				gEngine.PrintEffectDesc(23);

				gEngine.UnseenApparitionAttacks = 0;

				gGameState.Die = 1;

				SetNextStateFunc(gEngine.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				}));

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			base.CheckAfterCastBlast();

		Cleanup:

			;
		}

		public override void CheckAfterCastPower()
		{
			var hearthwatcherMonster = gMDB[23];

			Debug.Assert(hearthwatcherMonster != null);

			var giantWoodenStatueArtifact = gADB[28];

			Debug.Assert(giantWoodenStatueArtifact != null);

			// Summon Hearthwatcher (step 2)

			if ((giantWoodenStatueArtifact.IsInRoom(ActorRoom) || giantWoodenStatueArtifact.IsEmbeddedInRoom(ActorRoom)) && gGameState.HearthwatcherPassphraseSpoken)
			{
				gEngine.PrintMonsterAlive(giantWoodenStatueArtifact);

				gEngine.PrintEffectDesc(65);

				hearthwatcherMonster.SetInRoom(ActorRoom);

				giantWoodenStatueArtifact.SetInLimbo();

				gGameState.HearthwatcherPassphraseSpoken = false;

				SetNextStateFunc(gEngine.CreateInstance<IStartState>());

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			base.CheckAfterCastPower();

		Cleanup:

			;
		}
	}
}
