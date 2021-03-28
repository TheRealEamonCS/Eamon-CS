
// IPluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginConstants : EamonDD.Framework.Plugin.IPluginConstants
	{
		/// <summary></summary>
		long StartRoom { get; }

		/// <summary></summary>
		long NumSaveSlots { get; }

		/// <summary></summary>
		long ScaledHardinessUnarmedMaxDamage { get; }

		/// <summary></summary>
		double ScaledHardinessMaxDamageDivisor { get; }

		/// <summary></summary>
		string CommandPrompt { get; }

		/// <summary></summary>
		string PageSep { get; }

		/// <summary></summary>
		string RtProgVersion { get; }
	}
}
