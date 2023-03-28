
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : ICommandImpl
	{
		public IMonster _actorMonster;

		public IRoom _actorRoom;

		public virtual ICommand Command { get; set; }

		public virtual ICommandParser CommandParser { get; set; }

		public virtual IMonster ActorMonster
		{
			get
			{
				return _actorMonster;
			}

			set
			{
				if (gEngine.RevealContentCounter > 0)
				{
					gEngine.RevealContentMonster = value;
				}

				_actorMonster = value;
			}
		}

		public virtual IRoom ActorRoom
		{
			get
			{
				return _actorRoom;
			}

			set
			{
				if (gEngine.RevealContentCounter > 0)
				{
					gEngine.RevealContentRoom = value;
				}

				_actorRoom = value;
			}
		}

		public virtual IGameBase Dobj { get; set; }

		public virtual IArtifact DobjArtifact
		{
			get
			{
				return Command.Dobj as IArtifact;
			}
		}

		public virtual IMonster DobjMonster
		{
			get
			{
				return Command.Dobj as IMonster;
			}
		}

		public virtual IGameBase Iobj { get; set; }

		public virtual IArtifact IobjArtifact
		{
			get
			{
				return Command.Iobj as IArtifact;
			}
		}

		public virtual IMonster IobjMonster
		{
			get
			{
				return Command.Iobj as IMonster;
			}
		}

		public virtual string[] Synonyms { get; set; }

		public virtual long SortOrder { get; set; }

		public virtual string Verb { get; set; }

		public virtual IPrep Prep { get; set; }

		public virtual CommandType Type { get; set; }

		public virtual ContainerType ContainerType { get; set; }

		public virtual bool GetCommandCalled { get; set; }

		public virtual bool IsNew { get; set; }

		public virtual bool IsListed { get; set; }

		public virtual bool IsSentenceParserEnabled { get; set; }

		public virtual bool IsDobjPrepEnabled { get; set; }

		public virtual bool IsIobjEnabled { get; set; }

		public virtual bool IsDarkEnabled { get; set; }

		public virtual bool IsPlayerEnabled { get; set; }

		public virtual bool IsMonsterEnabled { get; set; }

		public virtual void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			gOut.Print("You can't {0} {1}.", Command.Verb, obj.GetTheName());
		}

		public virtual void PrintCantVerbIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You can't {0} {1}.", Command.Verb, artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintCantVerbThat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You can't {0} {1}.", Command.Verb, artifact.EvalPlural("that", "them"));
		}

		public virtual void PrintDoYouMeanObj1OrObj2(IGameBase obj1, IGameBase obj2)
		{
			Debug.Assert(obj1 != null && obj2 != null);

			gOut.Print("Do you mean \"{0}\" or \"{1}\"?", obj1.GetNoneName(showCharOwned: false), obj2.GetNoneName(showCharOwned: false));
		}

		public virtual void PrintWhyAttack(IGameBase obj)
		{
			Debug.Assert(obj != null);

			gOut.Print("Why would you attack {0}?", obj.GetTheName());
		}

		public virtual void PrintTakingFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("[Taking {0} first.]", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintRemovingFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("[Removing {0} first.]", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintBestLeftAlone(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} {1} best if left alone.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
		}

		public virtual void PrintTooHeavy(IArtifact artifact, bool getAll = false)
		{
			Debug.Assert(artifact != null);

			if (gEngine.IsRulesetVersion(5) && Command is IGetCommand)
			{
				if (getAll)
				{
					gOut.Print("{0} {1} too heavy.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
				}
				else
				{
					gOut.Print("{0} too heavy for you.", artifact.EvalPlural("It is", "They are"));
				}
			}
			else if (gEngine.IsRulesetVersion(62) && Command is IGetCommand && !(Command.NextState is IRequestCommand))
			{
				if (getAll)
				{
					gOut.Print("{0} can't be moved.", artifact.GetTheName(true));
				}
				else
				{
					gOut.Print("You can't budge {0}!", artifact.EvalPlural("it", "them"));
				}
			}
			else
			{
				gOut.Print("{0} {1} too heavy.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
			}
		}

		public virtual void PrintMustBeFreed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} must be freed.", artifact.GetTheName(true));
		}

		public virtual void PrintMustFirstOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You must first open {0}.", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintMustFirstClose(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You must first close {0}.", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintWorn(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} worn.", artifact.GetNoneName(true, false));
		}

		public virtual void PrintRemoved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} removed.", artifact.GetNoneName(true, false));
		}

		public virtual void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} opened.", artifact.GetNoneName(true, false));
		}

		public virtual void PrintClosed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} closed.", artifact.GetNoneName(true, false));
		}

		public virtual void PrintReceived(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (gEngine.IsRulesetVersion(5, 62))
			{
				gOut.Write("{0}Got {1}.", Environment.NewLine, artifact.EvalPlural("it", "them"));
			}
			else
			{
				gOut.Write("{0}{1} received.", Environment.NewLine, artifact.GetNoneName(true, false));
			}
		}

		public virtual void PrintRetrieved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (gEngine.IsRulesetVersion(5, 62))
			{
				gOut.Write("{0}Got {1}.", Environment.NewLine, artifact.EvalPlural("it", "them"));
			}
			else
			{
				gOut.Write("{0}{1} retrieved.", Environment.NewLine, artifact.GetNoneName(true, false));
			}
		}

		public virtual void PrintTaken(IArtifact artifact, bool getAll = false)
		{
			Debug.Assert(artifact != null);

			if (gEngine.IsRulesetVersion(5, 62) && !getAll)
			{
				gOut.Write("{0}Got {1}.", Environment.NewLine, artifact.EvalPlural("it", "them"));
			}
			else
			{
				gOut.Write("{0}{1} taken.", Environment.NewLine, artifact.GetNoneName(true, false));
			}
		}

		public virtual void PrintDropped(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Write("{0}{1} dropped.", Environment.NewLine, artifact.GetNoneName(true, false));
		}

		public virtual void PrintReadied(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (gEngine.IsRulesetVersion(5, 62))
			{
				gOut.Print("Readied.");
			}
			else
			{
				gOut.Print("{0} readied.", artifact.GetNoneName(true, false));
			}
		}

		public virtual void PrintNotOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} not open.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintAlreadyOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} already open.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintWontOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} won't open.", artifact.EvalPlural("It", "They"));
		}

		public virtual void PrintWontFit(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} won't fit.", artifact.EvalPlural("It", "They"));
		}

		public virtual void PrintFull(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} full.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintOutOfSpace(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} out of space.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} locked.", artifact.EvalPlural("It's", "They're"));
		}

		public virtual void PrintBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You broke {0}!", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintHaveToForceOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You'll have to force {0} open.", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintWearingRemoveFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (gEngine.IsRulesetVersion(5, 62))
			{
				gOut.Print("You're wearing {0}.", artifact.EvalPlural("it", "them"));
			}
			else
			{
				gOut.Print("You're wearing {0}.  Remove {0} first.", artifact.EvalPlural("it", "them"));
			}
		}

		public virtual void PrintWearingRemoveFirst01(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (gEngine.IsRulesetVersion(5, 62))
			{
				gOut.Print("You're wearing {0}.", artifact.GetTheName());
			}
			else
			{
				gOut.Print("You're wearing {0}.  Remove {1} first.", artifact.GetTheName(), artifact.EvalPlural("it", "them"));
			}
		}

		public virtual void PrintCantWearShieldWithWeapon(IArtifact shield, IArtifact weapon)
		{
			Debug.Assert(shield != null && weapon != null);

			gOut.Print("You can't wear {0} while using {1}.", shield.GetTheName(), weapon.GetTheName());
		}

		public virtual void PrintContainerNotEmpty(IArtifact artifact, ContainerType containerType, bool isPlural)
		{
			Debug.Assert(artifact != null && Enum.IsDefined(typeof(ContainerType), containerType));

			gOut.Print("{0} {1} {2} {3} {4}.  Remove it first.", 
				artifact.GetTheName(true), 
				artifact.EvalPlural("has", "have"), 
				isPlural ? artifact.GetContainerSomeStuffDesc() : artifact.GetContainerSomethingDesc(), 
				gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"), 
				artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintVerbItAll(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You {0} {1} all.", Command.Verb, artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintNoneLeft(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("There's none left.");
		}

		public virtual void PrintOkay()
		{
			gOut.Print("Okay.");
		}

		public virtual void PrintOkay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("Okay.");
		}

		public virtual void PrintFeelBetter(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You feel better!");
		}

		public virtual void PrintFeelWorse(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You feel worse!");
		}

		public virtual void PrintTryDifferentCommand(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("Try a different command.");
		}

		public virtual void PrintNotWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} a weapon.", artifact.EvalPlural("That isn't", "They aren't"));
		}

		public virtual void PrintNotReadyableWeapon(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} a weapon that you can use.", artifact.EvalPlural("That isn't", "They aren't"));
		}

		public virtual void PrintNotWhileCarryingObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You can't do that while carrying {0}.", artifact.GetTheName());
		}

		public virtual void PrintNotWhileWearingObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You can't do that while wearing {0}.", artifact.GetTheName());
		}

		public virtual void PrintWontLight(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} won't light.", artifact.EvalPlural("It", "They"));
		}

		public virtual void PrintLightObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You light {0}.", artifact.GetTheName());
		}

		public virtual void PrintExtinguishObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Write("{0}Extinguish {1} (Y/N): ", Environment.NewLine, artifact.GetTheName());
		}

		public virtual void PrintLightExtinguished(IArtifact artifact)
		{
			// do nothing
		}

		public virtual void PrintAlreadyWearingObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You're already wearing {0}!", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintWhamHitObj(IArtifact artifact)
		{
			gEngine.PrintWhamHitObj(artifact);
		}

		public virtual void PrintFullDesc(IArtifact artifact, bool showName, bool showVerboseName)
		{
			gEngine.PrintFullDesc(artifact, showName, showVerboseName);
		}

		public virtual void PrintObjAmountLeft(IArtifact artifact, long objAmount, bool objEdible)
		{
			Debug.Assert(artifact != null);

			gOut.Print("There {0}{1}{2}{3} left.",
				objAmount != 1 ? "are " : "is ",
				objAmount > 0 ? gEngine.GetStringFromNumber(objAmount, false, gEngine.Buf) : "no",
				objEdible ? " bite" : " swallow",
				objAmount != 1 ? "s" : "");
		}

		public virtual void PrintPrepContainerYouSee(IArtifact artifact, IList<IArtifact> containerArtifactList, ContainerType containerType, bool showCharOwned, IRecordNameListArgs recordNameListArgs = null)
		{
			Debug.Assert(artifact != null && containerArtifactList != null && containerArtifactList.Count > 0 && Enum.IsDefined(typeof(ContainerType), containerType));

			gEngine.Buf.SetFormat("{0}{1} {2}, you see ",
				Environment.NewLine,
				gEngine.EvalContainerType(containerType, "Inside", "On", "Under", "Behind"),
				artifact.GetTheName(showCharOwned: showCharOwned));

			if (recordNameListArgs == null)
			{
				recordNameListArgs = gEngine.CreateInstance<IRecordNameListArgs>(x =>
				{
					x.ArticleType = ArticleType.A;

					x.ShowCharOwned = showCharOwned;

					x.StateDescCode = StateDescDisplayCode.None;

					x.ShowContents = false;

					x.GroupCountOne = false;
				});
			}

			var rc = gEngine.GetRecordNameList(containerArtifactList.Cast<IGameBase>().ToList(), recordNameListArgs, gEngine.Buf);

			Debug.Assert(gEngine.IsSuccess(rc));

			gEngine.Buf.AppendFormat(".{0}", Environment.NewLine);

			gOut.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintNothingPrepContainer(IArtifact artifact, ContainerType containerType, bool showCharOwned)
		{
			Debug.Assert(artifact != null && Enum.IsDefined(typeof(ContainerType), containerType));

			gEngine.Buf.SetFormat("{0}There's nothing {1} {2}",
				Environment.NewLine,
				gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"),
				artifact.GetTheName(showCharOwned: showCharOwned));

			gEngine.Buf.AppendFormat(".{0}", Environment.NewLine);

			gOut.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintAttemptingToFlee(IArtifact artifact, Direction direction)
		{
			Debug.Assert(artifact != null || Enum.IsDefined(typeof(Direction), direction));

			if (artifact != null)
			{
				gEngine.Buf.SetFormat("{0}", artifact.GetDoorGateFleeDesc());
			}
			else if (direction == Direction.Up || direction == Direction.Down || direction == Direction.In || direction == Direction.Out)
			{
				gEngine.Buf.SetFormat(" {0}ward", direction.ToString().ToLower());
			}
			else
			{
				gEngine.Buf.SetFormat(" to the {0}", direction.ToString().ToLower());
			}

			gOut.Print("Attempting to flee{0}.", gEngine.Buf);
		}

		public virtual void PrintCantReadyWeaponWithShield(IArtifact weapon, IArtifact shield)
		{
			Debug.Assert(weapon != null && shield != null);

			gOut.Print("You can't use {0} while wearing {1}.", weapon.GetTheName(), shield.GetTheName());
		}

		public virtual void PrintPolitelyRefuses(IMonster monster)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0} politely refuse{1}.", monster.GetTheName(true), monster.EvalPlural("s", ""));
		}

		public virtual void PrintTakesTheMoney(IMonster monster)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0} take{1} the money.", monster.GetTheName(true), monster.EvalPlural("s", ""));
		}

		public virtual void PrintWontLetYou(IMonster monster)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0} won't let you!", monster.GetTheName(true));
		}

		public virtual void PrintHealthImproves(IMonster monster)
		{
			gEngine.PrintHealthImproves(monster);
		}

		public virtual void PrintHaventSavedGameYet(IMonster monster)
		{
			Debug.Assert(monster != null);

			gOut.Print("[You haven't saved a game yet but {0} will be left here should you choose to return.  Use \"quit hall\" if you don't want {1} to stay.]",
				monster.Name,
				monster.EvalGender("him", "her", "it"));
		}

		public virtual void PrintFullDesc(IMonster monster, bool showName, bool showVerboseName)
		{
			gEngine.PrintFullDesc(monster, showName, showVerboseName);
		}

		public virtual void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters)
		{
			gEngine.PrintHealthStatus(monster, includeUninjuredGroupMonsters);
		}

		public virtual void PrintGiveGoldPiecesTo(IMonster monster, long goldAmount)
		{
			Debug.Assert(monster != null && goldAmount > 0);

			gOut.Print("Give {0} gold piece{1} to {2}.", gEngine.GetStringFromNumber(goldAmount, false, gEngine.Buf), goldAmount != 1 ? "s" : "", monster.GetTheName());
		}

		public virtual void PrintActorIsWearing(IMonster monster, IList<IArtifact> monsterWornArtifactList, IRecordNameListArgs recordNameListArgs = null)
		{
			Debug.Assert(monster != null && monsterWornArtifactList != null && monsterWornArtifactList.Count > 0);

			var isCharMonster = monster.IsCharacterMonster();

			gEngine.Buf.SetFormat("{0}{1} {2} {3}",
				Environment.NewLine,
				isCharMonster ? "You" : monster.EvalPlural(monster.GetTheName(true, true, false, false, true), "They"),
				isCharMonster ? "are" : monster.EvalPlural("is", "are"),
				isCharMonster ? "wearing " : monster.EvalPlural("wearing ", "wearing among them "));

			if (recordNameListArgs == null)
			{
				recordNameListArgs = gEngine.CreateInstance<IRecordNameListArgs>(x =>
				{
					x.ArticleType = ArticleType.A;

					x.ShowCharOwned = isCharMonster ? false : true;

					x.StateDescCode = isCharMonster ? StateDescDisplayCode.AllStateDescs : StateDescDisplayCode.SideNotesOnly;

					x.ShowContents = isCharMonster ? true : false;

					x.GroupCountOne = false;
				});
			}

			var rc = gEngine.GetRecordNameList(monsterWornArtifactList.Cast<IGameBase>().ToList(), recordNameListArgs, gEngine.Buf);

			Debug.Assert(gEngine.IsSuccess(rc));

			gEngine.Buf.AppendFormat(".{0}", Environment.NewLine);

			gOut.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintActorIsCarrying(IMonster monster, IList<IArtifact> monsterCarriedArtifactList, IRecordNameListArgs recordNameListArgs = null)
		{
			Debug.Assert(monster != null && monsterCarriedArtifactList != null);

			var isCharMonster = monster.IsCharacterMonster();

			gEngine.Buf.SetFormat("{0}{1} {2} {3}",
				Environment.NewLine,
				isCharMonster ? "You" : monster.EvalPlural(monster.GetTheName(true, true, false, false, true), "They"),
				isCharMonster ? "are" : monster.EvalPlural("is", "are"),
				monsterCarriedArtifactList.Count == 0 ? "empty handed" :
				isCharMonster ? "carrying " : monster.EvalPlural("carrying ", "carrying among them "));

			if (monsterCarriedArtifactList.Count > 0)
			{
				if (recordNameListArgs == null)
				{
					recordNameListArgs = gEngine.CreateInstance<IRecordNameListArgs>(x =>
					{
						x.ArticleType = ArticleType.A;

						x.ShowCharOwned = isCharMonster ? false : true;

						x.StateDescCode = isCharMonster ? StateDescDisplayCode.AllStateDescs : StateDescDisplayCode.SideNotesOnly;

						x.ShowContents = isCharMonster ? true : false;

						x.GroupCountOne = false;
					});
				}

				var rc = gEngine.GetRecordNameList(monsterCarriedArtifactList.Cast<IGameBase>().ToList(), recordNameListArgs, gEngine.Buf);

				Debug.Assert(gEngine.IsSuccess(rc));
			}

			gEngine.Buf.AppendFormat(".{0}", Environment.NewLine);

			gOut.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintOpensConsumesAndHandsBack(IArtifact artifact, IMonster monster, bool objOpened, bool objEdible)
		{
			Debug.Assert(artifact != null && monster != null);

			var monsterName = monster.EvalPlural(monster.GetTheName(true), monster.GetArticleName(true, true, false, false, true));

			gOut.Print("{0}{1} takes a {2} and hands {3} back.",
				monsterName,
				objOpened ? string.Format(" opens {0},", artifact.GetTheName()) : "",
				objEdible ? "bite" : "drink",
				artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintConsumesItAll(IArtifact artifact, IMonster monster, bool objOpened)
		{
			Debug.Assert(artifact != null && monster != null);

			var monsterName = monster.EvalPlural(monster.GetTheName(true), monster.GetArticleName(true, true, false, false, true));

			gOut.Print("{0}{1} eats {2} all.",
				monsterName,
				objOpened ? string.Format(" opens {0} and", artifact.GetTheName()) : "",
				artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintConsumesItAllHandsBack(IArtifact artifact, IMonster monster, bool objOpened)
		{
			Debug.Assert(artifact != null && monster != null);

			var monsterName = monster.EvalPlural(monster.GetTheName(true), monster.GetArticleName(true, true, false, false, true));

			gOut.Print("{0}{1} drinks {2} all and hands {3} back.",
				monsterName,
				objOpened ? string.Format(" opens {0},", artifact.GetTheName()) : "",
				artifact.EvalPlural("it", "them"),
				artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			gOut.Print("You give {0} to {1}.", artifact.GetTheName(), monster.GetTheName());
		}

		public virtual void PrintObjBelongsToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			gOut.Print("{0} belongs to {1}.", artifact.GetTheName(true), monster.GetTheName());
		}

		public virtual void PrintFreeActorWithKey(IMonster monster, IArtifact key)
		{
			Debug.Assert(monster != null);

			gOut.Print("You free {0}{1}.", monster.GetTheName(), key != null ? string.Format(" with {0}", key.GetTheName()) : "");
		}

		public virtual void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			gOut.Print("You open {0} with {1}.", artifact.EvalPlural("it", "them"), key.GetTheName());
		}

		public virtual void PrintPutObjPrepContainer(IArtifact artifact, IArtifact container, ContainerType containerType)
		{
			Debug.Assert(artifact != null && container != null && Enum.IsDefined(typeof(ContainerType), containerType));

			gOut.Print("Done.");
		}

		public virtual void PrintActorRemovesObjPrepContainer(IMonster monster, IArtifact artifact, IArtifact container, ContainerType containerType, bool omitWeightCheck)
		{
			Debug.Assert(monster != null && artifact != null && container != null && Enum.IsDefined(typeof(ContainerType), containerType));

			var monsterName = monster.EvalPlural(monster.GetTheName(true), monster.GetArticleName(true, true, false, false, true));

			gOut.Print("{0} removes {1} from {2} {3}.", monsterName, artifact.GetArticleName(), gEngine.EvalContainerType(containerType, "inside", "on", "under", "behind"), omitWeightCheck ? container.GetArticleName() : container.GetTheName());
		}

		public virtual void PrintActorPicksUpObj(IMonster monster, IArtifact artifact)
		{
			Debug.Assert(monster != null && artifact != null);

			var monsterName = monster.EvalPlural(monster.GetTheName(true), monster.GetArticleName(true, true, false, false, true));

			gOut.Print("{0} picks up {1}.", monsterName, artifact.GetTheName());
		}

		public virtual void PrintActorReadiesObj(IMonster monster, IArtifact artifact)
		{
			Debug.Assert(monster != null && artifact != null);

			var monsterName = monster.EvalPlural(monster.GetTheName(true), monster.GetArticleName(true, true, false, false, true));

			gOut.Print("{0} readies {1}.", monsterName, artifact.GetArticleName());
		}

		public virtual void PrintActorRemovesObjPrepContainer01(IMonster monster, IArtifact artifact, IArtifact container, ContainerType containerType, bool omitWeightCheck)
		{
			Command.PrintActorPicksUpWeapon(monster);
		}

		public virtual void PrintActorPicksUpWeapon(IMonster monster)
		{
			Debug.Assert(monster != null);

			var monsterName = string.Format("An unseen {0}", monster.CheckNBTLHostility() ? "offender" : "entity");

			gOut.Print("{0} picks up a weapon.", monsterName);
		}

		public virtual void PrintActorReadiesWeapon(IMonster monster)
		{
			Debug.Assert(monster != null);

			var monsterName = string.Format("An unseen {0}", monster.CheckNBTLHostility() ? "offender" : "entity");

			gOut.Print("{0} readies a weapon.", monsterName);
		}

		public virtual void PrintBortVisitArtifact(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null && artifact != null);

			gOut.Print("Visiting Artifact Uid {0}:  {1}.", artifact.Uid, artifact.GetArticleName(true));
		}

		public virtual void PrintBortVisitMonster(IRoom room, IMonster monster)
		{
			Debug.Assert(room != null && monster != null);

			gOut.Print("Visiting Monster Uid {0}:  {1}.", monster.Uid, monster.GetArticleName(true));
		}

		public virtual void PrintBortVisitRoom(IRoom room)
		{
			Debug.Assert(room != null);

			gOut.Print("Visiting Room Uid {0}:  {1}.", room.Uid, room.Name);
		}

		public virtual void PrintBortRecallArtifact(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null && artifact != null);

			gOut.Print("Recalling Artifact Uid {0}:  {1}.", artifact.Uid, artifact.GetArticleName(true));
		}

		public virtual void PrintBortRecallMonster(IRoom room, IMonster monster)
		{
			Debug.Assert(room != null && monster != null);

			gOut.Print("Recalling Monster Uid {0}:  {1}.", monster.Uid, monster.GetArticleName(true));
		}

		public virtual void PrintBortArtifactRoomInvalid(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("The Artifact Room is invalid.");
		}

		public virtual void PrintBortMonsterRoomInvalid(IMonster monster)
		{
			Debug.Assert(monster != null);

			gOut.Print("The Monster Room is invalid.");
		}

		public virtual void PrintBortArtifactInvalid()
		{
			gOut.Print("The Artifact is invalid.");
		}

		public virtual void PrintBortMonsterInvalid()
		{
			gOut.Print("The Monster is invalid.");
		}

		public virtual void PrintBortRoomInvalid()
		{
			gOut.Print("The Room is invalid.");
		}

		public virtual void PrintHintQuestion(long hintNum, string question)
		{
			Debug.Assert(hintNum > 0 && question != null);

			gOut.Write("{0}{1,3}. {2}", Environment.NewLine, hintNum, question);
		}

		public virtual void PrintHintQuestion01(string question)
		{
			Debug.Assert(question != null);

			gOut.Print("{0}", question);
		}

		public virtual void PrintHintAnswer(string answer, StringBuilder buf)
		{
			Debug.Assert(answer != null && buf != null);

			gEngine.PrintMacroReplacedPagedString(answer, buf);
		}

		public virtual void PrintSayText(string printedPhrase)
		{
			Debug.Assert(printedPhrase != null);

			gOut.Print("Okay, \"{0}\"", printedPhrase);
		}

		public virtual void PrintBortUsage()
		{
			gOut.Print("Usage:  BORT [Action] [Uid|Name]{0}", Environment.NewLine);

			gOut.WriteLine("  {0,-22}{1,-22}", "Action", "Uid|Name");
			gOut.WriteLine("  {0,-22}{1,-22}", "-----------------", "--------------------");
			gOut.WriteLine("  {0,-22}{1,-22}", "VisitArtifact", "Artifact Uid or Name");
			gOut.WriteLine("  {0,-22}{1,-22}", "VisitMonster", "Monster Uid or Name");
			gOut.WriteLine("  {0,-22}{1,-22}", "VisitRoom", "Room Uid or Name");
			gOut.WriteLine("  {0,-22}{1,-22}", "RecallArtifact", "Artifact Uid or Name");
			gOut.WriteLine("  {0,-22}{1,-22}", "RecallMonster", "Monster Uid or Name");
			gOut.WriteLine("  {0,-22}", "RunGameEditor");
		}

		public virtual void PrintSettingsUsage()
		{
			gOut.Print("Usage:  SETTINGS [Option] [Value]{0}", Environment.NewLine);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "Option", "Value", "Setting");
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "------", "-----", "-------");
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseRooms", "True, False", gGameState.Vr);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseMonsters", "True, False", gGameState.Vm);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseArtifacts", "True, False", gGameState.Va);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseNames", "True, False", gGameState.Vn);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "MatureContent", "True, False", gGameState.MatureContent);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "EnhancedParser", "True, False", gGameState.EnhancedParser);

			if (gGameState.EnhancedParser)
			{
				gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "IobjPronounAffinity", "True, False", gGameState.IobjPronounAffinity);
				gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ShowPronounChanges", "True, False", gGameState.ShowPronounChanges);
				gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ShowFulfillMessages", "True, False", gGameState.ShowFulfillMessages);
			}

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "PauseCombatMs", "0 .. 10000", gGameState.PauseCombatMs);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "PauseCombatActions", "0 .. 25", gGameState.PauseCombatActions);
		}

		public virtual void PrintNotEnoughGold()
		{
			if (gEngine.IsRulesetVersion(5, 62) || gCharacter.HeldGold < 0)
			{
				gOut.Print("You aren't carrying that much gold{0}!", gEngine.IsRulesetVersion(62) ? "" : " of your own");
			}
			else
			{
				gOut.Print("You only have {0} gold piece{1}.",
					gEngine.GetStringFromNumber(gCharacter.HeldGold, false, gEngine.Buf),
					gCharacter.HeldGold != 1 ? "s" : "");
			}
		}

		public virtual void PrintMustFirstReadyWeapon()
		{
			if (gEngine.IsRulesetVersion(5, 62))
			{
				gOut.Print("You have no weapon ready!");
			}
			else
			{
				gOut.Print("You must first ready a weapon!");
			}
		}

		public virtual void PrintDontHaveItNotHere()
		{
			gOut.Print("{0} it and it's not here.", gEngine.IsRulesetVersion(5, 62) ? "You aren't carrying" : "You don't have");
		}

		public virtual void PrintDontHaveIt()
		{
			gOut.Print("{0} it.", gEngine.IsRulesetVersion(5, 62) ? "You aren't carrying" : "You don't have");
		}

		public virtual void PrintDontHaveIt02(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} {1}.", gEngine.IsRulesetVersion(5, 62) ? "You aren't carrying" : "You don't have", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintDontNeedTo()
		{
			gOut.Print("You don't need to.");
		}

		public virtual void PrintCantDoThat()
		{
			gOut.Print("You can't do that.");
		}

		public virtual void PrintCantVerbThat()
		{
			gOut.Print("You can't {0} that.", Command.Verb);
		}

		public virtual void PrintCantVerbHere()
		{
			gOut.Print("You can't {0} here.", Command.Verb);
		}

		public virtual void PrintBeMoreSpecific()
		{
			gOut.Print("Try to be more specific.");
		}

		public virtual void PrintNobodyHereByThatName()
		{
			gOut.Print("Nobody here by that name!");
		}

		public virtual void PrintNothingHereByThatName()
		{
			gOut.Print("Nothing here by that name!");
		}

		public virtual void PrintYouSeeNothingSpecial()
		{
			gOut.Print("You see nothing special.");
		}

		public virtual void PrintDontBeAbsurd()
		{
			gOut.Print("Don't be absurd.");
		}

		public virtual void PrintCalmDown()
		{
			if (gEngine.IsRulesetVersion(5, 62))
			{
				gOut.Print("There's nothing to flee from!");
			}
			else
			{
				gOut.Print("Calm down.");
			}
		}

		public virtual void PrintNoPlaceToGo()
		{
			gOut.Print("There's no place to go!");
		}

		public virtual void PrintAttackNonEnemy()
		{
			gOut.Write("{0}Attack non-enemy (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintAreYouSure()
		{
			gOut.Write("{0}Are you sure (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintReturnToMainHall()
		{
			gOut.Write("{0}Return to the Main Hall (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintReallyWantToQuit()
		{
			gOut.Write("{0}Do you really want to quit (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintChangeSaveName()
		{
			gOut.Write("{0}Change name of save (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintEnterSaveName()
		{
			gOut.Write("{0}Enter new name: ", Environment.NewLine);
		}

		public virtual void PrintEnterHintChoice()
		{
			gOut.Write("{0}{0}Enter your choice: ", Environment.NewLine);
		}

		public virtual void PrintAnotherHint()
		{
			gOut.Write("{0}Another (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintNothingHappens()
		{
			gEngine.PrintNothingHappens();
		}

		public virtual void PrintNoObviousWayToDoThat()
		{
			gOut.Print("There's no obvious way to do that.");
		}

		public virtual void PrintDontHaveTheKey()
		{
			gOut.Print("You don't have the key.");
		}

		public virtual void PrintSettingsChanged()
		{
			gOut.Print("Settings changed.");
		}

		public virtual void PrintGameRestored()
		{
			gOut.Print("Game restored.");
		}

		public virtual void PrintGameSaved()
		{
			gOut.Print("Game saved.");
		}

		public virtual void PrintGameNotSaved()
		{
			gOut.Print("Game not saved.");
		}

		public virtual void PrintNoHintsAvailable()
		{
			gOut.Print("There are no hints available for this adventure.");
		}

		public virtual void PrintNoHintsAvailableNow()
		{
			gOut.Print("There are no hints available at this point in the adventure.");
		}

		public virtual void PrintYourQuestion()
		{
			gOut.Print("Your question?");
		}

		public virtual void PrintNothingToDrop()
		{
			gOut.Print("There's nothing for you to drop.");
		}

		public virtual void PrintNothingToGet()
		{
			gOut.Print("There's nothing for you to get.");
		}

		public virtual void PrintAlreadyWearingArmor()
		{
			gOut.Print("You're already wearing armor!");
		}

		public virtual void PrintAlreadyWearingShield()
		{
			gOut.Print("You're already wearing a shield!");
		}

		public virtual void PrintZapDirectHit()
		{
			gEngine.PrintZapDirectHit();
		}

		public virtual bool IsAllowedInRoom()
		{
			return true;
		}

		public virtual bool ShouldAllowSkillGains()
		{
			return true;
		}

		public virtual bool ShouldAllowRedirectToGetCommand()
		{
			return !gEngine.IsRulesetVersion(5, 62);
		}

		public virtual bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			if (Command is IGiveCommand)
			{
				Debug.Assert(artifact != null);

				return artifact.IsCarriedByCharacter();
			}
			else if (Command is IRequestCommand)
			{
				return false;
			}
			else if (Command is IDropCommand)
			{
				Debug.Assert(artifact != null);

				return artifact.IsWornByCharacter();
			}
			else if (Command is IExamineCommand)
			{
				Debug.Assert(artifact != null);

				return Enum.IsDefined(typeof(ContainerType), Command.ContainerType) && !artifact.IsWornByCharacter();
			}
			else if (Command is IGetCommand)
			{
				return false;
			}
			else if (Command is ILightCommand)
			{
				Debug.Assert(room != null);

				Debug.Assert(artifact != null);

				return room.IsLit() && (artifact.LightSource != null ? artifact.IsCarriedByCharacter() : true);
			}
			else if (Command is IPutCommand)
			{
				Debug.Assert(artifact != null);

				return artifact.IsCarriedByCharacter();
			}
			else if (Command is IReadyCommand readyCommand)
			{
				Debug.Assert(artifact != null);

				var ac = artifact.GetArtifactCategory(readyCommand.ArtTypes, false);

				if (ac != null)
				{
					if (ac.Type == ArtifactType.Wearable)
					{
						return artifact.IsCarriedByCharacter();
					}
					else
					{
						return !artifact.IsReadyableByCharacter() || artifact.IsCarriedByCharacter();
					}
				}
				else
				{
					return true;
				}
			}
			else if (Command is IRemoveCommand)
			{
				Debug.Assert(artifact != null);

				return Command.CommandParser.ObjData == Command.CommandParser.IobjData || artifact.IsWornByCharacter();
			}
			else if (Command is IUseCommand useCommand)
			{
				Debug.Assert(artifact != null);

				var ac = artifact.GetArtifactCategory(useCommand.ArtTypes, false);

				if (ac != null)
				{
					if (ac.IsWeapon01())
					{
						return !artifact.IsReadyableByCharacter() || artifact.IsCarriedByCharacter();
					}
					else if (ac.Type == ArtifactType.Wearable)
					{
						return artifact.IsCarriedByCharacter();
					}
					else
					{
						return true;
					}
				}
				else
				{
					return true;
				}
			}
			else if (Command is IWearCommand)
			{
				Debug.Assert(artifact != null);

				return artifact.Wearable != null ? artifact.IsCarriedByCharacter() || artifact.IsWornByCharacter() : true;
			}
			else
			{
				return true;
			}
		}

		public virtual void Stage()
		{
			Debug.Assert(Command.ActorMonster != null);

			Debug.Assert(Command.ActorRoom != null);

			if (Command.ActorMonster.IsCharacterMonster())
			{
				Debug.Assert(Command.IsPlayerEnabled);

				if (Command.IsAllowedInRoom())
				{
					Command.Execute();
				}
				else
				{
					Command.PrintCantVerbHere();

					Command.NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else
			{
				Debug.Assert(Command.IsMonsterEnabled);

				Command.Execute();
			}

			gEngine.NextState = Command.NextState;
		}

		public virtual void Execute()
		{

		}

		public virtual string GetPrintedVerb()
		{
			return Command.Verb.ToUpper();
		}

		public virtual bool IsEnabled(IMonster monster)
		{
			Debug.Assert(monster != null);

			return monster.IsCharacterMonster() ? Command.IsPlayerEnabled : Command.IsMonsterEnabled;
		}

		public virtual bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			return true;
		}

		public virtual void CopyCommandData(ICommand destCommand, bool includeIobj = true)
		{
			Debug.Assert(destCommand != null);

			destCommand.CommandParser = Command.CommandParser;

			destCommand.ActorMonster = Command.ActorMonster;

			destCommand.ActorRoom = Command.ActorRoom;

			destCommand.Dobj = Command.Dobj;

			if (includeIobj)
			{
				destCommand.Iobj = Command.Iobj;

				destCommand.Prep = gEngine.CloneInstance(Command.Prep);
			}
		}

		public virtual void RedirectToGetCommand<T>(IArtifact artifact, bool printTaking = true) where T : class, ICommand
		{
			Debug.Assert(artifact != null);

			if (printTaking)
			{
				if (artifact.IsCarriedByContainer())
				{
					Command.PrintRemovingFirst(artifact);
				}
				else
				{
					Command.PrintTakingFirst(artifact);
				}
			}

			Command.NextState = gEngine.CreateInstance<IGetCommand>(x =>
			{
				x.PreserveNextState = true;
			});

			Command.CopyCommandData(Command.NextState as ICommand);

			Command.NextState.NextState = gEngine.CreateInstance<T>(x =>
			{
				x.GetCommandCalled = true;

				x.ContainerType = Command.ContainerType;
			});

			Command.CopyCommandData(Command.NextState.NextState as ICommand);
		}

		public CommandImpl()
		{
			// Here we make an exception to the "always use Command" rule

			SortOrder = Int64.MaxValue;

			IsListed = true;

			IsSentenceParserEnabled = true;

			IsPlayerEnabled = true;

			ContainerType = (ContainerType)(-1);
		}
	}
}
