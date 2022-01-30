
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework.Primitive.Classes;

namespace ARuncibleCargo.Game.Plugin
{
	public class PluginGlobals : EamonRT.Game.Plugin.PluginGlobals, Framework.Plugin.IPluginGlobals
	{
		public virtual IList<IArtifactLinkage> DoubleDoorList { get; set; }

		public override void InitSystem()
		{
			base.InitSystem();

			DoubleDoorList = new List<IArtifactLinkage>();
		}
	}
}
