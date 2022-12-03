
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void FinishParsingGetCommand()
		{
			var waterArtifact = gADB[77];

			Debug.Assert(waterArtifact != null);

			waterArtifact.SetInRoom(ActorRoom);

			base.FinishParsingGetCommand();

			waterArtifact.SetInLimbo();
		}

		public override void CheckPlayerCommand(bool afterFinishParsing)
		{
			Debug.Assert(NextCommand != null);

			if (afterFinishParsing)
			{
				// Can't light torch in Black room

				if (NextCommand is ILightCommand && DobjArtifact != null && (DobjArtifact.Uid == 16 || DobjArtifact.Uid == 17) && ActorRoom.Uid == 39)
				{
					gOut.Print("Your light is extinguished as though snuffed out by an unseen watcher.");

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}

				// Can't look at / examine anything in Black room

				else if ((NextCommand is ILookCommand || NextCommand is IExamineCommand) && Dobj != null && ActorRoom.Uid == 39)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}
				else
				{
					base.CheckPlayerCommand(afterFinishParsing);
				}
			}
			else
			{
				base.CheckPlayerCommand(afterFinishParsing);
			}
		}

		public virtual void FinishParsingThrowCommand()
		{
			ResolveRecord(false);
		}
	}
}
