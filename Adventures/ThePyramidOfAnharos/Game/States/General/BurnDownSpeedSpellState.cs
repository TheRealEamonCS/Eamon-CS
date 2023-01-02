
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : EamonRT.Game.States.BurnDownSpeedSpellState, IBurnDownSpeedSpellState
	{
		public override void Execute()
		{
			gEngine.PushRulesetVersion(5);

			base.Execute();

			gEngine.PopRulesetVersion();
		}
	}
}
