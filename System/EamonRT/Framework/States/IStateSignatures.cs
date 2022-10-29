
// IStateSignatures.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IStateSignatures
	{
		#region Properties

		/// <summary></summary>
		bool GotoCleanup { get; set; }

		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		IState NextState { get; set; }

		/// <summary></summary>
		bool PreserveNextState { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintObjBlocksTheWay(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="turnCounter"></param>
		void PrintLightAlmostOut(IArtifact artifact, long turnCounter);

		/// <summary></summary>
		/// <param name="commandList"></param>
		/// <param name="commandType"></param>
		/// <param name="newSeen"></param>
		void PrintCommands(IList<ICommand> commandList, CommandType commandType, ref bool newSeen);

		/// <summary></summary>
		void PrintNewCommandSeen();

		/// <summary></summary>
		void PrintCommandPrompt();

		/// <summary></summary>
		void PrintFulfillMessage(string inputStr);

		/// <summary></summary>
		void PrintDontFollowYou();

		/// <summary></summary>
		void PrintDontFollowYou02();

		/// <summary></summary>
		void PrintCantGoThatWay();

		/// <summary></summary>
		/// <param name="verb"></param>
		void PrintCantVerbThere(string verb);

		/// <summary></summary>
		void PrintRideOffIntoSunset();

		/// <summary></summary>
		void PrintLeaveAdventure();

		/// <summary></summary>
		void PrintEnemiesNearby();

		/// <summary></summary>
		void PrintSpeedSpellExpired();

		/// <summary></summary>
		/// <param name="eventType"></param>
		void ProcessEvents(EventType eventType);

		/// <summary></summary>
		/// <param name="target"></param>
		/// <param name="articleType"></param>
		/// <param name="nameType"></param>
		/// <param name="upshift"></param>
		/// <param name="groupCountOne"></param>
		/// <returns></returns>
		string GetDarkName(IGameBase target, ArticleType articleType, string nameType, bool upshift, bool groupCountOne);

		/// <summary></summary>
		/// <returns></returns>
		IList<long> GetLoopMonsterUidList();

		/// <summary></summary>
		void Stage();

		/// <summary></summary>
		void Execute();

		#endregion
	}
}
