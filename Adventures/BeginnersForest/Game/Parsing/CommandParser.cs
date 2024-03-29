﻿
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.Globals;

namespace BeginnersForest.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void CheckPlayerCommand(bool afterFinishParsing)
		{
			Debug.Assert(NextCommand != null);

			base.CheckPlayerCommand(afterFinishParsing);

			// Restrict commands while climbing cliff

			if (afterFinishParsing && ActorRoom.Uid == 15 && !(NextCommand.Type == CommandType.Movement || NextCommand.Type == CommandType.Miscellaneous || NextCommand is IExamineCommand || NextCommand is IHealCommand || NextCommand is ISmileCommand))
			{
				gOut.Print("You're clinging to the side of a cliff!");

				NextState = gEngine.CreateInstance<IStartState>();
			}
		}
	}
}
