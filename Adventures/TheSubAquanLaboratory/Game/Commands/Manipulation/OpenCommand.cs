
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterOpenArtifact)
			{
				// Large cabinet

				if (DobjArtifact.Uid == 11 && !gGameState.CabinetOpen)
				{
					gEngine.PrintEffectDesc(34);

					gGameState.CabinetOpen = true;
				}

				// Locker

				if (DobjArtifact.Uid == 51 && !gGameState.LockerOpen)
				{
					gEngine.PrintEffectDesc(36);

					gGameState.LockerOpen = true;
				}
			}
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Humming cabinet

			if (DobjArtifact.Uid == 49)
			{
				gEngine.PrintEffectDesc(35);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
