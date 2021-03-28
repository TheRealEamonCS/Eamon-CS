
// IParserData.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;

namespace EamonRT.Framework.Parsing
{
	/// <summary></summary>
	public interface IParserData
	{
		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		Func<string> QueryDescFunc { get; set; }

		/// <summary></summary>
		IGameBase Obj { get; set; }

		/// <summary></summary>
		IArtifact Artifact { get; }

		/// <summary></summary>
		IMonster Monster { get; }

		/// <summary></summary>
		IList<IGameBase> GetRecordList { get; set; }

		/// <summary></summary>
		IList<IGameBase> FilterRecordList { get; set; }

		/// <summary></summary>
		IList<Func<IGameBase, bool>> RecordWhereClauseList { get; set; }

		/// <summary></summary>
		Delegates.GetRecordListFunc GetRecordListFunc { get; set; }

		/// <summary></summary>
		Delegates.FilterRecordListFunc FilterRecordListFunc { get; set; }

		/// <summary></summary>
		Delegates.RevealEmbeddedArtifactFunc RevealEmbeddedArtifactFunc { get; set; }

		/// <summary></summary>
		Action RecordMatchFunc { get; set; }

		/// <summary></summary>
		Action RecordNotFoundFunc { get; set; }
	}
}
