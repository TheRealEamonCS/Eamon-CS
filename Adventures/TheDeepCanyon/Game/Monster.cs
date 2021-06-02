
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool CanMoveToRoom(bool fleeing)
		{
			// Fido can't flee or follow

			return Uid != 11 ? base.CanMoveToRoom(fleeing) : false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			// Fido and elephants need special handling

			return Uid != 11 && Uid != 24 && base.ShouldRefuseToAcceptGift(artifact);
		}

		public override bool CheckCourage()
		{
			Globals.PushRulesetVersion(5);

			var result = base.CheckCourage();

			Globals.PopRulesetVersion();

			return result;
		}

		public override string[] GetWeaponAttackDescs(IArtifact artifact)
		{
			// Falcon

			return artifact != null && artifact.Uid == 5 ? new string[] { "attack{0}" } : base.GetWeaponAttackDescs(artifact);
		}
	}
}
