﻿
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Enums = Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.Globals;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName, bool showVerboseName)
		{
			RetCode rc;

			rc = base.BuildPrintedFullDesc(buf, showName, showVerboseName);

			// Lich will ask to be freed

			if (gEngine.IsSuccess(rc) && Uid == 15 && !Seen)
			{
				var effect = gEDB[53];

				Debug.Assert(effect != null);

				buf.AppendPrint("{0}", effect.Desc);
			}

			return rc;
		}

		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			if (Uid < 8 || Uid > 10)
			{
				// Necromancer can't move into a dark area

				return Uid != 22 || roomUid != 54 ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
			}
			else
			{
				// Tree ents can't flee or follow

				return false;
			}
		}

		public override bool ShouldReadyWeapon()
		{
			// Necromancer never tries to pick up or ready weapon

			return Uid != 22 ? base.ShouldReadyWeapon() : false;
		}

		public override void AddHealthStatus(StringBuilder buf, bool appendNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			var index = gEngine.GetMonsterHealthStatusIndex(Hardiness, DmgTaken);

			if (index > 5)
			{
				result = "dead!";
			}
			else if (index == 5)
			{
				result = "at death's door, knocking loudly.";
			}
			else if (index == 4)
			{
				result = "gravely injured.";
			}
			else if (index == 3)
			{
				result = "badly hurt.";
			}
			else if (index == 2)
			{
				result = "hurt.";
			}
			else if (index == 1)
			{
				result = "still in good shape.";
			}
			else if (index < 1)
			{
				result = "in perfect health.";
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, appendNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}

		public override string[] GetWeaponMissDescs(IArtifact artifact)
		{
			var missDescs = base.GetWeaponMissDescs(artifact);

			Debug.Assert(missDescs != null && missDescs.Length >= 1);

			switch ((Enums.Weapon)artifact.GeneralWeapon.Field2)
			{
				case Enums.Weapon.Axe:
				case Enums.Weapon.Club:
				case Enums.Weapon.Spear:

					missDescs[0] = "Parried";

					break;
			}

			return missDescs;
		}
	}
}
