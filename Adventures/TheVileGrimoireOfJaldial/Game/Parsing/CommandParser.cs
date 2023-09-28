
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using TheVileGrimoireOfJaldial.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void FinishParsingHealCommand()
		{
			gEngine.PushRulesetVersion(0);

			base.FinishParsingHealCommand();

			gEngine.PopRulesetVersion();
		}

		public override void FinishParsingInventoryCommand()
		{
			gEngine.PushRulesetVersion(0);

			base.FinishParsingInventoryCommand();

			gEngine.PopRulesetVersion();
		}

		public override void FinishParsingSettingsCommand()
		{
			long longValue = 0;

			bool boolValue = false;

			var settingsCommand = NextCommand as Framework.Commands.ISettingsCommand;

			Debug.Assert(settingsCommand != null);

			if (CurrToken + 1 < Tokens.Length)
			{
				if (Tokens[CurrToken].Equals("showcombatdamage", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.ShowCombatDamage = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("exitdirnames", StringComparison.OrdinalIgnoreCase) && bool.TryParse(Tokens[CurrToken + 1], out boolValue))
				{
					settingsCommand.ExitDirNames = boolValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("weatherfreqpct", StringComparison.OrdinalIgnoreCase) && long.TryParse(Tokens[CurrToken + 1], out longValue) && longValue >= 0 && longValue <= 100)
				{
					settingsCommand.WeatherFreqPct = longValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("encounterfreqpct", StringComparison.OrdinalIgnoreCase) && long.TryParse(Tokens[CurrToken + 1], out longValue) && longValue >= 0 && longValue <= 100)
				{
					settingsCommand.EncounterFreqPct = longValue;

					CurrToken += 2;
				}
				else if (Tokens[CurrToken].Equals("flavorfreqpct", StringComparison.OrdinalIgnoreCase) && long.TryParse(Tokens[CurrToken + 1], out longValue) && longValue >= 0 && longValue <= 100)
				{
					settingsCommand.FlavorFreqPct = longValue;

					CurrToken += 2;
				}
				else
				{
					base.FinishParsingSettingsCommand();
				}
			}
			else
			{
				base.FinishParsingSettingsCommand();
			}
		}

		public virtual void FinishParsingSearchCommand()
		{
			if (CurrToken < Tokens.Length)
			{
				ParseName();

				if (!ObjData.Name.Equals("room", StringComparison.OrdinalIgnoreCase) && !ObjData.Name.Equals("area", StringComparison.OrdinalIgnoreCase) && !(ActorRoom.Uid == 89 && ObjData.Name.ContainsAny(new string[] { "tapestries", "tapestry" }, StringComparison.OrdinalIgnoreCase)))
				{
					ResolveRecord(false);
				}
			}
		}

		public virtual void FinishParsingWaitCommand()
		{
			long i;

			var waitCommand = NextCommand as Framework.Commands.IWaitCommand;

			Debug.Assert(waitCommand != null);

			if (CurrToken < Tokens.Length)
			{
				ParseName();

				// Wait up to 55 minutes in increments of 5

				if (long.TryParse(ObjData.Name, out i) && i > 0)
				{
					waitCommand.Minutes = i;

					while (waitCommand.Minutes % 5 != 0)
					{
						waitCommand.Minutes++;
					}

					if (waitCommand.Minutes > 55)
					{
						waitCommand.Minutes = 55;
					}
				}
			}

			// Restrict WaitCommand while enemies are present

			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				waitCommand.Minutes = 0;
			}
		}

		public override void ResolveRecordProcessWhereClauseList()
		{
			// Do a normal Record resolution, excluding Decorations (which will be in limbo)

			base.ResolveRecordProcessWhereClauseList();

			// If the Record couldn't be resolved and a Decoration was identified in ParseName, move it into the Room and try to resolve again

			if (ObjData.FilterRecordList.Count == 0 && ObjData.Cast<Framework.Parsing.IParserData>().DecorationArtifact != null)
			{
				ObjData.Cast<Framework.Parsing.IParserData>().DecorationArtifact.SetInRoom(ActorRoom);

				base.ResolveRecordProcessWhereClauseList();
			}
		}

		public override void SetLastNameStrings(IGameBase obj, string objDataName, IArtifact artifact, IMonster monster)
		{
			base.SetLastNameStrings(obj, objDataName, artifact, monster);

			// Decorations can also be referred to as "them" for ease of use

			if (artifact != null && (artifact.Uid == 41 || artifact.Uid == 42))
			{
				artifact.IsPlural = true;

				base.SetLastNameStrings(obj, objDataName, artifact, monster);

				artifact.IsPlural = false;
			}
		}

		public override void ParseName()
		{
			base.ParseName();

			var a = gADB[ObjData == DobjData ? 41 : 42];

			Debug.Assert(a != null);

			// Examined decorations

			if ((ActorRoom.Uid == 1 || ActorRoom.Uid == 4) && ObjData.Name.Contains("gate", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 1;
			}
			else if ((ActorRoom.Uid == 11 || ActorRoom.Uid == 16 || ActorRoom.Uid == 22) && ObjData.Name.ContainsAny(new string[] { "brook", "stream" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 5;
			}
			else if (ActorRoom.Uid == 12 && ObjData.Name.ContainsAny(new string[] { "pile", "offal" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 6;
			}
			else if (ActorRoom.Uid == 12 && ObjData.Name.Contains("rat", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 7;
			}
			else if (ActorRoom.Uid == 13 && ObjData.Name.ContainsAny(new string[] { "pile", "rocks", "pyramid" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 8;
			}
			else if (ActorRoom.Uid == 8 && ObjData.Name.Contains("elm", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 9;
			}
			else if (ActorRoom.Uid == 19 && ObjData.Name.ContainsAny(new string[] { "hole", "grave" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 10;
			}
			else if (ActorRoom.Uid == 20 && ObjData.Name.ContainsAny(new string[] { "skeleton", "animal", "creature" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 11;
			}
			else if (ActorRoom.Uid == 23 && ObjData.Name.Contains("pine", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 12;
			}
			else if (ActorRoom.Uid == 26 && ObjData.Name.ContainsAny(new string[] { "coffin", "hand", "skeleton" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 13;
			}
			else if (ActorRoom.Uid == 56 && ObjData.Name.ContainsAny(new string[] { "heap", "pile", "bone" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 15;
			}
			else if (ActorRoom.Uid == 62 && ObjData.Name.ContainsAny(new string[] { "fresco", "mural", "painting" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 17;
			}
			else if (ActorRoom.Uid == 62 && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 18;
			}
			else if ((ActorRoom.Uid == 64 || ActorRoom.Uid == 65) && ObjData.Name.Contains("door", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 19;
			}
			else if (ActorRoom.Uid == 65 && ObjData.Name.ContainsAny(new string[] { "fluid", "blood" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 20;
			}
			else if ((ActorRoom.Uid == 64 || ActorRoom.Uid == 65) && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 21;
			}
			else if (ActorRoom.Uid == 66 && ObjData.Name.ContainsAny(new string[] { "skeleton", "leather", "armor" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 22;
			}
			else if (ActorRoom.Uid == 66 && ObjData.Name.Contains("wall", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 23;
			}
			else if (ActorRoom.Uid == 68 && ObjData.Name.Contains("moss", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 24;
			}
			else if (ActorRoom.Uid == 69 && ObjData.Name.Contains("moss", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 25;
			}
			else if (ActorRoom.Uid == 69 && ObjData.Name.Contains("box", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 26;
			}
			else if (ActorRoom.Uid == 71 && ObjData.Name.Contains("alga", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 27;
			}
			else if (ActorRoom.Uid == 70 && ObjData.Name.Contains("groove", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 28;
			}
			else if (ActorRoom.Uid == 71 && ObjData.Name.Contains("bow", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 29;
			}
			else if (ActorRoom.Uid == 71 && ObjData.Name.Contains("hole", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 30;
			}
			else if (ActorRoom.Uid == 72 && ObjData.Name.ContainsAny(new string[] { "cloth", "strip" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 31;
			}
			else if (ActorRoom.Uid == 72 && ObjData.Name.ContainsAny(new string[] { "rock", "pile" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 32;
			}
			else if (ActorRoom.Uid == 73 && ObjData.Name.Contains("mummy", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 33;
			}
			else if (ActorRoom.Uid == 74 && ObjData.Name.ContainsAny(new string[] { "spider", "web" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 34;
			}
			else if (ActorRoom.Uid == 75 && ObjData.Name.ContainsAny(new string[] { "fresco", "mural", "painting" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 35;
			}
			else if (ActorRoom.Uid == 75 && ObjData.Name.ContainsAny(new string[] { "glyph", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 36;
			}
			else if (ActorRoom.Uid == 76 && ObjData.Name.Contains("etching", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 37;
			}
			else if (ActorRoom.Uid == 77 && ObjData.Name.Contains("face", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 38;
			}
			else if (ActorRoom.Uid == 82 && ObjData.Name.ContainsAny(new string[] { "goblin", "body" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 39;
			}
			else if (ActorRoom.Uid == 82 && ObjData.Name.ContainsAny(new string[] { "chain", "armor" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 40;
			}
			else if (ActorRoom.Uid == 84 && ObjData.Name.ContainsAny(new string[] { "shiny", "substance", "slime" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 41;
			}
			else if (ActorRoom.Uid == 84 && ObjData.Name.ContainsAny(new string[] { "boot", "mound", "earth" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 42;
			}
			else if (ActorRoom.Uid == 86 && ObjData.Name.ContainsAny(new string[] { "goblin", "bodies", "body" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 43;
			}
			else if (ActorRoom.Uid == 86 && ObjData.Name.ContainsAny(new string[] { "spoor", "dung" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 44;
			}
			else if (ActorRoom.Uid == 87 && ObjData.Name.ContainsAny(new string[] { "fog", "mist" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 45;
			}
			else if (ActorRoom.Uid == 88 && ObjData.Name.ContainsAny(new string[] { "pick", "marks" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 46;
			}
			else if (ActorRoom.Uid == 89 && ObjData.Name.ContainsAny(new string[] { "tapestries", "tapestry" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 47;
			}
			else if ((ActorRoom.Uid == 90 || ActorRoom.Uid == 93) && ObjData.Name.ContainsAny(new string[] { "mining", "tool" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 48;
			}
			else if (ActorRoom.Uid == 95 && ObjData.Name.ContainsAny(new string[] { "skeletal", "arm" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 49;
			}
			else if (ActorRoom.Uid == 96 && ObjData.Name.ContainsAny(new string[] { "pit", "hole" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 50;
			}
			else if (ActorRoom.Uid == 101 && ObjData.Name.Contains("skeleton", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 51;
			}
			else if (ActorRoom.Uid == 102 && ObjData.Name.ContainsAny(new string[] { "stain", "blood" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 52;
			}
			else if (ActorRoom.Uid == 103 && ObjData.Name.ContainsAny(new string[] { "etching", "carving" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 53;
			}
			else if (ActorRoom.Uid == 104 && ObjData.Name.ContainsAny(new string[] { "face", "mouth", "hole" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 54;
			}
			else if (ActorRoom.Uid == 105 && ObjData.Name.ContainsAny(new string[] { "pile", "bodies", "body" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 55;
			}
			else if (ActorRoom.Uid == 108 && ObjData.Name.ContainsAny(new string[] { "beach", "sea", "ocean" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 56;
			}
			else if (ActorRoom.Uid == 13 && ObjData.Name.Contains("pictograph", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 58;
			}
			else if (ActorRoom.Uid == 100 && ObjData.Name.Contains("rune", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 59;
			}
			else if (ActorRoom.Uid == 110 && ObjData.Name.ContainsAny(new string[] { "message", "wall" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 60;
			}
			else if (gActorRoom(this).IsFenceRoom() && ObjData.Name.Contains("fence", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 2;
			}
			else if (gActorRoom(this).IsGroundsRoom() && ObjData.Name.ContainsAny(new string[] { "foliage", "trees", "weeds", "plants", "grass", "lichen", "moss" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 3;
			}
			else if (gActorRoom(this).IsGroundsRoom() && !gActorRoom(this).IsSwampRoom() && ActorRoom.Uid != 18 && ObjData.Name.Contains("forest", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 3;
			}
			else if (gActorRoom(this).IsGroundsRoom() && !gActorRoom(this).IsSwampRoom() && ActorRoom.Uid != 16 && ActorRoom.Uid != 23 && ActorRoom.Uid != 39 && ActorRoom.Uid != 118 && ActorRoom.Uid != 119 && ActorRoom.Uid != 120 && ActorRoom.Uid != 121 && ObjData.Name.ContainsAny(new string[] { "tombstone", "gravestone" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 4;
			}
			else if (gActorRoom(this).IsCryptRoom() && ActorRoom.Uid != 91 && ActorRoom.Uid != 93 && ActorRoom.Uid != 97 && ObjData.Name.ContainsAny(new string[] { "floor", "dust" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 14;
			}
			else if (gActorRoom(this).IsBodyChamberRoom() && ObjData.Name.ContainsAny(new string[] { "body", "bodies", "internment", "opening", "chamber" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 16;
			}
			else if (gActorRoom(this).IsGroundsRoom() && !gActorRoom(this).IsSwampRoom() && ActorRoom.Uid != 16 && ActorRoom.Uid != 23 && ActorRoom.Uid != 39 && ActorRoom.Uid != 118 && ActorRoom.Uid != 119 && ActorRoom.Uid != 120 && ActorRoom.Uid != 121 && ObjData.Name.Contains("epitaph", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 57;
			}
			else if (gActorRoom(this).IsRainyRoom() && ObjData.Name.Contains("rain", StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 61;
			}
			else if (gActorRoom(this).IsFoggyRoom() && ObjData.Name.ContainsAny(new string[] { "fog", "mist", "haze" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field1 = 62;
			}

			// Read decorations

			if (ActorRoom.Uid == 13 && ObjData.Name.Contains("pictograph", StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 2;
			}
			else if (ActorRoom.Uid == 62 && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 3;
			}
			else if ((ActorRoom.Uid == 64 || ActorRoom.Uid == 65) && ObjData.Name.ContainsAny(new string[] { "rune", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 4;
			}
			else if (ActorRoom.Uid == 75 && ObjData.Name.ContainsAny(new string[] { "glyph", "writing", "inscription" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 5;
			}
			else if (ActorRoom.Uid == 100 && ObjData.Name.Contains("rune", StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 6;
			}
			else if (ActorRoom.Uid == 103 && ObjData.Name.ContainsAny(new string[] { "etching", "carving" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 7;
			}
			else if (ActorRoom.Uid == 110 && ObjData.Name.ContainsAny(new string[] { "message", "wall" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 8;
			}
			else if (gActorRoom(this).IsGroundsRoom() && !gActorRoom(this).IsSwampRoom() && ActorRoom.Uid != 16 && ActorRoom.Uid != 23 && ActorRoom.Uid != 39 && ActorRoom.Uid != 118 && ActorRoom.Uid != 119 && ActorRoom.Uid != 120 && ActorRoom.Uid != 121 && ObjData.Name.ContainsAny(new string[] { "tombstone", "gravestone", "epitaph" }, StringComparison.OrdinalIgnoreCase))
			{
				a.Field2 = 1;
			}

			if (a.Field1 > 0 || a.Field2 > 0)
			{
				a.Name = gEngine.CloneInstance(ObjData.Name);

				// Make note of the Decoration so it can be used later if the normal Artifact resolution process fails

				ObjData.Cast<Framework.Parsing.IParserData>().DecorationArtifact = a;
			}
		}

		public override void CheckPlayerCommand(bool afterFinishParsing)
		{
			Debug.Assert(NextCommand != null);

			base.CheckPlayerCommand(afterFinishParsing);

			if (afterFinishParsing)
			{
				// Restrict various commands while paralyzed

				if (!(NextCommand is ISmileCommand || (NextCommand.Type == CommandType.Miscellaneous && !(NextCommand is ISpeedCommand || NextCommand is IPowerCommand))) && gGameState.ParalyzedTargets.ContainsKey(gGameState.Cm))
				{
					gOut.Print("You can't do that while paralyzed!");

					NextState = gEngine.CreateInstance<IStartState>();
				}

				// Restrict GiveCommand and RequestCommand when targeting paralyzed Monster

				else if ((NextCommand is IGiveCommand || NextCommand is IRequestCommand) && IobjMonster != null && gGameState.ParalyzedTargets.ContainsKey(IobjMonster.Uid))
				{
					gOut.Print("You can't do that while {0} {1} paralyzed!", IobjMonster.GetTheName(), IobjMonster.EvalPlural("is", "are"));

					NextState = gEngine.CreateInstance<IStartState>();
				}

				// Restrict SearchCommand while enemies are present

				else if (NextCommand is ISearchCommand && gGameState.GetNBTL(Friendliness.Enemy) > 0)
				{
					NextCommand.PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}

				// Restrict Commands in the graveyard at night or in heavy fog

				else if ((NextCommand is IReadCommand || NextCommand is ISearchCommand) && gActorRoom(this).IsDimLightRoomWithoutGlowingMonsters() && gGameState.Ls <= 0)
				{
					gOut.Print("You'll need a bit more light for that!");

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					var waterWeirdMonster = gMDB[38];

					Debug.Assert(waterWeirdMonster != null);

					// Large fountain and water weird

					if (DobjArtifact != null && DobjArtifact.Uid != 24 && DobjArtifact.Uid != 40 && IobjArtifact != null && IobjArtifact.Uid == 24)
					{
						if (waterWeirdMonster.IsInRoom(ActorRoom))
						{
							gOut.Print("{0} won't let you get close enough to do that!", waterWeirdMonster.GetTheName(true));

							NextState = gEngine.CreateInstance<IMonsterStartState>();
						}
						else if (!gGameState.WaterWeirdKilled)
						{
							gEngine.PrintEffectDesc(100);

							waterWeirdMonster.SetInRoom(ActorRoom);

							NextState = gEngine.CreateInstance<IStartState>();
						}
					}
				}
			}
		}
	}
}
