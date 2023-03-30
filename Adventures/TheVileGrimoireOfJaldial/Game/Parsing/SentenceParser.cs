﻿
// SentenceParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Game.Attributes;
using EamonRT.Framework.Parsing;
using TheVileGrimoireOfJaldial.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Parsing
{
	[ClassMappings]
	public class SentenceParser : EamonRT.Game.Parsing.SentenceParser, ISentenceParser
	{
		/// <summary></summary>
		public override bool IsValidTokenCommandMatch()
		{
			// Disallow match of WaitCommand in the middle of a sentence using "it" (which is reserved for a pronoun)

			return (!(TokenCommand is IWaitCommand) || !RegexMatch.Groups[1].Value.Trim().Equals("it", StringComparison.OrdinalIgnoreCase)) && base.IsValidTokenCommandMatch();
		}
	}
}
