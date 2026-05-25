
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : State, IAfterPlayerMoveState
	{
		public virtual Direction Direction { get; set; }

		public virtual IArtifact DoorGateArtifact { get; set; }

		public virtual bool MoveMonsters { get; set; }

		/// <summary></summary>
		public virtual IRoom OldRoom { get; set; }

		/// <summary></summary>
		public virtual IRoom NewRoom { get; set; }

		/// <summary></summary>
		public virtual IArtifact LsArtifact { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(Direction == 0 || Enum.IsDefined(typeof(Direction), Direction));

			gEngine.PlayerMoved = true;

			gCommandParser.LastHimNameStr = "";

			gCommandParser.LastHerNameStr = "";

			gCommandParser.LastItNameStr = "";

			gCommandParser.LastThemNameStr = "";

			gGameState.R3 = gGameState.Ro;

			gGameState.Ro = gGameState.R2;

			OldRoom = gRDB[gGameState.R3];

			NewRoom = gRDB[gGameState.Ro];

			Debug.Assert(NewRoom != null);

			if (MoveMonsters)
			{
				gEngine.MoveMonsters(OldRoom, NewRoom, Direction);
			}

			ProcessEvents(EventType.AfterMoveMonsters);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			Debug.Assert(gCharMonster != null);

			gCharMonster.Location = gGameState.Ro;

			if (gEngine.EnableEnhancedCombat && !(gEngine.LastCommand is IRestoreCommand))
			{
				gCharMonster.Coord = OldRoom != null && Direction != 0 ? OldRoom.GetDirCoord(Direction) : 0;

				gCharMonster.Coord = gCharMonster.Coord.Clamp(0, NewRoom.MaxCoord);

				gCharMonster.FocusMonsterUid = 0;
			}

			if (gGameState.Ls > 0 && gGameState.Ro != gGameState.R3)
			{
				LsArtifact = gADB[gGameState.Ls];

				Debug.Assert(LsArtifact != null);

				if (!LsArtifact.IsCarriedByMonster(gCharMonster))
				{
					rc = LsArtifact.RemoveStateDesc(LsArtifact.GetProvidingLightDesc());

					Debug.Assert(gEngine.IsSuccess(rc));

					gGameState.Ls = 0;
				}
			}

			if (NewRoom.LightLvl > 0 && gGameState.Ls > 0)
			{
				gEngine.CheckToExtinguishLightSource();
			}

			ProcessEvents(EventType.AfterExtinguishLightSourceCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
			}

			gEngine.NextState = NextState;
		}

		public AfterPlayerMoveState()
		{
			Name = "AfterPlayerMoveState";

			MoveMonsters = true;
		}
	}
}
