
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public virtual bool RemoveRopeFromMesquiteTree { get; set; }

		public override void PrintRetrieved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			base.PrintRetrieved(artifact);

			// Remove rope from mesquite tree

			if (RemoveRopeFromMesquiteTree)
			{
				artifact.Type = ArtifactType.Treasure;

				artifact.Field1 = 0;

				artifact.Field2 = 0;

				artifact.Field3 = 0;

				artifact.Field4 = 0;

				artifact.Field5 = 0;

				ActorRoom.SetDir(Direction.Down, 0);

				var room = gRDB[37];

				Debug.Assert(room != null);

				room.SetDir(Direction.Up, 0);
			}
		}

		public override void ExecuteForPlayer()
		{
			var mesquiteTreeArtifact = gADB[70];

			Debug.Assert(mesquiteTreeArtifact != null);

			if (DobjArtifact != null && DobjArtifact.Uid == 69 && DobjArtifact.IsCarriedByContainer(mesquiteTreeArtifact))
			{
				RemoveRopeFromMesquiteTree = true;
			}

			base.ExecuteForPlayer();
		}
	}
}
