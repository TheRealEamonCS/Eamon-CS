
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEnforceMonsterWeightLimitsCheck)
			{
				// Enemies become allies... maybe

				var rl = gEngine.RollDice(1, 100, 0);

				// Give sacred bull the salt / Give python the hamster

				if ((IobjMonster.Uid == 16 && DobjArtifact.Uid == 20) || (IobjMonster.Uid == 20 && DobjArtifact.Uid == 21))
				{
					gOut.Print("{0} eats {1}.", IobjMonster.GetTheName(true), DobjArtifact.GetTheName());

					DobjArtifact.SetInLimbo();

					if (rl > 49)
					{
						IobjMonster.Friendliness = (Friendliness)200;

						IobjMonster.Reaction = Friendliness.Friend;

						if (IobjMonster.Uid == 16)
						{
							gGameState.BullFriendly = true;

							gOut.Print("You have made a friend!");
						}
						else
						{
							gGameState.PythonFriendly = true;

							gOut.Print("You have a friend for life!");
						}
					}
					else
					{
						IobjMonster.Friendliness = (Friendliness)150;

						IobjMonster.Reaction = Friendliness.Neutral;

						gEngine.PrintMonsterEmotes(IobjMonster);

						gOut.WriteLine();
					}

					GotoCleanup = true;
				}
			}
		}
	}
}
