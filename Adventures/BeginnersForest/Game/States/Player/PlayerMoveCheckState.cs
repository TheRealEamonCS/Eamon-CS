
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				// Exit gate

				if (gGameState.R2 == -2)
				{
					gOut.Print("The path is washed out!");

					GotoCleanup = true;
				}

				// Death by spooky water! / Death by moss!

				else if (gGameState.R2 == -33 || gGameState.R2 == -35)
				{
					var effectUid = gGameState.R2 == -33 ? 3 : 4;

					gEngine.ResetMonsterStats(gCharMonster);

					gEngine.PrintEffectDesc(effectUid);

					gGameState.Ro = 1;

					gGameState.R2 = gGameState.Ro;

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
					{
						x.MoveMonsters = false;
					});

					GotoCleanup = true;
				}

				// Don't step on any beavers!

				else if (gGameState.R2 == -34)
				{
					gEngine.PrintEffectDesc(1);

					gGameState.R2 = 33;

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>();

					GotoCleanup = true;
				}
			}
		}
	}
}
