
// Hint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game
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
						case 3:

							var room2 = gRDB[2];

							Debug.Assert(room2 != null);

							return room2.Seen;

						case 4:

							var ovalDoorArtifact = gADB[16];

							Debug.Assert(ovalDoorArtifact != null);

							return ovalDoorArtifact.Seen;

						case 5:

							var plaqueArtifact = gADB[9];

							Debug.Assert(plaqueArtifact != null);

							return plaqueArtifact.Seen;

						case 6:

							var room18 = gRDB[18];

							Debug.Assert(room18 != null);

							return room18.Seen;

						case 7:

							var aquatronArtifact = gADB[57];

							Debug.Assert(aquatronArtifact != null);

							return aquatronArtifact.Seen;

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
