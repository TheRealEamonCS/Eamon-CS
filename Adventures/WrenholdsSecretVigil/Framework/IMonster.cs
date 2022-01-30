
// IMonster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IMonster : Eamon.Framework.IMonster
	{
		/// <summary>Indicates whether this <see cref="IMonster">Monster</see> should refuse to accept a given <see cref="Eamon.Framework.IArtifact">Artifact</see> as a gift.</summary>
		/// <param name="artifact">The <see cref="Eamon.Framework.IArtifact">Artifact</see> to check.</param>
		/// <returns></returns>
		bool ShouldRefuseToAcceptGift01(Eamon.Framework.IArtifact artifact);
	}
}
