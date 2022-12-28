
// PutCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class PutCommand : EamonRT.Game.Commands.PutCommand, IPutCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPutArtifact)
			{
				var reliquariesArtifact = gADB[32];

				Debug.Assert(reliquariesArtifact != null);

				var mummyOfAnharosArtifact = gADB[34];

				Debug.Assert(mummyOfAnharosArtifact != null);

				var mapArtifact = gADB[51];

				Debug.Assert(mapArtifact != null);

				// Put reliquaries / mummy of Anharos in sarcophagus

				if ((DobjArtifact.Uid == 32 || DobjArtifact.Uid == 34) && IobjArtifact.Uid == 33 && reliquariesArtifact.IsCarriedByContainer(IobjArtifact) && mummyOfAnharosArtifact.IsCarriedByContainer(IobjArtifact))
				{
					for (var i = 57; i <= 58; i++)
					{
						gEngine.PrintEffectDesc(i);
					}

					reliquariesArtifact.SetInLimbo();

					mummyOfAnharosArtifact.Weight = -999;

					mapArtifact.SetInRoom(ActorRoom);
				}

				// Put Diamond of Purity in pedestal

				else if (DobjArtifact.Uid == 38 && IobjArtifact.Uid == 30)
				{
					for (var i = 2; i <= 4; i++)
					{
						gEngine.PrintEffectDesc(i);
					}

					gEngine.In.KeyPress(gEngine.Buf);

					gOut.Print("{0}", gEngine.LineSep);

					for (var i = 5; i <= 6; i++)
					{
						gEngine.PrintEffectDesc(i);
					}

					DobjArtifact.Weight = -999;

					gGameState.KV = 1;
				}
			}
		}
	}
}
