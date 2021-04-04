
// EDXAdventure.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace EamonDD.Game.Converters.EamonDeluxe
{
	/// <summary>
	/// </summary>
	public class EDXAdventure
	{
		public long _nr;

		public long _na;

		public long _ne;

		public long _nm;

		public long _nd;

		public long _rptr;

		public long _aptr;

		public long _eptr;

		public long _mptr;

		public virtual IList<EDXRoom> RoomList { get; set; }

		public virtual IList<EDXArtifact> ArtifactList { get; set; }

		public virtual IList<EDXMonster> MonsterList { get; set; }

		public virtual IList<EDXDesc> EffectList { get; set; }

		public virtual string Name { get; set; }

		public EDXAdventure()
		{
			RoomList = new List<EDXRoom>();

			ArtifactList = new List<EDXArtifact>();

			MonsterList = new List<EDXMonster>();

			EffectList = new List<EDXDesc>();
		}
	}
}
