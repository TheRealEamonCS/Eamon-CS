
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			base.ExecuteForPlayer();

			// Book of the Dark

			if (DobjArtifact.Uid == 3)
			{
				DobjArtifact.Readable.Field1++;

				if (DobjArtifact.Readable.Field1 > 18)
				{
					DobjArtifact.Readable.Field1 = 16;
				}
			}
		}
	}
}
