
// ISayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface ISayCommand : ICommand
	{
		/// <summary></summary>
		string OriginalPhrase { get; set; }

		/// <summary></summary>
		string PrintedPhrase { get; set; }

		/// <summary></summary>
		string ProcessedPhrase { get; set; }
	}
}
