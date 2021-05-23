
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Mouse trap

			if (DobjArtifact.Uid == 17)
			{
				// TODO: implement

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}

			// Leather bag

			else if (DobjArtifact.Uid == 2)
			{
				gOut.Print("The gold dust is in there, alright.");

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}

			// Box

			else if (DobjArtifact.Uid == 23)
			{
				var netArtifact = gADB[24];

				Debug.Assert(netArtifact != null);

				if (!gGameState.BoxOpened)
				{
					gOut.Print("The box had a net in it.");

					netArtifact.SetInRoom(ActorRoom);

					gGameState.BoxOpened = true;
				}
				else
				{
					gOut.Print("The box is empty.");
				}

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
