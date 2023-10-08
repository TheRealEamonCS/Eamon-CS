
// CloseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Bottle of kerosene / Rusty oil lantern

			if (DobjArtifact.Uid == 6 || DobjArtifact.Uid == 11)
			{
				PrintNotOpen(DobjArtifact);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Window

			else if (DobjArtifact.Uid == 137)
			{
				if (gGameState.OpenWindowRoomUids.Contains(ActorRoom.Uid))
				{
					PrintClosed(DobjArtifact);

					gGameState.OpenWindowRoomUids.Remove(ActorRoom.Uid);

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintNotOpen(DobjArtifact);

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
