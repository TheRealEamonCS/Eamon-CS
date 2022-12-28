
// MonsterHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Helpers
{
	[ClassMappings]
	public class MonsterHelper : Eamon.Game.Helpers.MonsterHelper, IMonsterHelper
	{
		public override void PrintDescGroupCount()
		{
			gEngine.PushRulesetVersion(0);

			base.PrintDescGroupCount();

			gEngine.PopRulesetVersion();
		}

		public override void PrintDescAttackCount()
		{
			gEngine.PushRulesetVersion(0);

			base.PrintDescAttackCount();

			gEngine.PopRulesetVersion();
		}

		public override void PrintDescCourage()
		{
			gEngine.PushRulesetVersion(0);

			base.PrintDescCourage();

			gEngine.PopRulesetVersion();
		}

		public override void PrintDescFriendliness()
		{
			gEngine.PushRulesetVersion(0);

			base.PrintDescFriendliness();

			gEngine.PopRulesetVersion();
		}
	}
}
