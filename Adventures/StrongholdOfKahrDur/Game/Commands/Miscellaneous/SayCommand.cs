
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.Globals;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintSayText)
			{
				// Restore monster stats to average for testing/debugging

				if (ProcessedPhrase.Equals("*brutis", StringComparison.OrdinalIgnoreCase))
				{
					var artUid = ActorMonster.Weapon;

					ActorMonster.Weapon = -1;

					gEngine.InitMonsterScaledHardinessValues();

					ActorMonster.Weapon = artUid;

					gOut.Print("Monster stats reduced.");

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}

				var cauldronArtifact = gADB[24];

				Debug.Assert(cauldronArtifact != null);

				// If the cauldron is present and the spell components (see effect #50) are in it then begin the spell casting process

				if (ProcessedPhrase.Equals("knock nikto mellon", StringComparison.OrdinalIgnoreCase) && (cauldronArtifact.IsCarriedByMonster(ActorMonster) || cauldronArtifact.IsInRoom(ActorRoom)) && gEngine.SpellReagentsInCauldron(cauldronArtifact))
				{
					gEngine.PrintEffectDesc(51);

					gGameState.UsedCauldron = true;
				}

				var lichMonster = gMDB[15];

				Debug.Assert(lichMonster != null);

				// Player will agree to free the Lich

				if (ProcessedPhrase.Equals("i will free you", StringComparison.OrdinalIgnoreCase) && ActorRoom.Uid == 109 && lichMonster.IsInRoom(ActorRoom) && lichMonster.Reaction > Friendliness.Enemy && gGameState.LichState < 2)
				{
					gEngine.PrintEffectDesc(54);

					gGameState.LichState = 1;
				}

				// Player actually frees the Lich

				if (ProcessedPhrase.Equals("barada lhain", StringComparison.OrdinalIgnoreCase) && ActorRoom.Uid == 109 && lichMonster.IsInRoom(ActorRoom) && gGameState.LichState == 1)
				{
					var helmArtifact = gADB[25];

					Debug.Assert(helmArtifact != null);

					gEngine.PrintEffectDesc(55);

					// Set freed Lich flag and give Wizard's Helm (25) to player (carried but not worn)

					gGameState.LichState = 2;

					helmArtifact.SetInRoom(ActorRoom);
				}
			}

		Cleanup:

			;
		}
	}
}
