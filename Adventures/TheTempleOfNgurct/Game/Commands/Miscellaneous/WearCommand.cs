
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.Globals;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Can't wear silk robes

			if (DobjArtifact.Uid == 75)
			{
				PrintDontNeedTo();

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0;
		}

		public WearCommand()
		{
			IsPlayerEnabled = true;
		}
	}
}
