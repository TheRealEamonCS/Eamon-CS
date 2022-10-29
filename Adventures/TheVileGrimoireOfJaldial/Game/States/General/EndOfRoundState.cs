
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEndRound)
			{
				gEngine.CarrionCrawlerFlails = false;

				gEngine.InitiativeMonsterUid = 0;

				if (gEngine.EncounterSurprises)
				{
					NextState = gEngine.CreateInstance<IPrintPlayerRoomState>();
				}
			}
		}
	}
}

