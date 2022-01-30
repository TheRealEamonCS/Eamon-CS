
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			//          Spook Reducer 2.0
			//  (c) 2012 Frank Black Productions

			if (eventType == EventType.AfterPrintSayText && ProcessedPhrase.Equals("less spooks", StringComparison.OrdinalIgnoreCase) && gGameState.SpookCounter < 8)
			{
				var spookMonster = gMDB[9];

				Debug.Assert(spookMonster != null);

				spookMonster.CurrGroupCount = spookMonster.CurrGroupCount > 0 ? 1 : 0;

				spookMonster.InitGroupCount = spookMonster.CurrGroupCount;

				spookMonster.GroupCount = spookMonster.CurrGroupCount;

				gGameState.SpookCounter = 8;

				gOut.Print("Less spooks it is!");

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			;
		}
	}
}
