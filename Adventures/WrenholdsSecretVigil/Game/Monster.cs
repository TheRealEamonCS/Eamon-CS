
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game
{
	[ClassMappings(typeof(IMonster))]
	public class Monster : Eamon.Game.Monster, Framework.IMonster
	{
		public override bool HasWornInventory()
		{
			// Only humanoids have a worn inventory list

			return Uid != 1 && Uid != 6 && Uid != 7 && Uid != 26;
		}

		public override bool HasCarriedInventory()
		{
			// Only humanoids have a carried inventory list

			return Uid != 1 && Uid != 6 && Uid != 7 && Uid != 26;
		}

		public override bool HasHumanNaturalAttackDescs()
		{
			return Uid == 4 || Uid == 5;
		}

		public override bool ShouldFleeRoom()
		{
			return Globals.DeviceOpened || base.ShouldFleeRoom();
		}
		
		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			return false;
		}

		public virtual bool ShouldRefuseToAcceptGift01(IArtifact artifact)
		{
			return base.ShouldRefuseToAcceptGift(artifact);
		}

		public override long GetFleeingMemberCount()
		{
			return Globals.DeviceOpened ? CurrGroupCount : base.GetFleeingMemberCount();
		}

		public override void AddHealthStatus(StringBuilder buf, bool addNewLine = true)
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
				var rl = gEngine.RollDice(1, 3, 6);

				var x = DmgTaken;

				x = (((long)((double)(x * 5) / (double)Hardiness)) + 1) * (x > 0 ? 1 : 0);

				result = "at death's door, knocking loudly.";

				if (x == 4)
				{
					result = "very badly injured.";

					if (Globals.MonsterCurses)
					{
						result += gEngine.GetMonsterCurse(this, rl + 3);
					}
				}
				else if (x == 3)
				{
					result = "in pain.";

					if (Globals.MonsterCurses)
					{
						result += gEngine.GetMonsterCurse(this, rl);
					}
				}
				else if (x == 2)
				{
					result = "hurting.";
				}
				else if (x == 1)
				{
					result = "still in good shape.";
				}
				else if (x < 1)
				{
					result = "in perfect health.";
				}
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, addNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}
	}
}
