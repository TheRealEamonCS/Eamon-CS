
// PutCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class PutCommand : EamonRT.Game.Commands.PutCommand, IPutCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPutArtifact)
			{
				// Put anything in bottomless pit destroys it

				if (IobjArtifact.Uid == 53)
				{
					if (ActorRoom.IsLit())
					{
						gOut.Print("{0} plummet{1} into the depths of the mountain!", DobjArtifact.GetTheName(true), DobjArtifact.EvalPlural("s", ""));
					}

					DobjArtifact.SetInLimbo();
				}

				// Put anything in lavafall / lava river destroys it

				else if (IobjArtifact.Uid == 54 || IobjArtifact.Uid == 55)
				{
					gOut.Print("{0} {1} into {2}!", DobjArtifact.GetTheName(true), DobjArtifact.EvalPlural("incinerates as it sinks", "incinerate as they sink"), IobjArtifact.GetTheName());

					gOut.Print("{0} {1} destroyed!", DobjArtifact.GetTheName(true), DobjArtifact.EvalPlural("is", "are"));

					DobjArtifact.SetInLimbo();
				}

				// Put anything in (running) rock crusher destroys it

				else if (IobjArtifact.Uid == 48 && gGameState.RockCrusherRunning)
				{
					gEngine.RockCrusherDestroysContents(ActorRoom);
				}

				// Put anything in (running) rock grinder destroys it

				else if (IobjArtifact.Uid == 49 && gGameState.RockGrinderRunning)
				{
					gEngine.RockGrinderDestroysContents(ActorRoom);
				}

				// Put anything in (running) debris sifter vibrates it

				else if (IobjArtifact.Uid == 50 && gGameState.DebrisSifterRunning)
				{
					gEngine.DebrisSifterVibratesContents(ActorRoom, DobjArtifact);
				}

				// Put anything in latrine moves it to sewage pit

				else if (IobjArtifact.Uid == 42)
				{
					gOut.Print("{0} fall{1} down into the darkness!", DobjArtifact.GetTheName(true), DobjArtifact.EvalPlural("s", ""));

					DobjArtifact.SetInRoomUid(141);
				}
			}
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			if (IobjArtifact.Uid == 42 || IobjArtifact.Uid == 53 || IobjArtifact.Uid == 54 || IobjArtifact.Uid == 55)
			{
				gEngine.ConvertTreasureToContainer(IobjArtifact);
			}

			// Put ore cart on tracks

			if (DobjArtifact.Uid == 46 && IobjArtifact.Uid == 44 && DobjArtifact.IsInRoom(ActorRoom))
			{
				var oreCartTracksArtifact = gADB[45];

				Debug.Assert(oreCartTracksArtifact != null);

				gEngine.PrintEffectDesc(62);

				DobjArtifact.SetCarriedByContainer(oreCartTracksArtifact, ContainerType.On);

				DobjArtifact.StateDesc = "";

				oreCartTracksArtifact.SetInRoom(ActorRoom);

				gGameState.OreCartTracksRoomUid = ActorRoom.Uid;

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Put rope on mesquite tree

			else if (DobjArtifact.Uid == 69 && IobjArtifact.Uid == 70)
			{
				gEngine.PrintEffectDesc(42);

				DobjArtifact.SetCarriedByContainer(IobjArtifact, ContainerType.On);

				DobjArtifact.Type = ArtifactType.DoorGate;

				DobjArtifact.Field1 = 37;

				DobjArtifact.Field2 = -1;

				DobjArtifact.Field3 = 0;

				DobjArtifact.Field4 = 0;

				DobjArtifact.Field5 = 0;

				ActorRoom.SetDirectionDoor(Direction.Down, DobjArtifact);

				var room = gRDB[37];

				Debug.Assert(room != null);

				room.SetDirectionDoor(Direction.Up, DobjArtifact);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}

			if (IobjArtifact.Uid == 42 || IobjArtifact.Uid == 53 || IobjArtifact.Uid == 54 || IobjArtifact.Uid == 55)
			{
				gEngine.ConvertContainerToTreasure(IobjArtifact);
			}
		}
	}
}
