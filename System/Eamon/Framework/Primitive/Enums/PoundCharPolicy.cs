
// PoundCharPolicy.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary></summary>
	public enum PoundCharPolicy : long
	{
		/// <summary></summary>
		None = 0,					// No pound chars on artifact names

		/// <summary></summary>
		PlayerArtifactsOnly,		// Pound chars only on player artifact names
		
		/// <summary></summary>
		AllArtifacts				// Pound chars on all artifacts in database
	}
}
