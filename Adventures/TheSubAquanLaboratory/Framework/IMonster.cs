
// IMonster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheSubAquanLaboratory.Framework
{
	/// <summary></summary>
	public interface IMonster : Eamon.Framework.IMonster
	{
		/// <summary>Indicates whether this <see cref="IMonster">Monster</see> is classified as an android.</summary>
		/// <returns>If so then <c>true</c>, otherwise <c>false</c>.</returns>
		bool IsAndroid();
	}
}
