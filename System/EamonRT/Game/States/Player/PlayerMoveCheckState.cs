﻿
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : State, IPlayerMoveCheckState
	{
		public bool _doorGateFound;

		public long _newRoomUid;

		public virtual Direction Direction { get; set; }

		public virtual IArtifact DoorGateArtifact { get; set; }

		public virtual bool Fleeing { get; set; }

		/// <summary></summary>
		public virtual IRoom OldRoom { get; set; }

		/// <summary></summary>
		public virtual IArtifact BlockingArtifact { get; set; }

		/// <summary></summary>
		public virtual long DoorGateArtifactUid { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(Enum.IsDefined(typeof(Direction), Direction) || DoorGateArtifact != null);

			Debug.Assert(gCharMonster != null);

			ProcessEvents(EventType.BeforeCanMoveToRoomCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			// Special blocking artifacts

			BlockingArtifact = gEngine.GetBlockedDirectionArtifact(gGameState.Ro, gGameState.R2, Direction);

			if (BlockingArtifact != null)
			{
				PrintObjBlocksTheWay(BlockingArtifact);

				goto Cleanup;
			}

			if (gGameState.R2 > 0 && gRDB[gGameState.R2] != null)
			{
				if (gCharMonster.CanMoveToRoomUid(gGameState.R2, Fleeing))
				{
					NextState = gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
					{
						x.Direction = Direction;

						x.DoorGateArtifact = DoorGateArtifact;
					});
				}
				else
				{
					PrintCantVerbThere(Fleeing ? "flee" : "move");
				}

				goto Cleanup;
			}

			OldRoom = gRDB[gGameState.Ro];

			Debug.Assert(OldRoom != null);

			DoorGateArtifactUid = DoorGateArtifact != null ? DoorGateArtifact.Uid : OldRoom.GetDirectionDoorUid(Direction);

			if (DoorGateArtifactUid > 0)
			{
				if (DoorGateArtifact == null)
				{
					DoorGateArtifact = gADB[DoorGateArtifactUid];

					Debug.Assert(DoorGateArtifact != null);
				}

				gEngine.CheckDoor(OldRoom, DoorGateArtifact, ref _doorGateFound, ref _newRoomUid);

				if (_doorGateFound)
				{
					if (OldRoom.IsViewable())
					{
						gEngine.RevealEmbeddedArtifact(OldRoom, DoorGateArtifact);
					}

					gGameState.R2 = _newRoomUid;

					if (gGameState.R2 > 0 && gRDB[gGameState.R2] != null)
					{
						NextState = gEngine.CreateInstance<IPlayerMoveCheckState>(x =>
						{
							x.Direction = Direction;

							x.DoorGateArtifact = DoorGateArtifact;

							x.Fleeing = Fleeing;
						});

						goto Cleanup;
					}
					else if (gGameState.R2 == 0 && OldRoom.IsViewable())
					{
						PrintObjBlocksTheWay(DoorGateArtifact);

						goto Cleanup;
					}
					else if (gGameState.R2 == 0 && OldRoom.IsLit())
					{
						PrintSomethingBlocksTheWay();

						goto Cleanup;
					}
				}
			}

			ProcessEvents(EventType.AfterBlockingArtifactCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (gGameState.R2 != gEngine.DirectionExit)
			{
				PrintCantGoThatWay();

				goto Cleanup;
			}

			PrintRideOffIntoSunset();

			PrintLeaveAdventure();

			gEngine.Buf.Clear();

			rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
			{
				gGameState.Die = 0;

				gEngine.ExitType = ExitType.FinishAdventure;

				gEngine.MainLoop.ShouldShutdown = true;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
			}

			gEngine.NextState = NextState;
		}

		public PlayerMoveCheckState()
		{
			Name = "PlayerMoveCheckState";
		}
	}
}
