
// EventHeap.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Eamon.Framework.Utilities;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.Utilities
{
	[ClassMappings]
	public class EventData : IEventData
	{
		public virtual string EventName { get; set; }

		public virtual object EventParam { get; set; }
	}

	[ClassMappings]
	public class EventHeap : IEventHeap
	{
		/// <summary></summary>
		public virtual IDictionary<long, IList<IEventData>> EventDictionary { get; set; }

		public virtual bool IsEmpty()
		{
			return EventDictionary.Count <= 0;
		}

		public virtual void Clear()
		{
			EventDictionary.Clear();
		}

		public virtual bool Insert(long key, string eventName, Func<long, IEventData, bool> duplicateFindFunc = null)
		{
			return Insert02(key, eventName, null, duplicateFindFunc);
		}

		public virtual bool Insert02(long key, string eventName, object eventParam, Func<long, IEventData, bool> duplicateFindFunc = null)
		{
			var eventData = gEngine.CreateInstance<IEventData>(x => 
			{ 
				x.EventName = eventName;
				
				x.EventParam = eventParam; 
			});
			
			return Insert03(key, eventData, duplicateFindFunc);
		}

		public virtual bool Insert03(long key, IEventData value, Func<long, IEventData, bool> duplicateFindFunc = null)
		{
			var result = false;

			IList<IEventData> listValue;

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
						listValue = new List<IEventData>();

						listValue.Add(value);

						EventDictionary.Add(key, listValue);
					}

					result = true;
				}
			}

			return result;
		}

		public virtual IList<KeyValuePair<long, IEventData>> Find(Func<long, IEventData, bool> findFunc = null)
		{
			var eventList = new List<KeyValuePair<long, IEventData>>();

			foreach (var entry in EventDictionary)
			{
				foreach (var eventData in entry.Value)
				{
					if (findFunc != null && findFunc(entry.Key, eventData))
					{
						eventList.Add(new KeyValuePair<long, IEventData>(entry.Key, eventData));
					}
				}
			}

			return eventList;
		}

		public virtual IList<KeyValuePair<long, IEventData>> Remove(Func<long, IEventData, bool> findFunc = null)
		{
			IList<IEventData> listValue;

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

		public virtual void RemoveMin(ref long key, ref IEventData value)
		{
			IList<IEventData> listValue;

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

		public virtual void PeekMin(ref long key, ref IEventData value)
		{
			IList<IEventData> listValue;

			if (EventDictionary.Count > 0)
			{
				key = EventDictionary.First().Key;

				listValue = EventDictionary[key];

				value = listValue[0];
			}
		}

		public EventHeap()
		{
			EventDictionary = new SortedDictionary<long, IList<IEventData>>();
		}
	}
}
