
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : ICommandImpl
	{
		public IMonster _actorMonster;

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
				if (Globals.RevealContentCounter > 0)
				{
					Globals.RevealContentMonster = value;
				}

				_actorMonster = value;
			}
		}

		public virtual IRoom ActorRoom { get; set; }

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

			gOut.Print("Do you mean \"{0}\" or \"{1}\"?", obj1.GetNoneName(showCharOwned: false), obj2.GetNoneName(showCharOwned: false, buf: Globals.Buf01));
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

		public virtual void PrintTooHeavy(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("{0} {1} too heavy.", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
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

			gOut.Write("{0}{1} received.", Environment.NewLine, artifact.GetNoneName(true, false));
		}

		public virtual void PrintRetrieved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Write("{0}{1} retrieved.", Environment.NewLine, artifact.GetNoneName(true, false));
		}

		public virtual void PrintTaken(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Write("{0}{1} taken.", Environment.NewLine, artifact.GetNoneName(true, false));
		}

		public virtual void PrintDropped(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Write("{0}{1} dropped.", Environment.NewLine, artifact.GetNoneName(true, false));
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

		public virtual void PrintAlreadyBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You already broke {0}!", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintHaveToForceOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("You'll have to force {0} open.", artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintWearingRemoveFirst(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (Globals.IsRulesetVersion(5, 15, 25))
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

			if (Globals.IsRulesetVersion(5, 15, 25))
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

			gOut.Print("You can't wear {0} while using {1}.", shield.GetTheName(), weapon.GetTheName(buf: Globals.Buf01));
		}

		public virtual void PrintContainerNotEmpty(IArtifact artifact, ContainerType containerType, bool isPlural)
		{
			Debug.Assert(artifact != null && Enum.IsDefined(typeof(ContainerType), containerType));

			gOut.Print("{0} {1} {2} {3} {4}.  Remove it first.", 
				artifact.GetTheName(true), 
				artifact.EvalPlural("has", "have"), 
				isPlural ? "some stuff" : "something", 
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

			gOut.Print("You've lit {0}.", artifact.GetTheName());
		}

		public virtual void PrintLightExtinguished(IArtifact artifact)
		{
			// do nothing
		}

		public virtual void PrintCantReadyWeaponWithShield(IArtifact weapon, IArtifact shield)
		{
			Debug.Assert(weapon != null && shield != null);

			gOut.Print("You can't use {0} while wearing {1}.", weapon.GetTheName(), shield.GetTheName(buf: Globals.Buf01));
		}

		public virtual void PrintPolitelyRefuses(IMonster monster)
		{
			Debug.Assert(monster != null);

			gOut.Print("{0} politely refuse{1}.", monster.GetTheName(true), monster.EvalPlural("s", ""));
		}

		public virtual void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			gOut.Print("You give {0} to {1}.", artifact.GetTheName(), monster.GetTheName(buf: Globals.Buf01));
		}

		public virtual void PrintObjBelongsToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			gOut.Print("{0} belongs to {1}.", artifact.GetTheName(true), monster.GetTheName(buf: Globals.Buf01));
		}

		public virtual void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			gOut.Print("You open {0} with {1}.", artifact.EvalPlural("it", "them"), key.GetTheName());
		}

		public virtual void PrintNotEnoughGold()
		{
			if (Globals.IsRulesetVersion(5, 15, 25) || gCharacter.HeldGold < 0)
			{
				gOut.Print("You aren't carrying that much gold of your own!");
			}
			else
			{
				gOut.Print("You only have {0} gold piece{1}.",
					gEngine.GetStringFromNumber(gCharacter.HeldGold, false, Globals.Buf),
					gCharacter.HeldGold != 1 ? "s" : "");
			}
		}

		public virtual void PrintMustFirstReadyWeapon()
		{
			if (Globals.IsRulesetVersion(5, 15, 25))
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
			gOut.Print("You don't have it and it's not here.");
		}

		public virtual void PrintDontHaveIt()
		{
			gOut.Print("You don't have it.");
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
			if (Globals.IsRulesetVersion(5, 15, 25))
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

		public virtual bool IsAllowedInRoom()
		{
			return true;
		}

		public virtual bool ShouldAllowSkillGains()
		{
			return true;
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

		public virtual bool ShouldPreTurnProcess()
		{
			return true;
		}

		public virtual void Execute()
		{

		}

		public virtual void PreExecute()
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

					Command.NextState = Globals.CreateInstance<IStartState>();
				}
			}
			else
			{
				Debug.Assert(Command.IsMonsterEnabled);

				Command.Execute();
			}

			Globals.NextState = Command.NextState;
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

				destCommand.Prep = Globals.CloneInstance(Command.Prep);
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

			Command.NextState = Globals.CreateInstance<IGetCommand>(x =>
			{
				x.PreserveNextState = true;
			});

			Command.CopyCommandData(Command.NextState as ICommand);

			Command.NextState.NextState = Globals.CreateInstance<T>(x =>
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
