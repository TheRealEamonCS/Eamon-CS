
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterRoundEnd)
			{
				Globals.CarrionCrawlerFlails = false;

				Globals.InitiativeMonsterUid = 0;

				if (Globals.EncounterSurprises)
				{
					NextState = Globals.CreateInstance<IPrintPlayerRoomState>();
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}

