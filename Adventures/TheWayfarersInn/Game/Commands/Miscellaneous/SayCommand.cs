
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintSayText)
			{
				var giantWoodenStatueArtifact = gADB[28];

				Debug.Assert(giantWoodenStatueArtifact != null);

				var mirrorArtifact = gADB[71];

				Debug.Assert(mirrorArtifact != null);

				var mirrorPassphraseString = Encoding.UTF8.GetString(Convert.FromBase64String("QmVhdXR5IGlzIGluIHRoZSBleWUgb2YgdGhlIGJlaG9sZGVy"));

				// Trigger mirror doorway (step 1)

				if ((mirrorArtifact.IsInRoom(ActorRoom) || mirrorArtifact.IsEmbeddedInRoom(ActorRoom)) && ProcessedPhrase.Equals(mirrorPassphraseString, StringComparison.OrdinalIgnoreCase))
				{
					gGameState.MirrorPassphraseSpoken = true;
				}

				var hearthwatcherPassphraseString = Encoding.UTF8.GetString(Convert.FromBase64String("SGVhcnRod2F0Y2hlciBwcm90ZWN0IHVz"));

				// Summon Hearthwatcher (step 1)

				if ((giantWoodenStatueArtifact.IsInRoom(ActorRoom) || giantWoodenStatueArtifact.IsEmbeddedInRoom(ActorRoom)) && ProcessedPhrase.Equals(hearthwatcherPassphraseString, StringComparison.OrdinalIgnoreCase))
				{
					gGameState.HearthwatcherPassphraseSpoken = true;
				}
			}
		}
	}
}
