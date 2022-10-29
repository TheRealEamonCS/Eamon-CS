
// DigCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class DigCommand : EamonRT.Game.Commands.Command, Framework.Commands.IDigCommand
	{
		public override void PrintCantVerbHere()
		{
			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				PrintEnemiesNearby();
			}
			else
			{
				gOut.Print("You cannot {0} here.", Verb);
			}
		}

		public override void Execute()
		{
			var buriedArtifacts = gADB.Records.Cast<Framework.IArtifact>().Where(a => a.IsBuriedInRoom(ActorRoom)).ToList();

			if (buriedArtifacts.Count > 0)
			{
				gOut.Print("You found something!");

				buriedArtifacts[0].SetInRoom(ActorRoom);
			}
			else
			{
				gOut.Print("You dig but find nothing.");
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0 && gActorRoom(this).IsDigCommandAllowedInRoom();
		}

		public DigCommand()
		{
			SortOrder = 440;

			IsNew = true;

			Name = "DigCommand";

			Verb = "dig";

			Type = CommandType.Miscellaneous;
		}
	}
}
