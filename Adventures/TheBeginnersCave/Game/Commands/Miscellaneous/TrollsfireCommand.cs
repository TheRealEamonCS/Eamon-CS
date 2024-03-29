﻿
// TrollsfireCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Commands
{
	/// <seealso cref="Framework.Commands.ITrollsfireCommand" />
	[ClassMappings]
	public class TrollsfireCommand : EamonRT.Game.Commands.Command, Framework.Commands.ITrollsfireCommand
	{
		public override void ExecuteForPlayer()
		{
			// Find Trollsfire in the game database

			var trollsfireArtifact = gADB[10];

			Debug.Assert(trollsfireArtifact != null);

			// If Trollsfire is in the player character's room

			if (trollsfireArtifact.IsInRoom(ActorRoom))
			{
				gOut.Print("Maybe you should pick it up first.");

				goto Cleanup;
			}

			// If Trollsfire is not being carried by the player character

			if (!trollsfireArtifact.IsCarriedByMonster(ActorMonster))
			{
				gOut.Print("Nothing happens.");

				goto Cleanup;
			}

			// If Trollsfire is currently alight then extinguish it

			if (gGameState.Trollsfire == 1)
			{
				gEngine.PrintEffectDesc(6);

				gGameState.Trollsfire = 0; 

				goto Cleanup;
			}

			// If Trollsfire is extinguished then light it

			gEngine.PrintEffectDesc(4);

			gGameState.Trollsfire = 1;

			// If Trollsfire is not wielded it burns the player character

			if (ActorMonster.Weapon != 10)
			{
				gEngine.PrintEffectDesc(5);

				// Create a combat component object to inflict the injury; these can be created and discarded at will

				var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
				{
					// The lambda is used if the combat component needs to set this Command's NextState property (e.g., player character dies)

					x.SetNextStateFunc = s => NextState = s;

					// In ExecuteForPlayer, ActorRoom is the player character room and ActorMonster is the player character monster

					x.ActorRoom = ActorRoom;

					x.Dobj = ActorMonster;

					// We want to bypass armor

					x.OmitArmor = true;
				});

				// Calculate and apply the damage to player character

				combatComponent.ExecuteCalculateDamage(1, 5);
		
				// Extinguish Trollsfire

				gGameState.Trollsfire = 0;
			}

		Cleanup:

			// If we're not going anywhere else, go into the monster processing loop

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public TrollsfireCommand()
		{
			SortOrder = 440;

			IsNew = true;

			Name = "TrollsfireCommand";

			Verb = "trollsfire";

			Type = CommandType.Miscellaneous;
		}
	}
}
