
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintArtifactFullDesc)
			{
				/*
				// Bottle of kerosene

				if (DobjArtifact.Uid == 6)		// TODO: remove if Field1 remains 9999
				{
					gOut.Print("{0} is appoximately {1}% full.", DobjArtifact.GetTheName(true), (long)Math.Floor(((double)DobjArtifact.Field1 / 9999.0) * 100.0));		// TODO: ensure synchronized with Field1
				}
				else
				*/

				// Wall Mural - please see Engine.cs for credits

				if (DobjArtifact.Uid == 109 && !gEngine.EnableScreenReaderMode)
				{
					gOut.Print("{0}", gEngine.LineSep);

					gOut.Write("{0}Press any key to continue: ", Environment.NewLine);

					gEngine.Buf.Clear();

					var rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNull, null, gEngine.IsCharAny);

					Debug.Assert(gEngine.IsSuccess(rc));

					gEngine.Thread.Sleep(150);

					gOut.Print("{0}", gEngine.LineSep);

					gOut.Print("{0}", Encoding.UTF8.GetString(Convert.FromBase64String(gEngine.WallMuralData)));
				}
			}
		}

		public override void ExecuteForPlayer()
		{
			// Look in the water well

			if (DobjArtifact != null && DobjArtifact.Uid == 17 && ContainerType == ContainerType.In)
			{
				gOut.Print("It's awfully dark down there.");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Look in/under the leather sectionals

			else if (DobjArtifact != null && DobjArtifact.Uid == 36 && ((ContainerType == ContainerType.In && !gGameState.LsInGoldFound) || (ContainerType == ContainerType.Under && !gGameState.LsUnderGoldFound)))
			{
				var goldFound = gEngine.RollDice(1, 5, 0);

				gOut.Print("You scrounge {0} {1} and turn up {2} gold piece{3}, probably lost from the pocket{3} of {4}unlucky traveler{3}!",
					ContainerType.ToString().ToLower(),
					DobjArtifact.GetTheName(),
					gEngine.GetStringFromNumber(goldFound, false, gEngine.Buf01),
					goldFound != 1 ? "s" : "",
					goldFound != 1 ? "" : "an ");

				gCharacter.HeldGold += goldFound;

				if (ContainerType == ContainerType.In)
				{
					gGameState.LsInGoldFound = true;
				}
				else
				{
					gGameState.LsUnderGoldFound = true;
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Look in the mirror

			else if (DobjArtifact != null && DobjArtifact.Uid == 71 && ContainerType == ContainerType.In)
			{
				gOut.Print("Your whole body is reflected back in {0}.", DobjArtifact.GetTheName());

				// Trigger mirror doorway (step 2)

				if (gGameState.MirrorPassphraseSpoken)
				{
					gEngine.PrintEffectDesc(149);

					DobjArtifact.Type = ArtifactType.DoorGate;

					DobjArtifact.Field1 = 66;

					DobjArtifact.Field2 = -1;

					DobjArtifact.Field3 = 0;

					DobjArtifact.Field4 = 0;

					DobjArtifact.Field5 = 0;
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Look through the jagged breach

			else if (DobjArtifact != null && DobjArtifact.Uid == 129 && Prep != null && (Prep.Name == "in" || Prep.Name == "into" || Prep.Name == "out" || Prep.Name == "through"))
			{
				gEngine.PrintEffectDesc(126);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Look through the window

			else if (DobjArtifact != null && DobjArtifact.Uid == 137 && Prep != null && (Prep.Name == "in" || Prep.Name == "into" || Prep.Name == "out" || Prep.Name == "through"))
			{
				gEngine.PrintEffectDesc(132);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
