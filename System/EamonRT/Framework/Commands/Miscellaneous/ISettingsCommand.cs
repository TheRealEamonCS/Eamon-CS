﻿
// ISettingsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface ISettingsCommand : ICommand
	{
		/// <summary></summary>
		bool? VerboseRooms { get; set; }

		/// <summary></summary>
		bool? VerboseMonsters { get; set; }

		/// <summary></summary>
		bool? VerboseArtifacts { get; set; }

		/// <summary></summary>
		bool? VerboseNames { get; set; }

		/// <summary></summary>
		bool? MatureContent { get; set; }

		/// <summary></summary>
		bool? InteractiveFiction { get; set; }

		/// <summary></summary>
		bool? EnhancedCombat { get; set; }

		/// <summary></summary>
		bool? EnhancedParser { get; set; }

		/// <summary></summary>
		bool? IobjPronounAffinity { get; set; }

		/// <summary></summary>
		bool? ShowPronounChanges { get; set; }

		/// <summary></summary>
		bool? ShowFulfillMessages { get; set; }

		/// <summary></summary>
		long? PauseCombatMs { get; set; }

		/// <summary></summary>
		long? PauseCombatActions { get; set; }
	}
}
