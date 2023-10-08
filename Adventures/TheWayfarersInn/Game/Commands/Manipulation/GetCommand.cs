
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void PrintRetrieved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Water

			if (artifact.Uid == 60)
			{
				if (IobjArtifact != null)
				{
					gOut.Print("You {0} {1} out of {2}.", IobjArtifact.Uid == 47 ? "pour" : "bail", artifact.GetTheName(), IobjArtifact.GetTheName());
				}
				else
				{
					gOut.Print("You pour {0} out of the bucket.", artifact.GetTheName());
				}
			}
			else
			{
				base.PrintRetrieved(artifact);
			}
		}

		public override void ExecuteForPlayer()
		{
			var artifactUids = new long[] { 117, 123, 127, 167, 174, 175 };

			base.ExecuteForPlayer();

			// Dire wolf pups / Glass jar / Horse harnesses / Forgecraft Codex / Plates / Utensils

			if (DobjArtifact != null && artifactUids.Contains(DobjArtifact.Uid))
			{
				DobjArtifact.Moved = true;
			}
		}
	}
}
