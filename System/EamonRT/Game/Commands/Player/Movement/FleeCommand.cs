
// FleeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : Command, IFleeCommand
	{
		public long _numExits;

		public Direction _randomDirection;

		public virtual Direction Direction { get; set; }

		/// <summary></summary>
		public virtual long NumExits
		{
			get
			{
				return _numExits;
			}

			set
			{
				_numExits = value;
			}
		}

		/// <summary></summary>
		public virtual Direction RandomDirection
		{
			get
			{
				return _randomDirection;
			}

			set
			{
				_randomDirection = value;
			}
		}

		public override void Execute()
		{
			Debug.Assert(Direction == 0 || Enum.IsDefined(typeof(Direction), Direction));

			if (DobjArtifact != null && DobjArtifact.DoorGate == null)
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!ActorMonster.CheckNBTLHostility())
			{
				PrintCalmDown();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtifact == null)
			{
				NumExits = 0;

				gEngine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref _numExits);

				if (NumExits == 0)
				{
					PrintNoPlaceToGo();

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}
			}

			ProcessEvents(EventType.AfterNumberOfExitsCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjArtifact == null)
			{
				if (Direction == 0)
				{
					RandomDirection = 0;

					gEngine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref _randomDirection);

					Direction = RandomDirection;
				}

				Debug.Assert(Enum.IsDefined(typeof(Direction), Direction));
			}

			PrintAttemptingToFlee(DobjArtifact, Direction);

			gGameState.R2 = DobjArtifact != null ? 0 : ActorRoom.GetDirs(Direction);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;

					x.DoorGateArtifact = DobjArtifact;

					x.Fleeing = true;
				});
			}
		}

		public FleeCommand()
		{
			Synonyms = new string[] { "retreat", "escape" };

			SortOrder = 100;

			IsDobjPrepEnabled = true;

			IsDarkEnabled = true;

			Uid = 70;

			Name = "FleeCommand";

			Verb = "flee";

			Type = CommandType.Movement;
		}
	}
}
