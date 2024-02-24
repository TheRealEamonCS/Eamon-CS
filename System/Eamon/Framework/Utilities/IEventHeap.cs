
// IEventHeap.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;

namespace Eamon.Framework.Utilities
{
	/// <summary></summary>
	public interface IEventData
	{
		/// <summary></summary>
		string EventName { get; set; }

		/// <summary></summary>
		object EventParam { get; set; }
	}

	/// <summary></summary>
	public interface IEventHeap
	{
		/// <summary></summary>
		bool IsEmpty();

		/// <summary></summary>
		void Clear();

		/// <summary></summary>
		bool Insert(long key, string eventName, Func<long, IEventData, bool> duplicateFindFunc = null);

		/// <summary></summary>
		bool Insert02(long key, string eventName, object eventParam, Func<long, IEventData, bool> duplicateFindFunc = null);

		/// <summary></summary>
		bool Insert03(long key, IEventData value, Func<long, IEventData, bool> duplicateFindFunc = null);

		/// <summary></summary>
		IList<KeyValuePair<long, IEventData>> Find(Func<long, IEventData, bool> findFunc = null);

		/// <summary></summary>
		IList<KeyValuePair<long, IEventData>> Remove(Func<long, IEventData, bool> findFunc = null);

		/// <summary></summary>
		void RemoveMin(ref long key, ref IEventData value);

		/// <summary></summary>
		void PeekMin(ref long key, ref IEventData value);
	}
}
