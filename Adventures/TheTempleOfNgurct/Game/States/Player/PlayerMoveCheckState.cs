
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				if (gGameState.R2 == -33)
				{
					gOut.Print("The oak door is locked from the inside!");

					GotoCleanup = true;
				}
				else if (gGameState.R2 == -55)
				{
					gOut.Print("The cell door is locked from the outside!");

					GotoCleanup = true;
				}

				// Down the sewage chute

				else if (gGameState.R2 == -60)
				{
					gEngine.PrintEffectDesc(24);

					gGameState.Die = 1;

					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;
				}
			}
		}
	}
}
