
// ITrigger.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface ITrigger : IGameBase, IComparable<ITrigger>
	{
		/// <summary></summary>
		long Occurrences { get; set; }

		/// <summary></summary>
		long SortOrder { get; set; }

		/// <summary></summary>
		TriggerType Type { get; set; }

		/// <summary></summary>
		long Field1 { get; set; }

		/// <summary></summary>
		long Field2 { get; set; }

		/// <summary></summary>
		long Field3 { get; set; }

		/// <summary></summary>
		long Field4 { get; set; }

		/// <summary></summary>
		long Field5 { get; set; }

		/// <summary></summary>
		bool IsActive();

		/// <summary></summary>
		IList<IScript> GetScriptList();
	}
}
