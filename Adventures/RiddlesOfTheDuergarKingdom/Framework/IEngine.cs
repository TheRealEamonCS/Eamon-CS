﻿
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;

namespace RiddlesOfTheDuergarKingdom.Framework
{
	/// <summary></summary>
	public interface IEngine : EamonRT.Framework.IEngine
	{
		long[] ArchaeologyDepartmentMonsterUids { get; set; }

		long[] ArchaeologyDepartmentStartRoomUids { get; set; }

		long[] ArchaeologyDepartmentAbandonedRoomUids { get; set; }

		long[] GradStudentStartRoomUids { get; set; }

		long[] RanchHandsStartRoomUids { get; set; }

		long[] SupportBeamsRoomUids { get; set; }

		long[] TracksRoomUids { get; set; }

		long[] LavaRiverRoomUids { get; set; }

		long[] BroilingRoomUids { get; set; }

		long[] BeachRoomUids { get; set; }

		/// <summary></summary>
		void ArchaeologyDepartmentAbandonsExcavationSite();

		/// <summary></summary>
		void SteamTurbineStopsRunning();

		/// <summary></summary>
		void RockCrusherDestroysContents(Eamon.Framework.IRoom room);

		/// <summary></summary>
		void RockGrinderDestroysContents(Eamon.Framework.IRoom room);

		/// <summary></summary>
		void DebrisSifterVibratesContents(Eamon.Framework.IRoom room, IArtifact artifact = null);
	}
}
