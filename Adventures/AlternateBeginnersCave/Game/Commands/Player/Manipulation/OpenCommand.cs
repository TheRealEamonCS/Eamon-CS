
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Unremarkable Box (Preserved text from the Classic Eamon version)

			if (eventType == EventType.BeforePrintArtifactOpen && DobjArtifact.Uid == 7 && !gGameState.OpenedBox)
			{
				gGameState.OpenedBox = true;

				gEngine.PrintEffectDesc(7);

				GotoCleanup = true;
			}
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Waspicide

			if (DobjArtifact.Uid == 5)
			{
				var waspMonster = gMDB[4];

				Debug.Assert(waspMonster != null);

				if (waspMonster.IsInRoom(ActorRoom))
				{
					DobjArtifact.SetInLimbo();

					gEngine.PrintEffectDesc(4);

					waspMonster.DmgTaken = waspMonster.Hardiness;

					var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.ActorRoom = ActorRoom;

						x.Dobj = waspMonster;
					});

					combatComponent.ExecuteCheckMonsterStatus();

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					gOut.Print("This isn't a good time to open it.");

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Grate - (sapphire inside)

			else if (DobjArtifact.Uid == 22)
			{
				var sapphireArtifact = gADB[11];

				Debug.Assert(sapphireArtifact != null);

				if (sapphireArtifact.IsCarriedByContainer(DobjArtifact))
				{
					sapphireArtifact.SetInRoom(ActorRoom);

					gEngine.PrintEffectDesc(3);

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					gOut.Print("It's already open.");

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else
			{
				base.Execute();
			}
		}
	}
}
