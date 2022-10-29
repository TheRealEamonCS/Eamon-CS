
// PlayerDeadState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

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

			Debug.Assert(gCharMonster != null);

			gEngine.ResetProperties(PropertyResetCode.SwitchContext);

			RestoreGame = false;

			gEngine.DeadMenu(PrintLineSep, ref _restoreGame);

			if (!RestoreGame)
			{
				if (NextState == null)
				{
					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}

				goto Cleanup;
			}

			gCommandParser.ActorMonster = gCharMonster;

			gCommandParser.InputBuf.SetFormat("restore");

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IProcessPlayerInputState>();
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
			}

			gEngine.NextState = NextState;
		}

		public PlayerDeadState()
		{
			Name = "PlayerDeadState";
		}
	}
}
