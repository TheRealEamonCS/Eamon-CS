
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void FinishParsingGetCommand()
		{
			ParseName();

			if (ObjData.Name.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
				{
					NextCommand.PrintEnemiesNearby();

					NextState = Globals.CreateInstance<IStartState>();
				}
				else
				{
					NextCommand.Cast<IGetCommand>().GetAll = true;
				}
			}
			else if (ActorRoom.Type == RoomType.Indoors && ObjData.Name.IndexOf("torch", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
				{
					NextCommand.PrintEnemiesNearby();

					NextState = Globals.CreateInstance<IStartState>();
				}
				else
				{
					gOut.Print("They are bolted firmly to the walls.");

					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
			}
			else
			{
				ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
				{
					r => r is IArtifact a && a.IsInRoom(ActorRoom),
					r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
					r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToCharacter(gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
				};

				ObjData.RecordNotFoundFunc = NextCommand.PrintCantVerbThat;

				ResolveRecord(false);
			}
		}
	}
}
