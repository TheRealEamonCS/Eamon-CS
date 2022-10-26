
// PropertyResetCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary></summary>
	public enum PropertyResetCode : long
	{
		/// <summary></summary>
		None = 0,

		/// <summary></summary>
		All,

		/// <summary></summary>
		PlayerDead,

		/// <summary></summary>
		ResurrectPlayer,

		/// <summary></summary>
		LoadDatabase,

		/// <summary></summary>
		RestoreDatabase,

		/// <summary></summary>
		RestoreGame,

		/// <summary></summary>
		SwitchContext,

		/// <summary></summary>
		RevealContainerContents
	}
}
