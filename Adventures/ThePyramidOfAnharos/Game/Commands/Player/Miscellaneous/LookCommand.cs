
// LookCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class LookCommand : EamonRT.Game.Commands.LookCommand, ILookCommand
	{
		public override void Execute()
		{
			ActorRoom.Seen = false;

			if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
			{
				// Hint at southern pyramid secret door

				if (ActorRoom.Uid == 12)
				{
					gEngine.PrintEffectDesc(41);
				}

				// Reveal hidden artifacts

				var artifactList = gEngine.GetArtifactList(a => a.Location == 6000 + ActorRoom.Uid);

				if (artifactList.Count > 0)
				{
					gOut.Print("You found something.");

					foreach (var artifact in artifactList)
					{
						artifact.SetInRoom(ActorRoom);
					}
				}
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}
	}
}
