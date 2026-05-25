
// RetreatCommand.cs

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
	public class RetreatCommand : Command, IRetreatCommand
	{
		public virtual long Range { get; set; }

		/// <summary>
		/// Gets or sets the retreat direction override.
		/// -1 = Disabled (default) — direction determined normally from coord comparison.
		///  0 = Retreat toward coord 0.
		///  1 = Retreat toward ActorRoom.MaxCoord.
		/// Used by wrap-around retreats in MonsterActionState where ActorMonster.Coord
		/// has been set equal to TargetCoord, making normal coord comparison ambiguous.
		/// </summary>
		public virtual long Direction { get; set; }

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

			ProcessEvents(EventType.BeforeRetreat);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			TargetCoord = DobjArtifact != null ? DobjArtifact.Coord : DobjMonster.Coord;

			// Positive Range means caller specified a distance — enable clamping messages.
			// Negative Range means use GetTravelRange() — no clamping message.
			PrintClamping = Range >= 1;

			if (Range < 0)
			{
				Range = ActorMonster.GetTravelRange();
			}

			// Wall is directly behind the actor — nowhere to retreat.
			if (GetDistanceToWall() <= 0)
			{
				PrintCantRetreat(ActorRoom);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			MoveAwayFromTarget();

			// PartialTurnMove: actor reached the target and moved <= 20% of full travel
			// range, so the retreat does not consume the full combat turn.
			if (TargetRange <= 0 && Range <= Math.Round(TravelRange * 0.2))
			{
				PartialTurnMove = true;

				NextState = gEngine.CreateInstance<IStartState>();
			}

			gEngine.PrintObjRetreats(ActorRoom, ActorMonster, Dobj, TargetRange);

			if (ActorMonster.CheckNBTLHostility())
			{
				gEngine.PauseCombat();
			}

			ProcessEvents(EventType.AfterRetreat);

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

			// Nothing to retreat into — skip silently.
			if (GetDistanceToWall() <= 0)
			{
				goto Cleanup;
			}

			// Monster retreats never print clamping messages.
			PrintClamping = false;

			MoveAwayFromTarget();

			// Same PartialTurnMove threshold as player path.
			if (TargetRange <= 0 && Range <= Math.Round(TravelRange * 0.2))
			{
				PartialTurnMove = true;
			}

			// Clamp final coord to room bounds after movement.
			ActorMonster.Coord = ActorMonster.Coord.Clamp(0, ActorRoom.MaxCoord);

			// Only print retreat message if the player is in the same room.
			if (gCharMonster.IsInRoom(ActorRoom))
			{
				gEngine.PrintObjRetreats(ActorRoom, ActorMonster, Dobj, TargetRange);

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

		public virtual void MoveAwayFromTarget()
		{
			// Store full unmodified travel range so the PartialTurnMove 20% threshold
			// check remains accurate after Range is clamped to MaxRange below.
			TravelRange = ActorMonster.GetTravelRange();

			MaxRange = TravelRange;

			var distanceToWall = GetDistanceToWall();

			// Cap movement to available wall distance — actor cannot retreat through the wall.
			if (MaxRange > distanceToWall)
			{
				MaxRange = distanceToWall;
			}

			if (Range < 1 || Range > MaxRange)
			{
				if (PrintClamping)
				{
					PrintClampingValue(1, MaxRange);
				}

				Range = Range.Clamp(1, MaxRange);
			}

			// Direction=-1 (default): derive direction from coord comparison. Actor retreats
			//   away from TargetCoord. Equal coords retreat toward the farther wall.
			// Direction=0: explicit retreat toward coord 0. Used by wrap-around retreats in
			//   MonsterActionState after ActorMonster.Coord is set equal to TargetCoord,
			//   making coord comparison unreliable.
			// Direction=1: explicit retreat toward MaxCoord, same scenario as Direction=0.
			if (Direction == 0)
			{
				ActorMonster.Coord -= Range;
			}
			else if (Direction == 1)
			{
				ActorMonster.Coord += Range;
			}
			else if (ActorMonster.Coord < TargetCoord)
			{
				// Actor is left of target — retreat left toward coord 0.
				ActorMonster.Coord -= Range;
			}
			else if (ActorMonster.Coord > TargetCoord)
			{
				// Actor is right of target — retreat right toward MaxCoord.
				ActorMonster.Coord += Range;
			}
			else if (ActorMonster.Coord < ActorRoom.MaxCoord - ActorMonster.Coord)
			{
				// Equal coords, actor closer to coord 0 — retreat right to maximize distance.
				ActorMonster.Coord += Range;
			}
			else
			{
				// Equal coords, actor at or past midpoint — retreat left to maximize distance.
				ActorMonster.Coord -= Range;
			}

			TargetRange = gEngine.GetRange(ActorMonster.Coord, TargetCoord);
		}

		public virtual long GetDistanceToWall()
		{
			// Returns space available in the retreat direction. Must be consistent with
			// the direction chosen in MoveAwayFromTarget or clamping will be incorrect.
			// Direction=0: retreating toward coord 0, space = ActorMonster.Coord.
			// Direction=1: retreating toward MaxCoord, space = MaxCoord - ActorMonster.Coord.
			// Direction=-1: derived from coord comparison, same logic as MoveAwayFromTarget.
			//   Equal coords: return the larger wall distance, consistent with MoveAwayFromTarget
			//   retreating toward the farther wall in that case.
			if (Direction == 0)
			{
				return ActorMonster.Coord;
			}
			else if (Direction == 1)
			{
				return ActorRoom.MaxCoord - ActorMonster.Coord;
			}
			else if (ActorMonster.Coord < TargetCoord)
			{
				return ActorMonster.Coord;
			}
			else if (ActorMonster.Coord > TargetCoord)
			{
				return ActorRoom.MaxCoord - ActorMonster.Coord;
			}
			else
			{
				return ActorMonster.Coord < ActorRoom.MaxCoord - ActorMonster.Coord
					? ActorRoom.MaxCoord - ActorMonster.Coord
					: ActorMonster.Coord;
			}
		}

		public RetreatCommand()
		{
			SortOrder = 107;

			IsPlayerEnabled = false;

			IsMonsterEnabled = false;

			Name = "RetreatCommand";

			Verb = "retreat";

			Type = CommandType.Movement;

			Range = -1;

			// Direction=-1 means derive direction from coord comparison — preserves
			// original behavior for all existing call sites that do not set this property.
			Direction = -1;
		}
	}
}
