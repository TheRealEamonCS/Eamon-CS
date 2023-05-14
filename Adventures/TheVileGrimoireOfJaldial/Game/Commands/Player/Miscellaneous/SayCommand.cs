
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintSayText)
			{
				var waterWeirdMonster = gMDB[38];

				Debug.Assert(waterWeirdMonster != null);

				var efreetiMonster = gMDB[50];

				Debug.Assert(efreetiMonster != null);

				var parchmentArtifact = gADB[33];

				Debug.Assert(parchmentArtifact != null);

				// Summon efreeti

				if ((parchmentArtifact.IsCarriedByMonster(ActorMonster) || parchmentArtifact.IsInRoom(ActorRoom)) && efreetiMonster.IsInLimbo() && ProcessedPhrase.Equals("rinnuk aukasker frudasdus", StringComparison.OrdinalIgnoreCase))
				{
					if (!gGameState.EfreetiKilled && ++gGameState.EfreetiSummons <= 3)
					{
						gEngine.PrintEffectDesc(95);

						efreetiMonster.SetInRoom(ActorRoom);
					}
					else
					{
						gEngine.PrintEffectDesc(96);

						parchmentArtifact.SetInLimbo();
					}
				}

				// Kill water weird

				else if (waterWeirdMonster.IsInRoom(ActorRoom) && ProcessedPhrase.Equals("avarchrom yarei uttoximo", StringComparison.OrdinalIgnoreCase))
				{
					gOut.Print("{0} jolts violently several times and then disintegrates.", waterWeirdMonster.GetTheName(true));

					waterWeirdMonster.DmgTaken = waterWeirdMonster.Hardiness;

					var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.ActorRoom = ActorRoom;

						x.Dobj = waterWeirdMonster;

						// x.OmitFinalNewLine = false;
					});

					combatComponent.ExecuteCheckMonsterStatus();
				}
			}
		}
	}
}
