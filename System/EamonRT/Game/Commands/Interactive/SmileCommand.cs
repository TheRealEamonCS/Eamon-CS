﻿
// SmileCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SmileCommand : Command, ISmileCommand
	{
		/// <summary></summary>
		public virtual IList<IMonster> SmilingMonsterList { get; set; }

		public override void ExecuteForPlayer()
		{
			SmilingMonsterList = gEngine.GetEmotingMonsterList(ActorRoom, ActorMonster);

			if (SmilingMonsterList.Count <= 0)
			{
				PrintOkay();

				goto Cleanup;
			}

			foreach (var monster in SmilingMonsterList)
			{
				gEngine.PrintMonsterEmotes(monster);
			}

			gOut.WriteLine();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public SmileCommand()
		{
			SortOrder = 310;

			Name = "SmileCommand";

			Verb = "smile";

			Type = CommandType.Interactive;
		}
	}
}
