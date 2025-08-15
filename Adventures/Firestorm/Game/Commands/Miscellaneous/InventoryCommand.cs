
// InventoryCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class InventoryCommand : EamonRT.Game.Commands.InventoryCommand, IInventoryCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeInventoryMonsterHealthStatus)
			{
				if (DobjMonster.IsCharacterMonster())
				{
					gOut.Print("You have {0} magic point{1}.", gGameState.MP, gGameState.MP != 1 ? "s" : "");
				}
			}
			else if (eventType == EventType.AfterInventoryMonsterHealthStatus)
			{
				if (DobjMonster.IsCharacterMonster() && gGameState.PZ == 1)
				{
					gOut.Print("*** You are poisoned.");
				}
			}
		}

		public override void ExecuteForPlayer()
		{
			if (DobjMonster != null && DobjMonster.Reaction == Friendliness.Enemy && ActorMonster.Weapon > 0)
			{
				gOut.Print("You are attacked!");

				var command = gEngine.CreateInstance<IAttackCommand>();

				CopyCommandData(command);

				NextState = command;
			}

			// Thorak Junior

			else if (DobjMonster != null && DobjMonster.Uid == 39 && DobjMonster.Reaction == Friendliness.Neutral)
			{
				DobjMonster.Reaction = Friendliness.Friend;

				base.ExecuteForPlayer();

				DobjMonster.Reaction = Friendliness.Neutral;
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
