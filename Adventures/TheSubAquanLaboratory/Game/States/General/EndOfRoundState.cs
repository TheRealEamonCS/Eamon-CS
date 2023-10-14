
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEndRound)
			{
				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				// Fake-looking back wall / Glass walls apply wall damage

				if (gEngine.AttackingWall && gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					gEngine.ApplyWallDamage(room);
				}

				// Energy mace fizzles

				if (gGameState.EnergyMaceCharge > 0)
				{
					var energyMaceArtifact = gADB[80];

					Debug.Assert(energyMaceArtifact != null);

					var monster = energyMaceArtifact.GetCarriedByMonster();

					if (energyMaceArtifact.IsInRoom(room) || energyMaceArtifact.IsCarriedByMonster(gCharMonster) || (monster != null && monster.IsInRoom(room)))
					{
						if (--gGameState.EnergyMaceCharge == 0)
						{
							var ac = energyMaceArtifact.GetCategory(0);

							Debug.Assert(ac != null);

							ac.Field1 = 0;

							ac.Field3 = 1;

							ac.Field4 = 6;

							energyMaceArtifact.Value = 15;

							gEngine.PrintEffectDesc(31);
						}
					}
				}

				// Laser scalpel fizzles

				if (gGameState.LaserScalpelCharge > 0)
				{
					var scalpelArtifact = gADB[76];

					Debug.Assert(scalpelArtifact != null);

					var monster = scalpelArtifact.GetCarriedByMonster();

					if (scalpelArtifact.IsInRoom(room) || scalpelArtifact.IsCarriedByMonster(gCharMonster) || (monster != null && monster.IsInRoom(room)))
					{
						if (--gGameState.LaserScalpelCharge == 0)
						{
							scalpelArtifact.Value = 15;

							gEngine.ConvertWeaponToGoldOrTreasure(scalpelArtifact, false);

							gEngine.PrintEffectDesc(32);
						}
					}
				}
			}
		}
	}
}

