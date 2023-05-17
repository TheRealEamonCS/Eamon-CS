
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.Globals;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Remove magical amulet in forest

			if (gGameState.Ro >= 65 && gGameState.Ro != 92 && gGameState.Ro != 93 && DobjArtifact.Uid == 18)
			{
				gOut.Print("If you remove {0}, you'll be paralysed with fear!", DobjArtifact.GetTheName());

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
