
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterBlockingArtifactCheck && gGameState.R2 == -17)
			{
				gEngine.PrintEffectDesc(108);

				GotoCleanup = true;
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
