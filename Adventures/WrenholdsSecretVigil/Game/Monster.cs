
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
			base.AddHealthStatus(buf, addNewLine);

			buf.Replace("badly injured", "very badly injured");

			if (Globals.MonsterCurses)
			{
				var rl = gEngine.RollDice(1, 3, 6);

				var effectUid = buf.IndexOf("very badly injured") >= 0 ? rl + 3 : buf.IndexOf("in pain") >= 0 ? rl : 0;

				if (effectUid > 0)
				{
					Globals.MonsterCurseFunc = () =>
					{
						var curseString = gEngine.GetMonsterCurse(this, effectUid);

						if (!string.IsNullOrWhiteSpace(curseString))
						{
							gOut.Print("{0}", curseString);
						}
					};
				}
			}
		}
	}
}
