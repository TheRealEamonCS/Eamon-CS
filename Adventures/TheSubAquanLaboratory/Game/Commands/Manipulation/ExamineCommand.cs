
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings(typeof(IExamineCommand))]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, Framework.Commands.IExamineCommand
	{
		public virtual bool ExamineConsole { get; set; }

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintArtifactFullDesc)
			{
				switch (DobjArtifact.Uid)
				{
					case 2:
					case 83:

						// Engraving/fake wall

						if (gGameState.FakeWallExamines < 2)
						{
							gOut.Print("Examining {0} reveals something curious:", DobjArtifact.GetTheName());
						}

						gGameState.FakeWallExamines++;

						gEngine.PrintEffectDesc(40 + gGameState.FakeWallExamines);

						if (gGameState.FakeWallExamines > 2)
						{
							gGameState.FakeWallExamines = 2;
						}

						break;

					case 23:

						// Magnetic fusion power plant

						gEngine.PrintEffectDesc(37);

						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorRoom = ActorRoom;

							x.Dobj = ActorMonster;

							x.OmitArmor = true;
						});

						combatComponent.ExecuteCalculateDamage(1, 6);

						break;

					case 25:

						// Pool pals

						if (!gGameState.Shark)
						{
							var largeHammerheadMonster = gMDB[7];

							Debug.Assert(largeHammerheadMonster != null);

							largeHammerheadMonster.SetInRoom(ActorRoom);

							var smallHammerheadMonster = gMDB[8];

							Debug.Assert(smallHammerheadMonster != null);

							smallHammerheadMonster.SetInRoom(ActorRoom);

							gEngine.PrintEffectDesc(1);

							gGameState.Shark = true;

							NextState = gEngine.CreateInstance<IStartState>();
						}

						break;

					case 45:

						RevealArtifact(46);

						break;

					case 63:

						if (ExamineConsole)
						{
							RevealArtifact(64, true);
						}
						else
						{
							RevealArtifact(65);
						}

						break;

					case 58:

						RevealArtifact(59);

						break;

					case 59:

						RevealArtifact(60);

						break;

					case 64:

						if (!ExamineConsole)
						{
							RevealArtifact(66);
						}

						break;

					case 66:

						RevealArtifact(67);

						break;

					case 67:

						RevealArtifact(68);

						break;

					case 68:

						RevealArtifact(69);

						break;

					case 69:

						RevealArtifact(70);

						break;

					case 62:

						RevealArtifact(63, true);

						break;
				}
			} 
		}

		public virtual void RevealArtifact(long artifactUid, bool examineConsole = false)
		{
			var artifact = gADB[artifactUid];

			Debug.Assert(artifact != null);

			if (!artifact.Seen)
			{
				artifact.SetInRoom(ActorRoom);

				var command = gEngine.CreateInstance<IExamineCommand>(x =>
				{
					((Framework.Commands.IExamineCommand)x).ExamineConsole = examineConsole;
				});

				CopyCommandData(command);

				command.Dobj = artifact;

				NextState = command;
			}
		}
	}
}
