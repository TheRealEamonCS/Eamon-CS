
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintArtifactReadText)
			{
				// saving throw vs. intellect for book trap warning

				if (DobjArtifact.Uid == 9)
				{
					if (gGameState.BookWarning == 0)
					{
						var rl = gEngine.RollDice(1, 22, 2);

						if (rl <= gCharacter.GetStat(Stat.Intellect))
						{
							gEngine.PrintEffectDesc(14);

							gGameState.BookWarning = 1;

							GotoCleanup = true;
						}
					}
					else
					{
						gEngine.PrintEffectDesc(15);
					}
				}
			}
			else if (eventType == EventType.AfterReadArtifact)
			{
				// book trap

				if (DobjArtifact.Uid == 9)
				{
					gOut.Print(ActorRoom.Uid == 26 ? "You fall into the sea and are eaten by a big fish." : "You flop three times and die.");

					gGameState.Die = 1;

					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;
				}
			}
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// change name of bottle

			if (DobjArtifact.Uid == 3)
			{
				DobjArtifact.Name = "healing potion";

				gOut.Print("It says, \"HEALING POTION\".");

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
