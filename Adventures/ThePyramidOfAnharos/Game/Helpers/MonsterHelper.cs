
// MonsterHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Helpers
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
	}
}
