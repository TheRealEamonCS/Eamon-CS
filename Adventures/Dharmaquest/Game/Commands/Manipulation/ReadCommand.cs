
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Riddle room inscription

			if (DobjArtifact.Uid == 81 && ActorRoom.Uid == 65)
			{
				var sphinxMonster = gMDB[31];

				Debug.Assert(sphinxMonster != null);

				var ingotsArtifact = gADB[24];

				Debug.Assert(ingotsArtifact != null);

				if (!gGameState.RiddleAnswered)
				{
					base.ExecuteForPlayer();

					gGameState.RiddleAnswered = true;

					gOut.Print("You hear a booming voice say, \"What is the answer to the riddle?\"");

					gOut.Write("{0}Answer: ", Environment.NewLine);

					var buf = new StringBuilder(gEngine.BufSize);

					buf.SetFormat("{0}", gEngine.In.ReadLine());

					if (buf.ToString().Equals("man", StringComparison.OrdinalIgnoreCase))
					{
						gGameState.RiddleSolved = true;

						gOut.Print("Correct!");

						gEngine.PrintEffectDesc(12);

						if (ingotsArtifact.IsInLimbo())
						{
							ingotsArtifact.SetInRoom(ActorRoom);
						}

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
					else
					{
						if (sphinxMonster.IsInLimbo())
						{
							sphinxMonster.SetInRoom(ActorRoom);
						}

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else
				{
					gOut.Print("You only get one chance!");

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
