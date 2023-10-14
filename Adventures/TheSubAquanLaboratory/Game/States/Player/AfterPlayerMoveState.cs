
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				if (gGameState.R3 == 43 && gGameState.Ro != gGameState.R3)
				{
					var glassWallsArtifact = gADB[84];

					Debug.Assert(glassWallsArtifact != null);

					if (!glassWallsArtifact.IsInLimbo())
					{
						var ovalDoorArtifact = gADB[16];

						Debug.Assert(ovalDoorArtifact != null);

						if (ovalDoorArtifact.IsInLimbo())
						{
							ovalDoorArtifact.SetInRoomUid(43);
						}
					}

					gGameState.Sterilize = false;
				}

				gEngine.WallArtifact = null;

				gEngine.WallZeroAc = null;

				gEngine.WallDamage = 0;

				gEngine.AttackingWall = false;
			}
		}
	}
}
