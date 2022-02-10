
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Eamon.ThirdParty;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game
{
	[ClassMappings(typeof(Eamon.Framework.IEngine))]
	public class Engine : EamonDD.Game.Engine, Framework.IEngine
	{
		public virtual long StartRoom { get; set; }

		public virtual long NumSaveSlots { get; set; }

		public virtual long ScaledHardinessUnarmedMaxDamage { get; set; }

		public virtual double ScaledHardinessMaxDamageDivisor { get; set; }

		public virtual bool EnforceMonsterWeightLimits { get; set; }

		public virtual bool UseMonsterScaledHardinessValues { get; set; }

		public virtual bool AutoDisplayUnseenArtifactDescs { get; set; }

		public virtual bool ExposeContainersRecursively { get; set; }

		public virtual PoundCharPolicy PoundCharPolicy { get; set; }

		public virtual PercentCharPolicy PercentCharPolicy { get; set; }

		public virtual void PrintPlayerRoom()
		{
			var room = gRDB[gGameState.Ro];

			Debug.Assert(room != null);

			Globals.Buf.Clear();

			var rc = room.IsLit() ? room.BuildPrintedFullDesc(Globals.Buf, verboseRoomDesc: gGameState.Vr, verboseMonsterDesc: gGameState.Vm, verboseArtifactDesc: gGameState.Va) : room.BuildPrintedTooDarkToSeeDesc(Globals.Buf);

			Debug.Assert(IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);
		}

		public virtual void PrintToWhom()
		{
			gOut.Write("{0}To whom? ", Environment.NewLine);
		}

		public virtual void PrintFromWhom()
		{
			gOut.Write("{0}From whom? ", Environment.NewLine);
		}

		public virtual void PrintVerbWhoOrWhat(ICommand command)
		{
			Debug.Assert(command != null);

			gOut.Write("{0}{1} who or what? ", Environment.NewLine, command.Verb.FirstCharToUpper());
		}

		public virtual void PrintVerbPrepWhoOrWhat(ICommand command)
		{
			Debug.Assert(command != null);

			gOut.Write("{0}{1} {2}who or what? ", Environment.NewLine, command.Verb.FirstCharToUpper(), command.IsDobjPrepEnabled && command.Prep != null && Enum.IsDefined(typeof(ContainerType), command.Prep.ContainerType) ? EvalContainerType(command.Prep.ContainerType, "inside ", "on ", "under ", "behind ") : "");
		}

		public virtual void PrintFromPrepWhat(ICommand command)
		{
			Debug.Assert(command != null);

			gOut.Write("{0}From {1}what? ", Environment.NewLine, Enum.IsDefined(typeof(ContainerType), command.ContainerType) ? EvalContainerType(command.ContainerType, "inside ", "on ", "under ", "behind ") : "");
		}

		public virtual void PrintPutObjPrepWhat(ICommand command, IArtifact artifact)
		{
			Debug.Assert(command != null && artifact != null);

			gOut.Write("{0}Put {1} {2} what? ", Environment.NewLine, artifact.EvalPlural("it", "them"), Enum.IsDefined(typeof(ContainerType), command.ContainerType) ? EvalContainerType(command.ContainerType, "inside", "on", "under", "behind") : "in");
		}

		public virtual void PrintUseObjOnWhoOrWhat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Write("{0}Use {1} on who or what? ", Environment.NewLine, artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} {1} alive!", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
		}

		public virtual void PrintLightOut(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} goes out.", artifact.GetTheName(true));
		}

		public virtual void PrintDeadBodyComesToLife(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} {1}", artifact.GetTheName(true), Globals.IsRulesetVersion(5, 15, 25) ? "comes alive!" : "comes to life!");
		}

		public virtual void PrintArtifactVanishes(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} vanish{1}!", artifact.GetTheName(true), artifact.EvalPlural("es", ""));
		}

		public virtual void PrintEnterExtinguishLightChoice(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Write("{0}It's not dark here.  Extinguish {1} (Y/N): ", Environment.NewLine, artifact.GetTheName());
		}

		public virtual void PrintArtifactIsWorth(IArtifact artifact, long goldAmount)
		{
			Debug.Assert(artifact != null);

			if (goldAmount > 0)
			{
				Globals.Buf01.SetFormat("{0} gold piece{1}", goldAmount, goldAmount != 1 ? "s" : "");
			}
			else
			{
				Globals.Buf01.SetFormat("nothing");
			}

			var ac = artifact.Drinkable;

			gOut.Write("{0}{1}{2} {3} worth {4}.",
				Environment.NewLine,
				artifact.GetTheName(true, false),
				ac != null && ac.Field2 < 1 && !artifact.Name.Contains("empty", StringComparison.OrdinalIgnoreCase) ? " (empty)" : "",
				artifact.EvalPlural("is", "are"),
				Globals.Buf01);
		}

		public virtual void PrintNothingHappens()
		{
			gOut.Print("Nothing happens.");
		}

		public virtual void PrintFullDesc(IArtifact artifact, bool showName)
		{
			Debug.Assert(artifact != null);

			Globals.Buf.Clear();

			var rc = artifact.BuildPrintedFullDesc(Globals.Buf, showName);

			Debug.Assert(IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);
		}

		public virtual void PrintMonsterCantFindExit(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0}", monster.GetCantFindExitDescString(room, monsterName, isPlural, fleeing));
		}

		public virtual void PrintMonsterMembersExitRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0}", monster.GetMembersExitRoomDescString(room, monsterName, isPlural, fleeing));
		}

		public virtual void PrintMonsterExitsRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing, Direction direction)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0}", monster.GetExitRoomDescString(room, monsterName, isPlural, fleeing, direction));
		}

		public virtual void PrintMonsterEntersRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing, Direction direction)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0}", monster.GetEnterRoomDescString(room, monsterName, isPlural, fleeing, direction));
		}

		public virtual void PrintMonsterGetsAngry(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			gOut.Write("{0}{1} get{2} angry!{3}",
				Environment.NewLine,
				monster.GetTheName(true),
				monster.EvalPlural("s", ""),
				printFinalNewLine ? Environment.NewLine : "");
		}

		public virtual void PrintMonsterEmotes(IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(monster != null);

			if (Globals.IsRulesetVersion(5, 15, 25) && monster.Reaction == Friendliness.Friend)
			{
				gOut.Write("{0}{1} {2}{3} back.",
					Environment.NewLine,
					monster.GetTheName(true),
					monster.EvalReaction("growl", "ignore", friendSmile ? "smile" : "wave"),
					monster.EvalPlural("s", ""));
			}
			else
			{
				gOut.Write("{0}{1} {2}{3} {4}you{5}",
					Environment.NewLine,
					monster.GetTheName(true),
					monster.EvalReaction("growl", "ignore", friendSmile ? "smile" : "wave"),
					monster.EvalPlural("s", ""),
					monster.Reaction != Friendliness.Neutral ? "at " : "",
					Globals.IsRulesetVersion(5, 15, 25) && monster.Reaction == Friendliness.Enemy ? "!" : ".");
			}
		}

		public virtual void PrintFullDesc(IMonster monster, bool showName)
		{
			Debug.Assert(monster != null);

			Globals.Buf.Clear();

			var rc = monster.BuildPrintedFullDesc(Globals.Buf, showName);

			Debug.Assert(IsSuccess(rc));

			gOut.Write("{0}", Globals.Buf);
		}

		public virtual void PrintHealthImproves(IMonster monster)
		{
			Debug.Assert(monster != null);

			var isCharMonster = monster.IsCharacterMonster();

			if (Globals.IsRulesetVersion(5, 15, 25))
			{
				gOut.Print("Some of {0} wounds seem to clear up.",
					isCharMonster ? "your" :
					monster.EvalPlural(monster.GetTheName(), monster.GetArticleName(false, true, false, true, Globals.Buf01)).AddPossessiveSuffix());
			}
			else
			{
				gOut.Print("{0} health improves!",
					isCharMonster ? "Your" :
					monster.EvalPlural(monster.GetTheName(true), monster.GetArticleName(true, true, false, true, Globals.Buf01)).AddPossessiveSuffix());
			}
		}

		public virtual void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters)
		{
			Debug.Assert(monster != null);

			var isCharMonster = monster.IsCharacterMonster();

			var isUninjuredGroupMonster = includeUninjuredGroupMonsters && monster.CurrGroupCount > 1 && monster.DmgTaken == 0;

			Globals.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				isCharMonster ? "You" :
				isUninjuredGroupMonster ? "They" :
				monster.GetTheName(true, true, false, true, Globals.Buf01),
				isCharMonster || isUninjuredGroupMonster ? "are" : "is");

			monster.AddHealthStatus(Globals.Buf);

			gOut.Write("{0}", Globals.Buf);
		}

		public virtual void PrintDoesntHaveIt(IMonster monster)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0}{1} have it.", monster.GetTheName(true), monster.EvalPlural(" doesn't", " don't"));
		}

		public virtual void PrintSpellOverloadsBrain(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s) && spell != null);

			gOut.Print("The strain of attempting to cast {0} overloads your brain and you forget it completely{1}.", spell.Name, Globals.IsRulesetVersion(5, 15, 25) ? "" : " for the rest of this adventure");
		}

		public virtual void PrintSpellAbilityIncreased(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s) && spell != null);

			gOut.Print("Your ability to cast {0} just increased!", spell.Name);
		}

		public virtual void PrintSpellCastFailed(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s) && spell != null);

			gOut.Print("Nothing happens.");
		}

		public virtual void PrintTooManyWeapons()
		{
			gOut.Print("As you enter the Main Hall, Lord William Missilefire approaches you and says, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		public virtual void PrintDeliverGoods()
		{
			gOut.Print("You deliver your goods to Sam Slicker, the local buyer for such things.  He examines your items and pays you what they are worth.");
		}

		public virtual void PrintYourWeaponsAre()
		{
			gOut.WriteLine("{0}Your weapons are:{0}{1}", Environment.NewLine, Globals.Buf);
		}

		public virtual void PrintEnterWeaponToSell()
		{
			gOut.Write("{0}Enter the number of a weapon to sell: ", Environment.NewLine);
		}

		public virtual void PrintAllWoundsHealed()
		{
			gOut.Print("All of your wounds are healed.");
		}

		public virtual void PrintYouHavePerished()
		{
			gOut.Print("You have perished.  Now what?");
		}

		public virtual void PrintRestoreSavedGame()
		{
			gOut.Write("{0} 1. Restore a saved game", Environment.NewLine);
		}

		public virtual void PrintStartOver()
		{
			gOut.Write("{0} 2. Start over (saved games will be deleted)", Environment.NewLine);
		}

		public virtual void PrintAcceptDeath()
		{
			gOut.Print(" 3. Give up, accept death");
		}

		public virtual void PrintEnterDeadMenuChoice()
		{
			gOut.Write("{0}Your choice: ", Environment.NewLine);
		}

		public virtual void PrintWakingUpMonsters()
		{
			gOut.WriteLine("Please wait a short while (waking up the monsters...)");
		}

		public virtual void PrintBaseProgramVersion()
		{
			gOut.WriteLine("[Base Program {0}]", Constants.RtProgVersion);
		}

		public virtual void PrintWelcomeToEamonCS()
		{
			gOut.Print("Welcome to the Eamon CS fantasy gaming system!");
		}

		public virtual void PrintWelcomeBack()
		{
			gOut.Print("Welcome back to {0}!", Globals.Module.Name);
		}

		public virtual void PrintEnterSeeIntroStoryChoice()
		{
			gOut.Write("{0}Would you like to see the introduction story again (Y/N) [N]: ", Environment.NewLine);
		}

		public virtual void PrintEnterWeaponNumberChoice()
		{
			gOut.Write("{0}Press the number of the weapon to select: ", Environment.NewLine);
		}

		public virtual void PrintNoIntroStory()
		{
			gOut.Print("There is no introduction story for this adventure.");
		}

		public virtual void PrintSavedGamesDeleted()
		{
			gOut.Print("Your saved games have been deleted.");
		}

		public virtual void PrintRestartGameUsingResume()
		{
			gOut.Print("You can restart the game by running the Resume[GameName] .bat or .sh file.");
		}

		public virtual void PrintMemorialService()
		{
			gOut.Print("When word of your untimely passing reaches the Main Hall, all your friends gather for a round at the bar.  They honor you with tales of your great deeds and with one last toast.  If you were there you'd be surprised to see the burly Irishman tear up.");
		}

		public virtual void PrintSavedGames()
		{
			gOut.Print("Saved games:");
		}

		public virtual void PrintSaveSlot(long saveSlot, string saveName, bool printFinalNewLine = false)
		{
			Debug.Assert(saveSlot > 0 && saveName != null);

			gOut.Write("{0}{1,3}. {2}{3}", Environment.NewLine, saveSlot, saveName, printFinalNewLine ? Environment.NewLine : "");
		}

		public virtual void PrintQuickSave(long saveSlot, string saveName)
		{
			Debug.Assert(saveSlot > 0 && saveName != null);

			gOut.Print("[QUICK SAVE {0}: {1}]", saveSlot, saveName);
		}

		public virtual void PrintUsingSlotInstead(long saveSlot)
		{
			Debug.Assert(saveSlot > 0);

			gOut.Print("[Using #{0} instead.]", saveSlot);
		}

		public virtual void PrintEnterSaveSlotChoice(long numMenuItems)
		{
			Debug.Assert(numMenuItems > 0);

			gOut.Write("{0}Enter 1-{1} for saved position: ", Environment.NewLine, numMenuItems);
		}

		public virtual void PrintEnterRestoreSlotChoice(long numMenuItems)
		{
			Debug.Assert(numMenuItems > 0);

			gOut.Write("{0}Your choice (1-{1}): ", Environment.NewLine, numMenuItems);
		}

		public virtual void PrintChangingHim(string himStr)
		{
			Debug.Assert(himStr != null);

			gOut.Print("{{Changing him:  \"{0}\"}}", himStr);
		}

		public virtual void PrintChangingHer(string herStr)
		{
			Debug.Assert(herStr != null);

			gOut.Print("{{Changing her:  \"{0}\"}}", herStr);
		}

		public virtual void PrintChangingIt(string itStr)
		{
			Debug.Assert(itStr != null);

			gOut.Print("{{Changing it:  \"{0}\"}}", itStr);
		}

		public virtual void PrintChangingThem(string themStr)
		{
			Debug.Assert(themStr != null);

			gOut.Print("{{Changing them:  \"{0}\"}}", themStr);
		}

		public virtual void PrintDiscardMessage(string inputStr)
		{
			Debug.Assert(inputStr != null);

			gOut.Print("{{Discarding:  \"{0}\"}}", inputStr);
		}

		public virtual void PrintGoodsPayment(bool goodsExist, long goldAmount)
		{
			gOut.Print("{0}He pays you {1} gold piece{2} total.", goodsExist ? Environment.NewLine : "", goldAmount, goldAmount != 1 ? "s" : "");
		}

		public virtual void PrintMacroReplacedPagedString(string str, StringBuilder buf)
		{
			Debug.Assert(str != null && buf != null);

			buf.Clear();

			var rc = ResolveUidMacros(str, buf, true, true);

			Debug.Assert(IsSuccess(rc));

			gOut.WriteLine();

			var pages = buf.ToString().Split(new string[] { Constants.PageSep }, StringSplitOptions.RemoveEmptyEntries);

			for (var i = 0; i < pages.Length; i++)
			{
				if (i > 0)
				{
					gOut.WriteLine("{0}{1}{0}", Environment.NewLine, Globals.LineSep);
				}

				gOut.Write("{0}", pages[i]);

				if (i < pages.Length - 1)
				{
					gOut.WriteLine();

					Globals.In.KeyPress(buf);
				}
			}

			gOut.WriteLine();
		}

		public virtual void BuildRevealContentsListDescString(IArtifact artifact, IList<IArtifact> revealContentsList, ContainerType containerType, bool? revealShowCharOwned, bool showCharOwned)
		{
			Debug.Assert(artifact != null && revealContentsList != null && revealContentsList.Count > 0 && Enum.IsDefined(typeof(ContainerType), containerType));

			Globals.Buf.SetFormat("{0}{1} {2} you find ",
				Environment.NewLine,
				EvalContainerType(containerType, "Inside", "On", "Under", "Behind"),
				artifact.GetTheName(false, showCharOwned, false, false, Globals.Buf01));

			var rc = GetRecordNameList(revealContentsList.Cast<IGameBase>().ToList(), ArticleType.A, revealShowCharOwned != null ? (bool)revealShowCharOwned : false, StateDescDisplayCode.None, false, false, Globals.Buf);

			Debug.Assert(IsSuccess(rc));

			Globals.Buf.AppendFormat(".{0}", Environment.NewLine);
		}

		public virtual long WeaponPowerCompare(IArtifact artifact1, IArtifact artifact2)
		{
			Debug.Assert(artifact1 != null && artifact2 != null);

			var ac1 = artifact1.GeneralWeapon;

			Debug.Assert(ac1 != null);

			var ac2 = artifact2.GeneralWeapon;

			Debug.Assert(ac2 != null);

			var result1 = ac1.Field3 * ac1.Field4;

			var result2 = ac2.Field3 * ac2.Field4;

			return result1 > result2 ? 1 : result1 < result2 ? -1 : 0;
		}

		public virtual long WeaponPowerCompare(long artifactUid1, long artifactUid2)
		{
			return WeaponPowerCompare(gADB[artifactUid1], gADB[artifactUid2]);
		}

		public virtual IArtifact GetMostPowerfulWeapon(IList<IArtifact> artifactList)
		{
			IArtifact cw = null;

			Debug.Assert(artifactList != null);

			foreach (var artifact in artifactList)
			{
				var ac = artifact.GeneralWeapon;

				if (ac != null && (cw == null || WeaponPowerCompare(artifact, cw) > 0))
				{
					cw = artifact;

					Debug.Assert(cw.Uid > 0);
				}
			}

			return cw;
		}

		public virtual long GetMostPowerfulWeaponUid(IList<IArtifact> artifactList)
		{
			Debug.Assert(artifactList != null);

			var cw = GetMostPowerfulWeapon(artifactList);

			return cw != null ? cw.Uid : 0;     // Note: -1 not returned!
		}

		public virtual void EnforceCharacterWeightLimits()
		{
			Globals.RevealContentCounter--;

			var artifactList = GetArtifactList(a => a.IsCarriedByCharacter() || a.IsWornByCharacter()).OrderByDescending(a => a.RecursiveWeight).ToList();

			foreach (var artifact in artifactList)
			{
				if (artifact.IsWornByCharacter())
				{
					if (artifact.Wearable == null || artifact.Wearable.Field1 > 0)
					{
						artifact.SetCarriedByCharacter();
					}
				}
				
				Debug.Assert(!artifact.IsUnmovable01());

				var charWeight = 0L;

				var rc = gCharacter.GetFullInventoryWeight(ref charWeight, recurse: true);

				Debug.Assert(IsSuccess(rc));

				if (charWeight > gCharacter.GetWeightCarryableGronds())
				{
					artifact.SetInRoomUid(StartRoom);
				}
				else
				{
					break;
				}
			}

			Globals.RevealContentCounter++;
		}

		public virtual void EnforceCharacterWeightLimits02(IRoom room = null, bool printOutput = false)
		{
			Globals.RevealContentCounter--;

			var enableOutput = gOut.EnableOutput;

			gOut.EnableOutput = printOutput;

			if (room == null)
			{
				room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);
			}

			var artifactList = GetArtifactList(a => a.IsCarriedByCharacter() || a.IsWornByCharacter()).OrderByDescending(a => a.RecursiveWeight).ToList();

			foreach (var artifact in artifactList)
			{
				Debug.Assert(!artifact.IsUnmovable01());

				var charWeight = 0L;

				var rc = gCharMonster.GetFullInventoryWeight(ref charWeight, recurse: true);

				Debug.Assert(IsSuccess(rc));

				if (charWeight > gCharMonster.GetWeightCarryableGronds())
				{
					if (artifact.IsWornByCharacter())
					{
						var removeCommand = Globals.CreateInstance<IRemoveCommand>(x =>
						{
							x.ActorMonster = gCharMonster;

							x.ActorRoom = room;

							x.Dobj = artifact;
						});

						removeCommand.Execute();
					}

					var dropCommand = Globals.CreateInstance<IDropCommand>(x =>
					{
						x.ActorMonster = gCharMonster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					dropCommand.Execute();
				}
				else
				{
					break;
				}
			}

			gOut.EnableOutput = enableOutput;

			Globals.RevealContentCounter++;
		}

		public virtual void AddUniqueCharsToArtifactAndMonsterNames()
		{
			var recordList = new List<IGameBase>();

			var artifactList = PoundCharPolicy == PoundCharPolicy.PlayerArtifactsOnly ? Globals.Database.ArtifactTable.Records.Where(a => a.IsCharOwned).ToList() :
									PoundCharPolicy == PoundCharPolicy.AllArtifacts ? Globals.Database.ArtifactTable.Records.ToList() :
									new List<IArtifact>();

			artifactList.Reverse();

			var i = 0;

			while (i < artifactList.Count && artifactList[i].IsCharOwned)
			{
				i++;
			}

			if (i > 0)
			{
				artifactList.Reverse(0, i);
			}

			if (artifactList.Count > i)
			{
				artifactList.Reverse(i, artifactList.Count - i);
			}

			recordList.AddRange(artifactList);

			var monsterList = PercentCharPolicy == PercentCharPolicy.AllMonsters ? Globals.Database.MonsterTable.Records.ToList() :
									new List<IMonster>();

			recordList.AddRange(monsterList);

			AddUniqueCharsToRecordNames(recordList);
		}

		public virtual void AddMissingDescs()
		{
			var monsterList = GetMonsterList(m => string.IsNullOrWhiteSpace(m.Desc) || m.Desc.Equals("NONE", StringComparison.OrdinalIgnoreCase));

			foreach (var monster in monsterList)
			{
				monster.Desc = string.Format("{0} {1}.", monster.EvalPlural("This is", "These are"), monster.GetArticleName());
			}

			var artifactList = GetArtifactList(a => string.IsNullOrWhiteSpace(a.Desc) || a.Desc.Equals("NONE", StringComparison.OrdinalIgnoreCase));

			foreach (var artifact in artifactList)
			{
				artifact.Desc = string.Format("{0} {1}.", artifact.EvalPlural("This is", "These are"), artifact.GetArticleName());
			}
		}

		public virtual void InitSaArray()
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				var i = (long)sv;

				gGameState.SetSa(i, gCharacter.GetSpellAbilities(i));
			}
		}

		public virtual void CreateCommands()
		{
			var commands = Globals.ClassMappingsDictionary.Keys.Where(x => x.GetInterfaces().Contains(typeof(ICommand)));

			foreach (var command in commands)
			{
				if (Globals.Module.NumDirs == 12 || !(command.IsSameOrSubclassOf(typeof(INeCommand)) || command.IsSameOrSubclassOf(typeof(INwCommand)) || command.IsSameOrSubclassOf(typeof(ISeCommand)) || command.IsSameOrSubclassOf(typeof(ISwCommand)) || command.IsSameOrSubclassOf(typeof(IInCommand)) || command.IsSameOrSubclassOf(typeof(IOutCommand))))
				{
					var command01 = Globals.CreateInstance<ICommand>(command);

					if (command01 != null && !string.IsNullOrWhiteSpace(command01.Verb))
					{
						Globals.CommandList.Add(command01);
					}
				}
			}

			Globals.CommandList = Globals.CommandList.OrderBy(x => x.SortOrder).ToList();
		}

		public virtual void InitArtifacts()
		{
			var artifactList = Globals.Database.ArtifactTable.Records.ToList();

			foreach (var artifact in artifactList)
			{
				TruncatePluralTypeEffectDesc(artifact.PluralType, Constants.ArtNameLen);
			}
		}

		public virtual void InitMonsters()
		{
			if (UseMonsterScaledHardinessValues)
			{
				InitMonsterScaledHardinessValues();
			}

			var monsterList = Globals.Database.MonsterTable.Records.ToList();

			foreach (var monster in monsterList)
			{
				monster.InitGroupCount = monster.GroupCount;

				monster.CurrGroupCount = monster.GroupCount;

				monster.ResolveReaction(gCharacter);

				if (EnforceMonsterWeightLimits && !monster.IsCharacterMonster())
				{
					var rc = monster.EnforceFullInventoryWeightLimits(recurse: true);

					Debug.Assert(IsSuccess(rc));
				}

				if (monster.Weapon > 0)
				{
					var artifact = gADB[monster.Weapon];

					if (artifact != null)
					{
						artifact.AddStateDesc(artifact.GetReadyWeaponDesc());
					}
				}

				TruncatePluralTypeEffectDesc(monster.PluralType, Constants.MonNameLen);
			}
		}

		public virtual void InitMonsterScaledHardinessValues()
		{
			var maxDamage = ScaledHardinessUnarmedMaxDamage;

			Debug.Assert(gCharMonster != null);

			if (gCharMonster.Weapon > 0)       // will always be most powerful weapon
			{
				var artifact = gADB[gCharMonster.Weapon];

				Debug.Assert(artifact != null);

				var ac = artifact.GeneralWeapon;

				Debug.Assert(ac != null);

				maxDamage = ac.Field3 * ac.Field4;
			}

			var damageFactor = (long)Math.Round((double)maxDamage / ScaledHardinessMaxDamageDivisor);

			if (damageFactor < 1)
			{
				damageFactor = 1;
			}

			var monsterList = Globals.Database.MonsterTable.Records.ToList();

			foreach (var m in monsterList)
			{
				SetScaledHardiness(m, damageFactor);
			}
		}

		public virtual IArtifact ConvertWeaponToArtifact(ICharacterArtifact weapon)
		{
			RetCode rc;

			Debug.Assert(weapon != null);

			var artifact = Globals.CreateInstance<IArtifact>(x =>
			{
				x.SetArtifactCategoryCount(1);

				x.Uid = Globals.Database.GetArtifactUid();

				x.Name = weapon.Name.Trim().TrimEnd('#');

				Debug.Assert(!string.IsNullOrWhiteSpace(x.Name));

				x.Desc = !string.IsNullOrWhiteSpace(weapon.Desc) ? Globals.CloneInstance(weapon.Desc) : null;

				x.Seen = true;

				x.IsCharOwned = true;

				x.IsListed = true;

				x.IsPlural = weapon.IsPlural;

				x.PluralType = weapon.PluralType;

				x.ArticleType = weapon.ArticleType;

				x.GetCategories(0).Field1 = weapon.Field1;

				x.GetCategories(0).Field2 = weapon.Field2;

				x.GetCategories(0).Field3 = weapon.Field3;

				x.GetCategories(0).Field4 = weapon.Field4;

				x.GetCategories(0).Field5 = weapon.Field5;

				if (weapon.Type != 0)
				{
					x.GetCategories(0).Type = weapon.Type;

					x.Value = weapon.Value;

					x.Weight = weapon.Weight;
				}
				else
				{
					var d = weapon.Field3 * weapon.Field4;

					x.GetCategories(0).Type = (weapon.Field1 >= 15 || d >= 25) ? ArtifactType.MagicWeapon : ArtifactType.Weapon;

					var imw = false;

					x.Value = (long)GetWeaponPriceOrValue(weapon, false, ref imw);

					x.Weight = 15;
				}

				var charWeight = 0L;

				rc = gCharacter.GetFullInventoryWeight(ref charWeight, recurse: true);

				Debug.Assert(IsSuccess(rc));

				if (x.Weight + charWeight <= gCharacter.GetWeightCarryableGronds())
				{
					x.SetCarriedByCharacter();
				}
				else
				{
					x.SetInRoomUid(StartRoom);
				}
			});

			rc = Globals.Database.AddArtifact(artifact);

			Debug.Assert(IsSuccess(rc));

			gGameState.SetImportedArtUids(gGameState.ImportedArtUidsIdx++, artifact.Uid);

			return artifact;
		}

		public virtual ICharacterArtifact ConvertArtifactToWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.GeneralWeapon;

			Debug.Assert(ac != null);

			var weapon = Globals.CreateInstance<ICharacterArtifact>(x =>
			{
				x.Name = artifact.Name.Trim().TrimEnd('#');

				Debug.Assert(!string.IsNullOrWhiteSpace(x.Name));

				if (!string.IsNullOrWhiteSpace(artifact.Desc))
				{
					Globals.Buf.Clear();

					var rc = ResolveUidMacros(artifact.Desc, Globals.Buf, true, true);

					Debug.Assert(IsSuccess(rc));

					if (Globals.Buf.Length <= Constants.CharArtDescLen)
					{
						x.Desc = Globals.CloneInstance(Globals.Buf.ToString());
					}
				}

				x.IsPlural = artifact.IsPlural;

				x.PluralType = artifact.PluralType;

				x.ArticleType = artifact.ArticleType;

				x.Value = artifact.Value;

				x.Weight = artifact.Weight;

				x.Type = ac.Type;

				x.Field1 = ac.Field1;

				x.Field2 = ac.Field2;

				x.Field3 = ac.Field3;

				x.Field4 = ac.Field4;

				x.Field5 = ac.Field5;
			});

			return weapon;
		}

		public virtual IMonster ConvertArtifactToMonster(IArtifact artifact, Action<IMonster> initialize = null, bool addToDatabase = false)
		{
			Debug.Assert(artifact != null);

			var monster = Globals.CreateInstance<IMonster>(x =>
			{
				x.Uid = Globals.Database.GetMonsterUid();

				x.IsUidRecycled = true;

				x.Name = Globals.CloneInstance(artifact.Name);

				x.Seen = artifact.Seen;

				x.ArticleType = artifact.ArticleType;

				x.StateDesc = Globals.CloneInstance(artifact.StateDesc);

				x.Desc = Globals.CloneInstance(artifact.Desc);

				x.IsListed = artifact.IsListed;

				x.PluralType = artifact.PluralType;

				x.Hardiness = 16;

				x.Agility = 15;

				x.GroupCount = 1;

				x.AttackCount = 1;

				x.Courage = 100;

				x.Location = artifact.Location;

				x.CombatCode = CombatCode.Weapons;

				x.NwDice = 1;

				x.NwSides = 4;

				x.Friendliness = (Friendliness)200;

				x.Gender = Gender.Male;

				x.CurrGroupCount = 1;

				x.Reaction = Friendliness.Friend;
			});

			if (initialize != null)
			{
				initialize(monster);
			}

			if (addToDatabase)
			{
				var rc = Globals.Database.AddMonster(monster);

				Debug.Assert(IsSuccess(rc));
			}

			return monster;
		}

		public virtual IMonster ConvertCharacterToMonster()
		{
			var monster = Globals.CreateInstance<IMonster>(x =>
			{
				x.Uid = Globals.Database.GetMonsterUid();

				x.Name = gCharacter.Name.Trim();

				x.Desc = string.Format("You are the {0} {1}.", gCharacter.EvalGender("mighty", "fair", "androgynous"), gCharacter.Name);

				x.Hardiness = gCharacter.GetStats(Stat.Hardiness);

				x.Agility = gCharacter.GetStats(Stat.Agility);

				x.GroupCount = 1;

				x.AttackCount = 1;

				x.CurrGroupCount = 1;

				x.Armor = ConvertArmorToArtifacts();

				x.Weapon = ConvertWeaponsToArtifacts();

				x.NwDice = 1;

				x.NwSides = 2;

				x.Friendliness = (Friendliness)200;

				x.Gender = gCharacter.Gender;

				x.Reaction = Friendliness.Friend;
			});

			var rc = Globals.Database.AddMonster(monster);

			Debug.Assert(IsSuccess(rc));

			return monster;
		}

		public virtual void ConvertMonsterToCharacter(IMonster monster, IList<IArtifact> weaponList)
		{
			Debug.Assert(monster != null && weaponList != null);

			ResetMonsterStats(monster);

			gCharacter.Name = monster.Name.Trim();

			gCharacter.SetStats(Stat.Hardiness, monster.Hardiness);

			gCharacter.SetStats(Stat.Agility, monster.Agility);

			gCharacter.Gender = monster.Gender;

			for (var i = 0; i < gCharacter.Weapons.Length; i++)
			{
				gCharacter.SetWeapons(i, (i < weaponList.Count ? ConvertArtifactToWeapon(weaponList[i]) : Globals.CreateInstance<ICharacterArtifact>()));

				gCharacter.GetWeapons(i).Parent = gCharacter;
			}

			gCharacter.AddUniqueCharsToWeaponNames();

			gOut.Print("{0}", Globals.LineSep);
		}

		public virtual void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			if (gGameState.Speed > 0)
			{
				monster.Agility /= 2;
			}

			gGameState.Speed = 0;
		}

		public virtual void SetArmorClass()
		{
			var artUids = new long[] { gGameState.Ar, gGameState.Sh };

			gCharacter.ArmorClass = Armor.SkinClothes;

			gCharacter.Armor = Globals.CreateInstance<ICharacterArtifact>(x =>
			{
				x.Parent = gCharacter;
			});

			gCharacter.Shield = Globals.CreateInstance<ICharacterArtifact>(x =>
			{
				x.Parent = gCharacter;
			});

			foreach (var artUid in artUids)
			{
				if (artUid > 0)
				{
					var artifact = gADB[artUid];

					Debug.Assert(artifact != null);

					var ac = artifact.Wearable;

					Debug.Assert(ac != null);

					gCharacter.ArmorClass += ac.Field1;

					var ca = (artUid == gGameState.Ar) ? gCharacter.Armor : gCharacter.Shield;

					ca.Name = artifact.Name.Trim().TrimEnd('#');

					Debug.Assert(!string.IsNullOrWhiteSpace(ca.Name));

					if (!string.IsNullOrWhiteSpace(artifact.Desc))
					{
						Globals.Buf.Clear();

						var rc = ResolveUidMacros(artifact.Desc, Globals.Buf, true, true);

						Debug.Assert(IsSuccess(rc));

						if (Globals.Buf.Length <= Constants.CharArtDescLen)
						{
							ca.Desc = Globals.CloneInstance(Globals.Buf.ToString());
						}
					}

					ca.IsPlural = artifact.IsPlural;

					ca.PluralType = artifact.PluralType;

					ca.ArticleType = artifact.ArticleType;

					ca.Value = artifact.Value;

					ca.Weight = artifact.Weight;

					ca.Type = ac.Type;

					ca.Field1 = ac.Field1;

					ca.Field2 = ac.Field2;

					ca.Field3 = ac.Field3;

					ca.Field4 = ac.Field4;

					ca.Field5 = ac.Field5;

					artifact.SetInLimbo();
				}
			}

			// gGameState.Ar = 0;

			// gGameState.Sh = 0;
		}

		public virtual void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			long c;

			Debug.Assert(weaponList != null);

			weaponList.Clear();

			var artifactList = GetArtifactList(a => a.IsWornByCharacter());

			foreach (var artifact in artifactList)
			{
				artifact.SetCarriedByCharacter();
			}

			do
			{
				c = 0;

				artifactList = GetArtifactList(a => a.IsCarriedByCharacter());

				foreach (var artifact in artifactList)
				{
					var ac = artifact.GeneralContainer;

					if (ac != null)
					{
						var artifactList01 = GetArtifactList(a => a.IsCarriedByContainer(artifact));

						foreach (var artifact01 in artifactList01)
						{
							if (artifact01.Seen == true || artifact01.GetCarriedByContainerContainerType() != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed())
							{
								artifact01.SetCarriedByCharacter();

								c = 1;
							}
						}
					}
				}
			}
			while (c == 1);

			artifactList = Globals.Database.ArtifactTable.Records.ToList();

			foreach (var artifact in artifactList)
			{
				artifact.Seen = false;

				if (artifact.IsCarriedByCharacter())
				{
					var ac = artifact.GeneralWeapon;

					if (ac != null && ac == artifact.GetCategories(0) && artifact.IsReadyableByCharacter())
					{
						weaponList.Add(artifact);

						artifact.SetInLimbo();
					}
				}
			}

			if (weaponList.Count > gCharacter.Weapons.Length)
			{
				gOut.Print("{0}", Globals.LineSep);
			}
		}

		public virtual void SellExcessWeapons(IList<IArtifact> weaponList)
		{
			Debug.Assert(weaponList != null);

			if (weaponList.Count > gCharacter.Weapons.Length)
			{
				PrintTooManyWeapons();

				while (weaponList.Count > gCharacter.Weapons.Length)
				{
					gOut.Print("{0}", Globals.LineSep);

					Globals.Buf.Clear();

					var rc = ListRecords(weaponList.Cast<IGameBase>().ToList(), true, true, Globals.Buf);

					Debug.Assert(IsSuccess(rc));

					PrintYourWeaponsAre();

					gOut.Print("{0}", Globals.LineSep);

					PrintEnterWeaponToSell();

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize01, null, ' ', '\0', false, null, ModifyCharToUpper, IsCharDigit, null);

					Debug.Assert(IsSuccess(rc));

					Globals.Thread.Sleep(150);

					var m = Convert.ToInt64(Globals.Buf.Trim().ToString());

					if (m >= 1 && m <= weaponList.Count)
					{
						weaponList[(int)m - 1].SetCarriedByCharacter();

						weaponList.RemoveAt((int)m - 1);
					}
				}
			}
		}

		public virtual void SellInventoryToMerchant(bool sellInventory = true)
		{
			var c = 0L;

			var w = 0L;

			PrintDeliverGoods();

			if (sellInventory)
			{
				var c2 = gCharacter.GetMerchantAdjustedCharisma();

				var rtio = GetMerchantRtio(c2);

				var artifactList = GetArtifactList(a => a.IsCarriedByCharacter());

				foreach (var artifact in artifactList)
				{
					var m = artifact.Gold != null ? artifact.Value : GetMerchantBidPrice(artifact.Value, rtio);

					if (m < 0)
					{
						m = 0;
					}

					PrintArtifactIsWorth(artifact, m);

					w = w + m;

					c = 1;
				}

				gCharacter.HeldGold += w;
			}

			PrintGoodsPayment(c == 1, w);

			Globals.In.KeyPress(Globals.Buf);
		}

		public virtual void DeadMenu(IMonster monster, bool printLineSep, ref bool restoreGame)
		{
			Debug.Assert(monster != null);

			restoreGame = false;

			ResetMonsterStats(monster);

			if (printLineSep)
			{
				gOut.Print("{0}", Globals.LineSep);
			}

			PrintYouHavePerished();

			PrintRestoreSavedGame();

			PrintStartOver();

			PrintAcceptDeath();

			gOut.Print("{0}", Globals.LineSep);

			PrintEnterDeadMenuChoice();

			Globals.Buf.Clear();

			var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, ModifyCharToUpper, IsChar1To3, null);

			Debug.Assert(IsSuccess(rc));

			var i = Convert.ToInt64(Globals.Buf.Trim().ToString());

			if (i == 3)
			{
				Globals.ExitType = ExitType.GoToMainHall;

				Globals.MainLoop.ShouldShutdown = false;
			}
			else if (i == 2)
			{
				Globals.ExitType = ExitType.StartOver;

				Globals.MainLoop.ShouldShutdown = false;
			}
			else
			{
				restoreGame = true;
			}
		}

		public virtual void LightOut(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

			Debug.Assert(IsSuccess(rc));

			gGameState.Ls = 0;

			PrintLightOut(artifact);
		}

		public virtual void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			if (monster.Reaction > Friendliness.Enemy)
			{
				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				if (!Enum.IsDefined(typeof(Friendliness), monster.Friendliness))
				{
					monster.Friendliness -= 100;

					monster.Friendliness = (Friendliness)((long)monster.Friendliness / 2);

					monster.Friendliness += 100;
				}
				else
				{
					monster.Friendliness--;
				}

				monster.Reaction--;

				if (monster.IsInRoom(room) && monster.Reaction == Friendliness.Enemy)
				{
					PrintMonsterGetsAngry(monster, printFinalNewLine);
				}
			}
		}

		public virtual void MonsterDies(IMonster ActorMonster, IMonster DobjMonster)
		{
			RetCode rc;

			// ActorMonster may be null or non-null

			Debug.Assert(DobjMonster != null && !DobjMonster.IsCharacterMonster());

			var room = DobjMonster.GetInRoom();

			Debug.Assert(room != null);

			if (DobjMonster.CurrGroupCount > 1)
			{
				if (DobjMonster.Weapon > 0)
				{
					var weapon = GetNthArtifact(DobjMonster.GetCarriedList(), DobjMonster.CurrGroupCount - 1, a => a.IsReadyableByMonster(DobjMonster) && a.Uid != DobjMonster.Weapon);

					if (weapon != null)
					{
						weapon.SetInRoom(room);
					}
				}

				DobjMonster.CurrGroupCount--;

				DobjMonster.DmgTaken = 0;

				if (EnforceMonsterWeightLimits)
				{
					rc = DobjMonster.EnforceFullInventoryWeightLimits(recurse: true);

					Debug.Assert(IsSuccess(rc));
				}
			}
			else
			{
				if (DobjMonster.Weapon > 0)
				{
					var weapon = gADB[DobjMonster.Weapon];

					Debug.Assert(weapon != null);

					rc = weapon.RemoveStateDesc(weapon.GetReadyWeaponDesc());

					Debug.Assert(IsSuccess(rc));

					DobjMonster.Weapon = -1;
				}

				DobjMonster.SetInLimbo();

				DobjMonster.CurrGroupCount = DobjMonster.GroupCount;

				// DobjMonster.ResolveReaction(gCharacter);

				DobjMonster.DmgTaken = 0;

				var artifactList = GetArtifactList(a => a.IsCarriedByMonster(DobjMonster) || a.IsWornByMonster(DobjMonster));

				foreach (var artifact in artifactList)
				{
					artifact.SetInRoom(room);
				}

				ProcessMonsterDeathEvents(DobjMonster);

				if (DobjMonster.DeadBody > 0)
				{
					var deadBody = gADB[DobjMonster.DeadBody];

					Debug.Assert(deadBody != null);

					if (!deadBody.IsCharOwned)
					{
						deadBody.SetInRoom(room);
					}
				}
			}
		}

		public virtual void ProcessMonsterDeathEvents(IMonster monster)
		{
			Debug.Assert(monster != null && !monster.IsCharacterMonster());

			// --> Add effects of monster's death here
		}

		public virtual void RevealDisguisedMonster(IRoom room, IArtifact artifact)
		{
			RetCode rc;

			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			var ac = artifact.DisguisedMonster;

			Debug.Assert(ac != null);

			PrintMonsterAlive(artifact);

			if (ac.Field2 > 0)
			{
				for (var i = 0; i < ac.Field3; i++)
				{
					var effect = gEDB[ac.Field2 + i];

					if (effect != null)
					{
						Globals.Buf.Clear();

						rc = effect.BuildPrintedFullDesc(Globals.Buf);
					}
					else
					{
						Globals.Buf.SetPrint("{0}", "???");

						rc = RetCode.Success;
					}

					Debug.Assert(IsSuccess(rc));

					gOut.Write("{0}", Globals.Buf);
				}
			}

			artifact.SetInLimbo();

			var monster = gMDB[ac.Field1];

			Debug.Assert(monster != null);

			monster.SetInRoomUid(gGameState.Ro);
		}

		public virtual void RevealEmbeddedArtifact(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			var shouldShowUnseenArtifacts = false;

			// move an embedded artifact into the room

			if (artifact.IsEmbeddedInRoom(room))
			{
				artifact.SetInRoom(room);

				var ac = artifact.DoorGate;

				if (ac != null)
				{
					ac.Field4 = 0;
				}

				shouldShowUnseenArtifacts = true;
			}

			if (!shouldShowUnseenArtifacts)
			{
				shouldShowUnseenArtifacts = AutoDisplayUnseenArtifactDescs;
			}

			if (!shouldShowUnseenArtifacts)
			{
				var command = gCommandParser.NextState as ICommand;

				shouldShowUnseenArtifacts = command != null && command.ShouldShowUnseenArtifacts(room, artifact);
			}

			// fully describe an unseen artifact

			if (shouldShowUnseenArtifacts && !artifact.Seen)
			{
				PrintFullDesc(artifact, false);

				artifact.Seen = true;
			}
		}

		public virtual void RevealContainerContents(IRoom room, long revealContentListIndex, ContainerType[] containerTypes, IList<string> containerContentsList = null)
		{
			RetCode rc;

			Debug.Assert(room != null);

			Debug.Assert(revealContentListIndex >= 0 && revealContentListIndex < Globals.RevealContentArtifactList.Count);

			if (containerTypes == null || containerTypes.Length < 1)
			{
				containerTypes = new ContainerType[] { ContainerType.Under, ContainerType.Behind };
			}

			var artifact = Globals.RevealContentArtifactList[(int)revealContentListIndex];

			Debug.Assert(artifact != null);

			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			var showCharOwned = !artifact.IsCarriedByCharacter() && !artifact.IsWornByCharacter();

			bool? revealShowCharOwned = null;

			foreach (var containerType in containerTypes)
			{
				var ac = EvalContainerType(containerType, artifact.InContainer, artifact.OnContainer, artifact.UnderContainer, artifact.BehindContainer);

				if (ac != null)
				{
					var contentsList = artifact.GetContainedList(containerType: containerType);

					var revealContentsList = new List<IArtifact>();

					foreach (var revealArtifact in contentsList)
					{
						revealContentsList.Add(revealArtifact);

						revealArtifact.Location = Globals.RevealContentLocationList[(int)revealContentListIndex];

						if (revealShowCharOwned == null)
						{
							revealShowCharOwned = !revealArtifact.IsCarriedByCharacter() && !revealArtifact.IsWornByCharacter();
						}

						var monster = revealArtifact.GetCarriedByMonster();

						if (monster == null)
						{
							monster = revealArtifact.GetWornByMonster();
						}

						var revealContainer = revealArtifact.GetCarriedByContainer();

						var revealContainerType = revealArtifact.GetCarriedByContainerContainerType();

						var revealContainerAc = revealContainer != null && Enum.IsDefined(typeof(ContainerType), revealContainerType) ? EvalContainerType(revealContainerType, revealContainer.InContainer, revealContainer.OnContainer, revealContainer.UnderContainer, revealContainer.BehindContainer) : null;

						if (revealArtifact.IsCarriedByCharacter() || revealArtifact.IsWornByCharacter())
						{
							var charWeight = 0L;

							rc = charMonster.GetFullInventoryWeight(ref charWeight, recurse: true);

							Debug.Assert(IsSuccess(rc));

							var revealArtifactTooHeavy = charWeight > charMonster.GetWeightCarryableGronds();

							if (revealArtifact.IsWornByCharacter())
							{
								if (revealArtifact.Wearable == null || revealArtifactTooHeavy)
								{
									revealArtifact.SetCarriedByCharacter();
								}
							}

							if (revealArtifact.IsCarriedByCharacter())
							{
								if (revealArtifactTooHeavy)
								{
									revealArtifact.SetInRoom(room);
								}
							}
						}
						else if (monster != null)
						{
							var artCount = 0L;

							var artWeight = revealArtifact.Weight;

							if (revealArtifact.GeneralContainer != null)
							{
								rc = revealArtifact.GetContainerInfo(ref artCount, ref artWeight, (ContainerType)(-1), true);

								Debug.Assert(IsSuccess(rc));
							}

							var monWeight = 0L;

							rc = monster.GetFullInventoryWeight(ref monWeight, recurse: true);

							Debug.Assert(IsSuccess(rc));

							var revealArtifactTooHeavy = EnforceMonsterWeightLimits && (artWeight > monster.GetWeightCarryableGronds() || monWeight > monster.GetWeightCarryableGronds() * monster.CurrGroupCount);

							if (revealArtifact.IsWornByMonster())
							{
								if (revealArtifact.Wearable == null || revealArtifactTooHeavy)
								{
									revealArtifact.SetCarriedByMonster(monster);
								}
							}

							if (revealArtifact.IsCarriedByMonster())
							{
								if (revealArtifactTooHeavy)
								{
									revealArtifact.SetInRoom(room);
								}
							}
						}
						else if (revealContainer != null && revealContainerAc != null)
						{
							var count = 0L;

							var weight = 0L;

							rc = revealContainer.GetContainerInfo(ref count, ref weight, revealContainerType, false);

							Debug.Assert(IsSuccess(rc));

							if (count > revealContainerAc.Field4 || weight > revealContainerAc.Field3)
							{
								revealArtifact.SetInRoom(room);
							}
						}
						else if (revealArtifact.IsEmbeddedInRoom())
						{
							if (artifact.IsInLimbo())
							{
								revealArtifact.SetInRoom(revealArtifact.GetEmbeddedInRoom());
							}
							else
							{
								revealArtifact.SetCarriedByContainer(artifact, containerType);

								revealContentsList.Remove(revealArtifact);
							}
						}
						else if (revealArtifact.IsInLimbo())
						{
							revealArtifact.SetInRoom(room);
						}
					}

					if (revealContentsList.Count > 0 && containerContentsList != null)
					{
						BuildRevealContentsListDescString(artifact, revealContentsList, containerType, revealShowCharOwned, showCharOwned);

						containerContentsList.Add(Globals.Buf.ToString());
					}
				}
			}
		}

		public virtual IArtifact GetBlockedDirectionArtifact(long ro, long r2, Direction dir)
		{
			return null;
		}

		public virtual ICommand GetCommandUsingToken(IMonster monster, string token, bool synonymMatch = true, bool partialMatch = true)
		{
			Debug.Assert(monster != null);

			Debug.Assert(!string.IsNullOrWhiteSpace(token));

			var command = Globals.CommandList.FirstOrDefault(x => x.Verb != null && x.Verb.Equals(token, StringComparison.OrdinalIgnoreCase) && x.IsEnabled(monster));

			if (command == null && synonymMatch)
			{
				command = Globals.CommandList.FirstOrDefault(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => s.Equals(token, StringComparison.OrdinalIgnoreCase)) != null && x.IsEnabled(monster));
			}

			if (command == null && partialMatch)
			{
				command = Globals.CommandList.FirstOrDefault(x => x.Verb != null && (x.Verb.StartsWith(token, StringComparison.OrdinalIgnoreCase) || x.Verb.EndsWith(token, StringComparison.OrdinalIgnoreCase)) && x.IsEnabled(monster));
			}

			if (command == null && synonymMatch && partialMatch)
			{
				command = Globals.CommandList.FirstOrDefault(x => x.Synonyms != null && x.Synonyms.FirstOrDefault(s => s.StartsWith(token, StringComparison.OrdinalIgnoreCase) || s.EndsWith(token, StringComparison.OrdinalIgnoreCase)) != null && x.IsEnabled(monster));
			}

			return command;
		}

		public virtual void CheckDoor(IRoom room, IArtifact artifact, ref bool found, ref long roomUid)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			found = false;

			roomUid = 0;

			if (!artifact.IsCharOwned)
			{
				var ac = artifact.DoorGate;

				if (ac != null)
				{
					if (artifact.IsInRoom(room) || artifact.IsEmbeddedInRoom(room) || artifact.IsCarriedByContainerContainerTypeExposedToRoom(room))
					{
						found = true;
					}
				}

				if (found)
				{
					if (artifact.Seen && room.IsLit())
					{
						ac.Field4 = 0;
					}

					if (ac.Field4 != 0)
					{
						found = false;
					}
				}

				if (found && ac.IsOpen())
				{
					roomUid = ac.Field1;
				}
			}
		}

		public virtual void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits)
		{
			Debug.Assert(room != null);

			Debug.Assert(monster != null);

			numExits = 0;

			var directionValues = EnumUtil.GetValues<Direction>();

			for (var i = 0; i < Globals.Module.NumDirs; i++)
			{
				var dv = directionValues[i];

				var found = false;

				var roomUid = 0L;

				var artUid = room.GetDirectionDoorUid(dv);

				if (artUid > 0)
				{
					var artifact = gADB[artUid];

					Debug.Assert(artifact != null);

					CheckDoor(room, artifact, ref found, ref roomUid);
				}
				else
				{
					roomUid = room.GetDirs(dv);
				}

				if (roomUid != 0 && (!monster.CanMoveToRoomUid(roomUid, fleeing) || GetBlockedDirectionArtifact(room.Uid, roomUid, dv) != null))
				{
					roomUid = 0;
				}

				if (IsValidRoomUid01(roomUid) && (monster.IsCharacterMonster() || (roomUid > 0 && gRDB[roomUid] != null)))
				{
					numExits++;
				}
			}
		}

		public virtual void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction, ref bool found, ref long roomUid)
		{
			long rl;

			Debug.Assert(room != null);

			Debug.Assert(room.Dirs.Count(x => x != 0 && x != room.Uid) > 0);

			Debug.Assert(monster != null);

			direction = 0;

			do
			{
				rl = RollDice(1, Globals.Module.NumDirs, 0);

				found = false;

				roomUid = 0;

				var artUid = room.GetDirectionDoorUid((Direction)rl);

				if (artUid > 0)
				{
					var artifact = gADB[artUid];

					Debug.Assert(artifact != null);

					CheckDoor(room, artifact, ref found, ref roomUid);
				}
				else
				{
					roomUid = room.GetDirs(rl);
				}

				if (roomUid != 0 && (!monster.CanMoveToRoomUid(roomUid, fleeing) || GetBlockedDirectionArtifact(room.Uid, roomUid, (Direction)rl) != null))
				{
					roomUid = 0;
				}
			}
			while (roomUid == 0 || roomUid == room.Uid || IsValidRoomDirectionDoorUid01(roomUid) || (!monster.IsCharacterMonster() && (roomUid < 1 || gRDB[roomUid] == null)));

			direction = (Direction)rl;
		}

		public virtual void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction)
		{
			var found = false;

			var roomUid = 0L;

			GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);
		}

		public virtual void MoveMonsterToRandomAdjacentRoom(IRoom room, IMonster monster, bool fleeing, bool callSleep, bool printOutput = true)
		{
			RetCode rc;

			Debug.Assert(room != null);

			Debug.Assert(monster != null);

			Debug.Assert(gCharMonster != null);

			var numExits = 0L;

			CheckNumberOfExits(room, monster, fleeing, ref numExits);

			var rl = fleeing ? monster.GetFleeingMemberCount() : monster.CurrGroupCount;

			var monster01 = Globals.CloneInstance(monster);

			Debug.Assert(monster01 != null);

			monster01.CurrGroupCount = rl;

			var monsterName = monster01.EvalInRoomLightLevel(rl > 1 ? "Unseen entities" : "An unseen entity", monster01.InitGroupCount > rl ? monster01.GetArticleName(true) : monster01.GetTheName(true));

			if (numExits == 0)
			{
				if (gCharMonster.IsInRoom(room) && printOutput)
				{
					PrintMonsterCantFindExit(monster, room, monsterName, rl > 1, fleeing);

					if (callSleep)
					{
						Globals.Thread.Sleep(gGameState.PauseCombatMs);
					}
				}

				if (rl < monster.CurrGroupCount)
				{
					monster.CurrGroupCount -= rl;

					Globals.LoopFailedMoveMemberCount = rl;
				}

				goto Cleanup;
			}

			if (rl < monster.CurrGroupCount)
			{
				monster.CurrGroupCount -= rl;

				if (gCharMonster.IsInRoom(room) && printOutput)
				{
					PrintMonsterMembersExitRoom(monster, room, monsterName, rl > 1, fleeing);

					if (callSleep)
					{
						Globals.Thread.Sleep(gGameState.PauseCombatMs);
					}
				}

				if (EnforceMonsterWeightLimits)
				{
					rc = monster.EnforceFullInventoryWeightLimits(recurse: true);

					Debug.Assert(IsSuccess(rc));
				}
			}
			else
			{
				Direction direction = 0;

				var found = false;

				var roomUid = 0L;

				GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);

				Debug.Assert(Enum.IsDefined(typeof(Direction), direction));

				Debug.Assert(roomUid > 0);

				if (gCharMonster.IsInRoom(room) && printOutput)
				{
					PrintMonsterExitsRoom(monster, room, monsterName, rl > 1, fleeing, direction);

					if (callSleep)
					{
						Globals.Thread.Sleep(gGameState.PauseCombatMs);
					}
				}

				monster.Location = roomUid;

				var room01 = gRDB[roomUid];

				Debug.Assert(room01 != null);

				var monsterName01 = monster.EvalInRoomLightLevel(rl > 1 ? "Unseen entities" : "An unseen entity", monster.GetArticleName(true));

				var direction01 = GetDirections(direction);

				Debug.Assert(direction01 != null);

				if (gCharMonster.IsInRoom(room01) && printOutput)
				{
					PrintMonsterEntersRoom(monster, room01, monsterName01, rl > 1, fleeing, direction01.EnterDir);

					if (callSleep)
					{
						Globals.Thread.Sleep(gGameState.PauseCombatMs);
					}
				}
			}

		Cleanup:

			;
		}

		public virtual IList<IMonster> GetRandomMonsterList(long numMonsters, params Func<IMonster, bool>[] whereClauseFuncs)
		{
			Debug.Assert(numMonsters > 0);

			var monsterList = new List<IMonster>();

			var origMonsterList = GetMonsterList(whereClauseFuncs);

			if (numMonsters > origMonsterList.Count)
			{
				numMonsters = origMonsterList.Count;
			}

			while (numMonsters > 0)
			{
				var rl = (int)RollDice(1, origMonsterList.Count, 0);

				monsterList.Add(origMonsterList[rl - 1]);

				origMonsterList.RemoveAt(rl - 1);

				numMonsters--;
			}

			return monsterList;
		}

		public virtual IList<IGameBase> FilterRecordList(IList<IGameBase> recordList, string name)
		{
			Debug.Assert(recordList != null);

			Debug.Assert(!string.IsNullOrWhiteSpace(name));

			var name01 = Globals.CloneInstance(name);

			var tokens = name01.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			long i = -1;

			if (tokens.Length > 1)
			{
				i = GetNumberFromString(tokens[0]);

				if (i > 0)
				{
					name01 = name01.Substring(tokens[0].Length + 1);
				}
				else
				{
					i = -1;
				}
			}

			var filteredRecordList = recordList.Where(r => 
			{
				return r.Name.Equals(r is IArtifact ? name : name01, StringComparison.OrdinalIgnoreCase);
			
			}).ToList();

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r => 
				{
					var result = false;

					if (r is IArtifact a)
					{
						result = a.IsPlural && a.GetPluralName01(Globals.Buf).Equals(name, StringComparison.OrdinalIgnoreCase);
					}
					else if (r is IMonster m)
					{
						result = m.GroupCount > 1 && m.GetPluralName01(Globals.Buf).Equals(name01, StringComparison.OrdinalIgnoreCase);
					}

					return result;

				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r => 
				{
					return r.Synonyms != null && r.Synonyms.FirstOrDefault(s => s.Equals(r is IArtifact ? name : name01, StringComparison.OrdinalIgnoreCase)) != null;

				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					return r.Name.StartsWith(r is IArtifact ? name : name01, StringComparison.OrdinalIgnoreCase) || r.Name.EndsWith(r is IArtifact ? name : name01, StringComparison.OrdinalIgnoreCase);
				
				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r => 
				{
					var result = false;

					if (r is IArtifact a)
					{
						result = a.IsPlural && (a.GetPluralName01(Globals.Buf).StartsWith(name, StringComparison.OrdinalIgnoreCase) || a.GetPluralName01(Globals.Buf01).EndsWith(name, StringComparison.OrdinalIgnoreCase));
					}
					else if (r is IMonster m)
					{
						result = m.GroupCount > 1 && (m.GetPluralName01(Globals.Buf).StartsWith(name01, StringComparison.OrdinalIgnoreCase) || m.GetPluralName01(Globals.Buf01).EndsWith(name01, StringComparison.OrdinalIgnoreCase));
					}

					return result;

				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r => 
				{
					return r.Synonyms != null && r.Synonyms.FirstOrDefault(s => s.StartsWith(r is IArtifact ? name : name01, StringComparison.OrdinalIgnoreCase) || s.EndsWith(r is IArtifact ? name : name01, StringComparison.OrdinalIgnoreCase)) != null;
				
				}).ToList();
			}

			filteredRecordList = filteredRecordList.Distinct().GroupBy(r =>
			{
				var result = r.Name.ToLower();

				if (r is IArtifact a && a.IsPlural)
				{
					result = a.GetPluralName01(Globals.Buf).ToLower();
				}
				else if (r is IMonster m && m.GroupCount > 1)
				{
					result = m.GetPluralName01(Globals.Buf).ToLower();
				}

				return result;

			}).Select(r => r.FirstOrDefault()).OrderBy(r => r is IArtifact ? 2 : 1).ToList();

			if (i > 0)
			{
				filteredRecordList.RemoveAll(r => r is IMonster m && (m.GroupCount == 1 || m.GroupCount < i));
			}

			return filteredRecordList;
		}

		public virtual IList<IArtifact> GetReadyableWeaponList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var monsterList = GetMonsterList(m => m.Uid != monster.Uid && m.Uid != charMonster.Uid && m.IsInRoom(room));

			var artifactList = GetArtifactList(a =>
			{
				var result = false;

				if (a.IsReadyableByMonster(monster))
				{
					if (a.IsCarriedByMonster(monster))
					{
						result = true;
					}
					else if (a.IsInRoom(room))
					{
						result = monsterList.FirstOrDefault(m => m.Weapon == -a.Uid - 1) == null && 
										(monster.Weapon == -a.Uid - 1 || a.Seen || !room.IsLit()) && 
										(charMonster.Weapon > 0 || !a.IsCharOwned || monster.Reaction == Friendliness.Friend);
					}
					else if (a.IsCarriedByContainerContainerTypeExposedToMonster(monster, ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(room, ExposeContainersRecursively))
					{
						result = !Globals.IsRulesetVersion(5) && 
										monsterList.FirstOrDefault(m => m.Weapon == -a.Uid - 1) == null && 
										(monster.Weapon == -a.Uid - 1 || a.GetCarriedByContainer().Seen || !room.IsLit()) && 
										(monster.Weapon == -a.Uid - 1 || a.Seen || !room.IsLit()) && 
										(charMonster.Weapon > 0 || !a.IsCharOwned || monster.Reaction == Friendliness.Friend);
					}
				}

				return result;

			}).OrderByDescending(a01 =>
			{
				if (monster.Weapon != -a01.Uid - 1)
				{
					var ac = a01.GeneralWeapon;

					Debug.Assert(ac != null);

					return ac.Field3 * ac.Field4;
				}
				else
				{
					return long.MaxValue;
				}
			}).ThenByDescending(a02 =>
			{
				if (a02.IsCarriedByMonster(monster))
				{
					return 2;
				}
				else if (a02.IsCarriedByMonster(monster, true))
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}).ToList();

			// filter out two-handed weapons if monster wearing shield

			var shield = monster.GetWornList().FirstOrDefault(a =>
			{
				var ac = a.Wearable;

				Debug.Assert(ac != null);

				return ac.Field1 == 1;
			});

			if (shield != null)
			{
				artifactList = artifactList.Where(a =>
				{
					var ac = a.GeneralWeapon;

					Debug.Assert(ac != null);

					return ac.Field5 < 2;
					
				}).ToList();
			}

			return artifactList;
		}

		public virtual IList<IMonster> GetHostileMonsterList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var monsterList =
					monster.Reaction == Friendliness.Friend ? GetMonsterList(m => m.Reaction == Friendliness.Enemy && m.IsInRoom(room) && m.IsAttackable(monster)) :
					monster.Reaction == Friendliness.Enemy ? GetMonsterList(m => m.Reaction == Friendliness.Friend && m.IsInRoom(room) && m.IsAttackable(monster)) :
					new List<IMonster>();

			return monsterList;
		}

		public virtual IList<IMonster> GetEmotingMonsterList(IRoom room, IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(room != null && monster != null);

			return room.IsLit() ? GetMonsterList(m => m.IsInRoom(room) && m != monster) : new List<IMonster>();
		}

		public virtual IList<IArtifact> BuildLoopWeaponArtifactList(IMonster monster)
		{
			Debug.Assert(monster != null);

			IList<IArtifact> artifactList = null;

			if (monster.CombatCode == CombatCode.NaturalWeapons && monster.Weapon <= 0)
			{
				artifactList = GetReadyableWeaponList(monster);

				if (artifactList != null && artifactList.Count > 0)
				{
					var wpnArtifact = artifactList[0];

					Debug.Assert(wpnArtifact != null);

					var ac = wpnArtifact.GeneralWeapon;

					Debug.Assert(ac != null);

					if (monster.Weapon != -wpnArtifact.Uid - 1 && monster.NwDice * monster.NwSides > ac.Field3 * ac.Field4)
					{
						artifactList = null;
					}
				}
			}
			else if ((monster.CombatCode == CombatCode.Weapons || monster.CombatCode == CombatCode.Attacks) && monster.Weapon < 0)
			{
				artifactList = GetReadyableWeaponList(monster);
			}

			return artifactList;
		}

		public virtual IList<IArtifact> GetImportedPlayerInventory()
		{
			var artifactList = new List<IArtifact>();

			if (gGameState != null)
			{
				for (var i = 0; i < gGameState.ImportedArtUidsIdx; i++)
				{
					var artifact = gADB[gGameState.GetImportedArtUids(i)];

					Debug.Assert(artifact != null);

					artifactList.Add(artifact);
				}
			}

			return artifactList;
		}

		public virtual void HideImportedPlayerInventory()
		{
			// Look up the player character

			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			// Grab a random Room - we'll use StartRoom here since it really doesn't matter

			var room = gRDB[StartRoom];

			Debug.Assert(room != null);

			// This queries the game database for all Artifacts brought in by the player character

			var artifactList = GetImportedPlayerInventory();

			// Suppress output to the console

			gOut.EnableOutput = false;

			foreach (var artifact in artifactList)
			{
				// If the Artifact is worn by the player character

				if (artifact.IsWornByCharacter())
				{
					// In order to keep the game state from getting messed up we have to simulate an actual RemoveCommand execution

					Globals.CurrState = Globals.CreateInstance<IRemoveCommand>(x =>
					{
						x.ActorMonster = charMonster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					// Execute the RemoveCommand... after this the Artifact will no longer be worn (and the game state will be properly updated)

					Globals.CurrCommand.Execute();
				}

				// In order to keep the game state from getting messed up we have to simulate an actual DropCommand execution

				Globals.CurrState = Globals.CreateInstance<IDropCommand>(x =>
				{
					x.ActorMonster = charMonster;

					x.ActorRoom = room;

					x.Dobj = artifact;
				});

				// Execute the DropCommand... after this the Artifact will no longer be carried (and the game state will be properly updated)

				Globals.CurrCommand.Execute();

				// Now we can move it into limbo without messing anything up

				artifact.SetInLimbo();
			}

			// Enable console output again

			gOut.EnableOutput = true;

			// We need to recreate the initial game state since we were creating and executing the fake Commands above

			CreateInitialState(false);
		}

		public virtual void RestoreImportedPlayerInventory()
		{
			// Look up the player character

			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			// grab a random Room - we'll use StartRoom here since it really doesn't matter

			var room = gRDB[StartRoom];

			Debug.Assert(room != null);

			// This queries the game database for all Artifacts brought in by the player character

			var artifactList = GetImportedPlayerInventory();

			// Suppress output to the console

			Globals.Out.EnableOutput = false;

			foreach (var artifact in artifactList)
			{
				// First make the Artifact carried by the player character... you don't need to put it in Room and then do
				// GetCommand because there isn't really any game state to mess up... but you could if you wanted to I guess
				// just to be safe

				artifact.SetCarriedByCharacter();

				// If the Artifact is Wearable, make sure it's worn (otherwise it will accidentally get sold to Sam Slicker)

				if (artifact.Wearable != null)
				{
					// In order to keep the game state from getting messed up we have to simulate an actual WearCommand execution

					Globals.CurrState = Globals.CreateInstance<IWearCommand>(x =>
					{
						x.ActorMonster = charMonster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					// Execute the WearCommand... after this the Artifact will be worn (and the game state will be properly updated)

					Globals.CurrCommand.Execute();
				}
			}

			// Enable console output again

			gOut.EnableOutput = true;

			// (no need to update Globals.CurrState to anything else since we're shutting down)
		}

		public virtual RetCode BuildCommandList(IList<ICommand> commands, CommandType cmdType, StringBuilder buf, ref bool newSeen)
		{
			StringBuilder buf02, buf03;
			RetCode rc;
			int i;

			if (commands == null || !Enum.IsDefined(typeof(CommandType), cmdType) || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf02 = new StringBuilder(Constants.BufSize);

			buf03 = new StringBuilder(Constants.BufSize);

			i = 0;

			foreach (var c in commands)
			{
				if (cmdType == CommandType.None || c.Type == cmdType)
				{
					i++;

					buf02.SetFormat("{0}{1}",
						c.GetPrintedVerb(),
						c.IsNew ? " (*)" : "");

					buf03.AppendFormat("{0,-15}{1}",
						buf02.ToString(),
						(i % 5) == 0 ? Environment.NewLine : "");

					if (c.IsNew)
					{
						newSeen = true;
					}
				}
			}

			buf.AppendFormat("{0}{1}{2}",
				Environment.NewLine,
				buf03.Length > 0 ? buf03.ToString() : "(None)",
				i == 0 || (i % 5) != 0 ? Environment.NewLine : "");

		Cleanup:

			return rc;
		}

		public virtual StringBuilder NormalizePlayerInput(StringBuilder buf)
		{
			Debug.Assert(buf != null);

			buf.SetFormat(" {0} ", buf.ToString().ToLower());

			// begin TODO: validate Artifact and Monster names don't contain these tokens (add to invalid name tokens array)

			buf.SetFormat("{0}", buf.ToString().Replace(" everything ", " all "));

			buf.SetFormat("{0}", buf.ToString().Replace(" except ", " but "));

			buf.SetFormat("{0}", buf.ToString().Replace(" excluding ", " but "));

			buf.SetFormat("{0}", buf.ToString().Replace(" omitting ", " but "));

			// end TODO

			foreach (var token in Constants.CommandSepTokens)
			{
				buf.SetFormat("{0}", buf.ToString().Replace(!Char.IsPunctuation(token[0]) ? " " + token + " " : token, " , "));
			}

			buf.SetFormat(" {0} ", Regex.Replace(buf.ToString(), @"\s+", " ").Trim());

			while (buf.ToString().IndexOf(" , , ") >= 0)
			{
				buf.SetFormat("{0}", buf.ToString().Replace(" , , ", " , "));
			}

			while (buf.ToString().StartsWith(" , "))
			{
				buf.SetFormat("{0}", buf.ToString().Substring(3));
			}

			while (buf.ToString().EndsWith(" , "))
			{
				buf.SetFormat("{0}", buf.ToString().Substring(0, buf.Length - 3));
			}

			return buf.Trim();
		}

		public virtual StringBuilder ReplacePrepositions(StringBuilder buf)
		{
			Debug.Assert(buf != null);

			buf.SetFormat(" {0} ", buf.ToString().ToLower());

			// begin TODO: validate Artifact and Monster names don't contain these tokens (add to invalid name tokens array)

			buf = buf.Replace(" in to ", " into ");

			buf = buf.Replace(" inside ", " in ");

			buf = buf.Replace(" from in ", " fromin ");

			buf = buf.Replace(" on to ", " onto ");

			buf = buf.Replace(" on top of ", " on ");

			buf = buf.Replace(" from on ", " fromon ");

			buf = buf.Replace(" below ", " under ").Replace(" beneath ", " under ").Replace(" underneath ", " under ");

			buf = buf.Replace(" from under ", " fromunder ");

			buf = buf.Replace(" in back of ", " behind ");

			buf = buf.Replace(" from behind ", " frombehind ");

			// end TODO

			return buf.Trim();
		}

		public virtual bool IsQuotedStringCommand(ICommand command)
		{
			Debug.Assert(command != null);

			// These Commands are free form and may contain quoted strings

			return command is ISaveCommand || command is ISayCommand;
		}

		public virtual bool ResurrectDeadBodies(IRoom room, params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			Debug.Assert(room != null);

			if (whereClauseFuncs == null || whereClauseFuncs.Length == 0)
			{
				whereClauseFuncs = new Func<IArtifact, bool>[]
				{
					a => (a.IsCarriedByCharacter() || a.IsInRoom(room)) && a.DeadBody != null
				};
			}

			var found = false;

			var artifactList = GetArtifactList(whereClauseFuncs);
			
			foreach (var artifact in artifactList)
			{
				var monster = Globals.Database.MonsterTable.Records.FirstOrDefault(m => m.DeadBody == artifact.Uid);

				if (monster != null && monster.GroupCount == 1)
				{
					monster.SetInRoom(room);

					monster.DmgTaken = 0;

					artifact.SetInLimbo();

					PrintDeadBodyComesToLife(artifact);

					found = true;
				}
			}
			
			return found;
		}

		public virtual bool MakeArtifactsVanish(IRoom room, params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			Debug.Assert(room != null);

			if (whereClauseFuncs == null || whereClauseFuncs.Length == 0)
			{
				whereClauseFuncs = new Func<IArtifact, bool>[]
				{
					a => a.IsInRoom(room) && !a.IsUnmovable()
				};
			}

			var artifactList = GetArtifactList(whereClauseFuncs);

			foreach (var artifact in artifactList)
			{
				artifact.SetInLimbo();

				PrintArtifactVanishes(artifact);
			}

			return artifactList.Count > 0;
		}

		public virtual bool CheckPlayerSpellCast(Spell spellValue, bool shouldAllowSkillGains)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), spellValue));

			var result = false;

			var rl = 0L;

			var s = spellValue;

			var spell = GetSpells(spellValue);

			Debug.Assert(spell != null);

			if (gGameState.GetSa(s) > 0 && gCharacter.GetSpellAbilities(s) > 0)
			{
				rl = RollDice(1, 100, 0);
			}

			if (rl == 100)
			{
				PlayerSpellCastBrainOverload(s, spell);

				goto Cleanup;
			}

			if (rl > 0 && rl < 95 && (rl < 5 || rl <= gGameState.GetSa(s)))
			{
				result = true;

				gGameState.SetSa(s, (long)((double)gGameState.GetSa(s) * .5 + 1));

				if (shouldAllowSkillGains)
				{
					rl = RollDice(1, 100, 0);

					rl += gCharacter.GetIntellectBonusPct();

					if (rl > gCharacter.GetSpellAbilities(s))
					{
						Globals.SpellSkillIncreaseFunc = () =>
						{
							if (!Globals.IsRulesetVersion(5, 15, 25))
							{
								PrintSpellAbilityIncreased(s, spell);
							}

							gCharacter.ModSpellAbilities(s, 2);

							if (gCharacter.GetSpellAbilities(s) > spell.MaxValue)
							{
								gCharacter.SetSpellAbilities(s, spell.MaxValue);
							}
						};
					}
				}
			}
			else
			{
				PrintSpellCastFailed(s, spell);

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		public virtual bool SaveThrow(Stat stat, long bonus = 0)
		{
			Debug.Assert(gCharMonster != null);

			var value = 0L;

			switch (stat)
			{
				case Stat.Hardiness:

					// This is the saving throw vs. opening doors, etc

					value = gCharMonster.Hardiness + bonus;

					break;

				case Stat.Agility:

					// This is the saving throw vs. avoiding traps, etc

					value = gCharMonster.Agility + bonus;

					break;

				case Stat.Intellect:

					// This is the saving throw vs. searching, etc

					value = gCharacter.GetStats(Stat.Intellect) + bonus;

					break;

				default:

					// This is the saving throw vs. death or magic

					value = (long)Math.Round((double)(gCharMonster.Agility + gCharacter.GetStats(Stat.Charisma) + gCharMonster.Hardiness) / 3.0) + bonus;

					break;
			}

			var rl = RollDice(1, 22, 2);

			return rl <= value;
		}
		public virtual void CheckToExtinguishLightSource()
		{
			Debug.Assert(gGameState.Ls > 0);

			var artifact = gADB[gGameState.Ls];

			Debug.Assert(artifact != null);

			var ac = artifact.LightSource;

			Debug.Assert(ac != null);

			if (ac.Field1 != -1)
			{
				PrintEnterExtinguishLightChoice(artifact);

				Globals.Buf.Clear();

				var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, ModifyCharToUpper, IsCharYOrN, null);

				Debug.Assert(IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

					Debug.Assert(IsSuccess(rc));

					gGameState.Ls = 0;
				}
			}
		}

		public virtual void TransportRoomContentsBetweenRooms(IRoom oldRoom, IRoom newRoom, bool includeEmbedded = true)
		{
			Debug.Assert(oldRoom != null);

			Debug.Assert(newRoom != null);

			var monsterList = GetMonsterList(m => m.IsInRoom(oldRoom)).ToList();

			foreach (var m in monsterList)
			{
				m.SetInRoom(newRoom);
			}

			var artifactList = GetArtifactList(a => a.IsInRoom(oldRoom)).ToList();

			foreach (var a in artifactList)
			{
				a.SetInRoom(newRoom);
			}

			if (includeEmbedded)
			{
				artifactList = GetArtifactList(a => a.IsEmbeddedInRoom(oldRoom)).ToList();

				foreach (var a in artifactList)
				{
					a.SetEmbeddedInRoom(newRoom);
				}
			}
		}

		// Note: this method should only be used when oldRoom and newRoom are logically equivalent

		public virtual void TransportPlayerBetweenRooms(IRoom oldRoom, IRoom newRoom, IEffect effect)
		{
			Debug.Assert(oldRoom != null);

			Debug.Assert(newRoom != null);

			var gameState = GetGameState();

			Debug.Assert(gameState != null);

			TransportRoomContentsBetweenRooms(oldRoom, newRoom);

			if (effect != null)
			{
				PrintEffectDesc(effect);
			}

			gameState.Ro = newRoom.Uid;

			gameState.R2 = gameState.Ro;
		}

		public virtual void CreateArtifactSynonyms(long artifactUid, params string[] synonyms)
		{
			Debug.Assert(synonyms != null && synonyms.Length > 0);

			var artifact = gADB[artifactUid];

			Debug.Assert(artifact != null);

			artifact.Synonyms = Globals.CloneInstance(synonyms);
		}

		public virtual void CreateMonsterSynonyms(long monsterUid, params string[] synonyms)
		{
			Debug.Assert(synonyms != null && synonyms.Length > 0);

			var monster = gMDB[monsterUid];

			Debug.Assert(monster != null);

			monster.Synonyms = Globals.CloneInstance(synonyms);
		}

		public virtual void GetOddsToHit(IMonster actorMonster, IMonster dobjMonster, IArtifactCategory ac, long af, ref long oddsToHit)
		{
			Debug.Assert(actorMonster != null);

			Debug.Assert(dobjMonster != null);

			Debug.Assert(ac == null || (ac != null && ac.IsWeapon01()));

			var x = actorMonster.Agility;

			var f = dobjMonster.Agility;

			var a = actorMonster.Armor;

			var d = dobjMonster.Armor;

			if (a > 8)
			{
				a = 8;
			}

			if (d > 8)
			{
				d = 8;
			}

			if (x > 30)
			{
				x = 30;
			}

			if (f > 30)
			{
				f = 30;
			}

			var odds = 50 + 2 * (x - f - a + d);

			if (ac != null)
			{
				d = ac.Field1;

				if (d > 50)
				{
					d = 50;
				}

				odds = (long)Math.Round((double)odds + ((double)d / 2.0));
			}

			if (actorMonster.IsCharacterMonster())
			{
				odds += ((af + gCharacter.ArmorExpertise) * (-af > gCharacter.ArmorExpertise ? 1 : 0));

				d = ac != null ? gCharacter.GetWeaponAbilities(ac.Field2) : 0;

				if (d > 122)
				{
					d = 122;
				}

				odds = (long)Math.Round((double)odds + ((double)d / 4.0));
			}

			oddsToHit = odds;
		}

		public virtual void CreateInitialState(bool printLineSep)
		{
			if (gGameState.Die != 1)
			{
				Globals.CurrState = Globals.CreateInstance<IAfterPlayerMoveState>();
			}
			else
			{
				Globals.CurrState = Globals.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = printLineSep;
				});
			}
		}

		public virtual void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			long rl = 0;

			if (whereClauseFuncs == null || whereClauseFuncs.Length == 0)
			{
				whereClauseFuncs = new Func<IMonster, bool>[]
				{
					m => !m.IsCharacterMonster() && m.Seen && m.Location == gGameState.R3
				};
			}

			var monsterList = GetMonsterList(whereClauseFuncs);

			foreach (var monster in monsterList)
			{
				if (monster.CanMoveToRoomUid(gGameState.Ro, false))
				{
					if (monster.Reaction == Friendliness.Enemy)
					{
						rl = RollDice(1, 100, 0);

						if (rl <= monster.Courage)
						{
							monster.Location = gGameState.Ro;
						}
					}
					else if (monster.Reaction == Friendliness.Friend)
					{
						monster.Location = gGameState.Ro;
					}
				}
			}
		}

		public virtual void RtProcessArgv(bool secondPass, ref bool nlFlag)
		{
			long i;

			for (i = 0; i < Globals.Argv.Length; i++)
			{
				if (Globals.Argv[i].Equals("--workingDirectory", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (Globals.Argv[i].Equals("--filePrefix", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (Globals.Argv[i].Equals("--ignoreMutex", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-im", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Globals.Argv[i].Equals("--disableValidation", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-dv", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Globals.Argv[i].Equals("--configFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && !secondPass)
					{
						Globals.ConfigFileName = Globals.Argv[i].Trim();
					}
				}
				else if (Globals.Argv[i].Equals("--filesetFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtFilesetFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--characterFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtCharacterFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--moduleFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-modfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtModuleFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--roomFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-rfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtRoomFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--artifactFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-afn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtArtifactFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--effectFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtEffectFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--monsterFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-monfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtMonsterFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--hintFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-hfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtHintFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--triggerFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-trgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtTriggerFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--scriptFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-sfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtScriptFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--gameStateFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-gsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length && secondPass)
					{
						Globals.Config.RtGameStateFileName = Globals.Argv[i].Trim();

						Globals.ConfigsModified = true;
					}
				}
				else if (Globals.Argv[i].Equals("--deleteGameState", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-dgs", StringComparison.OrdinalIgnoreCase))
				{
					if (secondPass)
					{
						Globals.DeleteGameStateFromMainHall = true;
					}
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						gOut.Print("{0}", Globals.LineSep);
					}

					gOut.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Globals.Argv[i]);

					nlFlag = true;
				}
			}
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual long ConvertWeaponsToArtifacts()
		{
			long cw = -1;

			foreach (var weapon in gCharacter.Weapons)
			{
				if (weapon.IsActive())
				{
					var artifact = ConvertWeaponToArtifact(weapon);

					Debug.Assert(artifact != null);

					var ac = artifact.GeneralWeapon;

					Debug.Assert(ac != null);

					if (artifact.IsCarriedByCharacter() && (cw == -1 || WeaponPowerCompare(artifact.Uid, cw) > 0) && (gGameState.Sh < 1 || ac.Field5 < 2))
					{
						cw = artifact.Uid;

						Debug.Assert(cw > 0);
					}
				}
			}

			return cw;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual long ConvertArmorToArtifacts()
		{
			RetCode rc;

			var armorNames = new string[]
			{
					"",
					"leather",
					"chain",
					"plate",
					"magic"
			};

			var a2 = (long)gCharacter.ArmorClass / 2;

			var x = (long)gCharacter.ArmorClass % 2;

			var s = a2 + ((4 - a2) * (a2 > 4 ? 1 : 0));

			if (a2 > 0)
			{
				var armor = gCharacter.Armor;

				Debug.Assert(armor != null);

				var artifact = Globals.CreateInstance<IArtifact>(y =>
				{
					y.SetArtifactCategoryCount(1);

					y.Uid = Globals.Database.GetArtifactUid();

					y.Name = armor.IsActive() ? Globals.CloneInstance(armor.Name) : string.Format("{0} armor", armorNames[s]);

					Debug.Assert(!string.IsNullOrWhiteSpace(y.Name));

					y.Desc = armor.IsActive() && !string.IsNullOrWhiteSpace(armor.Desc) ? Globals.CloneInstance(armor.Desc) : null;

					y.Seen = true;

					y.IsCharOwned = true;

					y.IsListed = true;

					if (armor.IsActive())
					{
						y.IsPlural = armor.IsPlural;

						y.PluralType = armor.PluralType;

						y.ArticleType = armor.ArticleType;

						y.GetCategories(0).Field1 = armor.Field1;

						y.GetCategories(0).Field2 = armor.Field2;

						y.GetCategories(0).Type = armor.Type;

						y.Value = armor.Value;

						y.Weight = armor.Weight;
					}
					else
					{
						y.IsPlural = false;

						y.PluralType = PluralType.None;

						y.ArticleType = ArticleType.Some;

						y.GetCategories(0).Field1 = a2 * 2;

						y.GetCategories(0).Field2 = 0;

						y.GetCategories(0).Type = ArtifactType.Wearable;

						var ima = false;

						y.Value = (long)GetArmorPriceOrValue(gCharacter.ArmorClass, false, ref ima);

						y.Weight = (a2 == 1 ? 15 : a2 == 2 ? 25 : 35);
					}

					var charWeight = 0L;

					rc = gCharacter.GetFullInventoryWeight(ref charWeight, recurse: true);

					Debug.Assert(IsSuccess(rc));

					if (y.Weight + charWeight <= gCharacter.GetWeightCarryableGronds())
					{
						y.SetWornByCharacter();
					}
					else
					{
						y.SetInRoomUid(StartRoom);
					}
				});

				rc = Globals.Database.AddArtifact(artifact);

				Debug.Assert(IsSuccess(rc));

				gGameState.SetImportedArtUids(gGameState.ImportedArtUidsIdx++, artifact.Uid);

				if (artifact.IsWornByCharacter())
				{
					gGameState.Ar = artifact.Uid;

					Debug.Assert(gGameState.Ar > 0);
				}
			}

			if (x == 1)
			{
				var shield = gCharacter.Shield;

				Debug.Assert(shield != null);

				var artifact = Globals.CreateInstance<IArtifact>(y =>
				{
					y.SetArtifactCategoryCount(1);

					y.Uid = Globals.Database.GetArtifactUid();

					y.Name = shield.IsActive() ? Globals.CloneInstance(shield.Name) : "shield";

					Debug.Assert(!string.IsNullOrWhiteSpace(y.Name));

					y.Desc = shield.IsActive() && !string.IsNullOrWhiteSpace(shield.Desc) ? Globals.CloneInstance(shield.Desc) : null;

					y.Seen = true;

					y.IsCharOwned = true;

					y.IsListed = true;

					if (shield.IsActive())
					{
						y.IsPlural = shield.IsPlural;

						y.PluralType = shield.PluralType;

						y.ArticleType = shield.ArticleType;

						y.GetCategories(0).Field1 = shield.Field1;

						y.GetCategories(0).Field2 = shield.Field2;

						y.GetCategories(0).Type = shield.Type;

						y.Value = shield.Value;

						y.Weight = shield.Weight;
					}
					else
					{
						y.IsPlural = false;

						y.PluralType = PluralType.S;

						y.ArticleType = ArticleType.A;

						y.GetCategories(0).Field1 = 1;

						y.GetCategories(0).Field2 = 0;

						y.GetCategories(0).Type = ArtifactType.Wearable;

						y.Value = Constants.ShieldPrice;

						y.Weight = 15;
					}

					var charWeight = 0L;

					rc = gCharacter.GetFullInventoryWeight(ref charWeight, recurse: true);

					Debug.Assert(IsSuccess(rc));

					if (y.Weight + charWeight <= gCharacter.GetWeightCarryableGronds())
					{
						y.SetWornByCharacter();
					}
					else
					{
						y.SetInRoomUid(StartRoom);
					}
				});

				rc = Globals.Database.AddArtifact(artifact);

				Debug.Assert(IsSuccess(rc));

				gGameState.SetImportedArtUids(gGameState.ImportedArtUidsIdx++, artifact.Uid);

				if (artifact.IsWornByCharacter())
				{
					gGameState.Sh = artifact.Uid;

					Debug.Assert(gGameState.Sh > 0);
				}
			}

			return (a2 + x) + (a2 >= 3 ? 2 : 0);
		}

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="damageFactor"></param>
		public virtual void SetScaledHardiness(IMonster monster, long damageFactor)
		{
			Debug.Assert(monster != null);

			Debug.Assert(damageFactor > 0);

			if (monster.Field1 > 0)
			{
				monster.Hardiness = damageFactor * monster.Field1;
			}
		}

		/// <summary></summary>
		/// <param name="s"></param>
		/// <param name="spell"></param>
		public virtual void PlayerSpellCastBrainOverload(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s));

			Debug.Assert(spell != null);

			PrintSpellOverloadsBrain(s, spell);

			gGameState.SetSa(s, 0);

			if (Globals.IsRulesetVersion(5, 15, 25))
			{
				gCharacter.SetSpellAbilities(s, 0);
			}
		}

		public Engine()
		{
			StartRoom = Constants.StartRoom;

			NumSaveSlots = Constants.NumSaveSlots;

			ScaledHardinessUnarmedMaxDamage = Constants.ScaledHardinessUnarmedMaxDamage;

			ScaledHardinessMaxDamageDivisor = Constants.ScaledHardinessMaxDamageDivisor;

			EnforceMonsterWeightLimits = true;

			PoundCharPolicy = PoundCharPolicy.AllArtifacts;

			PercentCharPolicy = PercentCharPolicy.None;
		}
	}
}
