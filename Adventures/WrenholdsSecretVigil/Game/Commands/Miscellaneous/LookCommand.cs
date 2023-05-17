
// LookCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class LookCommand : EamonRT.Game.Commands.LookCommand, ILookCommand
	{
		public override void ExecuteForPlayer()
		{
			ActorRoom.Seen = false;

			if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
			{
				var artTypes = new ArtifactType[] { ArtifactType.Treasure, ArtifactType.DoorGate };

				var goldCurtainArtifact = gADB[40];

				Debug.Assert(goldCurtainArtifact != null);

				var ac = goldCurtainArtifact.GetArtifactCategory(artTypes);

				Debug.Assert(ac != null);

				if (ActorRoom.Uid != 67 || ac.Type == ArtifactType.Treasure || ac.GetKeyUid() == 0)
				{
					var numRooms = gEngine.Module.NumRooms;

					var directionValues = EnumUtil.GetValues<Direction>();

					foreach (var dv in directionValues)
					{
						if (ActorRoom.GetDir(dv) < 0 && ActorRoom.GetDir(dv) >= -numRooms)
						{
							var direction = gEngine.GetDirection(dv);

							Debug.Assert(direction != null);

							gOut.Print("You found a secret passage {0}!", direction.Name.ToLower());

							ActorRoom.SetDir(dv, -ActorRoom.GetDir(dv));
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
