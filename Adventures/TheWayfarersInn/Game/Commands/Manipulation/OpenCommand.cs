
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Stuck north doorway (West Hallway)

			if (eventType == EventType.BeforePrintArtifactOpen && DobjArtifact.Uid == 44 && !gGameState.CharlotteArtisansStory)
			{
				gOut.Print("You unlock {0} with {1}, but {2} refuses to open.", DobjArtifact.EvalPlural("it", "them"), KeyArtifact.GetTheName(), DobjArtifact.GetTheName());

				DobjArtifact.DoorGate.SetOpen(false);

				GotoCleanup = true;
			}
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Bottle of kerosene / Rusty oil lantern

			if (DobjArtifact.Uid == 6 || DobjArtifact.Uid == 11)
			{
				PrintDontNeedTo02(DobjArtifact);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Stuck north doorway (West Hallway)

			else if (DobjArtifact.Uid == 44 && DobjArtifact.DoorGate.GetKeyUid() == 0 && !DobjArtifact.DoorGate.IsOpen() && !gGameState.CharlotteArtisansStory)
			{
				gEngine.PrintEffectDesc(2);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			
			// Stuck north doorway (Innkeeper's Quarters)

			else if (DobjArtifact.Uid == 73 && !DobjArtifact.DoorGate.IsOpen() && DobjArtifact.IsInRoomUid(37) && !gGameState.DiaryRead)
			{
				gEngine.PrintEffectDesc(4);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Stuck doorway (Bedroom)

			else if (DobjArtifact.Uid == 73 && !DobjArtifact.DoorGate.IsOpen() && DobjArtifact.IsInRoomUid(38) && !gGameState.CharlotteDeathSeen)
			{
				gEngine.PrintEffectDesc(2);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Window

			else if (DobjArtifact.Uid == 137)
			{
				if (!gGameState.OpenWindowRoomUids.Contains(ActorRoom.Uid))
				{
					PrintOpened(DobjArtifact);

					gGameState.OpenWindowRoomUids.Add(ActorRoom.Uid);
				}
				else
				{
					PrintAlreadyOpen(DobjArtifact);
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();

				// Stuck north doorway (West Hallway)

				if (DobjArtifact.Uid == 44 && DobjArtifact.DoorGate.GetKeyUid() == 0 && DobjArtifact.DoorGate.IsOpen() && !gGameState.CharlotteDeathSeen)
				{
					DobjArtifact.DoorGate.SetOpen(false);
				}
			}
		}
	}
}
