
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using System.Linq;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override long Location
		{
			get
			{
				var result = base.Location;

				if (gEngine.EnableMutateProperties)
				{
					switch (Uid)
					{
						case 81:		// Inscription
							{
								var roomUids = new long[] { 62, 63, 65, 70, 72 };

								if (roomUids.Contains(gGameState.Ro))
								{
									result = gGameState.Ro;
								}

								break;
							}

						default:

							// Do nothing

							break;
					}
				}

				return result;
			}

			set
			{
				base.Location = value;
			}
		}
	}
}
