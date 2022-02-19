
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace WrenholdsSecretVigil.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		Action MonsterCurseFunc { get; set; }

		/// <summary></summary>
		bool MonsterCurses { get; set; }

		/// <summary></summary>
		bool DeviceOpened { get; set; }
	}
}
