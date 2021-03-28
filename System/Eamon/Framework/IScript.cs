
// IScript.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework
{
	/// <summary></summary>
	public interface IScript : IGameBase, IComparable<IScript>
	{
		/// <summary></summary>
		long SortOrder { get; set; }

		/// <summary></summary>
		long TriggerUid { get; set; }

		/// <summary></summary>
		ScriptType Type { get; set; }

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
		void Execute();
	}
}
