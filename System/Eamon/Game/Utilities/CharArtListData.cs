
// CharArtListData.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Framework.Utilities;
using Eamon.Game.Attributes;

namespace Eamon.Game.Utilities
{
	[ClassMappings]
	public class CharArtListData : ICharArtListData
	{
		public virtual long CharUid { get; set; }

		public virtual Armor ArmorClass { get; set; }

		public virtual IArtifact Armor { get; set; }

		public virtual IArtifact Shield { get; set; }

		public virtual IList<IArtifact> Weapons { get; set; }

		public CharArtListData()
		{
			Weapons = new List<IArtifact>();
		}
	}
}
