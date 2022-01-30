
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Book

			if (eventType == EventType.AfterReadArtifact && DobjArtifact.Uid == 61)
			{
				DobjArtifact.SetInRoom(ActorRoom);

				gGameState.Ro = 58;

				gGameState.R2 = gGameState.Ro;

				NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
				{
					x.MoveMonsters = false;
				});

				GotoCleanup = true;
			}
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Brown potion

			if (DobjArtifact.Uid == 51)
			{
				gEngine.PrintEffectDesc(1);
			}

			// Yellow potion

			else if (DobjArtifact.Uid == 53)
			{
				gEngine.PrintEffectDesc(2);
			}

			// Red/black potion, fireball wand

			else if (DobjArtifact.Uid == 52 || DobjArtifact.Uid == 62 || DobjArtifact.Uid == 63)
			{
				gEngine.PrintEffectDesc(3);
			}

			// Wine

			else if (DobjArtifact.Uid == 69)
			{
				gEngine.PrintEffectDesc(4);
			}

			// Ring

			else if (DobjArtifact.Uid == 64)
			{
				gEngine.PrintEffectDesc(5);
			}
			else
			{
				base.Execute();
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0;
		}

		public ReadCommand()
		{
			IsPlayerEnabled = true;
		}
	}
}
