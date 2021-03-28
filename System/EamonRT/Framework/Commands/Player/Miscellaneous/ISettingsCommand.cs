
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
		bool? MatureContent { get; set; }

		/// <summary></summary>
		bool? EnhancedParser { get; set; }

		/// <summary></summary>
		bool? ShowPronounChanges { get; set; }

		/// <summary></summary>
		bool? ShowFulfillMessages { get; set; }

		/// <summary></summary>
		long? PauseCombatMs { get; set; }

		/// <summary></summary>
		void PrintUsage();
	}
}
