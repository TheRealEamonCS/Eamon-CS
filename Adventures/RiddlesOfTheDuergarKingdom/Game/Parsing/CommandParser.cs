
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public virtual void FinishParsingClimbCommand()
		{
			ResolveRecord(false);
		}

		public virtual void FinishParsingPushCommand()
		{
			ResolveRecord(false);
		}

		public override void CheckPlayerCommand(bool afterFinishParsing)
		{
			Debug.Assert(NextCommand != null);

			var blackSpiderMonster = gMDB[16];

			Debug.Assert(blackSpiderMonster != null);

			var fountainPenArtifact = gADB[41];

			Debug.Assert(fountainPenArtifact != null);

			base.CheckPlayerCommand(afterFinishParsing);

			if (afterFinishParsing)
			{
				// Can't enter wooden cart if (1) Professor Berescroft not met or (2) volcano erupting and at top of mine shaft or (3) stinking of sewage

				if (NextCommand is IGoCommand && DobjArtifact != null && DobjArtifact.Uid == 16 && (!gGameState.BerescroftMet || (gGameState.VolcanoErupting && ActorRoom.Uid == 31) || gGameState.SewagePitVisited))
				{
					if (!gGameState.BerescroftMet)
					{
						gOut.Print("Aren't you getting ahead of yourself?  Surely Professor Berescroft would want to meet with you first!");

						NextState = gEngine.CreateInstance<IStartState>();
					}
					else if (gGameState.VolcanoErupting && ActorRoom.Uid == 31)
					{
						gOut.Print("There's no time to waste - the volcano could erupt at any moment.  You need to flee for your life!");

						NextState = gEngine.CreateInstance<IStartState>();
					}
					else
					{
						gEngine.PrintEffectDesc(79);

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
				}

				// Can't open iron steam turbine when it's running

				else if (NextCommand is IOpenCommand && DobjArtifact != null && DobjArtifact.Uid == 29 && gGameState.SteamTurbineRunning)
				{
					gOut.Print("That's much too dangerous while it is still running.");

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}

				// Can't remove from debris sifter when it's running

				else if (NextCommand is IRemoveCommand && IobjArtifact != null && IobjArtifact.Uid == 50 && gGameState.DebrisSifterRunning)
				{
					gOut.Print("That's much too dangerous while it is still running.");

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}

				// Can't flee with black spider on face

				else if (NextCommand is IFleeCommand && blackSpiderMonster.IsInRoom(ActorRoom))
				{
					gOut.Print("Dealing with the mysterious spider on your face is a higher priority.");

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}

				// Can't blast black spider on face

				else if (NextCommand is IBlastCommand && DobjMonster != null && DobjMonster.Uid == 16)
				{
					gOut.Print("Blasting a spider on your face?  There must be an easier way.");

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}

				// Can't light oil lantern in placid lake

				else if (NextCommand is ILightCommand && DobjArtifact != null && DobjArtifact.Uid == 20 && ActorRoom.Uid == 40)
				{
					gOut.Print("That's not something you can do while swimming in the middle of a lake.");

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}

				// Can't close latrine

				else if (NextCommand is ICloseCommand && DobjArtifact != null && DobjArtifact.Uid == 42)
				{
					gOut.Print("You don't need to.");

					NextState = gEngine.CreateInstance<IStartState>();
				}

				// Can't leave with Professor Berescroft's fountain pen

				else if (NextCommand.Type == CommandType.Movement && !fountainPenArtifact.IsInLimbo())
				{
					gOut.Print("You haven't returned Professor Berescroft's fountain pen yet.");

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
		}
	}
}
