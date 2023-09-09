
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasHumanNaturalAttackDescs()
		{
			var monsterUids = new long[] { 1, 2, 6, 7, 9, 10, 11, 12, 13, 14, 15 };

			// Use appropriate natural attack descriptions for humans

			return monsterUids.Contains(Uid) ? true : base.HasHumanNaturalAttackDescs();
		}

		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			return fleeing ? roomUid >= 0 : base.CanMoveToRoomUid(roomUid, fleeing);
		}

		public override bool CanMoveInDirection(Direction dir, bool fleeing)
		{
			return fleeing ? dir >= Direction.North && dir <= Direction.Down : base.CanMoveInDirection(dir, fleeing);
		}

		public override void AddHealthStatus(StringBuilder buf, bool appendNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (IsDead())
			{
				result = "dead!";
			}
			else
			{
				var x = DmgTaken;

				x = (((long)((double)(x * 5) / (double)Hardiness)) + 1) * (x > 0 ? 1 : 0);

				result = "nearly dead.";

				if (x == 4)
				{
					result = "reeling about.";
				}
				else if (x == 3)
				{
					result = "severely wounded.";
				}
				else if (x == 2)
				{
					result = "weakening.";
				}
				else if (x == 1)
				{
					result = "scratched.";
				}
				else if (x < 1)
				{
					result = "in perfect shape.";
				}
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, appendNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}

		public override string[] GetWeaponAttackDescs(IArtifact artifact)
		{
			// Whip

			return artifact != null && artifact.Uid == 5 ? new string[] { "whip{0} at" } : base.GetWeaponAttackDescs(artifact);
		}
	}
}
