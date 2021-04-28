
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override long Location 
		{
			get
			{
				var result = base.Location;

				if (Globals.EnableGameOverrides && gGameState != null)
				{
					var room = gRDB[gGameState.Ro];

					if (room != null)
					{
						// Mount Everdes

						if (Uid == 1 && room.Type == RoomType.Outdoors)
						{
							var roomUids = new long[] { 2, 45, 46, 47, 48 };

							if (!roomUids.Contains(room.Uid))
							{
								result = room.Uid;
							}
						}

						// Purple sage / dust devil

						else if ((Uid == 2 || Uid == 3) && room.Type == RoomType.Outdoors)
						{
							result = room.Uid;
						}

						// Iron fence

						else if (Uid == 5)
						{
							var roomUids = new long[] { 52 };

							if (roomUids.Contains(room.Uid))
							{
								result = room.Uid;
							}
						}
					}
				}

				return result;
			}

			set
			{
				base.Location = value;
			}
		}

		public override bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In)
		{
			// Pack animals exposed in corral, even when it's closed

			return Uid != 72 || containerType != ContainerType.In ? base.ShouldExposeContentsToRoom(containerType) : true;
		}

		public override bool ShouldExposeInContentsWhenClosed()
		{
			// Pack animals exposed in corral, even when it's closed

			return Uid != 72 ? base.ShouldExposeInContentsWhenClosed() : true;
		}
	}
}
