
// IBeforePrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Utilities;

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IBeforePrintPlayerRoomEventState : IState
	{
		/// <summary></summary>
		bool ExitEventLoop { get; set; }

		/// <summary></summary>
		/// <param name="eventName"></param>
		/// <param name="eventParam"></param>
		void FireEvent(string eventName, object eventParam);

		/// <summary></summary>
		/// <param name="eventData"></param>
		void FireEvent02(EventData eventData);
	}
}
