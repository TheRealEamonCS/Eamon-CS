
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (BlastSpell || ActorMonster.Weapon > 0)
			{
				// ATTACK/BLAST statue

				if (DobjArtifact != null && DobjArtifact.Uid == 50)
				{
					gEngine.PrintMonsterAlive(DobjArtifact);

					if (BlastSpell)
					{
						PrintZapDirectHit();
					}

					DobjArtifact.SetInLimbo();

					var ngurctMonster = gMDB[53];

					Debug.Assert(ngurctMonster != null);

					ngurctMonster.SetInRoom(ActorRoom);

					var command = Globals.CreateInstance<IAttackCommand>(x =>
					{
						x.BlastSpell = BlastSpell;
					});

					CopyCommandData(command);

					command.Dobj = ngurctMonster;

					NextState = command;
				}

				// Fireball wand

				else if (!BlastSpell && DobjMonster != null && ActorMonster.Weapon == 63)
				{
					gOut.Write("{0}What is the trigger word? ", Environment.NewLine);

					Globals.Buf.SetFormat("{0}", Globals.In.ReadLine());

					if (!Globals.Buf.ToString().Equals("fire", StringComparison.OrdinalIgnoreCase))
					{
						gOut.Print("Wrong!  Nothing happens!");

						NextState = Globals.CreateInstance<IMonsterStartState>();

						goto Cleanup;
					}

					if (gGameState.WandCharges <= 0)
					{
						gOut.Print("The fireball wand is exhausted!");

						NextState = Globals.CreateInstance<IMonsterStartState>();

						goto Cleanup;
					}

					gGameState.WandCharges--;

					gOut.Print("The {0} is filled with an incandescent fireball!", ActorRoom.EvalRoomType("room", "area"));

					var slaveGirlFireballCheck = false;

					var slaveGirlArtifact = gADB[81];

					Debug.Assert(slaveGirlArtifact != null);

					var slaveGirlMonster = gMDB[54];

					Debug.Assert(slaveGirlMonster != null);

					if (slaveGirlArtifact.IsInRoom(ActorRoom))
					{
						slaveGirlMonster.SetInRoom(ActorRoom);

						slaveGirlMonster.Seen = true;

						slaveGirlFireballCheck = true;
					}

					var monsterList = gEngine.GetRandomMonsterList(9, m => !m.IsCharacterMonster() && m.Uid != DobjMonster.Uid && m.Seen && m.IsInRoom(ActorRoom));

					Debug.Assert(monsterList != null);

					if (monsterList.Count > 0)
					{
						monsterList.Insert(0, DobjMonster);
					}
					else
					{
						monsterList.Add(DobjMonster);
					}

					Globals.FireDamage = true;

					foreach (var m in monsterList)
					{
						var rl = gEngine.RollDice(1, 100, 0);

						var savedVsFire = (m.Hardiness / 4) > 4 && rl < 51;

						gEngine.MonsterGetsAggravated(m);

						var combatComponent = Globals.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorRoom = m.GetInRoom();

							x.Dobj = m;

							x.OmitArmor = true;
						});

						combatComponent.ExecuteCalculateDamage(savedVsFire ? 3 : 6, 6);

						Globals.Thread.Sleep(gGameState.PauseCombatMs);
					}

					Globals.FireDamage = false;

					if (slaveGirlFireballCheck)
					{
						slaveGirlMonster.Seen = false;

						if (slaveGirlMonster.IsInLimbo())
						{
							slaveGirlArtifact.SetInLimbo();
						}
						else
						{
							slaveGirlMonster.SetInLimbo();
						}
					}

					NextState = Globals.CreateInstance<IMonsterStartState>();

					goto Cleanup;
				}
				else
				{
					base.Execute();
				}
			}
			else
			{
				base.Execute();
			}

		Cleanup:

			;
		}
	}
}
