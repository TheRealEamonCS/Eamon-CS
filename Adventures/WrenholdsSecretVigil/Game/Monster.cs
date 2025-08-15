
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

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
			return Uid == 4 || Uid == 5 || base.HasHumanNaturalAttackDescs();
		}

		public override bool ShouldFleeRoom()
		{
			return gEngine.DeviceOpened || base.ShouldFleeRoom();
		}

		public override bool ShouldRefuseToAcceptGold()
		{
			return false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			return false;
		}

		public virtual bool ShouldRefuseToAcceptGift01(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return !gEngine.IsRulesetVersion(5, 62) && (Reaction == Friendliness.Enemy || (Reaction == Friendliness.Neutral && artifact.Value < 3000));
		}

		public override long GetFleeingMemberCount()
		{
			return gEngine.DeviceOpened ? CurrGroupCount : base.GetFleeingMemberCount();
		}

		public override void AddHealthStatus(StringBuilder buf, bool appendNewLine = true)
		{
			base.AddHealthStatus(buf, appendNewLine);

			buf.Replace("badly injured", "very badly injured");

			if (gEngine.MonsterCurses)
			{
				var rl = gEngine.RollDice(1, 3, 6);

				if (buf.IndexOf("in pain") >= 0)
				{
					gEngine.MiscEventFuncList02.Add(() =>
					{
						gEngine.PrintMonsterCurse(this, rl);
					});
				}
				else if (buf.IndexOf("very badly injured") >= 0)
				{
					gEngine.MiscEventFuncList02.Add(() =>
					{
						gEngine.PrintMonsterCurse(this, rl + 3);
					});
				}
			}
		}
	}
}
