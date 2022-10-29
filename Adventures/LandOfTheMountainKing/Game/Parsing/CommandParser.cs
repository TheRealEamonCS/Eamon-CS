
// CommandParser.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void FinishParsingSaveCommand()
		{
			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				gEngine.PrintEffectDesc(30); //Cannot save with enemies nearby
				NextState = gEngine.CreateInstance<IStartState>();
			}
			else
			{
				base.FinishParsingSaveCommand();
			}
		}
	}
}
