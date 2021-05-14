
// A2EAdventure.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonDD.Game.Converters.Apple2Eamon
{
	/// <summary>
	/// </summary>
	public class A2EAdventure
	{
		public long _nr;

		public long _na;

		public long _ne;

		public long _nm;

		public long _nd;

		public long _aptr;

		public long _eptr;

		public long _mptr;

		public long _type;

		public long _dlen;

		public long _rlen;

		public long _rnlen;

		public long _alen;

		public long _mlen;

		public string _ver;

		public virtual IList<A2ERoom> RoomList { get; set; }

		public virtual IList<A2EArtifact> ArtifactList { get; set; }

		public virtual IList<A2EMonster> MonsterList { get; set; }

		public virtual IList<A2EEffect> EffectList { get; set; }

		public virtual string Name { get; set; }

		public A2EAdventure()
		{
			RoomList = new List<A2ERoom>();

			ArtifactList = new List<A2EArtifact>();

			MonsterList = new List<A2EMonster>();

			EffectList = new List<A2EEffect>();
		}
	}
}
