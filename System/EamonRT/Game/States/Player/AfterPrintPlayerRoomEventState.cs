
// AfterPrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Reflection;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class AfterPrintPlayerRoomEventState : State, IAfterPrintPlayerRoomEventState
	{
		public long _eventTurn;

		public EventData _eventData;

		/// <summary></summary>
		public virtual string EventMethodName { get; set; }

		/// <summary></summary>
		public virtual MethodInfo EventMethodInfo { get; set; }

		/// <summary></summary>
		public virtual long EventTurn 
		{
			get
			{
				return _eventTurn;
			}

			set
			{
				_eventTurn = value;
			}
		}

		/// <summary></summary>
		public virtual EventData EventData
		{
			get
			{
				return _eventData;
			}

			set
			{
				_eventData = value;
			}
		}

		public override void Execute()
		{
			if (gEngine.CommandPromptSeen && !gEngine.ShouldPreTurnProcess)
			{
				goto Cleanup;
			}

			EventTurn = 0;

			EventData = null;

			gGameState.AfterPrintPlayerRoomEventHeap.PeekMin(ref _eventTurn, ref _eventData);

			while (EventData != null && !string.IsNullOrWhiteSpace(EventData.EventName) && EventTurn <= gGameState.CurrTurn)
			{
				gGameState.AfterPrintPlayerRoomEventHeap.RemoveMin(ref _eventTurn, ref _eventData);

				FireEvent02(EventData);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				EventTurn = 0;

				EventData = null;

				gGameState.AfterPrintPlayerRoomEventHeap.PeekMin(ref _eventTurn, ref _eventData);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IGetPlayerInputState>();
			}

			gEngine.NextState = NextState;
		}

		public virtual void FireEvent(string eventName, object eventParam)
		{
			FireEvent02(new EventData() { EventName = eventName, EventParam = eventParam });
		}

		public virtual void FireEvent02(EventData eventData)
		{
			Debug.Assert(eventData != null && !string.IsNullOrWhiteSpace(eventData.EventName));

			EventMethodName = string.Format("{0}{1}", eventData.EventName, !eventData.EventName.EndsWith("Event") ? "Event" : "");

			EventMethodInfo = GetType().GetMethod(EventMethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

			if (EventMethodInfo != null)
			{
				EventMethodInfo.Invoke(this, new object[] { eventData.EventParam });
			}
		}

		public AfterPrintPlayerRoomEventState()
		{
			Name = "AfterPrintPlayerRoomEventState";
		}
	}
}
