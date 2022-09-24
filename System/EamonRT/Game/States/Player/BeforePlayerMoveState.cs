
// BeforePlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BeforePlayerMoveState : State, IBeforePlayerMoveState
	{
		public virtual Direction Direction { get; set; }

		public virtual IArtifact DoorGateArtifact { get; set; }

		/// <summary></summary>
		public virtual IRoom OldRoom { get; set; }

		public override void Execute()
		{
			Debug.Assert(Enum.IsDefined(typeof(Direction), Direction) || DoorGateArtifact != null);

			OldRoom = gRDB[gGameState.Ro];

			Debug.Assert(OldRoom != null);

			gGameState.R2 = DoorGateArtifact != null ? 0 : OldRoom.GetDirs(Direction);

			ProcessEvents(EventType.AfterSetDestinationRoom);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (gGameState.GetNBTL(Friendliness.Enemy) > 0 && OldRoom.IsLit())
			{
				PrintEnemiesNearby();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;

					x.DoorGateArtifact = DoorGateArtifact;
				});
			}

			Globals.NextState = NextState;
		}

		public BeforePlayerMoveState()
		{
			Name = "BeforePlayerMoveState";
		}
	}
}
