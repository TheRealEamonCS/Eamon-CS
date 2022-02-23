
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace WrenholdsSecretVigil.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginGlobals : EamonRT.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		bool MonsterCurses { get; set; }

		/// <summary></summary>
		bool DeviceOpened { get; set; }
	}
}
