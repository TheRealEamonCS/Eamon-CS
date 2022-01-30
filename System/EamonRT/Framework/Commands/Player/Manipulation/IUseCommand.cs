
// IUseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface IUseCommand : ICommand
	{
		/// <summary></summary>
		ArtifactType[] ArtTypes { get; set; }
	}
}
