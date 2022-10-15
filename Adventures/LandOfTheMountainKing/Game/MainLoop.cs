
// MainLoop.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Commands;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Startup()
		{
			base.Startup();

			gLMKKP1.Lampdir = 7;
			gLMKKP1.NecklaceTaken = 0;
			gLMKKP1.SaidHello = 0;
			gLMKKP1.SwampMonsterKilled = 0;

			// SET ARMOR EXPERTISE TO 79:
			gLMKKP1.Armor = gCharacter.ArmorExpertise; //remember original value
			gCharacter.ArmorExpertise = 79;

			// SET WEAPON ABILITIES:
			gLMKKP1.Axe = gCharacter.GetWeaponAbility(Weapon.Axe);
			gLMKKP1.Bow = gCharacter.GetWeaponAbility(Weapon.Bow);
			gLMKKP1.Club = gCharacter.GetWeaponAbility(Weapon.Club);
			gLMKKP1.Spear = gCharacter.GetWeaponAbility(Weapon.Spear);
			gLMKKP1.Sword = gCharacter.GetWeaponAbility(Weapon.Sword);
			gCharacter.SetWeaponAbility(Weapon.Axe, 10);
			gCharacter.SetWeaponAbility(Weapon.Bow, 10);
			gCharacter.SetWeaponAbility(Weapon.Club, 10);
			gCharacter.SetWeaponAbility(Weapon.Spear, 10);
			gCharacter.SetWeaponAbility(Weapon.Sword, 10);

			// SET SPELL ABILITIES TO ZERO:
			gLMKKP1.blast = gCharacter.GetSpellAbility(Spell.Blast);
			gLMKKP1.heal = gCharacter.GetSpellAbility(Spell.Heal);
			gLMKKP1.speed = gCharacter.GetSpellAbility(Spell.Speed);
			gLMKKP1.power = gCharacter.GetSpellAbility(Spell.Power);
			gCharacter.SetSpellAbility(Spell.Blast, 0);
			gCharacter.SetSpellAbility(Spell.Heal, 0);
			gCharacter.SetSpellAbility(Spell.Speed, 0);
			gCharacter.SetSpellAbility(Spell.Power, 0);
			gGameState.Sa[(long)Spell.Blast] = 0;
			gGameState.Sa[(long)Spell.Heal] = 0;
			gGameState.Sa[(long)Spell.Speed] = 0;
			gGameState.Sa[(long)Spell.Power] = 0;

			// Initialize Hardiness and Agility:
			gLMKKP1.Agil = gCharMonster.Agility;
			gLMKKP1.Hard = gCharMonster.Hardiness;
			gCharMonster.Hardiness = 20;
			gCharMonster.Agility = 20;

			gEngine.HideImportedPlayerInventory();
		}

		public override void Shutdown()
		{
			// SET ARMOR EXPERTISE TO ORIGINAL VALUE:
			gCharacter.ArmorExpertise = gLMKKP1.Armor;

			// SET WEAPON ABILITIES TO ORIGINAL VALUES:
			gCharacter.SetWeaponAbility(Weapon.Axe, gLMKKP1.Axe);
			gCharacter.SetWeaponAbility(Weapon.Bow, gLMKKP1.Bow);
			gCharacter.SetWeaponAbility(Weapon.Club, gLMKKP1.Club);
			gCharacter.SetWeaponAbility(Weapon.Spear, gLMKKP1.Spear);
			gCharacter.SetWeaponAbility(Weapon.Sword, gLMKKP1.Sword);

			// SET SPELL ABILITIES TO Original values:
			gCharacter.SetSpellAbility(Spell.Blast, gLMKKP1.blast);
			gCharacter.SetSpellAbility(Spell.Heal, gLMKKP1.heal);
			gCharacter.SetSpellAbility(Spell.Speed, gLMKKP1.speed);
			gCharacter.SetSpellAbility(Spell.Power, gLMKKP1.power);
			gGameState.Sa[(long)Spell.Blast] = gLMKKP1.blast;
			gGameState.Sa[(long)Spell.Heal] = gLMKKP1.heal;
			gGameState.Sa[(long)Spell.Speed] = gLMKKP1.speed;
			gGameState.Sa[(long)Spell.Power] = gLMKKP1.power;

			// Set Hardiness and Agility to original values:
			gCharMonster.Hardiness = gLMKKP1.Hard;
			gCharMonster.Agility = gLMKKP1.Agil;

			gEngine.RestoreImportedPlayerInventory();

			base.Shutdown();
		}
	}
}
