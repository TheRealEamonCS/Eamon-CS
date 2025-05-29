
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override void CheckAfterCastBlast()
		{
			// BLAST elephants

			if (DobjMonster != null && DobjMonster.Uid == 24)
			{
				PrintZapDirectHit();

				gOut.Print("The power of Elzod protects the elephants from your magical attack!");

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			base.CheckAfterCastBlast();

		Cleanup:

			;
		}

		public override void CheckAfterCastPower()
		{
			Func<IArtifact, bool> findDeadBodiesFunc = a => a.DeadBody != null && (a.IsCarriedByMonster(ActorMonster) || a.IsInRoom(ActorRoom));

			var artifactList = gEngine.GetArtifactList(findDeadBodiesFunc);

			do
			{
				PowerEventRoll = gEngine.RollDice(1, 6, 0);
			}
			while (PowerEventRoll == 1 && artifactList.Count <= 0);

			var rl = gEngine.RollDice(1, 100, 0);

			switch (PowerEventRoll)
			{
				case 1:

					gEngine.ResurrectDeadBodies(ActorRoom, findDeadBodiesFunc);

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

							var dropCommand = gEngine.CreateInstance<IDropCommand>(x =>
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
						for (var i = 0; i < gGameState.ImportedArtUids.Count; i++)
						{
							var artifact = gADB[gGameState.ImportedArtUids[i]];

							Debug.Assert(artifact != null);

							if ((artifact.IsCarriedByMonster(ActorMonster) || artifact.IsInRoom(ActorRoom)) && artifact.GeneralWeapon != null)
							{
								if (rl > 50)
								{
									gOut.Print("{0} complexity goes down!", artifact.GetTheName(true).AddPossessiveSuffix());

									var dec = gEngine.RollDice(1, 5, 4);

									artifact.GeneralWeapon.Field1 -= dec;

									if (artifact.GeneralWeapon.Field1 < gEngine.MinWeaponComplexity)
									{
										artifact.GeneralWeapon.Field1 = gEngine.MinWeaponComplexity;
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
							var weapon = gEngine.GetWeapon(wv);

							Debug.Assert(weapon != null);

							if (rl > 50)
							{
								gOut.Print("Your {0} ability went down!", weapon.Name);

								var dec = gEngine.RollDice(1, 5, 4);

								gCharacter.ModWeaponAbility(wv, -dec);

								if (gCharacter.GetWeaponAbility(wv) < weapon.MinValue)
								{
									gCharacter.SetWeaponAbility(wv, weapon.MinValue);
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

						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = SetNextStateFunc;

							x.ActorRoom = ActorRoom;

							x.Dobj = ActorMonster;

							x.OmitArmor = true;
						});

						combatComponent.ExecuteCalculateDamage(1, 6);
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

						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = SetNextStateFunc;

							x.ActorRoom = ActorRoom;

							x.Dobj = ActorMonster;

							x.OmitArmor = true;
						});

						combatComponent.ExecuteCalculateDamage(1, 5);
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
							var weapon = gEngine.GetWeapon(wv);

							Debug.Assert(weapon != null);

							var inc = gEngine.RollDice(1, 5, 0);

							gCharacter.ModWeaponAbility(wv, inc);

							if (gCharacter.GetWeaponAbility(wv) > weapon.MaxValue)
							{
								gCharacter.SetWeaponAbility(wv, weapon.MaxValue);
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
							gCharacter.SetSpellAbility(spell, 0);

							gGameState.SetSa(spell, 0);
						}
					}
					else
					{
						gOut.Print("A puff of smoke rises from nowhere.");
					}

					break;
			}

			MagicState = MagicState.EndMagic;
		}
	}
}
