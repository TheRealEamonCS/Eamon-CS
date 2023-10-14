
// IExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheSubAquanLaboratory.Framework.Commands
{
	/// <summary></summary>
	public interface IExamineCommand : EamonRT.Framework.Commands.IExamineCommand
	{
		/// <summary></summary>
		bool ExamineConsole { get; set; }
	}
}
