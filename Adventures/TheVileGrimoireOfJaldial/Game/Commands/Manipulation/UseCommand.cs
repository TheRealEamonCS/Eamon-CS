
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Digging with shovel

			if (DobjArtifact.Uid == 7)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					var buriedCasketArtifact = gADB[35];

					Debug.Assert(buriedCasketArtifact != null);

					if (ActorRoom.Uid == 5 && buriedCasketArtifact.IsInLimbo())
					{
						gEngine.PrintEffectDesc(92);

						buriedCasketArtifact.SetInRoom(ActorRoom);
					}
					else
					{
						var digResult = ActorRoom.EvalRoomType("The floor is far to hard to dig into!", "You dig for a while but find nothing of interest.");

						gOut.Print(digResult);
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Bailing fountain water with wooden bucket

			else if (DobjArtifact.Uid == 6)
			{
				var waterWeirdMonster = gMDB[38];

				Debug.Assert(waterWeirdMonster != null);

				var largeFountainArtifact = gADB[24];

				Debug.Assert(largeFountainArtifact != null);

				var waterArtifact = gADB[40];

				Debug.Assert(waterArtifact != null);

				if (ActorRoom.Uid == 116 && !waterArtifact.IsInLimbo())
				{
					if (waterWeirdMonster.IsInRoom(ActorRoom))
					{
						gOut.Print("{0} won't let you get close enough to do that!", waterWeirdMonster.GetTheName(true));
					}
					else if (!gGameState.WaterWeirdKilled)
					{
						gEngine.PrintEffectDesc(100);

						waterWeirdMonster.SetInRoom(ActorRoom);

						NextState = gEngine.CreateInstance<IStartState>();
					}
					else if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
					{
						gEngine.PrintEffectDesc(93);

						waterArtifact.SetInLimbo();

						largeFountainArtifact.Desc = largeFountainArtifact.Desc.Replace("squirts", "squirted");
					}
					else
					{
						PrintEnemiesNearby();

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else
				{
					gOut.Print("That doesn't do anything right now.");
				}

				if (NextState == null)
				{
					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
			}

			// Using bronze cross on undead

			else if (DobjArtifact.Uid == 37)
			{
				var deadMonsterUids = new long[] { 3, 4, 6 };
				
				var wardedMonsterUids = new long[] { 7, 8, 9, 14, 16 };

				var deadMonsterList = gEngine.GetMonsterList(m => deadMonsterUids.Contains(m.Uid) && m.IsInRoom(ActorRoom));

				foreach(var m in deadMonsterList)
				{
					gOut.Print("{0} is destroyed by the cross, its body disintegrates!", m.GetTheName(true));

					m.SetInLimbo();

					m.DmgTaken = 0;
				}

				var wardedMonsterList = gEngine.GetMonsterList(m => wardedMonsterUids.Contains(m.Uid) && m.IsInRoom(ActorRoom));

				foreach (var m in wardedMonsterList)
				{
					gOut.Print("{0} is warded off by the cross!", m.GetTheName(true));

					gEngine.MoveMonsterToRandomAdjacentRoom(ActorRoom, m, true, false);
				}

				if (deadMonsterList.Count == 0 && wardedMonsterList.Count == 0)
				{
					gOut.Print("Nothing happens.");
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
