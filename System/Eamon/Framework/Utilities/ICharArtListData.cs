
// ICharArtListData.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Utilities
{
	/// <summary></summary>
	public interface ICharArtListData
	{
		long CharUid { get; set; }

		Armor ArmorClass { get; set; }

		IArtifact Armor { get; set; }

		IArtifact Shield { get; set; }

		IList<IArtifact> Weapons { get; set; }
	}
}
