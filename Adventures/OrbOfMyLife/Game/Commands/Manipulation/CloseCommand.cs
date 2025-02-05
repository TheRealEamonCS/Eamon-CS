
// CloseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// If wooden box closed silence witch's scream

			if (DobjArtifact.Uid == 10)
			{
				if (gGameState.SCR > 0)
				{
					gOut.Print("You close the box and the scream stops.");

					gGameState.SCR = 0;

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintNotOpen(DobjArtifact);

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Gate of Light

			else if (DobjArtifact.Uid == 13)
			{
				if (gGameState.IC)
				{
					gOut.Print("You don't feel any gate.");
				}
				else
				{
					PrintNotOpen(DobjArtifact);
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
