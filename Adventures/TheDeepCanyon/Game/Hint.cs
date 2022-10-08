
// Hint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game
{
	[ClassMappings]
	public class Hint : Eamon.Game.Hint, IHint
	{
		public override bool Active
		{
			get
			{
				if (Globals.EnableMutateProperties)
				{
					switch (Uid)
					{
						case 3:

							var shovelArtifact = gADB[18];

							Debug.Assert(shovelArtifact != null);

							return shovelArtifact.Seen;

						case 4:

							var netArtifact = gADB[24];

							Debug.Assert(netArtifact != null);

							return netArtifact.Seen;

						case 5:

							var mouseTrapArtifact = gADB[17];

							Debug.Assert(mouseTrapArtifact != null);

							return mouseTrapArtifact.Seen;

						case 6:

							var falconArtifact = gADB[5];

							Debug.Assert(falconArtifact != null);

							return falconArtifact.Seen;

						case 7:

							var fidoMonster = gMDB[11];

							Debug.Assert(fidoMonster != null);

							return fidoMonster.Seen;

						case 8:

							var elephantsMonster = gMDB[24];

							Debug.Assert(elephantsMonster != null);

							return elephantsMonster.Seen;

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
