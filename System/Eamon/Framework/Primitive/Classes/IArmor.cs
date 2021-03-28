
// IArmor.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface IArmor
	{
		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		string MarcosName { get; set; }

		/// <summary></summary>
		long MarcosPrice { get; set; }

		/// <summary></summary>
		long MarcosNum { get; set; }

		/// <summary></summary>
		long ArtifactValue { get; set; }
	}
}
