
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Parsing;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void FinishParsingInventoryCommand()
		{
			gEngine.PushRulesetVersion(0);

			base.FinishParsingInventoryCommand();

			gEngine.PopRulesetVersion();
		}

		public virtual void FinishParsingSetCommand()
		{
			ResolveRecord(false);
		}
	}
}
