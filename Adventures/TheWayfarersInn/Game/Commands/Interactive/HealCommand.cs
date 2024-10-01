
// HealCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class HealCommand : EamonRT.Game.Commands.HealCommand, IHealCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjMonster != null);

			// Unseen apparition / Charlotte

			if (DobjMonster.Uid == 2 || DobjMonster.Uid == 4)
			{
				PrintCantVerbObj(DobjMonster);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
