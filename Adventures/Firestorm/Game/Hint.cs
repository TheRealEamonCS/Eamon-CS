
// Hint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game
{
	[ClassMappings]
	public class Hint : Eamon.Game.Hint, IHint
	{
		public override bool Active
		{
			get
			{
				if (gEngine.EnableMutateProperties)
				{
					switch (Uid)
					{
						case 9:

							var ghostMonster = gMDB[29];

							Debug.Assert(ghostMonster != null);

							return ghostMonster.Seen;

						case 11:

							var treasureChestArtifact = gADB[19];

							Debug.Assert(treasureChestArtifact != null);

							return treasureChestArtifact.Seen;

						case 12:

							return gGameState != null && gGameState.PZ == 1;

						case 13:

							var teleportationCoinArtifact = gADB[53];

							Debug.Assert(teleportationCoinArtifact != null);

							var diamondShapedGemArtifact = gADB[55];

							Debug.Assert(diamondShapedGemArtifact != null);

							return !teleportationCoinArtifact.IsInLimbo() && !diamondShapedGemArtifact.IsInLimbo();

						case 14:

							var room50 = gRDB[50];

							Debug.Assert(room50 != null);

							return room50.Seen;

						case 15:
						case 16:

							var room69 = gRDB[69];

							Debug.Assert(room69 != null);

							return room69.Seen;

						case 17:

							var largeOakDoorArtifact = gADB[59];

							Debug.Assert(largeOakDoorArtifact != null);

							return largeOakDoorArtifact.Seen;

						case 18:

							var room83 = gRDB[83];

							Debug.Assert(room83 != null);

							return room83.Seen;

						default:

							return base.Active;
					}
				}
				else
				{
					return base.Active;
				}
			}

			set
			{
				base.Active = value;
			}
		}
	}
}
