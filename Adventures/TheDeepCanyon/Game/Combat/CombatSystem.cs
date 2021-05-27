
// CombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void PrintHealthStatus()
		{
			base.PrintHealthStatus();

			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			if (DfMonster.IsDead())
			{
				gOut.Print("{0}{1} dead, Jim.", Environment.NewLine, DfMonster.IsCharacterMonster() || room.IsLit() ? DfMonster.EvalGender("He's", "She's", "It's") : "It's");
			}
		}

		public override void CheckMonsterStatus()
		{
			Debug.Assert(DfMonster != null);

			Debug.Assert(_d2 >= 0);

			DfMonster.DmgTaken += _d2;

			if (!OmitMonsterStatus || OfMonster == DfMonster)
			{
				PrintHealthStatus();
			}

			if (DfMonster.IsDead())
			{
				var room = DfMonster.GetInRoom();

				Debug.Assert(room != null);

				var ringArtifact = gADB[22];

				Debug.Assert(ringArtifact != null);

				if (DfMonster.IsCharacterMonster())
				{
					// Resurrect

					if (ringArtifact.IsCarriedByCharacter() || ringArtifact.IsWornByCharacter())
					{
						DfWeapon = DfMonster.Weapon > 0 ? gADB[DfMonster.Weapon] : null;

						if (DfWeapon != null)
						{
							gOut.EnableOutput = false;

							var dropCommand = Globals.CreateInstance<IDropCommand>(x =>
							{
								x.ActorMonster = DfMonster;

								x.ActorRoom = room;

								x.Dobj = DfWeapon;
							});

							dropCommand.Execute();

							gOut.EnableOutput = true;
						}

						gOut.Print("{0}", Globals.LineSep);

						gOut.Write("{0}Press any key to continue: ", Environment.NewLine);

						Globals.Buf.Clear();

						var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNull, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						Globals.Thread.Sleep(150);

						gOut.Print("{0}", Globals.LineSep);

						gEngine.PrintEffectDesc(3);

						room = gRDB[1];

						room.Seen = false;

						DfMonster.SetInRoom(room);

						gGameState.R2 = gGameState.Ro;

						DfMonster.DmgTaken = 0;

						var stat = gEngine.GetStats(Stat.Hardiness);

						Debug.Assert(stat != null);

						DfMonster.Hardiness -= (long)Math.Round((double)DfMonster.Hardiness * 0.4);

						if (DfMonster.Hardiness < stat.MinValue)
						{
							DfMonster.Hardiness = stat.MinValue;
						}

						stat = gEngine.GetStats(Stat.Agility);

						Debug.Assert(stat != null);

						DfMonster.Agility -= (long)Math.Round((double)DfMonster.Agility * 0.4);

						if (DfMonster.Agility < stat.MinValue)
						{
							DfMonster.Agility = stat.MinValue;
						}

						ringArtifact.SetInLimbo();

						DfMonster.EnforceFullInventoryWeightLimits(recurse: true);

						if (SetNextStateFunc != null)
						{
							SetNextStateFunc(Globals.CreateInstance<IStartState>());
						}
					}
					else
					{
						gGameState.Die = 1;

						if (SetNextStateFunc != null)
						{
							SetNextStateFunc(Globals.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							}));
						}
					}
				}
				else
				{
					// Resurrect

					if (ringArtifact.IsCarriedByMonster(DfMonster) || ringArtifact.IsWornByMonster(DfMonster))
					{
						Globals.ResurrectMonsterUid = DfMonster.Uid;

						DfMonster.SetInLimbo();

						ringArtifact.SetInLimbo();
					}
					else
					{
						gEngine.MonsterDies(OfMonster, DfMonster);
					}
				}
			}

			CombatState = CombatState.EndAttack;
		}
	}
}
