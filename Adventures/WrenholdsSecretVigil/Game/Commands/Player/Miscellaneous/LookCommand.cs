
// LookCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class LookCommand : EamonRT.Game.Commands.LookCommand, ILookCommand
	{
		public override void Execute()
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
					var numRooms = Globals.Module.NumRooms;

					var directionValues = EnumUtil.GetValues<Direction>();

					foreach (var dv in directionValues)
					{
						if (ActorRoom.GetDirs(dv) < 0 && ActorRoom.GetDirs(dv) >= -numRooms)
						{
							var direction = gEngine.GetDirections(dv);

							Debug.Assert(direction != null);

							gOut.Print("You found a secret passage {0}!", direction.Name.ToLower());

							ActorRoom.SetDirs(dv, -ActorRoom.GetDirs(dv));
						}
					}
				}
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}
	}
}
