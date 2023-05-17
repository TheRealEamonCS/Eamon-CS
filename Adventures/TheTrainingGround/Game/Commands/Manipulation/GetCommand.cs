
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheTrainingGround.Game.Plugin.Globals;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public virtual bool RevealSecretPassage { get; set; }

		public override void PrintTaken(IArtifact artifact, bool getAll = false)
		{
			Debug.Assert(artifact != null);

			base.PrintTaken(artifact, getAll);

			// Taking Purple book reveals secret passage

			if (artifact.Uid == 27 && ActorMonster.Uid == gGameState.Cm && ActorRoom.Uid == 24 && !gGameState.LibrarySecretPassageFound)
			{
				RevealSecretPassage = true;

				gGameState.LibrarySecretPassageFound = true;
			}
		}

		public override void ExecuteForPlayer()
		{
			base.ExecuteForPlayer();

			if (RevealSecretPassage)
			{
				gEngine.PrintEffectDesc(12);

				ActorRoom.SetDir(Direction.East, 25);
			}
		}
	}
}
