
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintSayText)
			{
				var hammerArtifact = gADB[24];

				Debug.Assert(hammerArtifact != null);

				var magicWordsSpoken = ProcessedPhrase.Equals("thor", StringComparison.OrdinalIgnoreCase) || ProcessedPhrase.Equals("thor's hammer", StringComparison.OrdinalIgnoreCase);

				var hammerPresent = hammerArtifact.IsCarriedByCharacter() || hammerArtifact.IsInRoom(ActorRoom);

				// Hammer of Thor

				if (magicWordsSpoken && hammerPresent)
				{
					var command = Globals.CreateInstance<IUseCommand>();

					CopyCommandData(command);

					command.Dobj = hammerArtifact;

					NextState = command;

					GotoCleanup = true;
				}
			}
		}
	}
}
