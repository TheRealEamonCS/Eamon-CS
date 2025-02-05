
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// If wooden box opened emit witch's scream

			if (DobjArtifact.Uid == 10)
			{
				if (gGameState.SCR <= 0)
				{
					gEngine.PrintEffectDesc(21);

					gGameState.SCR = 5;
				}
				else
				{
					PrintAlreadyOpen(DobjArtifact);
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Gate of Light

			else if (DobjArtifact.Uid == 13)
			{
				gOut.Print("{0}", gGameState.IC ? "You don't feel any gate." : "As you reach for the gate, the light causes your hands severe pain.");

				NextState = gEngine.CreateInstance<IMonsterStartState>();			// NOTE: IStartState in original
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
