
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			var zephetteMonster = gMDB[4];

			Debug.Assert(zephetteMonster != null);

			base.ProcessArtifact(artifact, ac, ref nlFlag);

			// Healing herbs

			if (artifact.Uid == 41 && zephetteMonster.IsInRoom(ActorRoom))
			{
				gOut.WriteLine();

				gEngine.PrintEffectDesc(65);
			}
		}
	}
}
