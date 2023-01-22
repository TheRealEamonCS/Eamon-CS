
// Hint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
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
					IRoom room, room02 = null;

					IArtifact artifact, artifact02 = null;

					switch (Uid)
					{
						case 4:

							room = gRDB[67];

							Debug.Assert(room != null);

							return room.Seen;

						case 5:

							room = gRDB[6];

							Debug.Assert(room != null);

							return room.Seen;

						case 6:

							room = gRDB[14];

							Debug.Assert(room != null);

							room02 = gRDB[16];

							Debug.Assert(room02 != null);

							return room.Seen || room02.Seen;

						case 7:

							room = gRDB[22];

							Debug.Assert(room != null);

							return room.Seen;

						case 8:

							room = gRDB[26];

							Debug.Assert(room != null);

							return room.Seen;

						case 9:

							room = gRDB[27];

							Debug.Assert(room != null);

							return room.Seen;

						case 10:

							room = gRDB[28];

							Debug.Assert(room != null);

							return room.Seen;

						case 11:

							room = gRDB[30];

							Debug.Assert(room != null);

							return room.Seen;

						case 12:

							room = gRDB[31];

							Debug.Assert(room != null);

							return room.Seen;

						case 13:

							room = gRDB[42];

							Debug.Assert(room != null);

							return room.Seen;

						case 14:

							room = gRDB[59];

							Debug.Assert(room != null);

							return room.Seen;

						case 15:

							artifact = gADB[38];

							Debug.Assert(artifact != null);

							return artifact.Seen;

						case 16:

							room = gRDB[31];

							Debug.Assert(room != null);

							artifact = gADB[38];

							Debug.Assert(artifact != null);

							return room.Seen && artifact.Seen;

						case 17:

							artifact = gADB[30];

							Debug.Assert(artifact != null);

							artifact02 = gADB[38];

							Debug.Assert(artifact02 != null);

							return artifact02.IsCarriedByContainer(artifact);

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
