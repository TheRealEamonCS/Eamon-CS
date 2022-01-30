
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.Plugin;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Plugin
{
	public class PluginGlobals : EamonDD.Game.Plugin.PluginGlobals, IPluginGlobals
	{
		public IState _currState;

		public virtual StringBuilder Buf01 { get; set; } = new StringBuilder(Constants.BufSize);

		public virtual StringBuilder Buf02 { get; set; } = new StringBuilder(Constants.BufSize);

		public virtual IList<ICommand> CommandList { get; set; }

		public virtual IList<ICommand> LastCommandList { get; set; }

		public virtual long LoopMonsterUid { get; set; }

		public virtual long LoopMemberNumber { get; set; }

		public virtual long LoopAttackNumber { get; set; }

		public virtual long LoopGroupCount { get; set; }

		public virtual long LoopFailedMoveMemberCount { get; set; }

		public virtual IMonster LoopLastDobjMonster { get; set; }

		public virtual new Framework.IEngine Engine
		{
			get
			{
				return (Framework.IEngine)base.Engine;
			}

			set
			{
				if (base.Engine != value)
				{
					base.Engine = value;
				}
			}
		}

		public virtual IIntroStory IntroStory { get; set; }

		public virtual IMainLoop MainLoop { get; set; }

		public virtual ISentenceParser SentenceParser { get; set; }

		public virtual ICommandParser CommandParser { get; set; }

		public virtual IState CurrState 
		{
			get
			{
				return _currState;
			}

			set
			{
				if (_currState != null && RevealContentCounter > 0 && RevealContentArtifactList != null && RevealContentArtifactList.Count > 0)
				{
					_currState.ProcessRevealContentArtifactList();
				}

				_currState = value;
			}
		}

		public virtual IState NextState { get; set; }

		public virtual IGameState GameState { get; set; }

		public virtual ICharacter Character { get; set; }

		public virtual ExitType ExitType { get; set; }

		public virtual string CommandPrompt { get; set; }

		public virtual ICommand CurrCommand 
		{ 
			get
			{
				return CurrState as ICommand;
			}
		}

		public virtual ICommand NextCommand 
		{ 
			get
			{
				return NextState as ICommand;
			}
		}

		public virtual ICommand LastCommand
		{
			get
			{
				return LastCommandList.Count > 0 ? LastCommandList[LastCommandList.Count - 1] : null;
			}
		}

		public virtual bool CommandPromptSeen { get; set; }

		public virtual bool PlayerMoved { get; set; }

		public virtual bool GameRunning
		{
			get
			{
				return ExitType == ExitType.None;
			}
		}

		public virtual bool DeleteGameStateAfterLoop
		{
			get
			{
				return ExitType == ExitType.GoToMainHall || ExitType == ExitType.StartOver || ExitType == ExitType.FinishAdventure || ExitType == ExitType.DeleteCharacter;
			}
		}

		public virtual bool StartOver
		{
			get
			{
				return ExitType == ExitType.StartOver;
			}
		}

		public virtual bool ErrorExit
		{
			get
			{
				return ExitType == ExitType.Error;
			}
		}

		public virtual bool ExportCharacterGoToMainHall
		{
			get
			{
				return ExitType == ExitType.GoToMainHall || ExitType == ExitType.FinishAdventure;
			}
		}

		public virtual bool ExportCharacter
		{
			get
			{
				return ExitType == ExitType.FinishAdventure;
			}
		}

		public virtual bool DeleteCharacter
		{
			get
			{
				return ExitType == ExitType.DeleteCharacter;
			}
		}

		public override bool EnableGameOverrides
		{
			get
			{
				return base.EnableGameOverrides && GameState != null;
			}
		}

		public override void InitSystem()
		{
			base.InitSystem();

			CommandList = new List<ICommand>();

			LastCommandList = new List<ICommand>();

			IntroStory = CreateInstance<IIntroStory>();

			MainLoop = CreateInstance<IMainLoop>();

			SentenceParser = CreateInstance<ISentenceParser>();

			CommandParser = CreateInstance<ICommandParser>();

			CommandPrompt = Constants.CommandPrompt;
		}
	}
}
