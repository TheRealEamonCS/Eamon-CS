
// PlayerDeadState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PlayerDeadState : State, IPlayerDeadState
	{
		public bool _restoreGame;

		public virtual bool PrintLineSep { get; set; }

		/// <summary></summary>
		public virtual bool RestoreGame 
		{ 
			get
			{
				return _restoreGame;
			}

			set
			{
				_restoreGame = value;
			}
		}

		public override void Execute()
		{
			if (gGameState.Die <= 0)
			{
				goto Cleanup;
			}

			RestoreGame = false;

			Debug.Assert(gCharMonster != null);

			gEngine.DeadMenu(gCharMonster, PrintLineSep, ref _restoreGame);

			if (!RestoreGame)
			{
				if (NextState == null)
				{
					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}

				goto Cleanup;
			}

			gEngine.ClearActionLists();

			gSentenceParser.Clear();

			gCommandParser.Clear();

			gCommandParser.ActorMonster = gCharMonster;

			gCommandParser.InputBuf.SetFormat("restore");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IProcessPlayerInputState>();
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

			Globals.NextState = NextState;
		}

		public PlayerDeadState()
		{
			Uid = 30;

			Name = "PlayerDeadState";
		}
	}
}
