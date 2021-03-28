
// BurnDownLightSourceState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class BurnDownLightSourceState : EamonRT.Game.States.BurnDownLightSourceState, IBurnDownLightSourceState
	{
		public override void PrintLightAlmostOutCheck()
		{
			Debug.Assert(LsArtifact != null && LsArtAc != null);

			if (LsArtifact.Uid == 1)
			{
				if (LsArtAc.Field1 <= 10 && gEngine.RollDice(1, 100, 0) > 50)
				{
					gOut.Print("{0} flickers momentarily.", LsArtifact.GetTheName(true, buf: Globals.Buf01));
				}
			}
			else
			{
				base.PrintLightAlmostOutCheck();
			}
		}

		public override void DecrementLightTurnCounter()
		{
			Debug.Assert(LsArtifact != null && LsArtAc != null);

			var room = gRDB[gGameState.Ro] as Framework.IRoom;

			Debug.Assert(room != null);

			// Torch is affected by rain and fog; lantern not so much

			if (LsArtifact.Uid == 1 && room.IsRainyRoom())
			{
				LsArtAc.Field1 -= (room.GetWeatherIntensity() * 2);
			}
			else if (LsArtifact.Uid == 1 && room.IsFoggyRoom())
			{
				LsArtAc.Field1 -= (long)Math.Round(room.GetWeatherIntensity() * 1.5);
			}
			else
			{
				LsArtAc.Field1--;
			}
		}
	}
}
