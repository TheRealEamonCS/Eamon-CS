
// CommandParser.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Parsing
{
	[ClassMappings]
	public class CommandParser : EamonRT.Game.Parsing.CommandParser, ICommandParser
	{
		public override void FinishParsingGetCommand()
		{
			ParseName();

			if (ObjData.Name.Equals("all", StringComparison.OrdinalIgnoreCase))
			{
				NextCommand.Cast<IGetCommand>().GetAll = true;
			}
			else if ((ActorRoom.Uid == 4 || ActorRoom.Uid == 20 || ActorRoom.Uid == 22) && ObjData.Name.IndexOf("torch", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				gOut.Print("All torches are bolted to the wall and cannot be removed.");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				ObjData.RecordWhereClauseList = new List<Func<IGameBase, bool>>()
				{
					r => r is IArtifact a && a.IsInRoom(ActorRoom),
					r => r is IArtifact a && a.IsEmbeddedInRoom(ActorRoom),
					r => r is IArtifact a && (a.IsCarriedByContainerContainerTypeExposedToMonster(ActorMonster, gEngine.ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(ActorRoom, gEngine.ExposeContainersRecursively))
				};

				ObjData.RecordNotFoundFunc = NextCommand.PrintCantVerbThat;

				ResolveRecord(false);
			}
		}
	}
}
