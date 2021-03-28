
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;

namespace EamonRT.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginGlobals : EamonDD.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		StringBuilder Buf01 { get; set; }

		/// <summary></summary>
		StringBuilder Buf02 { get; set; }

		/// <summary></summary>
		IList<ICommand> CommandList { get; set; }

		/// <summary></summary>
		IList<ICommand> LastCommandList { get; set; }

		/// <summary></summary>
		long LoopMonsterUid { get; set; }

		/// <summary></summary>
		long LoopMemberNumber { get; set; }

		/// <summary></summary>
		long LoopAttackNumber { get; set; }

		/// <summary></summary>
		long LoopGroupCount { get; set; }

		/// <summary></summary>
		long LoopFailedMoveMemberCount { get; set; }

		/// <summary></summary>
		IMonster LoopLastDfMonster { get; set; }

		/// <summary></summary>
		new IEngine Engine { get; set; }

		/// <summary></summary>
		IIntroStory IntroStory { get; set; }

		/// <summary></summary>
		IMainLoop MainLoop { get; set; }

		/// <summary></summary>
		ISentenceParser SentenceParser { get; set; }

		/// <summary></summary>
		ICommandParser CommandParser { get; set; }

		/// <summary></summary>
		IState CurrState { get; set; }

		/// <summary></summary>
		IState NextState { get; set; }

		/// <summary></summary>
		IGameState GameState { get; set; }

		/// <summary></summary>
		ICharacter Character { get; set; }

		/// <summary></summary>
		ExitType ExitType { get; set; }

		/// <summary></summary>
		string CommandPrompt { get; set; }

		/// <summary></summary>
		ICommand CurrCommand { get; }

		/// <summary></summary>
		ICommand NextCommand { get; }

		/// <summary></summary>
		ICommand LastCommand { get; }

		/// <summary></summary>
		bool CommandPromptSeen { get; set; }

		/// <summary></summary>
		bool PlayerMoved { get; set; }

		/// <summary></summary>
		bool GameRunning { get; }

		/// <summary></summary>
		bool DeleteGameStateAfterLoop { get; }

		/// <summary></summary>
		bool StartOver { get; }

		/// <summary></summary>
		bool ErrorExit { get; }

		/// <summary></summary>
		bool ExportCharacterGoToMainHall { get; }

		/// <summary></summary>
		bool ExportCharacter { get; }

		/// <summary></summary>
		bool DeleteCharacter { get; }
	}
}
