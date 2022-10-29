
// EatCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheTempleOfNgurct.Game.Plugin.Globals;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class EatCommand : EamonRT.Game.Commands.EatCommand, IEatCommand
	{
		public virtual long DmgTaken { get; set; }

		public override void PrintVerbItAll(IArtifact artifact)
		{
			// Carcass

			if (artifact.Uid == 67)
			{
				PrintOkay(artifact);
			}
		}

		public override void PrintFeelBetter(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (DmgTaken > 0)
			{
				gOut.Print("Some of your wounds seem to clear up.");
			}
		}

		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			DmgTaken = ActorMonster.DmgTaken;

			base.Execute();
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0;
		}

		public EatCommand()
		{
			IsPlayerEnabled = true;
		}
	}
}
