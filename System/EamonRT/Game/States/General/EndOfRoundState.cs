
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : State, IEndOfRoundState
	{
		/// <summary></summary>
		public virtual IList<IMonster> ResetMonsterList { get; set; }

		public override void Execute()
		{
			ResetMonsterList = gEngine.Database.MonsterTable.Records.ToList();

			foreach (var monster in ResetMonsterList)
			{
				monster.InitGroupCount = monster.CurrGroupCount;
			}

			ProcessEvents(EventType.AfterEndRound);

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>(); 
			}

			gEngine.NextState = NextState;
		}

		public EndOfRoundState()
		{
			Name = "EndOfRoundState";
		}
	}
}
