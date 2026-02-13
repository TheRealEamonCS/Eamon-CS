
// ArtifactExtendedAttributes.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	public enum ArtifactExtendedAttributes : ulong
	{
		/// <summary></summary>
		IsRequestable = 1,

		/// <summary></summary>
		IsInContainerOpenedFromTop = 2,

		/// <summary></summary>
		ShouldExposeInContentsWhenClosed = 4,

		/// <summary></summary>
		ShouldAddContents = 8,

		/// <summary></summary>
		ShouldAddToHeldWpnUids = 16,

		/// <summary></summary>
		ShouldRevealContentsWhenMovedIntoLimbo = 32,

		/// <summary></summary>
		ShouldShowContentsWhenExamined = 64,

		/// <summary></summary>
		ShouldShowVerboseNameContentsNameList = 128,

		/// <summary></summary>
		ShouldShowVerboseNameStateDesc = 256,
	}
}
