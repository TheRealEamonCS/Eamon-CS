
// PlayerDeadState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.States
{
	[ClassMappings]
	public class PlayerDeadState : EamonRT.Game.States.PlayerDeadState, IPlayerDeadState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			var thorakMonster = gMDB[27];

			Debug.Assert(thorakMonster != null);

			if (eventType == EventType.BeforePlayerDeadPrintDeadMenu)
			{
				if (!thorakMonster.IsInLimbo() && !gGameState.DeathStory)
				{
					gOut.Print("{0}", gEngine.LineSep);

					gEngine.PrintEffectDesc(55);

					gEngine.In.KeyPress(gEngine.Buf);

					gGameState.DeathStory = true;
				}
			}
		}
	}
}
