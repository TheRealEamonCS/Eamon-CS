
// Hint.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using TheVileGrimoireOfJaldial.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game
{
	[ClassMappings]
	public class Hint : Eamon.Game.Hint, IHint
	{
		public override bool Active
		{
			get
			{
				if (Globals.EnableGameOverrides)
				{
					Framework.IRoom room = null;

					switch (Uid)
					{
						case 5:

							var woodenBucketArtifact = gADB[6] as Framework.IArtifact;

							Debug.Assert(woodenBucketArtifact != null);

							return woodenBucketArtifact.Seen02;

						case 6:

							var shovelArtifact = gADB[7] as Framework.IArtifact;

							Debug.Assert(shovelArtifact != null);

							return shovelArtifact.Seen02;

						case 7:

							var ropeArtifact = gADB[38] as Framework.IArtifact;

							Debug.Assert(ropeArtifact != null);

							return ropeArtifact.Seen02;

						case 8:

							var bronzeCrossArtifact = gADB[37] as Framework.IArtifact;

							Debug.Assert(bronzeCrossArtifact != null);

							return bronzeCrossArtifact.Seen02;

						case 9:

							var torchArtifact = gADB[1];

							Debug.Assert(torchArtifact != null && torchArtifact.LightSource != null);

							return torchArtifact.LightSource.Field1 <= 10;

						case 10:

							room = (gGameState != null ? gRDB[gGameState.Ro] : null) as Framework.IRoom;

							return room != null && room.IsCryptRoom();

						case 11:

							var waterWeirdMonster = gMDB[38] as Framework.IMonster;

							Debug.Assert(waterWeirdMonster != null);

							return waterWeirdMonster.Seen02;

						case 12:

							var jaldialRemainsArtifact = gADB[57] as Framework.IArtifact;

							Debug.Assert(jaldialRemainsArtifact != null);

							return jaldialRemainsArtifact.Seen02;

						case 13:

							room = (gGameState != null ? gRDB[gGameState.Ro] : null) as Framework.IRoom;

							return room != null && room.IsGroundsRoom() && gGameState.Day > 0;

						case 14:

							room = (gGameState != null ? gRDB[gGameState.Ro] : null) as Framework.IRoom;

							return room != null && room.IsGroundsRoom() && gGameState.WeatherType != WeatherType.None;

						case 15:

							var tombstoneArtifact = gADB[10] as Framework.IArtifact;

							Debug.Assert(tombstoneArtifact != null);

							return tombstoneArtifact.Seen02;

						case 16:

							room = gRDB[110] as Framework.IRoom;

							Debug.Assert(room != null);

							var jaldialMonster = gMDB[43] as Framework.IMonster;

							Debug.Assert(jaldialMonster != null);

							return room.Seen02 && jaldialMonster.Seen02;

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
