
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforePlayerRoomPrint && ShouldPreTurnProcess())
			{
				Debug.Assert(gCharMonster != null);

				var characterRoom = gCharMonster.GetInRoom();

				Debug.Assert(characterRoom != null);

				// Events only occur in lit rooms

				if (characterRoom.IsLit())
				{
					// Kobolds appear (30% chance)

					if (!gGameState.KoboldsAppear && characterRoom.Uid > 10 && characterRoom.Uid < 16)
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl < 31)
						{
							for (var i = 6; i <= 9; i++)
							{
								var koboldMonster = gMDB[i];

								Debug.Assert(koboldMonster != null);

								koboldMonster.SetInRoom(characterRoom);
							}

							gGameState.KoboldsAppear = true;
						}
					}

					var zapfMonster = gMDB[15];

					Debug.Assert(zapfMonster != null);

					var staffArtifact = gADB[33];

					Debug.Assert(staffArtifact != null);

					// Zapf the Conjurer brings in strangers (15% Chance)

					if (zapfMonster.IsInRoom(characterRoom) && zapfMonster.Seen && !zapfMonster.CheckNBTLHostility() && staffArtifact.IsCarriedByMonster(zapfMonster))
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl < 16)
						{
							gEngine.PrintEffectDesc(16);

							// Exclude character monster

							rl = gEngine.RollDice(1, Globals.Database.MonsterTable.Records.Count - 1, 0);

							var summonedMonster = gMDB[rl];

							Debug.Assert(summonedMonster != null);

							if (!summonedMonster.IsInRoom(characterRoom) && summonedMonster.Seen)
							{
								gOut.Print("<<POOF!!>>  {0} appears!", summonedMonster.GetTheName(true, true, false, true));

								// Only reset for dead monsters

								if (summonedMonster.IsInLimbo())
								{
									summonedMonster.DmgTaken = 0;
								}

								summonedMonster.SetInRoom(characterRoom);
							}
						}
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
