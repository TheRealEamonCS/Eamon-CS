﻿
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;

namespace StrongholdOfKahrDur.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		/// <param name="cauldronArtifact"></param>
		/// <returns></returns>
		bool SpellReagentsInCauldron(IArtifact cauldronArtifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monsterUid"></param>
		void SummonMonster(IRoom room, long monsterUid);
	}
}
