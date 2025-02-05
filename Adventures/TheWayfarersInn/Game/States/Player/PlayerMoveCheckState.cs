
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeCanMoveToRoomCheck)
			{
				var childsApparitionMonster = gMDB[4];

				Debug.Assert(childsApparitionMonster != null);

				var childsSkeletonArtifact = gADB[54];

				Debug.Assert(childsSkeletonArtifact != null);

				// Exit lit room without showing Charlotte her remains

				if (gCharRoom.IsViewable() && ((Enum.IsDefined(typeof(Direction), Direction) && gCharRoom.IsDirectionRoom(Direction)) || DoorGateArtifact != null) && childsApparitionMonster.IsInRoom(gCharRoom) && childsSkeletonArtifact.IsCarriedByMonster(gCharMonster, true) && !gGameState.CharlotteBonesGiven)
				{
					gEngine.PrintEffectDesc(19);

					NextState = gEngine.CreateInstance<IMonsterStartState>();

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				// Fall through wooden bridge

				if (gGameState.R2 == -59)
				{
					gEngine.PrintEffectDesc(1);

					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;
				}

				// Fall into Tiaga Gorge

				else if (gGameState.R2 == -60)
				{
					gEngine.PrintEffectDesc(3);

					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;
				}

				// Enter Wayfarers Inn east wing

				else if (gGameState.R2 == -61)
				{
					gEngine.PrintEffectDesc(109);

					GotoCleanup = true;
				}
			}
		}
	}
}
