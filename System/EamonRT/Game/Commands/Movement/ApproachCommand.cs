
// ApproachCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ApproachCommand : Command, IApproachCommand
	{
		public virtual long Range { get; set; }

		public virtual bool PartialTurnMove { get; set; }

		/// <summary></summary>
		public virtual long TargetCoord { get; set; }

		/// <summary></summary>
		public virtual long TargetRange { get; set; }

		/// <summary></summary>
		public virtual long TravelRange { get; set; }

		/// <summary></summary>
		public virtual long MaxRange { get; set; }

		/// <summary></summary>
		public virtual bool PrintClamping { get; set; }

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			ProcessEvents(EventType.BeforeApproach);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			TargetCoord = DobjArtifact != null ? DobjArtifact.Coord : DobjMonster.Coord;

			TargetRange = gEngine.GetRange(ActorMonster.Coord, TargetCoord);

			// Target is already here — nothing to approach.
			if (TargetRange <= 0)
			{
				PrintObjIsHere(Dobj);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			// Positive Range means caller specified a distance — enable clamping messages.
			// Negative Range means use GetTravelRange() — no clamping message.
			PrintClamping = Range >= 1;

			if (Range < 0)
			{
				Range = ActorMonster.GetTravelRange();
			}

			MoveTowardTarget();

			// PartialTurnMove: actor reached the target and moved <= 20% of full travel
			// range, so the approach does not consume the full combat turn.
			if (TargetRange <= 0 && Range <= Math.Round(TravelRange * 0.2))
			{
				PartialTurnMove = true;

				NextState = gEngine.CreateInstance<IStartState>();
			}

			gEngine.PrintObjApproaches(ActorRoom, ActorMonster, Dobj, TargetRange);

			if (ActorMonster.CheckNBTLHostility())
			{
				gEngine.PauseCombat();
			}

			ProcessEvents(EventType.AfterApproach);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public override void ExecuteForMonster()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			Debug.Assert(gCharMonster != null);

			TargetCoord = DobjArtifact != null ? DobjArtifact.Coord : DobjMonster.Coord;

			TargetRange = gEngine.GetRange(ActorMonster.Coord, TargetCoord);

			// Target is already here — nothing to approach, skip silently.
			if (TargetRange <= 0)
			{
				goto Cleanup;
			}

			// Monster approaches never print clamping messages.
			PrintClamping = false;

			MoveTowardTarget();

			// Same PartialTurnMove threshold as player path.
			if (TargetRange <= 0 && Range <= Math.Round(TravelRange * 0.2))
			{
				PartialTurnMove = true;
			}

			// Clamp final coord to room bounds after movement.
			ActorMonster.Coord = ActorMonster.Coord.Clamp(0, ActorRoom.MaxCoord);

			// Only print approach message if the player is in the same room.
			if (gCharMonster.IsInRoom(ActorRoom))
			{
				gEngine.PrintObjApproaches(ActorRoom, ActorMonster, Dobj, TargetRange);

				if (ActorMonster.CheckNBTLHostility())
				{
					gEngine.PauseCombat();
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public virtual void MoveTowardTarget()
		{
			// Store full unmodified travel range so the PartialTurnMove 20% threshold
			// check remains accurate after Range is clamped to MaxRange below.
			TravelRange = ActorMonster.GetTravelRange();

			MaxRange = TravelRange;

			// Cap movement to the remaining distance to target — actor cannot overshoot.
			if (MaxRange > TargetRange)
			{
				MaxRange = TargetRange;
			}

			if (Range < 1 || Range > MaxRange)
			{
				if (PrintClamping)
				{
					PrintClampingValue(1, MaxRange);
				}

				Range = Range.Clamp(1, MaxRange);
			}

			// Move toward TargetCoord. Direction is always unambiguous here —
			// ApproachCommand is never called with equal coords (TargetRange <= 0
			// exits early above), so the two branches are always distinct.
			if (ActorMonster.Coord < TargetCoord)
			{
				// Actor is left of target — move right.
				ActorMonster.Coord += Range;
			}
			else
			{
				// Actor is right of target — move left.
				ActorMonster.Coord -= Range;
			}

			TargetRange = gEngine.GetRange(ActorMonster.Coord, TargetCoord);
		}

		public ApproachCommand()
		{
			SortOrder = 103;

			IsPlayerEnabled = false;

			IsMonsterEnabled = false;

			Name = "ApproachCommand";

			Verb = "approach";

			Type = CommandType.Movement;

			// Default Range=-1 signals the command to call GetTravelRange() itself.
			Range = -1;
		}
	}
}
