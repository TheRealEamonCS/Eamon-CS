
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void FinishParsingCloseCommand()
		{
			ParseName();

			// Close eyes

			if (ObjData.Name.IndexOf("eye", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				var gateOfLightArtifact = gADB[13];

				Debug.Assert(gateOfLightArtifact != null);

				gOut.Print("{0}", gGameState.IC ? "They're already closed!" : "You close your eyes.");

				gateOfLightArtifact.DoorGate.SetOpen(true);

				gGameState.ICV = true;

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				ResolveRecord(false);
			}
		}

		public override void FinishParsingExamineCommand()
		{
			var bookOfTheDarkArtifact = gADB[3];

			Debug.Assert(bookOfTheDarkArtifact != null);

			var tarpArtifact = gADB[39];

			Debug.Assert(tarpArtifact != null);

			var oldPaperArtifact = gADB[40];

			Debug.Assert(oldPaperArtifact != null);

			ParseName();

			NextCommand.ContainerType = NextCommand.Prep != null ? NextCommand.Prep.ContainerType : (ContainerType)(-1);

			if (ObjData.Name.Equals("room", StringComparison.OrdinalIgnoreCase) || ObjData.Name.Equals("area", StringComparison.OrdinalIgnoreCase))
			{
				var command = gEngine.CreateInstance<ILookCommand>();

				NextCommand.CopyCommandData(command);

				NextState = command;
			}
			else if (ActorRoom.Uid == 1 && (ObjData.Name.IndexOf("feath", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("mattre", StringComparison.OrdinalIgnoreCase) >= 0))
			{
				gOut.Print("The feathers are nice!");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else if (ActorRoom.Uid == 1 && (ObjData.Name.IndexOf("bed", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("silk", StringComparison.OrdinalIgnoreCase) >= 0))
			{
				gOut.Print("Nice, huh?");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else if (ActorRoom.Uid == 2 && bookOfTheDarkArtifact.IsEmbeddedInRoom(ActorRoom) && ObjData.Name.IndexOf("books", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				gOut.Print("There is only one loose one.");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else if (ActorRoom.Uid == 25 && tarpArtifact.IsEmbeddedInRoom(ActorRoom) && ObjData.Name.IndexOf("table", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				tarpArtifact.SetInRoom(ActorRoom);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else if (ActorRoom.Uid == 38 && ActorRoom.GetDir(2) < 0 && (ObjData.Name.IndexOf("shelf", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("shelves", StringComparison.OrdinalIgnoreCase) >= 0))
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0 || gGameState.IV)
				{
					gEngine.PrintEffectDesc(30);

					ActorRoom.SetDir(2, 8);

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					gEngine.PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else if (ActorRoom.Uid == 44 && ActorRoom.GetDir(2) < 0 && ObjData.Name.IndexOf("fireplace", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0 || gGameState.IV)
				{
					gEngine.PrintEffectDesc(29);

					ActorRoom.SetDir(2, 46);

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					gEngine.PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else if (ActorRoom.Uid == 44 && oldPaperArtifact.IsInLimbo() && (ObjData.Name.IndexOf("bear", StringComparison.OrdinalIgnoreCase) >= 0 || ObjData.Name.IndexOf("rug", StringComparison.OrdinalIgnoreCase) >= 0))
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0 || gGameState.IV)
				{
					gOut.Print("You find a paper stuffed in its mouth!");

					oldPaperArtifact.SetInRoom(ActorRoom);

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					gEngine.PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else
			{
				ObjData.RecordWhereClauseList = GetRecordWhereClauseListExamineCommand();

				if (!Enum.IsDefined(typeof(ContainerType), NextCommand.ContainerType))
				{
					ObjData.RevealEmbeddedArtifactFunc = (r, a) => { };
				}

				ObjData.RecordMatchFunc = RecordMatch01;

				ObjData.RecordNotFoundFunc = NextCommand.PrintYouSeeNothingSpecial;

				ResolveRecord();
			}
		}

		public override void FinishParsingGetCommand()
		{
			var bookOfTheDarkArtifact = gADB[3];

			Debug.Assert(bookOfTheDarkArtifact != null);

			ParseName();

			if (ObjData.Name.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IGetCommand>().GetAll = true;
			}
			else if (ActorRoom.Uid == 2 && ObjData.Name.IndexOf("books", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				gOut.Print(bookOfTheDarkArtifact.IsEmbeddedInRoom(ActorRoom) ? "There is only one loose one." : "They are held fast.");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				ObjData.RecordWhereClauseList = GetRecordWhereClauseListGetCommand();

				ObjData.RecordNotFoundFunc = NextCommand.PrintCantVerbThat;

				ResolveRecord(false);
			}
		}

		public override void FinishParsingOpenCommand()
		{
			ParseName();

			// Open eyes

			if (ObjData.Name.IndexOf("eye", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				var hogardMonster = gMDB[7];

				Debug.Assert(hogardMonster != null);

				var jolardMonster = gMDB[8];

				Debug.Assert(jolardMonster != null);

				var gateOfLightArtifact = gADB[13];

				Debug.Assert(gateOfLightArtifact != null);

				gOut.Print("{0}", gGameState.IC ? "You open your eyes." : "They're already open!");

				if (hogardMonster.IsInRoom(ActorRoom))
				{
					gOut.Print("{0} vanishes!", hogardMonster.GetTheName(true));

					hogardMonster.SetInLimbo();
				}

				if (jolardMonster.IsInRoom(ActorRoom))
				{
					gOut.Print("{0} vanishes!", jolardMonster.GetTheName(true));

					jolardMonster.SetInLimbo();
				}

				gateOfLightArtifact.DoorGate.SetOpen(false);

				gGameState.ICV = false;

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else if (ActorRoom.Uid == 1 && ActorRoom.GetDir(3) > 0 && ObjData.Name.IndexOf("panel", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				gOut.Print("It's already open!");

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else
			{
				ResolveRecord(false);
			}
		}

		public override void CheckPlayerCommand(bool afterFinishParsing)
		{
			Debug.Assert(NextCommand != null);

			base.CheckPlayerCommand(afterFinishParsing);

			if (afterFinishParsing)
			{
				// Restrict GiveCommand when targeting unconscious Darrk Ness

				if (NextCommand is IGiveCommand && IobjMonster != null && IobjMonster.Uid == 1 && IobjMonster.StateDesc.Length > 0)
				{
					gOut.Print("You can't do that while {0} {1} unconscious!", IobjMonster.GetTheName(), IobjMonster.EvalPlural("is", "are"));

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
			else
			{
				// Restrict commands while eyes closed

				if (!ActorRoom.IsViewable())			// TODO: accept dobj when appropriate ???
				{
					if (NextCommand is IGetCommand)
					{
						gOut.Print("You can't find anything.");

						NextState = gEngine.CreateInstance<IStartState>();
					}
					else if (NextCommand is ILookCommand || NextCommand is IExamineCommand)
					{
						gOut.Print("You see nothing.");

						NextState = gEngine.CreateInstance<IStartState>();
					}
					else if (NextCommand is IAttackCommand)
					{
						gOut.Print("You swing wild --- nothing hit.");

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
					else if (NextCommand is ISmileCommand || NextCommand is IWaveCommand)
					{
						gOut.Print("Okay.");

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
					else if (NextCommand is IReadCommand)
					{
						gOut.Print("You can't see.");

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}

				// Restrict commands while invisible

				if (gGameState.IV)
				{
					if (NextCommand is IGiveCommand)
					{
						gOut.Print("You can't do that now.");

						NextState = gEngine.CreateInstance<IStartState>();
					}
					else if (NextCommand is IInventoryCommand)
					{
						gOut.Print("You see nothing.");

						NextState = gEngine.CreateInstance<IStartState>();
					}
					else if (NextCommand is ISmileCommand || NextCommand is IWaveCommand)
					{
						gOut.Print("Okay.");

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
				}
			}
		}
	}
}
