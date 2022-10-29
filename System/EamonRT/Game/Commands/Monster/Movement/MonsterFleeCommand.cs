
// MonsterFleeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class MonsterFleeCommand : Command, IMonsterFleeCommand
	{
		public override void Execute()
		{
			if (ActorMonster.ShouldFleeRoom())
			{
				gEngine.MoveMonsterToRandomAdjacentRoom(ActorRoom, ActorMonster, true, true);
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public MonsterFleeCommand()
		{
			SortOrder = 800;

			// IsDobjPrepEnabled = true;

			IsDarkEnabled = true;

			IsPlayerEnabled = false;

			IsMonsterEnabled = true;

			Name = "MonsterFleeCommand";

			Verb = "flee";

			Type = CommandType.Movement;
		}
	}
}
