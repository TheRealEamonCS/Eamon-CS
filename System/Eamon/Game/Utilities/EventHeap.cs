
// EventHeap.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eamon.Game.Utilities
{
	public class EventData
	{
		public virtual string EventName { get; set; }

		public virtual object EventParam { get; set; }
	}

	public class EventHeap
	{
		public virtual IDictionary<long, IList<EventData>> EventDictionary { get; set; }

		public virtual bool IsEmpty()
		{
			return EventDictionary.Count <= 0;
		}

		public virtual void Clear()
		{
			EventDictionary.Clear();
		}

		public virtual bool Insert(long key, string eventName, Func<long, EventData, bool> duplicateFindFunc = null)
		{
			return Insert02(key, eventName, null, duplicateFindFunc);
		}

		public virtual bool Insert02(long key, string eventName, object eventParam, Func<long, EventData, bool> duplicateFindFunc = null)
		{
			return Insert03(key, new EventData() { EventName = eventName, EventParam = eventParam }, duplicateFindFunc);
		}

		public virtual bool Insert03(long key, EventData value, Func<long, EventData, bool> duplicateFindFunc = null)
		{
			var result = false;

			IList<EventData> listValue;

			if (key >= 0 && value != null && !string.IsNullOrWhiteSpace(value.EventName))
			{
				var eventList = Find(duplicateFindFunc);

				if (eventList.Count == 0)
				{
					if (EventDictionary.TryGetValue(key, out listValue))
					{
						listValue.Add(value);
					}
					else
					{
						listValue = new List<EventData>();

						listValue.Add(value);

						EventDictionary.Add(key, listValue);
					}

					result = true;
				}
			}

			return result;
		}

		public virtual IList<KeyValuePair<long, EventData>> Find(Func<long, EventData, bool> findFunc = null)
		{
			var eventList = new List<KeyValuePair<long, EventData>>();

			foreach (var entry in EventDictionary)
			{
				foreach (var eventData in entry.Value)
				{
					if (findFunc != null && findFunc(entry.Key, eventData))
					{
						eventList.Add(new KeyValuePair<long, EventData>(entry.Key, eventData));
					}
				}
			}

			return eventList;
		}

		public virtual IList<KeyValuePair<long, EventData>> Remove(Func<long, EventData, bool> findFunc = null)
		{
			IList<EventData> listValue;

			var eventList = Find(findFunc);

			foreach (var entry in eventList)
			{
				if (EventDictionary.TryGetValue(entry.Key, out listValue) && listValue.Remove(entry.Value) && listValue.Count <= 0)
				{
					EventDictionary.Remove(entry.Key);
				}
			}

			return eventList;
		}

		public virtual void RemoveMin(ref long key, ref EventData value)
		{
			IList<EventData> listValue;

			if (EventDictionary.Count > 0)
			{
				key = EventDictionary.First().Key;

				listValue = EventDictionary[key];

				value = listValue[0];

				listValue.RemoveAt(0);

				if (listValue.Count <= 0)
				{
					EventDictionary.Remove(key);
				}
			}
		}

		public virtual void PeekMin(ref long key, ref EventData value)
		{
			IList<EventData> listValue;

			if (EventDictionary.Count > 0)
			{
				key = EventDictionary.First().Key;

				listValue = EventDictionary[key];

				value = listValue[0];
			}
		}

		public EventHeap()
		{
			EventDictionary = new SortedDictionary<long, IList<EventData>>();
		}
	}
}
