
// ParserData.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Parsing;

namespace EamonRT.Game.Parsing
{
	[ClassMappings]
	public class ParserData : IParserData
	{
		public virtual string Name { get; set; }

		public virtual Func<string> QueryDescFunc { get; set; }

		public virtual IGameBase Obj { get; set; }

		public virtual IArtifact Artifact
		{
			get
			{
				return Obj as IArtifact;
			}
		}

		public virtual IMonster Monster
		{
			get
			{
				return Obj as IMonster;
			}
		}

		public virtual IList<IGameBase> GetRecordList { get; set; }

		public virtual IList<IGameBase> FilterRecordList { get; set; }

		public virtual IList<Func<IGameBase, bool>> RecordWhereClauseList { get; set; }

		public virtual Delegates.GetRecordListFunc GetRecordListFunc { get; set; }

		public virtual Delegates.FilterRecordListFunc FilterRecordListFunc { get; set; }

		public virtual Delegates.RevealEmbeddedArtifactFunc RevealEmbeddedArtifactFunc { get; set; }

		public virtual Action RecordMatchFunc { get; set; }

		public virtual Action RecordNotFoundFunc { get; set; }
	}
}
