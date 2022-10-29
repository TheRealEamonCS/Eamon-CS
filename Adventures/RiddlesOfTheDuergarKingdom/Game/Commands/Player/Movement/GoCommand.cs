
// GoCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class GoCommand : EamonRT.Game.Commands.GoCommand, IGoCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Bottomless pit / Lavafall / Lava river

			if (DobjArtifact.Uid == 53 || DobjArtifact.Uid == 54 || DobjArtifact.Uid == 55)
			{
				gEngine.PrintEffectDesc(DobjArtifact.Uid == 53 ? 66 : 69);

				gGameState.Die = 1;

				NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});
			}
			else
			{
				base.Execute();
			}
		}
	}
}
