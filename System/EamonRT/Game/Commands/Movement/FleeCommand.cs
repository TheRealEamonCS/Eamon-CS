
// FleeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

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

		public override void ExecuteForPlayer()
		{
			Debug.Assert(Direction == 0 || Enum.IsDefined(typeof(Direction), Direction));

			if (DobjArtifact != null && DobjArtifact.DoorGate == null)
			{
				PrintDontFollowYou();

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!ActorMonster.CheckNBTLHostility())
			{
				PrintCalmDown();

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtifact == null)
			{
				NumExits = 0;

				gEngine.CheckNumberOfExits(ActorRoom, ActorMonster, true, ref _numExits);

				if (NumExits == 0)
				{
					if (Enum.IsDefined(typeof(Direction), Direction))
					{
						PrintAttemptingToFlee(DobjArtifact, Direction);

						PrintCantGoThatWay();
					}
					else
					{
						PrintNoPlaceToGo();
					}

					NextState = gEngine.CreateInstance<IStartState>();

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

			gGameState.R2 = DobjArtifact != null ? 0 : ActorRoom.GetDir(Direction);

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IPlayerMoveCheckState>(x =>
				{
					x.Direction = Direction;

					x.DoorGateArtifact = DobjArtifact;

					x.Fleeing = true;
				});
			}
		}

		public override void ExecuteForMonster()
		{
			if (ActorMonster.ShouldFleeRoom())
			{
				gEngine.MoveMonsterToRandomAdjacentRoom(ActorRoom, ActorMonster, true, true);
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public FleeCommand()
		{
			Synonyms = new string[] { "retreat", "escape" };

			SortOrder = 100;

			IsDobjPrepEnabled = true;

			IsDarkEnabled = true;

			IsMonsterEnabled = true;

			Name = "FleeCommand";

			Verb = "flee";

			Type = CommandType.Movement;
		}
	}
}
