
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Parsing;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void FinishParsingGetCommand()
		{
			var waterArtifact = gADB[78];

			Debug.Assert(waterArtifact != null);

			waterArtifact.SetInRoom(ActorRoom);

			base.FinishParsingGetCommand();

			waterArtifact.SetInLimbo();
		}

		public virtual void FinishParsingThrowCommand()
		{
			ResolveRecord(false);
		}
	}
}
