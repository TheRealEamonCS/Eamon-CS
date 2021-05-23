
// PowerCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class PowerCommand : EamonRT.Game.Commands.PowerCommand, IPowerCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterCastSpellCheck)
			{
				var artifactList = gEngine.GetArtifactList(a => a.Uid >= 29 && a.Uid <= 51 && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom)));

				do
				{
					PowerEventRoll = gEngine.RollDice(1, 6, 0);
				}
				while (PowerEventRoll == 1 && artifactList.Count <= 0);

				var rl = gEngine.RollDice(1, 100, 0);

				switch (PowerEventRoll)
				{
					case 1:		// TODO: reuse gEngine.ResurrectDeadBodies if possible
						
						foreach (var artifact in artifactList)
						{
							var monster = Globals.Database.MonsterTable.Records.FirstOrDefault(m => m.DeadBody == artifact.Uid);

							if (monster != null && monster.GroupCount == 1)
							{
								monster.SetInRoom(ActorRoom);

								monster.DmgTaken = 0;

								artifact.SetInLimbo();

								gOut.Print("{0} comes alive!", artifact.GetTheName(true));
							}
						}

						break;

					case 2:

						var processed = false;

						artifactList = ActorMonster.GetCarriedList();

						foreach (var artifact in artifactList)
						{
							if (rl > 80)
							{
								gOut.Print("{0} disappear{1}!", artifact.GetTheName(true), artifact.EvalPlural("s", ""));

								gOut.EnableOutput = false;

								var dropCommand = Globals.CreateInstance<IDropCommand>(x =>
								{
									x.ActorMonster = ActorMonster;

									x.ActorRoom = ActorRoom;

									x.Dobj = artifact;
								});

								dropCommand.Execute();

								gOut.EnableOutput = true;

								artifact.SetInLimbo();

								processed = true;

								break;
							}

							rl = gEngine.RollDice(1, 100, 0);
						}

						if (!processed)
						{
							for (var i = 0; i < gGameState.ImportedArtUidsIdx; i++)
							{
								var artifact = gADB[gGameState.ImportedArtUids[i]];

								Debug.Assert(artifact != null);

								if (artifact.GeneralWeapon != null)
								{
									if (rl > 50)
									{
										Globals.Buf.SetFormat("{0}", artifact.GetTheName(true));

										gEngine.GetPossessiveName(Globals.Buf);

										gOut.Print("{0} complexity goes down!", Globals.Buf.ToString());

										var dec = gEngine.RollDice(1, 5, 4);

										artifact.GeneralWeapon.Field1 -= dec;

										if (artifact.GeneralWeapon.Field1 < Constants.MinWeaponComplexity)
										{
											artifact.GeneralWeapon.Field1 = Constants.MinWeaponComplexity;
										}

										processed = true;

										break;
									}

									rl = gEngine.RollDice(1, 100, 0);
								}
							}
						}

						if (!processed)
						{
							var weaponValues = EnumUtil.GetValues<Weapon>();

							foreach (var wv in weaponValues)
							{
								var weapon = gEngine.GetWeapons(wv);

								Debug.Assert(weapon != null);

								if (rl > 50)
								{
									gOut.Print("Your {0} ability went down!", weapon.Name);

									var dec = gEngine.RollDice(1, 5, 4);

									gCharacter.ModWeaponAbilities(wv, -dec);

									if (gCharacter.GetWeaponAbilities(wv) < weapon.MinValue)
									{
										gCharacter.SetWeaponAbilities(wv, weapon.MinValue);
									}

									processed = true;

									break;
								}

								rl = gEngine.RollDice(1, 100, 0);
							}
						}

						if (!processed)
						{
							gOut.Print("A large flame erupts from nowhere, which burns you badly.");

							var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
							{
								x.SetNextStateFunc = s => NextState = s;

								x.DfMonster = ActorMonster;

								x.OmitArmor = true;
							});

							combatSystem.ExecuteCalculateDamage(1, 6);
						}

						break;

					case 3:

						if (rl > 80)
						{
							gOut.Print("You have been healed!");

							ActorMonster.DmgTaken = 0;
						}
						else
						{
							gOut.Print("You are knocked to the ground by a strong gust of wind.");

							var combatSystem = Globals.CreateInstance<ICombatSystem>(x =>
							{
								x.SetNextStateFunc = s => NextState = s;

								x.DfMonster = ActorMonster;

								x.OmitArmor = true;
							});

							combatSystem.ExecuteCalculateDamage(1, 5);
						}

						break;

					case 4:

						gOut.Print("You feel a small gust of wind hit your face, but nothing else.");

						break;

					case 5:

						if (rl > 80)
						{
							gOut.Print("All your weapon abilities went up!");

							var weaponValues = EnumUtil.GetValues<Weapon>();

							foreach (var wv in weaponValues)
							{
								var weapon = gEngine.GetWeapons(wv);

								Debug.Assert(weapon != null);

								var inc = gEngine.RollDice(1, 5, 0);

								gCharacter.ModWeaponAbilities(wv, inc);

								if (gCharacter.GetWeaponAbilities(wv) > weapon.MaxValue)
								{
									gCharacter.SetWeaponAbilities(wv, weapon.MaxValue);
								}
							}
						}
						else
						{
							gOut.Print("A flash of light appears in the distance but nothing happens.");
						}

						break;

					default:

						if (rl > 90)
						{
							gOut.Print("You just forgot all of your spells!");

							var spellValues = EnumUtil.GetValues<Spell>();

							foreach (var spell in spellValues)
							{
								gCharacter.SetSpellAbilities(spell, 0);

								gGameState.SetSa(spell, 0);
							}
						}
						else
						{
							gOut.Print("A puff of smoke rises from nowhere.");
						}

						break;
				}

				GotoCleanup = true;

				goto Cleanup;
			}
			else
			{
				base.ProcessEvents(eventType);
			}

		Cleanup:

			;
		}
	}
}
