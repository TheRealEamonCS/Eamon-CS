
// PluginConstants.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using EamonRT.Framework.Plugin;

namespace EamonRT.Game.Plugin
{
	public class PluginConstants : EamonDD.Game.Plugin.PluginConstants, IPluginConstants
	{
		public virtual long StartRoom { get; protected set; } = 1;

		public virtual long NumSaveSlots { get; protected set; } = 5;

		public virtual long ScaledHardinessUnarmedMaxDamage { get; protected set; } = 20;

		public virtual double ScaledHardinessMaxDamageDivisor { get; protected set; } = 2.0;

		public virtual string CommandPrompt { get; protected set; } = "> ";

		public virtual string PageSep { get; protected set; } = "@@PB";

		public virtual string RtProgVersion { get; protected set; }

		public PluginConstants()
		{
			RtProgVersion = ProgVersion;
		}
	}
}
