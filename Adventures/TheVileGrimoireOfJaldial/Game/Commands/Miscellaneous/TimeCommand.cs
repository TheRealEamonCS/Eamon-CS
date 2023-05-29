
// TimeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class TimeCommand : EamonRT.Game.Commands.Command, Framework.Commands.ITimeCommand
	{
		public override void ExecuteForPlayer()
		{
			if (gActorRoom(this).IsGroundsRoom())
			{
				if (gGameState.Hour <= 4 || gGameState.Hour >= 23)
				{
					gEngine.PrintEffectDesc(98);
				}
				else if (gGameState.Hour <= 11)
				{
					gOut.Print("A good estimate would be around {0} a.m.", gGameState.Hour);
				}
				else
				{
					gOut.Print("A good estimate would be around {0} p.m.", gGameState.Hour > 12 ? gGameState.Hour - 12 : 12);
				}
			}
			else if (gActorRoom(this).IsCryptRoom())
			{
				gEngine.PrintEffectDesc(99);
			}
			else
			{
				gEngine.PrintEffectDesc(104);
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public TimeCommand()
		{
			SortOrder = 450;

			IsNew = true;

			Name = "TimeCommand";

			Verb = "time";

			Type = CommandType.Miscellaneous;
		}
	}
}
