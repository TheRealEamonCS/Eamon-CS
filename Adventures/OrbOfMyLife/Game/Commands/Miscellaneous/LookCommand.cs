
// LookCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class LookCommand : EamonRT.Game.Commands.LookCommand, ILookCommand
	{
		public override void ExecuteForPlayer()
		{
			ActorRoom.Seen = false;

			if (gGameState.GetNBTL(Friendliness.Enemy) <= 0 || gGameState.IV)
			{
				// Reveal hidden artifacts

				var artifactList = gEngine.GetArtifactList(a => a.Location == 6000 + ActorRoom.Uid);

				if (artifactList.Count > 0)
				{
					gOut.Print("You found something.");

					foreach (var artifact in artifactList)
					{
						artifact.SetInRoom(ActorRoom);
					}
				}

				// Reveal secret doors / forest exit

				if (ActorRoom.Uid != 38 && ActorRoom.Uid != 44)
				{
					var numRooms = gEngine.Module.NumRooms;

					var directionValues = EnumUtil.GetValues<Direction>();

					foreach (var dv in directionValues)
					{
						var direction = gEngine.GetDirection(dv);

						Debug.Assert(direction != null);

						if (ActorRoom.GetDir(dv) < 0 && ActorRoom.GetDir(dv) >= -numRooms)
						{
							if (ActorRoom.Uid == 1)
							{
								gOut.Print("You discover a sliding panel on the east wall!");
							}
							else
							{
								gOut.Print("You found a secret passage {0}!", direction.Name.ToLower());
							}

							ActorRoom.SetDir(dv, -ActorRoom.GetDir(dv));
						}
						else if (ActorRoom.Uid == 3 && ActorRoom.GetDir(dv) == 35)
						{
							gOut.Print("You see a slight clearing {0}!", direction.Name.ToLower());
						}
					}
				}
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}
	}
}
