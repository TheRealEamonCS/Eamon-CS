
// IStateSignatures.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Primitive.Enums;

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IStateSignatures
	{
		#region Properties

		/// <summary></summary>
		long Uid { get; set; }

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
		void PrintEnemiesNearby();

		/// <summary></summary>
		/// <param name="eventType"></param>
		void ProcessEvents(EventType eventType);

		/// <summary></summary>
		/// <param name="printOutput"></param>
		void ProcessRevealContentArtifactList(bool printOutput = true);

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
		bool ShouldPreTurnProcess();

		/// <summary></summary>
		void Execute();

		/// <summary></summary>
		void PreExecute();

		#endregion
	}
}
