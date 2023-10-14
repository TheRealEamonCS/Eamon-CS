
// InventoryCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class InventoryCommand : EamonRT.Game.Commands.InventoryCommand, IInventoryCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// Unseen apparition

			if (ActorRoom.IsLit() && DobjMonster != null && DobjMonster.Uid == 2)
			{
				PrintDontBeAbsurd();

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Charlotte

			else if (ActorRoom.IsLit() && DobjMonster != null && DobjMonster.Uid == 4)
			{
				gOut.Print("{0} is carrying a ghostly teddy bear.", DobjMonster.GetTheName(true));

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
