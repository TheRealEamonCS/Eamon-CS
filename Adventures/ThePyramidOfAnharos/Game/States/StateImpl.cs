
// StateImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using System.Collections.Generic;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.States
{
	[ClassMappings]
	public class StateImpl : EamonRT.Game.States.StateImpl, IStateImpl
	{
		public override IList<long> GetLoopMonsterUidList()
		{
			IList<long> monsterUidList = null;

			// Guards reanimate and attack

			if (gEngine.GuardsAttack)
			{
				monsterUidList = new List<long> { 20 };

				gEngine.GuardsAttack = false;
			}
			else
			{
				monsterUidList = base.GetLoopMonsterUidList();
			}

			return monsterUidList;
		}
	}
}
