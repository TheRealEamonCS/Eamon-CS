
// Hint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game
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
						case 5:

							var mirrorArtifact = gADB[71];

							Debug.Assert(mirrorArtifact != null);

							var notebookArtifact = gADB[114];

							Debug.Assert(notebookArtifact != null);

							return mirrorArtifact.Seen && notebookArtifact.Seen;

						case 6:

							var childsSkeletonArtifact = gADB[54];

							Debug.Assert(childsSkeletonArtifact != null);

							return childsSkeletonArtifact.Seen;

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
