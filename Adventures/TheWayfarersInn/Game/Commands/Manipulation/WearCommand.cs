
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeWearArtifact)
			{
				var armorArtifact = gGameState.Ar > 0 ? gADB[gGameState.Ar] : null;

				// Fine clothing

				if (DobjArtifact.Uid == 185 && armorArtifact != null)
				{
					PrintWearingRemoveFirst01(armorArtifact);

					NextState = gEngine.CreateInstance<IStartState>();

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.AfterWearArtifact)
			{
				// Fine clothing

				if (DobjArtifact.Uid == 185 && gGameState.FineClothingEnchanted)
				{
					var stat = gEngine.GetStat(Stat.Charisma);

					Debug.Assert(stat != null);

					// TODO: gEngine.PrintEffectDesc ???

					if (gCharacter.GetStat(Stat.Charisma) < stat.MaxValue)
					{
						gCharacter.ModStat(Stat.Charisma, 1);
					}
				}
			}
		}
	}
}
