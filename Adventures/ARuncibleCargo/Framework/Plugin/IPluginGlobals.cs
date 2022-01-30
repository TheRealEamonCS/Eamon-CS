
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework.Primitive.Classes;

namespace ARuncibleCargo.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		IList<IArtifactLinkage> DoubleDoorList { get; set; }
	}
}
