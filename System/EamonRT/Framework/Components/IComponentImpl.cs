
// IComponentImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Components
{
	/// <summary></summary>
	public interface IComponentImpl : IComponentSignatures
	{
		/// <summary></summary>
		IComponent Component { get; set; }
	}
}
